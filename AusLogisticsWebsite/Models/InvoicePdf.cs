using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    static public class InvoicePdf
    {
        static public Document BuildPdf(Document pdf, TransportOrder order)
        {

            pdf.Open();

            var titleFont = FontFactory.GetFont("Arial", 18, Font.BOLD);
            var subTitleFont = FontFactory.GetFont("Arial", 16, Font.BOLD);
            var boldFont = FontFactory.GetFont("Arial", 12, Font.BOLD);
            var bodyFont = FontFactory.GetFont("Arial", 12, Font.NORMAL);

            PdfPTable table1 = new PdfPTable(4) { WidthPercentage = 100 };

            // Row 1
            PdfPCell row1col1cell = new PdfPCell(new Phrase("Australian Logistics", titleFont));
            row1col1cell.Colspan = 2;
            row1col1cell.HorizontalAlignment = 0; //0=Left, 1=Centre, 2=Right
            row1col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row1col1cell);
            
            PdfPCell row1col2cell = new PdfPCell(new Phrase("1 Package Way\nMelbourne, VIC.3000\n", bodyFont));
            row1col2cell.Colspan = 2;
            row1col2cell.HorizontalAlignment = 2;
            row1col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row1col2cell);

            // Row 2
            PdfPCell row2col1cell = new PdfPCell(new Phrase("Tax Invoice\n ", subTitleFont));
            row2col1cell.Colspan = 4;
            row2col1cell.HorizontalAlignment = 0;
            row2col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row2col1cell);

            // Row 3
            PdfPCell row3col1cell = new PdfPCell(new Phrase("Customer:", boldFont));
            row3col1cell.Colspan = 2;
            row3col1cell.HorizontalAlignment = 0;
            row3col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row3col1cell);

            string invoiceNumber = string.Format("Invoice #: {0}", order.InvoiceNumber);
            PdfPCell row3col2cell = new PdfPCell(new Phrase(invoiceNumber, boldFont));
            row3col2cell.Colspan = 2;
            row3col2cell.HorizontalAlignment = 2;
            row3col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row3col2cell);

            // Row 4
            string customerName = string.Format("{0} {1}\nMember Class: {2}\n ", order.OrderMember.FirstName, order.OrderMember.LastName, order.OrderMember.MemberClassName);
            PdfPCell row4col1cell = new PdfPCell(new Phrase(customerName, bodyFont));
            row4col1cell.Colspan = 2;
            row4col1cell.HorizontalAlignment = 0;
            row4col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row4col1cell);

            string invoiceDateTime = DateTime.Now.ToString();
            PdfPCell row4col2cell = new PdfPCell(new Phrase(invoiceDateTime + "\n ", bodyFont));
            row4col2cell.Colspan = 2;
            row4col2cell.HorizontalAlignment = 2;
            row4col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row4col2cell);

            //Row 5
            PdfPCell row5col1cell = new PdfPCell(new Phrase("Billing Address:", boldFont));
            row5col1cell.Colspan = 2;
            row5col1cell.HorizontalAlignment = 0;
            row5col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row5col1cell);

            PdfPCell row5col2cell = new PdfPCell(new Phrase("Consignment Details:", boldFont));
            row5col2cell.Colspan = 2;
            row5col2cell.HorizontalAlignment = 0;
            row5col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row5col2cell);

            // Row 6
            string billingAddress = ""; //string.Format("{0} {1}\n{2},{3}.{4}\n ", order.OrderMember.Number, order.OrderMember.Address, order.OrderMember.Suburb, order.OrderMember.State, order.OrderMember.PostCode);
            PdfPCell row6col1cell = new PdfPCell(new Phrase(billingAddress, bodyFont));
            row6col1cell.Colspan = 2;
            row6col1cell.HorizontalAlignment = 0;
            row6col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row6col1cell);

            string consignmentDetails = string.Format("Weight: {0} kg\nTrucks: {1}\nDistance: {2} km\nTrip Time: {3} Hrs\nDelivery Date: {4}\n ", order.DeliveryWeight, order.TrucksRequired, order.Route.RouteMeters/1000, order.Route.DeliverySeconds/3600, order.Route.DeliveryDateTime);
            PdfPCell row6col2cell = new PdfPCell(new Phrase(consignmentDetails, bodyFont));
            row6col2cell.Colspan = 2;
            row6col2cell.HorizontalAlignment = 0;
            row6col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row6col2cell);

            //Row 7
            PdfPCell row7col1cell = new PdfPCell(new Phrase("Pickup Address:", boldFont));
            row7col1cell.Colspan = 2;
            row7col1cell.HorizontalAlignment = 0;
            row7col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row7col1cell);

            PdfPCell row7col2cell = new PdfPCell(new Phrase("Delivery Address:", boldFont));
            row7col2cell.Colspan = 2;
            row7col2cell.HorizontalAlignment = 0;
            row7col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row7col2cell);

            // Row 8
            string pickupAddress = string.Format("{0} {1}\n{2},{3}.{4}\n ", order.Route.Origin.Number, order.Route.Origin.Address, order.Route.Origin.Suburb, order.Route.Origin.State, order.Route.Origin.PostCode);
            PdfPCell row8col1cell = new PdfPCell(new Phrase(pickupAddress, bodyFont));
            row8col1cell.Colspan = 2;
            row8col1cell.HorizontalAlignment = 0;
            row8col1cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row8col1cell);

            string deliveryAddress = string.Format("{0} {1}\n{2},{3}.{4}\n ", order.Route.Destination.Number, order.Route.Destination.Address, order.Route.Destination.Suburb, order.Route.Destination.State, order.Route.Destination.PostCode);
            PdfPCell row8col2cell = new PdfPCell(new Phrase(deliveryAddress, bodyFont));
            row8col2cell.Colspan = 2;
            row8col2cell.HorizontalAlignment = 0;
            row8col2cell.Border = Rectangle.NO_BORDER;
            table1.AddCell(row8col2cell);

            pdf.Add(table1);

            PdfPTable table2 = new PdfPTable(4) { WidthPercentage = 100 };
            float[] columnWidths = new float[] { 50, 10, 20, 20 };
            table2.SetWidths(columnWidths);

            // Row 9
            PdfPCell row9col1cell = new PdfPCell(new Phrase("ITEM", boldFont));
            row9col1cell.HorizontalAlignment = 0;
            table2.AddCell(row9col1cell);

            PdfPCell row9col2cell = new PdfPCell(new Phrase("QTY", boldFont));
            row9col2cell.HorizontalAlignment = 1;
            table2.AddCell(row9col2cell);

            PdfPCell row9col3cell = new PdfPCell(new Phrase("RATE", boldFont));
            row9col3cell.HorizontalAlignment = 2;
            table2.AddCell(row9col3cell);

            PdfPCell row9col4cell = new PdfPCell(new Phrase("TOTAL", boldFont));
            row9col4cell.HorizontalAlignment = 2;
            table2.AddCell(row9col4cell);

            // Row 10 Dynamic
            foreach (OrderItem item in order.OrderItems)
            {
                PdfPCell row10col1cell = new PdfPCell(new Phrase(item.ItemName, bodyFont));
                row10col1cell.HorizontalAlignment = 0;
                table2.AddCell(row10col1cell);

                PdfPCell row10col2cell = new PdfPCell(new Phrase(Convert.ToString(item.Quantity), bodyFont));
                row10col2cell.HorizontalAlignment = 1;
                table2.AddCell(row10col2cell);

                PdfPCell row10col3cell = new PdfPCell(new Phrase(item.Rate.ToString("C"), bodyFont));
                row10col3cell.HorizontalAlignment = 2;
                table2.AddCell(row10col3cell);

                PdfPCell row10col4cell = new PdfPCell(new Phrase(item.Total.ToString("C"), bodyFont));
                row10col4cell.HorizontalAlignment = 2;
                table2.AddCell(row10col4cell);
            }

            pdf.Add(table2);

            PdfPTable table3 = new PdfPTable(4) { WidthPercentage = 100 };

            // Row 11
            PdfPCell row11col1cell = new PdfPCell(new Phrase("", boldFont));
            row11col1cell.Colspan = 2;
            row11col1cell.HorizontalAlignment = 0;
            row11col1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11col1cell);

            PdfPCell row11col2cell = new PdfPCell(new Phrase("Subtotal:", bodyFont));
            row11col2cell.HorizontalAlignment = 2;
            row11col2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11col2cell);

            PdfPCell row11col3cell = new PdfPCell(new Phrase(order.OrderSubTotal.ToString("C"), bodyFont));
            row11col3cell.HorizontalAlignment = 2;
            row11col3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11col3cell);

            // Row 11A -Rebate Credit
            PdfPCell row11Acol1cell = new PdfPCell(new Phrase("", boldFont));
            row11Acol1cell.Colspan = 2;
            row11Acol1cell.HorizontalAlignment = 0;
            row11Acol1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Acol1cell);

            PdfPCell row11Acol2cell = new PdfPCell(new Phrase("Discount:", bodyFont));
            row11Acol2cell.HorizontalAlignment = 2;
            row11Acol2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Acol2cell);
                        
            PdfPCell row11Acol3cell = new PdfPCell(new Phrase(order.DiscountApplied.ToString("C"), bodyFont));
            row11Acol3cell.HorizontalAlignment = 2;
            row11Acol3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Acol3cell);

            // Row 11B -Rebate Credit
            PdfPCell row11Bcol1cell = new PdfPCell(new Phrase("", boldFont));
            row11Bcol1cell.Colspan = 2;
            row11Bcol1cell.HorizontalAlignment = 0;
            row11Bcol1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Bcol1cell);

            PdfPCell row11Bcol2cell = new PdfPCell(new Phrase("Rebate Credit:", bodyFont));
            row11Bcol2cell.HorizontalAlignment = 2;
            row11Bcol2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Bcol2cell);

            PdfPCell row11Bcol3cell = new PdfPCell(new Phrase(order.RebateCredit.ToString("C"), bodyFont));
            row11Bcol3cell.HorizontalAlignment = 2;
            row11Bcol3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row11Bcol3cell);

            // Row 12
            PdfPCell row12col1cell = new PdfPCell(new Phrase("", boldFont));
            row12col1cell.Colspan = 2;
            row12col1cell.HorizontalAlignment = 0;
            row12col1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row12col1cell);

            PdfPCell row12col2cell = new PdfPCell(new Phrase("GST:", bodyFont));
            row12col2cell.HorizontalAlignment = 2;
            row12col2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row12col2cell);

            PdfPCell row12col3cell = new PdfPCell(new Phrase(order.OrderGst.ToString("C"), bodyFont));
            row12col3cell.HorizontalAlignment = 2;
            row12col3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row12col3cell);

            // Row 13
            PdfPCell row13col1cell = new PdfPCell(new Phrase("", boldFont));
            row13col1cell.Colspan = 2;
            row13col1cell.HorizontalAlignment = 0;
            row13col1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row13col1cell);

            PdfPCell row13col2cell = new PdfPCell(new Phrase("Grand Total:", boldFont));
            row13col2cell.HorizontalAlignment = 2;
            row13col2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row13col2cell);

            PdfPCell row13col3cell = new PdfPCell(new Phrase(order.OrderTotal.ToString("C"), boldFont));
            row13col3cell.HorizontalAlignment = 2;
            row13col3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row13col3cell);

            // Row 14 - Rebate Balance
            PdfPCell row14col1cell = new PdfPCell(new Phrase("", bodyFont));
            row14col1cell.Colspan = 2;
            row14col1cell.HorizontalAlignment = 0;
            row14col1cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row14col1cell);

            PdfPCell row14col2cell = new PdfPCell(new Phrase("Rebate Balance:", bodyFont));
            row14col2cell.HorizontalAlignment = 2;
            row14col2cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row14col2cell);

            PdfPCell row14col3cell = new PdfPCell(new Phrase(order.RebateBalance.ToString("C"), bodyFont));
            row14col3cell.HorizontalAlignment = 2;
            row14col3cell.Border = Rectangle.NO_BORDER;
            table3.AddCell(row14col3cell);


            pdf.Add(table3);


            pdf.Close();

            return pdf;
        }
    }
}