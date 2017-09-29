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

    public static class SalaryChartReportModel
    {
        public static List<DataPoint> GetSalaryHistogramPoints(List<MemberModel> members)
        {
            List<DataPoint> points = new List<DataPoint>();

            List<SalaryPoint> salaries = new List<SalaryPoint>();

            members.ForEach(m => salaries.Add(new SalaryPoint{ Salary = m.Salary }));

            IEnumerable<DataPoint> query =  from s in salaries
                                            group s by s.SalaryRange into grp
                                            select new DataPoint(grp.First().SalaryRange, grp.Count());

            points = query.OrderBy(s => s.XValue).ToList();

            return points;
        }

     
    }

    public class SalaryPoint
    {
        public decimal Salary { get; set; }

        public double SalaryRange
        {
            get
            {
                decimal roundedSalary = Math.Round((this.Salary / 10000.0M), 0);
                return Convert.ToDouble(Convert.ToDouble(roundedSalary) * 10000.0); ;
            }
        }
    }
}