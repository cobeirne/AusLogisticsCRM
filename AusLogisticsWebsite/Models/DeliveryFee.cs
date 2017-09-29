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

    public class DeliveryFee
    {
        public decimal DeliveryWeight { get; set; }

        public int TrucksRequired { get; set; }

        public decimal TruckMaxLoad { get; set; }

        public GeoRoute DeliveryRoute { get; set; }

        public decimal InterstateFee { get; set; }

        public string CsvFilePath { get; set; }

        public string[] CsvLines { get; set; }

        public DeliveryClass DeliveryClass { get; set; }

        public DeliveryFee()
        { }

        public DeliveryFee(decimal deliveryWeight, decimal truckMaxLoad, GeoRoute deliveryRoute) :
            this(Default.CurrentHttpContext.Server.MapPath("~/App_Code/" + Properties.Settings.Default.DeliveryClassFileName), deliveryWeight, truckMaxLoad, deliveryRoute)
        { }

        public DeliveryFee (string csvFilePath, decimal deliveryWeight, decimal truckMaxLoad, GeoRoute deliveryRoute)
        {
            this.CsvFilePath = csvFilePath;

            this.DeliveryWeight = deliveryWeight;
            this.TruckMaxLoad = truckMaxLoad;
            this.DeliveryRoute = deliveryRoute;
            this.InterstateFee = 0M;
        }

        public void CalculateTrucksRequired()
        {
            decimal weightTruckRatio = Math.Ceiling(this.DeliveryWeight / this.TruckMaxLoad);
            this.TrucksRequired = Convert.ToInt32(weightTruckRatio);
            this.TrucksRequired = this.TrucksRequired;
        }

        public void CalculateInterstateDeliveryFee()
        {
            if(this.DeliveryRoute.RouteStates.Count > 1)
            {
                UpdateClassification();
                this.InterstateFee = this.DeliveryClass.Fee;
            }
            else
            {
                this.InterstateFee = 0M;
            }
        }

        public void UpdateClassification()
        {
            try
            {
                this.CsvLines = System.IO.File.ReadAllLines(this.CsvFilePath);

                IEnumerable<DeliveryClass> query =
                     (from csvLine in this.CsvLines.Skip(1)
                      let splitLine = csvLine.Split(',')
                      select new DeliveryClass(splitLine[0], splitLine[1], splitLine[2]));

                this.DeliveryClass = query.First((c => c.MinNumberOfTrucks <= this.TrucksRequired && c.MaxNumberOfTrucks >= this.TrucksRequired));
            }
            catch (Exception e)
            {
                throw new Exception("Select All Classifications Exception", e.InnerException);
            }
        } 
    }
}