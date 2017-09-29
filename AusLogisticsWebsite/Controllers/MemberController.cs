using AusLogisticsLibrary.Models;
using AusLogisticsWebsite.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;

namespace AusLogisticsWebsite.Controllers
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public class MemberController
    {
        public SqlDbModel DbContext { get; set; }


        public MemberController()
        {
            this.DbContext = new SqlDbModel(Properties.Settings.Default.DbFileName);
        }


        public bool AddMember(MemberModel member)
        {
            bool memberAdded = false;

            bool memberExists = CheckMemberExists(member);

            if (!memberExists)
            {
                try
                {
                    member.MemberClassId = GetMemberClassId(member.Salary, member.DateJoined);

                    CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);
                    
                    this.DbContext.Open();

                    SqlCommand cmd = new SqlCommand("INSERT INTO Membership (FirstName,LastName,MemberClassId,DateJoined,DateOfBirth,Salary,Gender) VALUES (@FirstName,@LastName,@MemberClassId,@DateJoined,@DateOfBirth,@Salary,@Gender)", this.DbContext.Connection);

                    cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = member.FirstName;
                    cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = member.LastName;
                    cmd.Parameters.Add("@MemberClassId", System.Data.SqlDbType.Int).Value = member.MemberClassId;
                    cmd.Parameters.Add("@DateJoined", System.Data.SqlDbType.DateTime).Value = member.DateJoined;

                    string dateOfBirth = string.Format("{0:d}", member.DateOfBirth.Date);
                    cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(dateOfBirth, Properties.Settings.Default.CryptoKey);
                    cmd.Parameters.Add("@Salary", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Salary.ToString(), Properties.Settings.Default.CryptoKey);
                    cmd.Parameters.Add("@Gender", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Gender.ToString(), Properties.Settings.Default.CryptoKey);

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
            }

            return memberAdded;
        }

        public int GetMemberClassId(decimal salary, DateTime dateJoined)
        {
            int classId = 0;

            MemberClassModel memberClass = null;

            List<MemberModel> members = SelectMembers();

            decimal goldLowestSalary = GoldReportModel.GetLowestGoldSalary(members, Properties.Settings.Default.GoldTopPercentile);
            decimal silverLowestSalary = SilverReportModel.GetLowestSilverSalary(members, Properties.Settings.Default.SilverTopPercentile);
            DateTime loyaltyDate = LoyaltyReportModel.GetLatestLoyaltyDate(Properties.Settings.Default.LoyalityYears);
            
            if(salary >= goldLowestSalary)
            {
                memberClass = SelectMemberClass("Gold");
                classId = memberClass.MemberClassId;
            }
            else if(salary >= silverLowestSalary)
            {
                memberClass = SelectMemberClass("Silver");
                classId = memberClass.MemberClassId;
            }
            else if (dateJoined <= loyaltyDate)
            {
                memberClass = SelectMemberClass("Loyalty");
                classId = memberClass.MemberClassId;
            }
            else
            {
                memberClass = SelectMemberClass("Regular");
                classId = memberClass.MemberClassId;
            }

            return classId;
        }

        public bool CheckMemberExists(MemberModel member)
        {
            bool memberExists = true;

            SqlDataReader reader = null;

            try
            {
                CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Membership WHERE (FirstName = @FirstName) AND (LastName=@LastName) AND (DateOfBirth = @DateOfBirth) AND (Gender = @Gender)", this.DbContext.Connection);

                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = member.FirstName;
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = member.LastName;

                string dateOfBirth = string.Format("{0:d}", member.DateOfBirth.Date);
                cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(dateOfBirth, Properties.Settings.Default.CryptoKey);
                cmd.Parameters.Add("@Gender", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Gender.ToString(), Properties.Settings.Default.CryptoKey);

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

        public bool CheckMemberExists(int membershipId)
        {
            bool memberExists = true;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Membership WHERE (MembershipId = @MembershipId)", this.DbContext.Connection);

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


        public bool DeleteMember(int membershipId)
        {
            bool memberDeleted = false;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("Delete FROM Membership WHERE (MembershipId = @MembershipId)", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.VarChar).Value = membershipId;

                int deletedRecords = cmd.ExecuteNonQuery();

                if (deletedRecords > 0)
                {
                    memberDeleted = true;
                }

                cmd.Connection.Close();

            }
            catch(Exception)
            { }

            return memberDeleted;
        }

        public MemberModel SelectMember(int membershipId)
        {
            MemberModel newMember = null;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT Membership.*, MemberClass.MemberClass FROM Membership INNER JOIN MemberClass ON Membership.MemberClassId = MemberClass.MemberClassId WHERE Membership.MembershipId = @MembershipId", this.DbContext.Connection);

                cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.Int).Value = membershipId;

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                    newMember = new MemberModel
                    {
                        MembershipId = Convert.ToInt32(reader["MembershipId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        MemberClassName = reader["MemberClass"].ToString(),
                        DateJoined = Convert.ToDateTime(reader["DateJoined"]),
                        DateOfBirth = Convert.ToDateTime(crypto.DecryptString(reader["DateOfBirth"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Salary = Convert.ToDecimal(crypto.DecryptString(reader["Salary"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Gender = Convert.ToChar(crypto.DecryptString(reader["Gender"].ToString(), Properties.Settings.Default.CryptoKey))
                    };
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

            return newMember;
        }

        public MemberModel SelectMember(string FirstName, string LastName, DateTime DateOfBirth)
        {
            MemberModel newMember = null;

            SqlDataReader reader = null;

            try
            {
                CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT Membership.*, MemberClass.MemberClass FROM Membership INNER JOIN MemberClass ON Membership.MemberClassId = MemberClass.MemberClassId WHERE (Membership.FirstName = @FirstName) AND (Membership.LastName = @LastName) AND (Membership.DateOfBirth = @DateOfBirth)", this.DbContext.Connection);

                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = FirstName;
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = LastName;

                string dateOfBirth = string.Format("{0:d}", DateOfBirth.Date);
                cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(dateOfBirth, Properties.Settings.Default.CryptoKey); 

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    newMember = new MemberModel
                    {
                        MembershipId = Convert.ToInt32(reader["MembershipId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        MemberClassName = reader["MemberClass"].ToString(),
                        DateJoined = Convert.ToDateTime(reader["DateJoined"]),
                        DateOfBirth = Convert.ToDateTime(crypto.DecryptString(reader["DateOfBirth"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Salary = Convert.ToDecimal(crypto.DecryptString(reader["Salary"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Gender = Convert.ToChar(crypto.DecryptString(reader["Gender"].ToString(), Properties.Settings.Default.CryptoKey))
                    };
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

            return newMember;
        }


        public MemberModel SelectMember(MemberModel member)
        {
            MemberModel newMember = new MemberModel();

            SqlDataReader reader = null;

            try
            {
                CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                this.DbContext.Open();
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM Membership WHERE (FirstName = @FirstName) AND (LastName=@LastName) AND (DateOfBirth = @DateOfBirth) AND (Gender = @Gender)", this.DbContext.Connection);

                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = member.FirstName;
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = member.LastName;

                string dateOfBirth = string.Format("{0:d}", member.DateOfBirth.Date);
                cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(dateOfBirth, Properties.Settings.Default.CryptoKey);
                cmd.Parameters.Add("@Gender", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Gender.ToString(), Properties.Settings.Default.CryptoKey);

                reader = cmd.ExecuteReader();


                if (reader.Read())
                {

                    newMember = new MemberModel
                    {
                        MembershipId = Convert.ToInt32(reader["MembershipId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        DateJoined = Convert.ToDateTime(reader["DateJoined"]),
                        DateOfBirth = Convert.ToDateTime(crypto.DecryptString(reader["DateOfBirth"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Salary = Convert.ToDecimal(crypto.DecryptString(reader["Salary"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Gender = Convert.ToChar(crypto.DecryptString(reader["Gender"].ToString(), Properties.Settings.Default.CryptoKey))
                    };
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

            return newMember;
        }


        public List<MemberModel> SelectMembers()
        {
            List<MemberModel> selectMembers = new List<MemberModel>();

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT Membership.*, MemberClass.MemberClass FROM Membership INNER JOIN MemberClass ON Membership.MemberClassId = MemberClass.MemberClassId", this.DbContext.Connection);

                reader = cmd.ExecuteReader();

                CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                while (reader.Read())
                {
                    MemberModel newMemebr = new MemberModel
                    {
                        MembershipId = Convert.ToInt32(reader["MembershipId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        DateJoined = Convert.ToDateTime(reader["DateJoined"]),
                        DateOfBirth = Convert.ToDateTime(crypto.DecryptString(reader["DateOfBirth"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Salary = Convert.ToDecimal(crypto.DecryptString(reader["Salary"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Gender = Convert.ToChar(crypto.DecryptString(reader["Gender"].ToString(), Properties.Settings.Default.CryptoKey)),
                        MemberClassName = reader["MemberClass"].ToString()
                    };

                    selectMembers.Add(newMemebr);
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

            return selectMembers;
        }

        public List<MemberModel> SelectMembers(int memberId = 0, string firstName = "", string lastName = "")
        {
            List<MemberModel> selectMembers = new List<MemberModel>();

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                Dictionary<string, string> searchParams = new Dictionary<string, string>();
                
                if(memberId != 0)
                {
                    searchParams.Add("@MemberId", "(Membership.MembershipId LIKE @MemberId)");
                }

                if (firstName != "")
                {
                    searchParams.Add("@FirstName", "(Membership.FirstName LIKE @FirstName)");
                }

                if (lastName != "")
                {
                    searchParams.Add("@LastName", "(Membership.LastName LIKE @LastName)");
                }

                StringBuilder query = new StringBuilder();

                query.Append("SELECT Membership.*, MemberClass.MemberClass FROM Membership INNER JOIN MemberClass ON Membership.MemberClassId = MemberClass.MemberClassId");

                if (searchParams.Count > 0)
                {
                    query.Append(" WHERE ");

                    int paramNo = 0;

                    foreach (var param in searchParams)
                    {
                        paramNo += 1;
                        query.Append(string.Format(param.Value));

                        if(paramNo < searchParams.Count)
                        {
                            query.Append(" AND ");
                        }
                    }
                }


                SqlCommand cmd = new SqlCommand(query.ToString(), this.DbContext.Connection);
                cmd.Parameters.Add("@MemberId", System.Data.SqlDbType.Int).Value = memberId;
                cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = firstName;
                cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = lastName;

                reader = cmd.ExecuteReader();

                CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                while (reader.Read())
                {
                    MemberModel newMemebr = new MemberModel
                    {
                        MembershipId = Convert.ToInt32(reader["MembershipId"]),
                        FirstName = reader["FirstName"].ToString(),
                        LastName = reader["LastName"].ToString(),
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        MemberClassName = Convert.ToString(reader["MemberClass"]),
                        DateJoined = Convert.ToDateTime(reader["DateJoined"]),
                        DateOfBirth = Convert.ToDateTime(crypto.DecryptString(reader["DateOfBirth"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Salary = Convert.ToDecimal(crypto.DecryptString(reader["Salary"].ToString(), Properties.Settings.Default.CryptoKey)),
                        Gender = Convert.ToChar(crypto.DecryptString(reader["Gender"].ToString(), Properties.Settings.Default.CryptoKey))
                    };

                    selectMembers.Add(newMemebr);
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

            return selectMembers;
        }

        public MemberClassModel SelectMemberClass(string memberClassName)
        {
            MemberClassModel memberClass = null;

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM MemberClass WHERE (MemberClass = @MemberClass)", this.DbContext.Connection);

                cmd.Parameters.Add("@MemberClass", System.Data.SqlDbType.VarChar).Value = memberClassName;

                reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    memberClass = new MemberClassModel
                    {
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        MemberClass = reader["MemberClass"].ToString()
                    };
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

            return memberClass;
        }

        public List<MemberClassModel> SelectMemberClasses()
        {
            List<MemberClassModel> memberClasses = new List<MemberClassModel>();

            SqlDataReader reader = null;

            try
            {
                this.DbContext.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM MemberClass", this.DbContext.Connection);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    MemberClassModel memberClass = new MemberClassModel
                    {
                        MemberClassId = Convert.ToInt32(reader["MemberClassId"]),
                        MemberClass = reader["MemberClass"].ToString()
                    };

                    memberClasses.Add(memberClass);
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

            return memberClasses.ToList();
        }


        public bool UpdateMember(MemberModel member)
        {
            bool memberUpdated = false;

            bool memberExists = CheckMemberExists(member.MembershipId);

            if (memberExists)
            {
                try
                {
                    CryptoController crypto = new CryptoController(Properties.Settings.Default.CryptoSalt, Properties.Settings.Default.CryptoKeyLegnth);

                    member.MemberClassId = GetMemberClassId(member.Salary, member.DateJoined);

                    this.DbContext.Open();

                    SqlCommand cmd = new SqlCommand("UPDATE Membership SET FirstName=@FirstName,LastName=@LastName,MemberClassId=@MemberClassId,DateJoined=@DateJoined,DateOfBirth=@DateOfBirth,Salary=@Salary,Gender=@Gender WHERE MembershipId=@MembershipId", this.DbContext.Connection);

                    cmd.Parameters.Add("@MembershipId", System.Data.SqlDbType.Int).Value = member.MembershipId;
                    cmd.Parameters.Add("@FirstName", System.Data.SqlDbType.VarChar).Value = member.FirstName;
                    cmd.Parameters.Add("@LastName", System.Data.SqlDbType.VarChar).Value = member.LastName;
                    cmd.Parameters.Add("@MemberClassId", System.Data.SqlDbType.Int).Value = member.MemberClassId;
                    cmd.Parameters.Add("@DateJoined", System.Data.SqlDbType.DateTime).Value = member.DateJoined;

                    string dateOfBirth = string.Format("{0:d}", member.DateOfBirth.Date);
                    cmd.Parameters.Add("@DateOfBirth", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(dateOfBirth, Properties.Settings.Default.CryptoKey);
                    cmd.Parameters.Add("@Salary", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Salary.ToString(), Properties.Settings.Default.CryptoKey);
                    cmd.Parameters.Add("@Gender", System.Data.SqlDbType.VarChar).Value = crypto.EncryptString(member.Gender.ToString(), Properties.Settings.Default.CryptoKey);

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
            }

            return memberUpdated;
        }

        public int UpdateMemberClasses()
        {
            List<MemberModel> members = SelectMembers();

            int classesUpdated = 0;

            foreach (var member in members)
            {
                
                member.MemberClassId = GetMemberClassId(member.Salary, member.DateJoined);
                
                if(UpdateMember(member))
                {
                    classesUpdated += 1;
                }
            }

            return classesUpdated;
        }
    }
}