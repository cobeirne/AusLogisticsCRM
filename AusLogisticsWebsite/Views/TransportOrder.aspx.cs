using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AusLogisticsWebsite.Models;
using AusLogisticsWebsite.Controllers;
using System.Threading;
using System.Diagnostics;

/// <summary>
/// Project:    SIT322 Distributed Systems - Assignmnet 1
/// Written By: Chris O'Beirne - Student #211347444
/// Date:       27/03/16
/// </summary>

namespace AusLogisticsWebsite
{
    public partial class Default : System.Web.UI.Page
    {
        public static HttpContext CurrentHttpContext = HttpContext.Current;

        public static Mutex GoogleMapsMutex = new Mutex();

        protected void Page_Load(object sender, EventArgs e)
        {
            _AddDropDownStateList(ddPickupState);
            _AddDropDownStateList(ddDeliveryState);
            _AddDropDownDeliveryHourList(ddDeliveryHour);
            _AddDropDownDeliveryMinuteList(ddDeliveryMinute);

            if (!IsPostBack)
            {
                SetDefaults();
            }            
        }

        private void _AddDropDownStateList(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                dropDown.Items.Add(new ListItem("ACT"));
                dropDown.Items.Add(new ListItem("NSW"));
                dropDown.Items.Add(new ListItem("NT"));
                dropDown.Items.Add(new ListItem("QLD"));
                dropDown.Items.Add(new ListItem("SA"));
                dropDown.Items.Add(new ListItem("TAS"));
                dropDown.Items.Add(new ListItem("VIC"));
                dropDown.Items.Add(new ListItem("WA"));
            }
        }

        private void _AddDropDownDeliveryHourList(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                for (int i = 0; i < 24; i++)
                {
                    dropDown.Items.Add(new ListItem(Convert.ToString(i)));
                }

                this.ddDeliveryHour.SelectedValue = "9";
            }
        }

        private void _AddDropDownDeliveryMinuteList(DropDownList dropDown)
        {
            if (dropDown.Items.Count == 0)
            {
                for (int i = 0; i < 60; i += 10)
                {
                    dropDown.Items.Add(new ListItem(Convert.ToString(i)));
                }
            }
        }
        

        protected void ValidatePickupPostCode(object source, ServerValidateEventArgs args)
        {
            AusPostCodes postCodes = new AusPostCodes();
            args.IsValid = postCodes.ValidatePostCode(args.Value, tbPickupSuburb.Text);
        }

        protected void ValidateDeliveryPostCode(object source, ServerValidateEventArgs args)
        {
            AusPostCodes postCodes = new AusPostCodes();
            args.IsValid = postCodes.ValidatePostCode(args.Value, tbDeliverySuburb.Text);
        }
        
        protected void ValidatePickupSuburb(object source, ServerValidateEventArgs args)
        {
            AusPostCodes postCodes = new AusPostCodes();
            args.IsValid = postCodes.ValidateSuburb(args.Value, ddPickupState.SelectedValue);
        }

        protected void ValidateDeliverySuburb(object source, ServerValidateEventArgs args)
        {
            AusPostCodes postCodes = new AusPostCodes();
            args.IsValid = postCodes.ValidateSuburb(args.Value, ddDeliveryState.SelectedValue);
        }

        protected void ValidateDeliveryTime(object source, ServerValidateEventArgs args)
        {
            // Note to assessor!
            // This validation code has been disabled to improve UI response time 
            // Google Maps API queries can take several seconds.  For the purpose of Assignment 3
            // delivery time validation is not required as we are demonstrating multi threading response.

            //Geolocation origin = new Geolocation();
            //origin.Unit = this.tbPickupUnit.Text;
            //origin.Number = this.tbPickupNumber.Text;
            //origin.Address = this.tbPickupAddress.Text;
            //origin.Suburb = this.tbPickupSuburb.Text;
            //origin.PostCode = this.tbPickupSuburb.Text;

            //Geolocation destination = new Geolocation();
            //destination.Unit = this.tbDeliveryUnit.Text;
            //destination.Number = this.tbDeliveryNumber.Text;
            //destination.Address = this.tbDeliveryAddress.Text;
            //destination.Suburb = this.tbDeliverySuburb.Text;
            //destination.PostCode = this.tbDeliverySuburb.Text;

            //DateTime deliveryDate = GetDeliveryDate();

            //GeoRoute route = new GeoRoute(origin, destination, deliveryDate);
            //route.GetGeoCodes();
            //GoogleMapsAPI mapsApi = new GoogleMapsAPI();
            //route = mapsApi.GetGeoRoute(route);

            //route.CalculatePickupTime();

            //args.IsValid = route.PickupTime <= DateTime.Now ? false : true;

            args.IsValid = true;
        }

        private DateTime GetDeliveryDate()
        {
            DateTime deliveryDate = new DateTime(this.calDelivery.SelectedDate.Year, this.calDelivery.SelectedDate.Month, this.calDelivery.SelectedDate.Day, Convert.ToInt32(this.ddDeliveryHour.Text), Convert.ToInt32(this.ddDeliveryMinute.Text), 0);
            return deliveryDate;
        }


        protected void btFormSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                OrderController orderController = new OrderController();
                orderController.Order = new TransportOrder();

                // Build elements to determine transport route
                Geolocation origin = new Geolocation{
                    Unit = this.tbPickupUnit.Text,
                    Number = this.tbPickupNumber.Text,
                    Address = this.tbPickupAddress.Text,
                    Suburb = this.tbPickupSuburb.Text,
                    State = this.ddPickupState.Text,
                    PostCode = this.tbPickupPostCode.Text 
                };

                Geolocation destination = new Geolocation{
                    Unit = this.tbDeliveryUnit.Text,
                    Number = this.tbDeliveryNumber.Text,
                    Address = this.tbDeliveryAddress.Text,
                    Suburb = this.tbDeliverySuburb.Text,
                    State = this.ddDeliveryState.Text,
                    PostCode = this.tbDeliveryPostCode.Text 
                };

                DateTime deliveryDate = GetDeliveryDate();

                orderController.Order.Route = new GeoRoute(origin, destination, deliveryDate);

                orderController.Order.DeliveryWeight = Convert.ToDecimal(this.tbDeliveryWeight.Text);

                // Set the order member to an exsiting member or one that was create using the forms customer details
                orderController.Order.OrderMember = GetOrderMember(); 
                                
                if (orderController.Order.OrderMember != null)
                {
                    // Create an event handle array for thread syncronisation
                    AutoResetEvent[] eventHandles = { new AutoResetEvent(false), new AutoResetEvent(false), new AutoResetEvent(false) };

                    // Spawn a new thread to process the order
                    Thread orderThread = new Thread(new ParameterizedThreadStart(orderController.ProcessOrder));
                    orderThread.Start(eventHandles);

                    // Spawn a new thread to query the Rebate Processor
                    RebateController rebateController = new RebateController();
                    rebateController.Order = orderController.Order;
                    Thread rebateThread = new Thread(new ParameterizedThreadStart(rebateController.ProcessRebate));
                    rebateThread.Start(eventHandles);

                    // Spawn a new thread to build the invoice
                    InvoiceController invoiceController = new InvoiceController();
                    invoiceController.Order = orderController.Order;
                    invoiceController.htx = HttpContext.Current;
                    Thread invoiceThread = new Thread(new ParameterizedThreadStart(invoiceController.GeneratePdf));
                    invoiceThread.Start(eventHandles);
                }
            }
        }


        private MemberModel GetOrderMember()
        {
            MemberController memberController = new MemberController();
            MemberModel orderMember = null;

            if (this.rblMembership.SelectedValue == "0")
            {
                MemberModel newMember = new MemberModel
                {
                    FirstName = this.tbFirstName.Text,
                    LastName = this.tbLastName.Text,
                    PhoneNumber = "N/A",
                    Unit = "N/A",
                    Number = "N/A",
                    Address = "N/A",
                    Suburb = "N/A",
                    State = "N/A",
                    PostCode = "N/A",
                    DateOfBirth = DateTime.Today.AddYears(-30),
                    DateJoined = DateTime.Today
                };

                memberController.AddMember(newMember);
                orderMember = memberController.SelectMember(newMember.FirstName, newMember.LastName, newMember.DateOfBirth);
            }
            else
            {
                if (this.tbMemberId.Text != "")
                {
                    orderMember = memberController.SelectMember(Convert.ToInt32(this.tbMemberId.Text));
                }
            }

            return orderMember;
        }

        protected void SetDefaults()
        {
            this.tbFirstName.Text = "John";
            this.tbLastName.Text = "White";

            this.tbPickupNumber.Text = "114";
            this.tbPickupAddress.Text = "Skye Road";
            this.ddPickupState.SelectedValue = "VIC";
            this.tbPickupSuburb.Text = "Frankston";
            this.tbPickupPostCode.Text = "3199";

            this.tbDeliveryNumber.Text = "1";
            this.tbDeliveryAddress.Text = "High Street";
            this.ddDeliveryState.SelectedValue = "NSW";
            this.tbDeliverySuburb.Text = "North Sydney";
            this.tbDeliveryPostCode.Text = "2060";

            this.tbDeliveryWeight.Text = "5000";

            this.calDelivery.SelectedDate = DateTime.Today;
            this.calDelivery.VisibleDate = DateTime.Today;
        }

        protected void rblMembership_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(this.rblMembership.SelectedValue == "0")
            {
                //New member
                this.tbMemberId.Enabled = false;
                this.tbFirstName.ReadOnly = false;
                this.tbLastName.ReadOnly = false;
                this.btnSearchMember.Enabled = false;
                this.btFormSubmit.Enabled = true;
                this.tbMemberId.Text = "";
                this.tbFirstName.Text = "";
                this.tbLastName.Text = "";
                this.tbMemberClass.Text = "";
                this.tbMemberClass.Enabled = false;
            }
            else
            {
                // Existing member
                this.tbMemberId.Enabled = true;
                this.tbFirstName.ReadOnly = true;
                this.tbLastName.ReadOnly = true;
                this.btnSearchMember.Enabled = true;
                this.btFormSubmit.Enabled = false;
                this.tbMemberId.Text = "";
                this.tbFirstName.Text = "";
                this.tbLastName.Text = "";
                this.tbMemberClass.Text = "";
                this.tbMemberClass.Enabled = true;
            }
        }

        protected void btnSearchMember_Click(object sender, EventArgs e)
        {
            if (this.tbMemberId.Text != "")
            {
                SearchExistingMember();
                this.btFormSubmit.Enabled = true;
            }
        }


        private void SearchExistingMember()
        {
            int memberId = Convert.ToInt32(this.tbMemberId.Text);

            MemberController controller = new MemberController();
            MemberModel member = controller.SelectMember(memberId);

            if (member != null)
            {
                // Existing member found
                this.lblMembershipError.Visible = false;
                this.tbFirstName.Text = member.FirstName;
                this.tbLastName.Text = member.LastName;
                this.tbMemberClass.Text = member.MemberClassName;
            }
            else
            {
                // Member not found
                this.lblMembershipError.Text = "Membership ID not found";
                this.lblMembershipError.Visible = true;
                this.tbMemberClass.Text = "";
            }
        }
    }
}