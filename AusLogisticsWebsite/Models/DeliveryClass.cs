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

    public class DeliveryClass
    {
        public string DeliveryClassification { get; set; }

        public int MinNumberOfTrucks { get; set; }

        public int MaxNumberOfTrucks { get; set; }

        public decimal Fee { get; set; }

        public DeliveryClass()
        { }

        public DeliveryClass(string classification, string truckRange, string fee)
        {
            this.DeliveryClassification = classification;

            if (truckRange.Contains("-"))
            {
                string[] rangeSplit = truckRange.Split('-');
                this.MinNumberOfTrucks = Convert.ToInt32(rangeSplit[0].Trim());
                this.MaxNumberOfTrucks = Convert.ToInt32(rangeSplit[1].Trim());
            }
            else
            {
                this.MinNumberOfTrucks = Convert.ToInt32(truckRange.Trim());
                this.MaxNumberOfTrucks = Convert.ToInt32(truckRange.Trim());
            }
            this.Fee = Convert.ToDecimal(fee);
        }
    }
}