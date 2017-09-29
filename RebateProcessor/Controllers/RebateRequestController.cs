using AusLogisticsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using RebateProcessor.Models;
using RebateProcessor.Controllers;


namespace RebateProcessor.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class RebateRequestController
    {
        public RebateRequestMessage RebateRequest { get; set; }

        public RebateReplyMessage RebateReply { get; set; }

        public void ProcessRebate(object message)
        {
            // Parse the request into a Rebate Request object
            _ParseRequestMessage(message);

            _ConsoleWriteRequest();
                        
            this.RebateReply = new RebateReplyMessage(RebateRequest.MessageId);

            // Use mutex to give this thread exclusive access to the database
            Program.DbAccessMutex.WaitOne();

            _CalculateMemberDiscount();

            _CalculateMemberRebate();

            _SaveBooking();

            _SetBookingInvoiceNumber();

            // Allow other threads to access the database
            Program.DbAccessMutex.ReleaseMutex();

            _SendReplyMessage();

            _ConsoleWriteReply();
        }

        private void _ParseRequestMessage(object message)
        {
            XmlDocument xmlMessage = new XmlDocument();
            xmlMessage.InnerXml = (string)message;
            RebateRequestMessage requestMessage = new RebateRequestMessage(xmlMessage);

            this.RebateRequest = requestMessage;
        }

        private void _CalculateMemberDiscount()
        {
            OrderDiscount orderDiscount = new OrderDiscount(this.RebateRequest.OrderSubTotal, RebateRequest.MemberClassId);
            this.RebateReply.DiscountRate = orderDiscount.DiscountRate;
        }


        private void _CalculateMemberRebate()
        {
            MemberAccountController accountController = new MemberAccountController();
            
            if(!accountController.CheckMemberExists(this.RebateRequest.MemberId))
            {
                accountController.AddMember(this.RebateRequest.MemberId);
            }

            // Get current member balance
            this.RebateReply.RebateBalance = accountController.SelectMemberBalance(this.RebateRequest.MemberId);

            // Credit the current balance i.e. rebate from previous booking
            this.RebateReply.RebateCredit = this.RebateReply.RebateBalance;

            // Adjust the balance by the amount credited
            this.RebateReply.RebateBalance -= this.RebateReply.RebateCredit;

            // Calculate the rebate for the current order
            OrderRebate rebate = new OrderRebate(RebateRequest.MemberClassId);

            // Adjust the balance by the current book rebate i.e. to be credit on next booking
            this.RebateReply.RebateBalance += rebate.RebateAmount;

            accountController.UpdateMemberBalance(this.RebateRequest.MemberId, this.RebateReply.RebateBalance);
        }

        private void _SaveBooking()
        {
            BookingController bookingController = new BookingController();

            bookingController.AddBooking(this.RebateRequest, this.RebateReply);
        }

        private void _SetBookingInvoiceNumber()
        {
            BookingController bookingController = new BookingController();

            this.RebateReply.InvoiceNumber = bookingController.SelectBookingId(this.RebateRequest.MessageId);
        }

        private void _SendReplyMessage()
        {
            this.RebateReply.BuildXmlDoc();

            Program.Gateway.SendMessage(this.RebateReply.ToString());
        }

        private void _ConsoleWriteRequest()
        {
            Console.WriteLine();
            Console.WriteLine(">> Rebate Request Received  <<");
            Console.WriteLine("Message ID: {0}", this.RebateRequest.MessageId);
            Console.WriteLine("Member ID: {0}", this.RebateRequest.MemberId);
            Console.WriteLine("Member Class ID: {0}", this.RebateRequest.MemberClassId);
            Console.WriteLine("Order Sub Total: {0:C}", this.RebateRequest.OrderSubTotal);
        }

        private void _ConsoleWriteReply()
        {
            Console.WriteLine();
            Console.WriteLine(">> Rebate Reply Sent  <<");
            Console.WriteLine("Message ID: {0}", this.RebateReply.MessageId);
            Console.WriteLine("Correllation ID: {0}", this.RebateReply.CorrellationId);
            Console.WriteLine("Invoice Number: {0}", this.RebateReply.InvoiceNumber);
            Console.WriteLine("Discount Rate: {0}%", this.RebateReply.DiscountRate * 100.0M);
            Console.WriteLine("Rebate Credit: {0:C}", this.RebateReply.RebateCredit);
            Console.WriteLine("Rebate Balance: {0:C}", this.RebateReply.RebateBalance);
        }
    }
}
