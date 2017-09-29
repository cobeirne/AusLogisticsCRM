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

    public class StateHoliday
    {
        public DateTime TransitDate { get; set; }

        public string State { get; set; }

        public PublicHoliday Holiday { get; set; }

        public bool PublicHoliday { get; set; }

        public string CsvFilePath { get; set; }

        public StateHoliday(DateTime transitDate) :
            this(Default.CurrentHttpContext.Server.MapPath("~/App_Code/" + Properties.Settings.Default.PublicHolidaysFileName), transitDate)
        { }

        public StateHoliday(string csvFilePath, DateTime transitDate)
        {
            this.CsvFilePath = csvFilePath;
            this.TransitDate = transitDate;
            GetPublicHoliday();
        }

        public void GetPublicHoliday()
        {
            this.PublicHoliday = false;

            try
            {
                string[] csvLines = System.IO.File.ReadAllLines(this.CsvFilePath);

                IEnumerable<PublicHoliday> query = from csvLine in csvLines.Skip(1)
                                                   let splitLine = csvLine.Split(',')
                                                   select new PublicHoliday(splitLine[0], splitLine[1],
                                                   splitLine[2], splitLine[3], splitLine[4], splitLine[5],
                                                   splitLine[6], splitLine[7], splitLine[8], splitLine[9]);

                this.Holiday = query.First((p => p.HolidayDate == this.TransitDate.Day && p.HolidayMonth == this.TransitDate.Month));

                this.PublicHoliday = true;
            }
            catch (Exception)
            {
            }
        }
    }
}