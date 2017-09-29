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

    public class RebateReplyMessage
    {
        public string MessageId { get; set; }

        public string CorrellationId { get; set; }

        public int InvoiceNumber { get; set; }

        public decimal DiscountRate { get; set; }

        public decimal RebateCredit { get; set; }

        public decimal RebateBalance { get; set; }
        
        public RebateReplyMessage(string correlationId)
        {
            this.MessageId = Guid.NewGuid().ToString();
            this.CorrellationId = correlationId;
        }

        public RebateReplyMessage(XmlDocument xmlOrder)
        {
            _ParseXmlMessage(xmlOrder);
        }

        public XmlDocument MessageXmlDocument { get; set; }

        public void BuildXmlDoc()
        {
            // Create an XML document using order info
            XmlDocument xmlDoc = new XmlDocument();

            XmlDeclaration xmlDeclaration = xmlDoc.CreateXmlDeclaration("1.0","UTF-8", null);
            XmlElement root = xmlDoc.DocumentElement;
            xmlDoc.InsertBefore(xmlDeclaration, root);

            XmlElement rootElement = xmlDoc.CreateElement(string.Empty, "root", string.Empty);
            xmlDoc.AppendChild(rootElement);

            XmlElement headerElement = xmlDoc.CreateElement(string.Empty, "header", string.Empty);
            rootElement.AppendChild(headerElement);

            XmlElement messageElement = xmlDoc.CreateElement(string.Empty, "message-id", string.Empty);
            XmlText messageIdText = xmlDoc.CreateTextNode(this.MessageId);
            messageElement.AppendChild(messageIdText);
            headerElement.AppendChild(messageElement);

            XmlElement correlationElement = xmlDoc.CreateElement(string.Empty, "correlation-id", string.Empty);
            XmlText correlationIdText = xmlDoc.CreateTextNode(this.CorrellationId);
            correlationElement.AppendChild(correlationIdText);
            headerElement.AppendChild(correlationElement);

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

            XmlElement discountRateElement = xmlDoc.CreateElement(string.Empty, "discount-rate", string.Empty);
            XmlText discountRateIdText = xmlDoc.CreateTextNode(this.DiscountRate.ToString());
            discountRateElement.AppendChild(discountRateIdText);
            bodyElement.AppendChild(discountRateElement);

            XmlElement rebateCreditElement = xmlDoc.CreateElement(string.Empty, "rebate-credit", string.Empty);
            XmlText rebateCreditText = xmlDoc.CreateTextNode(this.RebateCredit.ToString());
            rebateCreditElement.AppendChild(rebateCreditText);
            bodyElement.AppendChild(rebateCreditElement);

            XmlElement rebateBalanceElement = xmlDoc.CreateElement(string.Empty, "rebate-balance", string.Empty);
            XmlText rebateBalanceText = xmlDoc.CreateTextNode(this.RebateBalance.ToString());
            rebateBalanceElement.AppendChild(rebateBalanceText);
            bodyElement.AppendChild(rebateBalanceElement);

            this.MessageXmlDocument = xmlDoc;
        }

        private void _ParseXmlMessage(XmlDocument xmlDoc)
        {
            // Use Xpath to parse data from the XML document
            try
            {
                this.MessageId = xmlDoc.SelectSingleNode("root/header/message-id").InnerText;
                this.CorrellationId = xmlDoc.SelectSingleNode("root/header/correlation-id").InnerText;
                this.InvoiceNumber = Convert.ToInt32(xmlDoc.SelectSingleNode("root/body/invoice-number").InnerText);
                this.DiscountRate = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/discount-rate").InnerText);
                this.RebateCredit = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/rebate-credit").InnerText);
                this.RebateBalance = Convert.ToDecimal(xmlDoc.SelectSingleNode("root/body/rebate-balance").InnerText);
            }
            catch (Exception e)
            {
                throw new Exception("Rebate response message format invalid.", e.InnerException);
            }
        }
        
        public override string ToString()
        {
            return this.MessageXmlDocument.InnerXml;
        }
    }
}