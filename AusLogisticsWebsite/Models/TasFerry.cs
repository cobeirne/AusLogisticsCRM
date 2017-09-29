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

    public class TasFerry
    {
        public FixedFee FerryFee { get; set; }

        public decimal Fee { get; set; }

        public List<string> RouteStates { get; set; }

        public TasFerry(List<string> routeStates)
        {
            this.FerryFee = new FixedFee("TasFerry");
            this.RouteStates = routeStates;
        }

        public TasFerry(string csvFileName, List<string> routeStates)
        {
            this.FerryFee = new FixedFee(csvFileName, "TasFerry");
            this.RouteStates = routeStates;
        }
        
        public void CalculateFerryFee()
        {
            if (this.RouteStates.Contains("TAS"))
            {
                this.Fee = this.FerryFee.FeeAmount;
            }
            else
            {
                this.Fee = 0.0M;
            }
        }
    }
}