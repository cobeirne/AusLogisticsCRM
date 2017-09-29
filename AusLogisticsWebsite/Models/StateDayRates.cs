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

    public class StateDayRates
    {
        public string StateCode { get; set; }

        public List<StateShiftRates> StateShifts { get; set; }

        public StateDayRates(string location, string weekdayStartTime, string weekdayEndTime, 
            string weekdayRate, string weekendStartTime, string weekendEndTime, 
            string weekendRate, string publicHolidayStartTime, string publicHolidayEndTime,
            string publicHolidayRate)
        {
            this.StateCode = location;
            this.StateShifts = new List<StateShiftRates>();

            StateShifts.Add(new StateShiftRates(location, "WEEKDAY", weekdayStartTime, weekdayEndTime, weekdayRate));
            StateShifts.Add(new StateShiftRates(location, "WEEKEND", weekendStartTime, weekendEndTime, weekendRate));
            StateShifts.Add(new StateShiftRates(location, "HOLIDAY", publicHolidayStartTime, publicHolidayEndTime, publicHolidayRate));
        }
    }
}