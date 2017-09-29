using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AusLogisticsLibrary.Models
{
    /// <summary>
    /// Project:    SIT322 Distributed Systems - Assignmnet 2
    /// Written By: Chris O'Beirne - Student #211347444
    /// Date:       24/04/16
    /// </summary>

    public class SqlDbModel : IDisposable
    {
        public string ConnectionString { get; set; }

        public SqlConnection Connection { get; set; }

        public SqlDbModel()
        { }

        public SqlDbModel(string dbFileName)
        {
            this.ConnectionString = string.Format(@"Data Source=(LocalDB)\v11.0; AttachDbFilename='|DataDirectory|\{0}'; Integrated Security=true", dbFileName);
        }

        public SqlDbModel(string databaseFolder, string dbFileName)
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, databaseFolder));
            this.ConnectionString = string.Format(@"Data Source=(LocalDB)\v11.0; AttachDbFilename='|DataDirectory|\{0}'; Integrated Security=true", dbFileName);
        }


        public void Open()
        {
            this.Connection = new SqlConnection(this.ConnectionString);
            this.Connection.Open();
        }

        public void Close()
        {
            this.Connection.Close();
        }


        public void Dispose()
        {
            if (this.Connection != null)
            {
                this.Connection.Close();
            }
        }
    }
}