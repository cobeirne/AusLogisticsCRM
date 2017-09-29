using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebateProcessor.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class OrderRebate
    {
        public int MemberClass { get; set; }

        public decimal RebateAmount { get; set; }

        public OrderRebate(int memberClass)
        {
            this.MemberClass = memberClass;

            _CalculateRebate();
        }

        private void _CalculateRebate()
        {
            this.RebateAmount = 0.0M;

            switch (this.MemberClass)
            {
                case 1: // Gold
                    this.RebateAmount = Properties.Settings.Default.GoldRebate;
                    break;

                case 2: // Silver
                    this.RebateAmount = Properties.Settings.Default.SilverRebate;
                    break;

                case 3: // Loyalty
                    this.RebateAmount = Properties.Settings.Default.LoyaltyRebate;
                    break;

                case 4: // Regular
                    this.RebateAmount = Properties.Settings.Default.RegularRebate;
                    break;

                default:
                    this.RebateAmount = 0.0M;
                    break;
            }
        }
    }
}
