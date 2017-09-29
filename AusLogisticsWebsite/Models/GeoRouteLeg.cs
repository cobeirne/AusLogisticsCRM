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

    public class GeoRouteLeg
    {
        public DateTime LegStartTime { get; set; }

        public DateTime LegFinishTime { get; set; }

        public string State { get; set; }

        public string DayType { get; set; }

        public bool DayShift { get; set; }

        public int LegMeters { get; set; }

        public double LegSeconds { get; set; }

        public decimal LegRate { get; set; }

        public bool BreakPeriod { get; set; }

        public StateRates LegStateRates { get; set; }

        public GeoRouteLeg()
        {
        }

        public GeoRouteLeg(string stateCode)
        {
            this.LegStateRates = new StateRates(stateCode);
        }

        public GeoRouteLeg(string csvFilePath, string stateCode)
        {
            this.LegStateRates = new StateRates(csvFilePath, stateCode);
        }

        public void UpdateStateRates()
        {
            this.LegStateRates = new StateRates(this.State);
        }

        public void UpdateStateRates(string csvFilePath)
        {
            this.LegStateRates = new StateRates(csvFilePath, this.State);
        }


        public void UpdateShiftRate()
        {
            string dayType = "WEEKDAY";

            dayType = ((this.LegStartTime.DayOfWeek == DayOfWeek.Saturday) || (this.LegStartTime.DayOfWeek == DayOfWeek.Sunday)) ? "WEEKEND" : dayType;

            StateHoliday publicHoliday = new StateHoliday(this.LegStartTime);
            dayType = (publicHoliday.PublicHoliday == true) ? "HOLIDAY" : dayType;

            if(this.BreakPeriod)
            {
                this.LegRate = 0.0M;
                dayType = "BREAK";
            }
            else if (this.DayShift)
            {
                this.LegRate = this.LegStateRates.DayRates[0].StateShifts.First(t => t.ShiftType == dayType).Rate;
            }
            else
            {
                this.LegRate = this.LegStateRates.DayRates[1].StateShifts.First(t => t.ShiftType == dayType).Rate;
            }

            this.DayType = dayType;
        }

        public void UpdateLegSeconds()
        {
            this.LegSeconds = (this.LegFinishTime - this.LegStartTime).TotalSeconds;
        }
    }
}