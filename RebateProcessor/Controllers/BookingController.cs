using AusLogisticsLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RebateProcessor.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 3 
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       21/05/16
    /// </summary>

    public class BookingController
    {
        public SqlDbModel DbContext { get; set; }

        public BookingController()
        {
            this.DbContext = new SqlDbModel("App_Data", Properties.Settings.Default.DbFileName);
        }

        public BookingController(string connectionString)
        {
            this.DbContext = new SqlDbModel();
            this.DbContext.ConnectionString = connectionString;
        }


        public bool AddBooking(RebateRequestMessage rebateRequest, RebateReplyMessage rebateReply)
        {
            bool memberAdded = false;
            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO Booking (BookingMemberId,BookingMemberClassId,BookingOriginNumber,BookingOriginAddress,BookingOriginSuburb,BookingOriginState,BookingOriginPostCode,BookingDestinationNumber,BookingDestinationAddress,BookingDestinationSuburb,BookingDestinationState,BookingDestinationPostCode,BookingDeliveryDateTime,BookingDeliveryWeight,BookingTrucksRequired,BookingOrderSubTotal,BookingOrderGstRate,BookingRebateCredit,BookingDiscountRate,RequestMessageId) VALUES (@BookingMemberId,@BookingMemberClassId,@BookingOriginNumber,@BookingOriginAddress,@BookingOriginSuburb,@BookingOriginState,@BookingOriginPostCode,@BookingDestinationNumber,@BookingDestinationAddress,@BookingDestinationSuburb,@BookingDestinationState,@BookingDestinationPostCode,@BookingDeliveryDateTime,@BookingDeliveryWeight,@BookingTrucksRequired,@BookingOrderSubTotal,@BookingOrderGstRate,@BookingRebateCredit,@BookingDiscountRate,@RequestMessageId)", this.DbContext.Connection);

                cmd.Parameters.Add("@BookingMemberId", System.Data.SqlDbType.Int).Value = rebateRequest.MemberId;
                cmd.Parameters.Add("@BookingMemberClassId", System.Data.SqlDbType.Int).Value = rebateRequest.MemberClassId;
                cmd.Parameters.Add("@BookingOriginNumber", System.Data.SqlDbType.VarChar).Value = rebateRequest.OriginNumber;
                cmd.Parameters.Add("@BookingOriginAddress", System.Data.SqlDbType.VarChar).Value = rebateRequest.OriginAddress;
                cmd.Parameters.Add("@BookingOriginSuburb", System.Data.SqlDbType.VarChar).Value = rebateRequest.OriginSuburb;
                cmd.Parameters.Add("@BookingOriginState", System.Data.SqlDbType.VarChar).Value = rebateRequest.OriginState;
                cmd.Parameters.Add("@BookingOriginPostCode", System.Data.SqlDbType.VarChar).Value = rebateRequest.OriginPostCode;
                cmd.Parameters.Add("@BookingDestinationNumber", System.Data.SqlDbType.VarChar).Value = rebateRequest.DestinationNumber;
                cmd.Parameters.Add("@BookingDestinationAddress", System.Data.SqlDbType.VarChar).Value = rebateRequest.DestinationAddress;
                cmd.Parameters.Add("@BookingDestinationSuburb", System.Data.SqlDbType.VarChar).Value = rebateRequest.DestinationSuburb;
                cmd.Parameters.Add("@BookingDestinationState", System.Data.SqlDbType.VarChar).Value = rebateRequest.DestinationState;
                cmd.Parameters.Add("@BookingDestinationPostCode", System.Data.SqlDbType.VarChar).Value = rebateRequest.DestinationPostCode;
                cmd.Parameters.Add("@BookingDeliveryDateTime", System.Data.SqlDbType.DateTime).Value = rebateRequest.DeliveryDateTime;
                cmd.Parameters.Add("@BookingDeliveryWeight", System.Data.SqlDbType.Decimal).Value = rebateRequest.DeliveryWeight;
                cmd.Parameters.Add("@BookingTrucksRequired", System.Data.SqlDbType.Int).Value = rebateRequest.TrucksRequired;
                cmd.Parameters.Add("@BookingOrderSubTotal", System.Data.SqlDbType.Decimal).Value = rebateRequest.OrderSubTotal;
                cmd.Parameters.Add("@BookingOrderGstRate", System.Data.SqlDbType.Decimal).Value = rebateRequest.OrderGstRate;
                cmd.Parameters.Add("@BookingRebateCredit", System.Data.SqlDbType.Decimal).Value = rebateReply.RebateCredit;
                cmd.Parameters.Add("@BookingDiscountRate", System.Data.SqlDbType.Decimal).Value = rebateReply.DiscountRate;
                cmd.Parameters.Add("@RequestMessageId", System.Data.SqlDbType.VarChar).Value = rebateRequest.MessageId;

                int rowsAdded = cmd.ExecuteNonQuery();

                if (rowsAdded > 0)
                {
                    memberAdded = true;
                }
            }
            catch (Exception)
            {
                memberAdded = false;
            }

            return memberAdded;
        }

        public int SelectBookingId(string requestMessageId)
        {
            int bookingId = 0;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Booking WHERE (RequestMessageId = @RequestMessageId)", this.DbContext.Connection);

                cmd.Parameters.Add("@RequestMessageId", System.Data.SqlDbType.VarChar).Value = requestMessageId;

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    bookingId = Convert.ToInt32(reader["BookingId"]);
                }

                cmd.Connection.Close();

            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return bookingId;
        }

    }
}
