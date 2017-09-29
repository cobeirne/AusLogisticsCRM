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

    public class Suburb
    {
        public string Name;
        public string State;
        public string PostCode;
    }

    public class AusPostCodes
    {
        public string CsvFilePath;
        public string[] CsvLines;
        public List<Suburb> Suburbs;

        public AusPostCodes() : this(HttpContext.Current.Server.MapPath("~/App_Code/" + Properties.Settings.Default.PostCodeFileName))
        { }

        public AusPostCodes(string csvFilePath)
        {
            this.CsvFilePath = csvFilePath;
            _ReadAllCsvLines();
            _SelectAllSuburbs();
        }

        private void _ReadAllCsvLines()
        {
            try
            {
                this.CsvLines = System.IO.File.ReadAllLines(this.CsvFilePath);
            }
            catch(Exception e)
            {
                throw new Exception("Read All CSV Lines Exception", e.InnerException);
            }
        }

        private void _SelectAllSuburbs()
        {
            try
            {
                IEnumerable<Suburb> query =
                     from csvLine in this.CsvLines
                     let splitLine = csvLine.Split(',')
                     select new Suburb()
                     {
                         Name = splitLine[1].Substring(1, splitLine[1].Length - 2),
                         State = splitLine[2].Substring(1, splitLine[2].Length - 2),
                         PostCode = splitLine[0].Substring(1, splitLine[0].Length - 2)
                     };

                this.Suburbs = query.Skip(1).ToList();
            }
            catch (Exception e)
            {
                throw new Exception("Select All Suburbs Exception", e.InnerException);
            }
        }

        public bool ValidatePostCode(string postCode, string suburb)
        {
            int codeCount = this.Suburbs.Count(
                s => s.PostCode == postCode &&
                s.Name.ToUpper() == suburb.ToUpper());

            bool postCodeIsValid = codeCount > 0 ? true : false;

            return postCodeIsValid;
        }

        public bool ValidateSuburb(string suburbName, string state)
        {
            int suburbCount = this.Suburbs.Count(
                s => s.Name.ToUpper() == suburbName.ToUpper() && 
                s.State.ToUpper() == state.ToUpper());

            bool suburbIsValid = suburbCount > 0 ? true : false;

            return suburbIsValid;
        }
    }
}