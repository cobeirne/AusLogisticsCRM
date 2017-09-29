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

    public class InsuranceFee
    {
        public FixedFee InsuranceRate { get; set; }

        public decimal Fee { get; set; }

        public int RouteMeters { get; set; }
      
        public InsuranceFee(int routeMeters)
        {
            this.InsuranceRate = new FixedFee("InsurancePerKm");
            this.RouteMeters = routeMeters;
            CalculateInsuranceFee();
        }

        public void CalculateInsuranceFee()
        {
            this.Fee = this.InsuranceRate.FeeAmount * (this.RouteMeters / 1000);
        }
    }
}