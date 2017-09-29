using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class TransportOrder
    {
        public int InvoiceNumber { get; set; }

        public MemberModel OrderMember { get; set; }

        public GeoRoute Route { get; set; }

        public decimal DeliveryWeight { get; set; }

        public int TrucksRequired { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public decimal OrderSubTotal { get; set; }

        public decimal GstRate { get; set; }

        public decimal OrderGst { get; set; }
                      
        public decimal DiscountApplied { get; set; }

        public decimal RebateCredit { get; set; }

        public decimal RebateBalance { get; set; }

        public decimal OrderTotal { get; set; }

        public TransportOrder()
        {
            this.OrderItems = new List<OrderItem>();
            this.DiscountApplied = 0.0M;
            this.RebateCredit = 0.0M;
            this.GstRate = Properties.Settings.Default.GstRate;
        }

        public void CalculateOrderSubtotal()
        {
            this.OrderSubTotal = this.OrderItems.Sum(i => i.Total);
        }

        public void CalculateOrderGst()
        {
            this.OrderGst = (this.OrderSubTotal - this.DiscountApplied - this.RebateCredit ) * this.GstRate;
        }

        public void CalculateOrderTotal()
        {
            this.OrderTotal = (this.OrderSubTotal - this.DiscountApplied - this.RebateCredit) + this.OrderGst;
        }
    }

    public class OrderItem
    {
        public string ItemName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Rate { get; set; }

        public decimal Total
        {
            get { return this.Quantity * this.Rate; }
        }

        public OrderItem(string itemName, decimal quantity, decimal rate)
        {
            this.ItemName = itemName;
            this.Quantity = quantity;
            this.Rate = rate;
        }
    }
}