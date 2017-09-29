using iTextSharp.text;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace AusLogisticsWebsite.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 1
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       27/03/16
    /// </summary>

    public class InvoiceController
    {
        public TransportOrder Order { get; set; }

        public HttpContext htx { get; set; }

        public void GeneratePdf(object eventHandles)
        {
            var autoEventHandles = (AutoResetEvent[])eventHandles;

            // Block the current thread until the Rebate Controller signals that the rebate has been processed.
            autoEventHandles[2].WaitOne();

            this.Order.CalculateOrderSubtotal();
            this.Order.CalculateOrderGst();
            this.Order.CalculateOrderTotal();

            Document pdfDocument = new Document(PageSize.A4, 25, 25, 25, 25);

            string path = htx.Server.MapPath("../Invoices/");
            PdfWriter.GetInstance(pdfDocument, new FileStream(path + "TaxInvoice" + this.Order.InvoiceNumber + ".pdf", FileMode.Create));
            pdfDocument = InvoicePdf.BuildPdf(pdfDocument, this.Order);            
        }
    }
}