using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public class SalaryModel
    {
        public int EmployeeNo { get; set; }

        public decimal Salary { get; set; }

        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public SalaryModel()
        { }

        public SalaryModel(string empNo, string salary, string fromDate, string toDate)
        {
            char[] leadingChars = { '"', '\\', '"' };
            char[] trailingChars = { '\\', '"', '"' };

            this.EmployeeNo = Convert.ToInt32((empNo.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.Salary = Convert.ToDecimal((salary.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.FromDate = Convert.ToDateTime((fromDate.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.ToDate = Convert.ToDateTime((toDate.TrimStart(leadingChars)).TrimEnd(trailingChars));
        }
    }
}