using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace AusLogisticsLibrary.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class RebateRequestMessage
    {
        public string MessageId { get; set; }

        public int InvoiceNumber { get; set; }

        public int MemberId { get; set; }

        public int MemberClassId { get; set; }

        public string OriginNumber { get; set; }

        public string OriginAddress { get; set; }

        public string OriginSuburb { get; set; }
        
        public string OriginState { get; set; }

        public string OriginPostCode { get; set; }

        public string DestinationNumber { get; set; }

        public string DestinationAddress { get; set; }

        public string DestinationSuburb { get; set; }

        public string DestinationState { get; set; }

        public string DestinationPostCode { get; set; }

        public DateTime DeliveryDateTime { get; set; }

        public decimal DeliveryWeight { get; set; }

        public int TrucksRequired { get; set; }

        public decimal OrderSubTotal { get; set; }

        public decimal OrderGstRate { get; set; }


        public RebateRequestMessage()
        {
            this.MessageId = Guid.NewGuid().ToString();
        }

        public RebateRequestMessage(XmlDocument xmlOrder)
        {
            _ParseXmlMessage(xmlOrder);
        }

        public XmlDocument MessageXmlDocument { get; set; }

        public void BuildXmlDoc()
        {
            // Build and XML document using Rebate info
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0","UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement rootElement = xmlDoc.CreateElement(string.Empty, "root", string.Empty);
            xmlDoc.AppendChild(rootElement);

            XmlElement headerElement = xmlDoc.CreateElement(string.Empty, "header", string.Empty);
            rootElement.AppendChild(headerElement);

            XmlElement messageIdElement = xmlDoc.CreateElement(string.Empty, "message-id", string.Empty);
            XmlText messageIdText = xmlDoc.CreateTextNode(this.MessageId);
            messageIdElement.AppendChild(messageIdText);
            headerElement.AppendChild(messageIdElement);

            XmlElement typeElement = xmlDoc.CreateElement(string.Empty, "message-type", string.Empty);
            XmlText typeText = xmlDoc.CreateTextNode("rebate-request");
            typeElement.AppendChild(typeText);
            headerElement.AppendChild(typeElement);
            
            XmlElement bodyElement = xmlDoc.CreateElement(string.Empty, "body", string.Empty);
            rootElement.AppendChild(bodyElement);

            XmlElement invoiceIdElement = xmlDoc.CreateElement(string.Empty, "invoice-number", string.Empty);
            XmlText invoiceIdText = xmlDoc.CreateTextNode(this.InvoiceNumber.ToString());
            invoiceIdElement.AppendChild(invoiceIdText);
            bodyElement.AppendChild(invoiceIdElement);

            XmlElement memberIdElement = xmlDoc.CreateElement(string.Empty, "member-id", string.Empty);
            XmlText memberIdText = xmlDoc.CreateTextNode(this.MemberId.ToString());
            memberIdElement.AppendChild(memberIdText);
            bodyElement.AppendChild(memberIdElement);

            XmlElement memberClassIdElement = xmlDoc.CreateElement(string.Empty, "member-class-id", string.Empty);
            XmlText memberClassIdText = xmlDoc.CreateTextNode(this.MemberClassId.ToString());
            memberClassIdElement.AppendChild(memberClassIdText);
            bodyElement.AppendChild(memberClassIdElement);

            XmlElement originNumberElement = xmlDoc.CreateElement(string.Empty, "origin-number", string.Empty);
            XmlText originNumberText = xmlDoc.CreateTextNode(this.OriginNumber);
            originNumberElement.AppendChild(originNumberText);
            bodyElement.AppendChild(originNumberElement);

            XmlElement originAddressElement = xmlDoc.CreateElement(string.Empty, "origin-address", string.Empty);
            XmlText originAddressText = xmlDoc.CreateTextNode(this.OriginAddress);
            originAddressElement.AppendChild(originAddressText);
            bodyElement.AppendChild(originAddressElement);

            XmlElement originSuburbElement = xmlDoc.CreateElement(string.Empty, "origin-suburb", string.Empty);
            XmlText originSuburbText = xmlDoc.CreateTextNode(this.OriginSuburb);
            originSuburbElement.AppendChild(originSuburbText);
            bodyElement.AppendChild(originSuburbElement);

            XmlElement originStateElement = xmlDoc.CreateElement(string.Empty, "origin-state", string.Empty);
            XmlText originStateText = xmlDoc.CreateTextNode(this.OriginState);
            originStateElement.AppendChild(originStateText);
            bodyElement.AppendChild(originStateElement);

            XmlElement originPostCodeElement = xmlDoc.CreateElement(string.Empty, "origin-postcode", string.Empty);
            XmlText originPostCodeText = xmlDoc.CreateTextNode(this.OriginPostCode);
            originPostCodeElement.AppendChild(originPostCodeText);
            bodyElement.AppendChild(originPostCodeElement);
            
            XmlElement destinationNumberElement = xmlDoc.CreateElement(string.Empty, "destination-number", string.Empty);
            XmlText destinationNumberText = xmlDoc.CreateTextNode(this.DestinationNumber);
            destinationNumberElement.AppendChild(destinationNumberText);
            bodyElement.AppendChild(destinationNumberElement);

            XmlElement destinationAddressElement = xmlDoc.CreateElement(string.Empty, "destination-address", string.Empty);
            XmlText destinationAddressText = xmlDoc.CreateTextNode(this.DestinationAddress);
            destinationAddressElement.AppendChild(destinationAddressText);
            bodyElement.AppendChild(destinationAddressElement);

            XmlElement destinationSuburbElement = xmlDoc.CreateElement(string.Empty, "destination-suburb", string.Empty);
            XmlText destinationSuburbText = xmlDoc.CreateTextNode(this.DestinationSuburb);
            destinationSuburbElement.AppendChild(destinationSuburbText);
            bodyElement.AppendChild(destinationSuburbElement);

            XmlElement destinationStateElement = xmlDoc.CreateElement(string.Empty, "destination-state", string.Empty);
            XmlText destinationStateText = xmlDoc.CreateTextNode(this.DestinationState);
            destinationStateElement.AppendChild(destinationStateText);
            bodyElement.AppendChild(destinationStateElement);

            XmlElement destinationPostCodeElement = xmlDoc.CreateElement(string.Empty, "destination-postcode", string.Empty);
            XmlText destinationPostCodeText = xmlDoc.CreateTextNode(this.DestinationPostCode);
            destinationPostCodeElement.AppendChild(destinationPostCodeText);
            bodyElement.AppendChild(destinationPostCodeElement);

            XmlElement deliveryDateElement = xmlDoc.CreateElement(string.Empty, "delivery-date", string.Empty);
            XmlText deliveryDateText = xmlDoc.CreateTextNode(this.DeliveryDateTime.ToString());
            deliveryDateElement.AppendChild(deliveryDateText);
            bodyElement.AppendChild(deliveryDateElement);

            XmlElement weightElement = xmlDoc.CreateElement(string.Empty, "delivery-weight", string.Empty);
            XmlText weightText = xmlDoc.CreateTextNode(this.DeliveryWeight.ToString());
            weightElement.AppendChild(weightText);
            bodyElement.AppendChild(weightElement);

            XmlElement trucksElement = xmlDoc.CreateElement(string.Empty, "trucks-required", string.Empty);
            XmlText trucksText = xmlDoc.CreateTextNode(this.TrucksRequired.ToString());
            trucksElement.AppendChild(trucksText);
            bodyElement.AppendChild(trucksElement);

            XmlElement orderTotalElement = xmlDoc.CreateElement(string.Empty, "order-subtotal", string.Empty);
            XmlText orderTotalText = xmlDoc.CreateTextNode(this.OrderSubTotal.ToString());
            orderTotalElement.AppendChild(orderTotalText);
            bodyElement.AppendChild(orderTotalElement);

            XmlElement gstRateElement = xmlDoc.CreateElement(string.Empty, "gst-rate", string.Empty);
            XmlText gstRateText = xmlDoc.CreateTextNode(this.OrderGstRate.ToString());
            gstRateElement.AppendChild(gstRateText);
            bodyElement.AppendChild(gstRateElement);

            this.MessageXmlDocument = xmlDoc;
        }

        private void _ParseXmlMessage(XmlDocument xmlDoc)
        {
            // Use Xpath to parse data from the XML document
            try
            {
                this.MessageId = xmlDoc.SelectSingleNode("root/header/message-id").InnerText;
                this.InvoiceNumber = Convert.ToInt32(xmlDoc.SelectSingleNode("root/body/invoice-number").InnerText);
                this.MemberId = Convert.ToInt32(xmlDoc.SelectSingleNode("root/body/member-id").InnerText);
                this.MemberClassId = Convert.ToInt32(xmlDoc.SelectSingleNode("root/body/member-class-id").InnerText);

                this.OriginNumber = xmlDoc.SelectSingleNode("root/body/origin-number").InnerText;
                this.OriginAddress = xmlDoc.SelectSingleNode("root/body/origin-address").InnerText;
                this.OriginSuburb = xmlDoc.SelectSingleNode("root/body/origin-suburb").InnerText;
                this.OriginPostCode = xmlDoc.SelectSingleNode("root/body/origin-postcode").InnerText;
                this.OriginState = xmlDoc.SelectSingleNode("root/body/origin-state").InnerText;

                this.DestinationNumber = xmlDoc.SelectSingleNode("root/body/destination-number").InnerText;
                this.DestinationAddress = xmlDoc.SelectSingleNode("root/body/destination-address").InnerText;
                this.DestinationSuburb = xmlDoc.SelectSingleNode("root/body/destination-suburb").InnerText;
                this.DestinationPostCode = xmlDoc.SelectSingleNode("root/body/destination-postcode").InnerText;
                this.DestinationState = xmlDoc.SelectSingleNode("root/body/destination-state").InnerText;

                this.DeliveryDateTime = Convert.ToDateTime(xmlDoc.SelectSingleNode("root/body/delivery-date").InnerText);
                this.DeliveryWeight = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/delivery-weight").InnerText);
                this.TrucksRequired = Convert.ToInt32(xmlDoc.SelectSingleNode("root/body/trucks-required").InnerText);
                this.OrderSubTotal = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/order-subtotal").InnerText);
                this.OrderGstRate = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/gst-rate").InnerText);
            }
            catch (Exception e)
            {
                throw new Exception("Rebate request message format invalid.", e.InnerException);
            }
        }
        
        public override string ToString()
        {
            return this.MessageXmlDocument.InnerXml;
        }
    }
}