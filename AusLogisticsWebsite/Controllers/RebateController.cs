using AusLogisticsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using AusLogisticsLibrary.Controllers;
using AusLogisticsLibrary.Models;
using System.Xml;

namespace AusLogisticsWebsite.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class RebateController
    {
        public TransportOrder Order { get; set; }

        public MqGatewayController Gateway { get; set; }

        public RebateRequestMessage RequestMessage { get; set; }

        private object _EventHandles;

        public RebateController()
        {
            this.Gateway = new MqGatewayController("Web",
                Properties.Settings.Default.BrokerUrl,
                Properties.Settings.Default.ProducerQueue, 
                Properties.Settings.Default.SubscriberQueue);
        }

        public void ProcessRebate(object eventHandles)
        {
            _EventHandles = eventHandles;
            var autoEventHandles = (AutoResetEvent[])this._EventHandles;

            // Block the current thread until the Order Controller signals that the Order has been pre-processed.
            autoEventHandles[0].WaitOne();

            // Create a new rebate request message to be sent in XML format to the Rebate Processor via ActiveMQ.
            this.RequestMessage = new RebateRequestMessage();

            this.Order.CalculateOrderSubtotal();

            this.RequestMessage.InvoiceNumber = this.Order.InvoiceNumber;
            this.RequestMessage.MemberId = this.Order.OrderMember.MembershipId;
            this.RequestMessage.MemberClassId = this.Order.OrderMember.MemberClassId;
            this.RequestMessage.OriginNumber = this.Order.Route.Origin.Number;
            this.RequestMessage.OriginAddress = this.Order.Route.Origin.Address;
            this.RequestMessage.OriginSuburb = this.Order.Route.Origin.Suburb;
            this.RequestMessage.OriginPostCode = this.Order.Route.Origin.PostCode;
            this.RequestMessage.OriginState = this.Order.Route.Origin.State;
            this.RequestMessage.DestinationNumber = this.Order.Route.Destination.Number;
            this.RequestMessage.DestinationAddress = this.Order.Route.Destination.Address;
            this.RequestMessage.DestinationSuburb = this.Order.Route.Destination.Suburb;
            this.RequestMessage.DestinationPostCode = this.Order.Route.Destination.PostCode;
            this.RequestMessage.DestinationState = this.Order.Route.Destination.State;
            this.RequestMessage.TrucksRequired = this.Order.TrucksRequired;
            this.RequestMessage.DeliveryDateTime = this.Order.Route.DeliveryDateTime;
            this.RequestMessage.DeliveryWeight = this.Order.DeliveryWeight;
            this.RequestMessage.OrderSubTotal = this.Order.OrderSubTotal;
            this.RequestMessage.OrderGstRate = this.Order.GstRate;

            this.RequestMessage.BuildXmlDoc();

            // Set the MqGateway callback delegate
            this.Gateway.OnMessageReceived += Subscribe_OnRebateReceived;

            // Send the XML message to the Rebate Processor
            this.Gateway.SendMessage(this.RequestMessage.ToString());

            // Block the current thread until signalled by the 'Subscribe_OnRebateReceived' callback that the corellating reply message has been received.
            autoEventHandles[1].WaitOne();            
        }


        private void Subscribe_OnRebateReceived(string message)
        {
            // On receipt of a ActiveMq message, parse to a reply message object
            XmlDocument messageXml = new XmlDocument { InnerXml = message };
            RebateReplyMessage replyMessage = new RebateReplyMessage(messageXml);

            // Only process messages that have a correlation id that matches the original request message
            if (this.RequestMessage.MessageId == replyMessage.CorrellationId)
            {
                // Write debug diagnostics
                Debug.WriteLine(">> Rebate Reply Received <<");
                Debug.WriteLine("Rebate Reply Received, Message ID: " + replyMessage.MessageId);
                Debug.WriteLine("Matching Correllation ID: " + replyMessage.CorrellationId);
                Debug.WriteLine("Invoice: {0}", replyMessage.InvoiceNumber);
                Debug.WriteLine("Rebate Discount: {0}", replyMessage.DiscountRate);
                Debug.WriteLine("Rebate Credit: {0}", replyMessage.RebateCredit);
                Debug.WriteLine("Rebate Balance: {0}", replyMessage.RebateBalance);

                Gateway.Dispose();

                // Update the order with info provided by the Rebate Processor
                this.Order.InvoiceNumber = replyMessage.InvoiceNumber;
                this.Order.DiscountApplied = this.Order.OrderSubTotal * replyMessage.DiscountRate;
                this.Order.RebateCredit = replyMessage.RebateCredit;
                this.Order.RebateBalance = replyMessage.RebateBalance;

                // Signal the current thread and the Invoice thread to continue
                var autoEventHandles = (AutoResetEvent[])this._EventHandles;
                autoEventHandles[1].Set();
                autoEventHandles[2].Set();
            }
        }        
    }
}