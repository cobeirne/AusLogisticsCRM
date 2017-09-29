using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AusLogisticsWebsite.Models;
using System.Threading;

namespace AusLogisticsWebsite.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class OrderController
    {
        public TransportOrder Order { get; set; }

        public void ProcessOrder(object eventHandles)
        {
            var autoEventHandles = (AutoResetEvent[])eventHandles;

            try
            {
                Default.GoogleMapsMutex.WaitOne();

                this.Order.Route.GetGeoCodes();

                GoogleMapsAPI mapsApi = new GoogleMapsAPI();
                mapsApi.GetGeoRoute(this.Order.Route);
            }
            finally
            {
                Default.GoogleMapsMutex.ReleaseMutex();
            }

            this.Order.Route.CalculatePickupTime();

            decimal maxTruckLoad = Convert.ToDecimal(Properties.Settings.Default.TruckMaxLoad);
            DeliveryFee deliveryFee = new DeliveryFee(this.Order.DeliveryWeight, maxTruckLoad, this.Order.Route);
            deliveryFee.CalculateTrucksRequired();
            deliveryFee.CalculateInterstateDeliveryFee();
            this.Order.TrucksRequired = deliveryFee.TrucksRequired;
            this.Order.OrderItems.Add(new OrderItem("Delivery Fee", 1, deliveryFee.InterstateFee));

            TasFerry tasFerryFee = new TasFerry(this.Order.Route.RouteStates);
            tasFerryFee.CalculateFerryFee();
            if (tasFerryFee.Fee > 0)
            {
                this.Order.OrderItems.Add(new OrderItem("TAS Ferry Toll", 1, tasFerryFee.Fee));
            }

            InsuranceFee insuranceFee = new InsuranceFee(Order.Route.RouteMeters);
            insuranceFee.CalculateInsuranceFee();
            this.Order.OrderItems.Add(new OrderItem("Insurance", Order.Route.RouteMeters / 1000, insuranceFee.InsuranceRate.FeeAmount));

            Order.Route.SplitRouteByState();

            Order.Route.SplitRouteLegsByBreak();

            Order.Route.RouteLegs.ForEach(l => l.UpdateStateRates());
            
            Order.Route.SplitRouteLegsByDay();

            Order.Route.SplitRouteLegsByShift();

            Order.Route.RouteLegs.ForEach(l => l.UpdateShiftRate());

            Order.Route.RouteLegs.ForEach(l => l.UpdateLegSeconds());

            foreach (GeoRouteLeg leg in Order.Route.RouteLegs)
            {
                decimal itemQty = Convert.ToDecimal(((leg.LegSeconds / 360) * this.Order.TrucksRequired)) / 10;
                itemQty = Math.Round(itemQty, 2);

                string itemName;
                itemName = string.Format("Driver Labour: {0} {1} {2} \nStart: {3}\nFinish: {4}", leg.State, leg.DayType, leg.DayShift ? "DAY" : "NIGHT", leg.LegStartTime, leg.LegFinishTime);

                this.Order.OrderItems.Add(new OrderItem(itemName, itemQty, leg.LegRate));
            }

            // Once the order has been processed, signal the Rebate Processor thread to continue
            autoEventHandles[0].Set();
        }
    }
}