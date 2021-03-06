﻿using System;
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

    public static class LoyaltyReportModel
    {
        public static DateTime GetLatestLoyaltyDate(int loyaltyYears)
        {
            DateTime latestLoyaltyDate = DateTime.Now.AddYears(-loyaltyYears);

            return latestLoyaltyDate;
        }
    }
}