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

    public class OrderDiscount
    {
        public decimal OrderAmount { get; set; }

        public int MemberClass { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal DiscountRate { get; set; }

        public OrderDiscount(decimal orderAmount, int memberClass)
        {
            this.OrderAmount = orderAmount;
            this.MemberClass = memberClass;

            _CalculateDiscount();
        }

        private void _CalculateDiscount()
        {
            this.DiscountRate = 0.0M;
            this.DiscountAmount = 0.0M;

            switch (this.MemberClass)
            {
                case 1: // Gold
                    if (this.OrderAmount >= Properties.Settings.Default.GoldDiscountOrderMin)
                    {
                        this.DiscountRate = Properties.Settings.Default.GoldDiscount;
                        this.DiscountAmount = OrderAmount * Properties.Settings.Default.GoldDiscount;
                    }
                    break;

                case 2: // Silver
                    if (this.OrderAmount >= Properties.Settings.Default.SilverDiscountOrderMin)
                    {
                        this.DiscountRate = Properties.Settings.Default.SilverDiscount;
                        this.DiscountAmount = OrderAmount * Properties.Settings.Default.SilverDiscount;
                    }
                    break;

                case 3: // Loyalty
                    if (this.OrderAmount >= Properties.Settings.Default.LoyaltyDiscountOrderMin)
                    {
                        this.DiscountRate = Properties.Settings.Default.LoyaltyDiscount;
                        this.DiscountAmount = OrderAmount * Properties.Settings.Default.LoyaltyDiscount;
                    }
                    break;

                case 4: // Regular
                    if (this.OrderAmount >= Properties.Settings.Default.RegularDiscountOrderMin)
                    {
                        this.DiscountRate = Properties.Settings.Default.RegularDiscount;
                        this.DiscountAmount = OrderAmount * Properties.Settings.Default.RegularDiscount;
                    }
                    break;

                default:
                    this.DiscountRate = 0.0M;
                    this.DiscountAmount = 0.0M;
                    break;
            }
        }
    }
}
