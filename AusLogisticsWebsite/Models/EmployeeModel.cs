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

    public class EmployeeModel
    {
        public int EmployeeNo { get; set; }

        public DateTime BirthDate { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public char Gender { get; set; }

        public DateTime HireDate { get; set; }

        public EmployeeModel()
        { }

        public EmployeeModel(string emp_no, string birthDate, string firstName, string lastName, string gender, string hireDate)
        {
            char[] leadingChars = { '"', '\\', '"'};
            char[] trailingChars = { '\\', '"', '"' };

            this.EmployeeNo = Convert.ToInt32((emp_no.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.BirthDate = Convert.ToDateTime((birthDate.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.FirstName = (firstName.TrimStart(leadingChars)).TrimEnd(trailingChars);
            this.LastName = (lastName.TrimStart(leadingChars)).TrimEnd(trailingChars);
            this.Gender = Convert.ToChar((gender.TrimStart(leadingChars)).TrimEnd(trailingChars));
            this.HireDate = Convert.ToDateTime((hireDate.TrimStart(leadingChars)).TrimEnd(trailingChars));
        }
    }
}