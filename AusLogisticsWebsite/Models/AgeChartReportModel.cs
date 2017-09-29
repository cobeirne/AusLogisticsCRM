using AusLogisticsWebsite.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Collections;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public static class AgeChartReportModel
    {
        public static List<DataPoint> GetAgeHistogramPoints(List<MemberModel> members)
        {
            List<DataPoint> points = new List<DataPoint>();

            List<AgePoint> ages = new List<AgePoint>();

            members.ForEach(m => ages.Add(new AgePoint{ Age =  (DateTime.Now.Date.Year - m.DateOfBirth.Year) }));

            IEnumerable<DataPoint> query =  from a in ages
                                            group a by a.AgeRange into grp
                                            select new DataPoint(grp.First().AgeRange, grp.Count());

            points = query.OrderBy(s => s.XValue).ToList();

            return points;
        }
    }

    public class AgePoint
    {
        public decimal Age { get; set; }

        public double AgeRange
        {
            get
            {
                decimal roundedAge = Math.Round((this.Age / 5.0M), 0);
                return Convert.ToDouble(Convert.ToDouble(roundedAge) * 5.0); ;
            }
        }
    }
}