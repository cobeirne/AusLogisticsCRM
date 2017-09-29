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

    public static class SilverReportModel
    {
        public static decimal GetLowestSilverSalary(List<MemberModel> members, decimal silverPercentile)
        {
            decimal lowestSilverSalary = 0.0M;

            if (members != null)
            {
                decimal totalMembers = Convert.ToDecimal(members.Count);

                int totalSilverMembers = Convert.ToInt32(totalMembers * (silverPercentile / 100.0M));

                if (totalSilverMembers > 0)
                {
                    List<MemberModel> silverMembers = members.OrderByDescending(m => m.Salary).Take(totalSilverMembers).ToList();

                    lowestSilverSalary = silverMembers.OrderBy(m => m.Salary).First().Salary;
                }
            }

            return lowestSilverSalary;
        }
    }
}