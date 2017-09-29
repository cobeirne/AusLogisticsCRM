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

    public class DeliveryTimer
    {
        public GeoRoute Route { get; set; }

        public int TasFerrySeconds { get; set; }

        public int MaxDrivingHours { get; set; }

        public int MinRestHours { get; set; }

        public int RestSeconds { get; set; }

        public DeliveryTimer()
        { }

        public DeliveryTimer(GeoRoute route)
        {
            this.Route = route;
            this.TasFerrySeconds = Properties.Settings.Default.TasFerrySeconds;
            this.MaxDrivingHours = Properties.Settings.Default.MaxDrivingMinutes / 60;
            this.MinRestHours = Properties.Settings.Default.MinBreakMinutes / 60;
        }

        public int CalculateDeliverySeconds()
        {                        
            int deliverySeconds = Route.RouteSeconds;

            bool ferryOnRoute = Route.RouteStates.Contains("TAS") ? true : false;
            if (ferryOnRoute)
            {
                deliverySeconds -= TasFerrySeconds;
            }

            int deliveryHours = deliverySeconds / 60 / 60;
            int drivingDays = Math.Abs(deliveryHours / this.MaxDrivingHours);

            int restHours = drivingDays * this.MinRestHours;
            this.RestSeconds = restHours * 60 * 60;

            deliverySeconds += this.RestSeconds;

            if (ferryOnRoute)
            {
                deliverySeconds += TasFerrySeconds;
            }

            return deliverySeconds;
        }

        public DateTime CalculatePickupTime(DateTime deliveryTimeRequired, int routeSeconds)
        {
            DateTime pickupTime = deliveryTimeRequired.AddSeconds(-routeSeconds);

            return pickupTime;
        }
    }
}