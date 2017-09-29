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

    public class FixedFee
    {
        public string FeeName { get; set; }

        public decimal FeeAmount { get; set; }

        public string CsvFilePath { get; set; }

        public FixedFee(string feeName) :
            this(Default.CurrentHttpContext.Server.MapPath("~/App_Code/" + Properties.Settings.Default.FixedFeesFileName), feeName)
        { }

        public FixedFee(string csvFilePath, string feeName)
        {
            this.CsvFilePath = csvFilePath;
            this.FeeName = feeName;
            GetFixedFee();
        }

        public void GetFixedFee()
        {
            try
            {
                string[] csvLines = System.IO.File.ReadAllLines(this.CsvFilePath);

                IEnumerable<KeyValuePair<string,string>> query =   from csvLine in csvLines.Skip(1)
                                                    let splitLine = csvLine.Split(',')
                                                    select new KeyValuePair<string,string>(splitLine[0], splitLine[1]);

                string feeValue = query.First((f => f.Key == this.FeeName)).Value;
                this.FeeAmount = Convert.ToDecimal(feeValue);
            }
            catch (Exception e)
            {
                throw new Exception("Update Fixed Fee Exception", e.InnerException);
            }
        }

    }
}