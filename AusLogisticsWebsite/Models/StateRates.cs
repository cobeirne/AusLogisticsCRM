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

    public class StateRates
    {
        public List<StateDayRates> DayRates { get; set; }

        public string StateCode { get; set; }

        public string CsvFileName { get; set; }

        public HttpContext httpContext { get; set; }

        public StateRates(string stateCode) : this(Properties.Settings.Default.HourlyRatesFileName, stateCode)
        { }

        public StateRates(string csvFileName, string stateCode)
        {
            this.CsvFileName = Default.CurrentHttpContext.Server.MapPath("~/App_Code/" + csvFileName);
            this.StateCode = stateCode;
            GetStateRates();
        }

        public void GetStateRates()
        {
            try
            {
                string[] csvLines = System.IO.File.ReadAllLines(this.CsvFileName);

                IEnumerable<StateDayRates> query = from csvLine in csvLines.Skip(1)
                                                    let splitLine = csvLine.Split(',')
                                                    select new StateDayRates(splitLine[0], splitLine[1],
                                                    splitLine[2], splitLine[3], splitLine[4], splitLine[5],
                                                    splitLine[6], splitLine[7], splitLine[8], splitLine[9]);

                this.DayRates = query.Where(s => s.StateCode == this.StateCode).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Get State Rates Exception", e.InnerException);
            }
        }

    }
}