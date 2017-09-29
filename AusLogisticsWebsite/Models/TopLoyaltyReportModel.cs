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

    public static class TopLoyaltyReportModel
    {
        public static List<MemberModel> GetTopLoyaltyMemebers(List<MemberModel> members, decimal topPercentile)
        {
            List<MemberModel> reportMemebers = new List<MemberModel>();
            
            if (members != null)
            {
                decimal totalMembers = Convert.ToDecimal(members.Count);

                int totalPercentileMembers = Convert.ToInt32(totalMembers * (topPercentile / 100.0M));

                if (totalPercentileMembers > 0)
                {
                    List<MemberModel> percentileMembers = members.OrderBy(m => m.DateJoined).Take(totalPercentileMembers).ToList();

                    reportMemebers = percentileMembers.OrderBy(m => m.DateJoined).ToList();
                }
            }

            return reportMemebers;
        }
    }
}