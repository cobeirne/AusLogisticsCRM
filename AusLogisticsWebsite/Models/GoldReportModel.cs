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

    public static class GoldReportModel
    {
        public static decimal GetLowestGoldSalary(List<MemberModel> members, decimal goldPercentile)
        {
            decimal lowestGoldSalary = 0.0M;

            if (members != null)
            {
                decimal totalMembers = Convert.ToDecimal(members.Count);

                int totalGoldMembers = Convert.ToInt32(totalMembers * (goldPercentile / 100.0M));

                if (totalGoldMembers > 0)
                {
                    List<MemberModel> goldMembers = members.OrderByDescending(m => m.Salary).Take(totalGoldMembers).ToList();

                    lowestGoldSalary = goldMembers.OrderBy(m => m.Salary).First().Salary;
                }
            }

            return lowestGoldSalary;
        }
    }
}