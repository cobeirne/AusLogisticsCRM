using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class GeoRoute
    {
        public Geolocation Origin { get; set; }

        public Geolocation Destination { get; set; }

        public int RouteSeconds { get; set; }

        public int RouteMeters { get; set; }   

        public List<string> RouteStates { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public int DeliverySeconds { get; set; }

        public DateTime PickupTime { get; set; }

        public List<GeoRouteLeg> RouteLegs { get; set; } 

        public GeoRoute()
        { }

        public GeoRoute(Geolocation origin, Geolocation destination, DateTime deliveryTime)
        {
            this.Origin = origin;
            this.Destination = destination;
            this.DeliveryDateTime = deliveryTime;
            this.RouteLegs = new List<GeoRouteLeg>();
        }

        public void GetGeoCodes()
        {
            GoogleMapsAPI mapsApi = new GoogleMapsAPI();
            this.Origin = mapsApi.GetGeoCode(this.Origin);
            this.Destination = mapsApi.GetGeoCode(this.Destination);
        }

        public void CalculatePickupTime()
        {
            DeliveryTimer timer = new DeliveryTimer(this);
            this.DeliverySeconds = timer.CalculateDeliverySeconds();
            this.PickupTime = timer.CalculatePickupTime(this.DeliveryDateTime, this.RouteSeconds);
        }

        public void SplitRouteByState()
        {
            DateTime currentStartTime = this.PickupTime;
            int secondsSplit = this.RouteSeconds / RouteStates.Count;

            foreach(string state in this.RouteStates)
            {
                this.RouteLegs.Add(new GeoRouteLeg
                {
                    LegStartTime = currentStartTime,
                    LegFinishTime = currentStartTime.AddSeconds(secondsSplit - 1),
                    State = state,
                    LegSeconds = secondsSplit
                });

                currentStartTime = currentStartTime.AddSeconds(secondsSplit);
            }
        }
                
        public List<GeoRouteLeg> OffsetLegs(List<GeoRouteLeg> legs , int offsetMinutes, DateTime firstStartDate)
        {
            List<GeoRouteLeg> offsetLegs = legs.Where(l => l.LegStartTime >= firstStartDate).ToList();

            foreach(GeoRouteLeg leg in offsetLegs)
            {
                leg.LegStartTime = leg.LegStartTime.AddMinutes(offsetMinutes);
                leg.LegFinishTime = leg.LegFinishTime.AddMinutes(offsetMinutes);
            }

            return legs;
        }


        public void SplitRouteLegsByBreak()
        {
            this.RouteLegs.OrderBy(l => l.LegStartTime);

            int breaksRequired = ((RouteSeconds / 60) / Properties.Settings.Default.MaxDrivingMinutes) - 1 ;
            int breaksTaken = 0;

            while (breaksTaken < breaksRequired)
            {
                breaksTaken = this.RouteLegs.Count(l => l.BreakPeriod == true);

                DateTime lastBreakFinish;

                if (breaksTaken != 0)
                {
                    lastBreakFinish = this.RouteLegs.Last(l => l.BreakPeriod == true).LegFinishTime;
                }
                else
                {
                    lastBreakFinish = this.RouteLegs.First().LegStartTime.AddSeconds(-1);
                }

                DateTime nextBreakStart = lastBreakFinish.AddMinutes(Properties.Settings.Default.MaxDrivingMinutes).AddSeconds(1);

                GeoRouteLeg splitLeg = this.RouteLegs.First(l => l.LegStartTime >= nextBreakStart);

                this.RouteLegs.Add(new GeoRouteLeg
                {
                    LegStartTime = nextBreakStart,
                    LegFinishTime = splitLeg.LegFinishTime,
                    State = splitLeg.State,
                    LegStateRates = splitLeg.LegStateRates,
                    BreakPeriod = false
                });

                splitLeg.LegFinishTime = nextBreakStart.AddSeconds(-1);

                OffsetLegs(this.RouteLegs, Properties.Settings.Default.MinBreakMinutes, nextBreakStart);

                this.RouteLegs.Add(new GeoRouteLeg
                {
                    LegStartTime = nextBreakStart,
                    LegFinishTime = nextBreakStart.AddMinutes(Properties.Settings.Default.MinBreakMinutes).AddSeconds(-1),
                    State = splitLeg.State,
                    LegStateRates = splitLeg.LegStateRates,
                    BreakPeriod = true
                });

                this.RouteLegs = this.RouteLegs.OrderBy(l => l.LegStartTime).ToList();

                breaksTaken = this.RouteLegs.Count(l => l.BreakPeriod == true);
            }

            this.RouteLegs = this.RouteLegs.Where(l => l.LegFinishTime > l.LegStartTime).ToList();

            this.RouteLegs = this.RouteLegs;
        }



        public List<GeoRouteLeg> SplitLegsByDay(List<GeoRouteLeg> legs)
        {
            legs.OrderBy(l => l.LegStartTime);
            GeoRouteLeg lastLeg = legs.Last();

            DateTime nextDayStart = GetNextDayStart(lastLeg.LegStartTime);

            if (lastLeg.LegFinishTime < nextDayStart)
            {
                return legs;
            }
            else
            {
                legs.Add(new GeoRouteLeg
                {
                    LegStartTime = nextDayStart,
                    LegFinishTime = lastLeg.LegFinishTime,
                    State = lastLeg.State,
                    LegStateRates = lastLeg.LegStateRates,
                    BreakPeriod = lastLeg.BreakPeriod
                });

                lastLeg.LegFinishTime = nextDayStart.AddSeconds(-1);

                return SplitLegsByDay(legs);
            }            
        }

        public void SplitRouteLegsByDay()
        {
            List<GeoRouteLeg> splitLegs = new List<GeoRouteLeg>();

            this.RouteLegs.OrderBy(l => l.LegStartTime);

            foreach(GeoRouteLeg leg in this.RouteLegs)
            {
                List<GeoRouteLeg> workingLegs = new List<GeoRouteLeg> { leg };
                splitLegs.AddRange(SplitLegsByDay(workingLegs));
            }

            this.RouteLegs = splitLegs;
        }

        public List<GeoRouteLeg> SplitLegsByShift(List<GeoRouteLeg> legs)
        {
            legs.OrderBy(l => l.LegStartTime);
            GeoRouteLeg lastLeg = legs.Last();

            DateTime stateDayShiftStart = lastLeg.LegStateRates.DayRates[0].StateShifts[0].StartTime;
            DateTime stateDayShiftFinish = lastLeg.LegStateRates.DayRates[0].StateShifts[0].FinishTime;

            DateTime dayShiftStart = new DateTime(lastLeg.LegStartTime.Year, lastLeg.LegStartTime.Month, lastLeg.LegStartTime.Day, stateDayShiftStart.Hour, stateDayShiftStart.Minute, stateDayShiftStart.Second);
            DateTime dayShiftEnd = new DateTime(lastLeg.LegStartTime.Year, lastLeg.LegStartTime.Month, lastLeg.LegStartTime.Day, stateDayShiftFinish.Hour, stateDayShiftFinish.Minute, stateDayShiftFinish.Second);
            
            bool startInDayShift = (dayShiftStart <= lastLeg.LegStartTime && lastLeg.LegStartTime < dayShiftEnd) ? true : false;
            bool startInEveningShift = (lastLeg.LegStartTime >= dayShiftEnd) ? true : false;
            bool startInNightShift = (lastLeg.LegStartTime < dayShiftStart) ? true : false;

            bool finsihInDayShift = (dayShiftStart <= lastLeg.LegFinishTime && lastLeg.LegFinishTime < dayShiftEnd) ? true : false;
            bool finishInEveningShift = (lastLeg.LegFinishTime >= dayShiftEnd) ? true : false;
            bool finishInNightShift = (lastLeg.LegFinishTime < dayShiftStart) ? true : false;

            lastLeg.DayShift = startInDayShift;

            if ((startInDayShift && finsihInDayShift) || (startInEveningShift && finishInEveningShift) || (startInNightShift && finishInNightShift))
            {
                
                return legs;
            }
            else
            {
                DateTime nextShiftStart;
                if (startInDayShift)
                {
                    nextShiftStart = new DateTime(lastLeg.LegStartTime.Year, lastLeg.LegStartTime.Month, lastLeg.LegStartTime.Day, dayShiftEnd.Hour, dayShiftEnd.Minute, dayShiftEnd.Second);
                }
                else if (startInEveningShift)
                {
                    nextShiftStart = GetNextDayStart(lastLeg.LegStartTime);
                }
                else // startInNightShift
                {
                    nextShiftStart = new DateTime(lastLeg.LegStartTime.Year, lastLeg.LegStartTime.Month, lastLeg.LegStartTime.Day, dayShiftStart.Hour, dayShiftStart.Minute, dayShiftStart.Second);
                }

                legs.Add(new GeoRouteLeg
                {
                    LegStartTime = nextShiftStart,
                    LegFinishTime = lastLeg.LegFinishTime,
                    State = lastLeg.State,
                    LegStateRates = lastLeg.LegStateRates,
                    BreakPeriod = lastLeg.BreakPeriod
                });

                lastLeg.LegFinishTime = nextShiftStart.AddSeconds(-1);

                return SplitLegsByShift(legs);
            }
        }

        public void SplitRouteLegsByShift()
        {
            List<GeoRouteLeg> splitLegs = new List<GeoRouteLeg>();

            this.RouteLegs.OrderBy(l => l.LegStartTime);

            foreach (GeoRouteLeg leg in this.RouteLegs)
            {
                List<GeoRouteLeg> workingLegs = new List<GeoRouteLeg> { leg };

                splitLegs.AddRange(SplitLegsByShift(workingLegs));
            }

            this.RouteLegs = splitLegs;
        }

        private DateTime GetNextDayStart(DateTime currentDay)
        {
            return new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 0, 0, 0).AddDays(1);
        }

        private DateTime GetCurrentDayStart(DateTime currentDay)
        {
            return new DateTime(currentDay.Year, currentDay.Month, currentDay.Day, 0, 0, 0);
        }

    }
}