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

    public class StateShiftRates
    {
        public string StateCode { get; set; }

        public string ShiftType { get; set; }

        public bool DayShift { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public decimal Rate { get; set; }

        public StateShiftRates(string stateCode, string shiftType, string startTime, string finishTime, string rate)
        {
            this.StateCode = stateCode;
            this.ShiftType = shiftType;

            string[] startSplit = startTime.Split(':');
            this.StartTime = new DateTime(2016, 1, 1, Convert.ToInt32(startSplit[0]), Convert.ToInt32(startSplit[1]),0);

            string[] endSplit = finishTime.Split(':');
            this.FinishTime = new DateTime(2016, 1, 1, Convert.ToInt32(endSplit[0]), Convert.ToInt32(endSplit[1]), 0);

            this.DayShift = this.StartTime < this.FinishTime ? true : false;

            this.Rate = Convert.ToDecimal(rate);
        }
    }
}