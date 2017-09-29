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

    public class MemberModel
    {
        public int MembershipId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int MemberClassId { get; set; }

        public string MemberClassName { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime DateOfBirth { get; set; }

        public decimal Salary { get; set; }
        
        public char Gender { get; set; }

        public string PhoneNumber { get; set; }

        public string Unit { get; set; }

        public string Number { get; set; }

        public string Address { get; set; }

        public string Suburb { get; set; }

        public string State { get; set; }

        public string PostCode { get; set; }
    }
}