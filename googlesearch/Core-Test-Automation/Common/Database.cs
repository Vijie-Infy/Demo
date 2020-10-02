using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Core_Test_Automation.Common
{
    public class Database
    {
        /// <summary>
        /// Runs query for a database and stores the result in a 2 dimensional array.
        /// </summary>
        /// <param name="connection">Connection parameters to access database.</param>
        /// <param name="query">Full query to execute.</param>
        public void RunQuery(string connection, string query)
        {
            // Establish DB connection.
            var DBConnection = new SqlConnection(connection);
            DBConnection.Open();

            // Execute query.
            var MemTable = new SqlCommand();
            MemTable.CommandText = query;
            MemTable.Connection = DBConnection;
            var dr = MemTable.ExecuteReader();
            var Memberid = 0;

            while (dr.Read())
            {
                Memberid = dr.GetInt32(0);
                Console.WriteLine(Memberid);
            }
        }
    }
}
