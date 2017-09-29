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

    public class MemberAccountController
    {
        public SqlDbModel DbContext { get; set; }

        public MemberAccountController()
        {
            this.DbContext = new SqlDbModel("App_Data", Properties.Settings.Default.DbFileName);
        }

        public MemberAccountController(string connectionString)
        {
            this.DbContext = new SqlDbModel();
            this.DbContext.ConnectionString = connectionString;
        }


        public bool CheckMemberExists(int membershipId)
        {
            bool memberExists = true;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM MemberAccount WHERE (MembershipId = @MembershipId)", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.VarChar).Value = membershipId;

                reader = cmd.ExecuteReader();

                int members = 0;

                while (reader.Read())
                {
                    members += 1;
                }

                if (members == 0)
                {
                    memberExists = false;
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

            return memberExists;
        }

        public bool AddMember(int memberId)
        {
            bool memberAdded = false;
            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("INSERT INTO MemberAccount (MembershipId,MemberAccountBalance) VALUES (@MembershipId,@MemberAccountBalance)", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.Int).Value = memberId;
                cmd.Parameters.Add("@MemberAccountBalance", System.Data.SqlDbType.Decimal).Value = 0.0M;

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

        public decimal SelectMemberBalance(int memberId)
        {
            decimal memberAccountBalance = 0.0M;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM MemberAccount WHERE (MembershipId = @MembershipId)", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.Int).Value = memberId;

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    memberAccountBalance = Convert.ToDecimal(reader["MemberAccountBalance"]);
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

            return memberAccountBalance;
        }

        public bool UpdateMemberBalance(int memberId, decimal newBalance)
        {
            bool memberUpdated = false;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("UPDATE MemberAccount SET MemberAccountBalance=@MemberAccountBalance WHERE MembershipId=@MembershipId", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.Int).Value = memberId;
                cmd.Parameters.Add("@MemberAccountBalance", System.Data.SqlDbType.Decimal).Value = newBalance;

                int rowsUpdated = cmd.ExecuteNonQuery();

                if (rowsUpdated > 0)
                {
                    memberUpdated = true;
                }

                cmd.Connection.Close();

            }
            catch (Exception)
            { }
            finally
            {
                this.DbContext.Connection.Close();
            }

            return memberUpdated;
        }
    }
}
