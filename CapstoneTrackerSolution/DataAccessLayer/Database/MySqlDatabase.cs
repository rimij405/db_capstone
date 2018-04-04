/*
    MySqlDatabase.cs
    Database connector.
    ***
    04/03/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Connect to a MySqlDatabase.
    /// </summary>
    public class MySqlDatabase : IDatabase
    {

        // Fields.

        private MySqlConnection connection = null;
        private bool isConnected = false;
        private bool isOpened = false;

        // Constructor(s).

        public MySqlDatabase(MySqlConfiguration _config)
        {
            this.Configure(_config);
        }

        public bool Close()
        {
            try
            {
                connection.Close();
            }
            catch(MySqlException E)
            {
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// Configures the database connection string.
        /// </summary>
        /// <param name="configuration">A configuration object that will create the connection string.</param>
        /// <returns>Returns reference to this <see cref="IDatabase"/> object.</returns>
        public MySqlDatabase Configure(MySqlConfiguration configuration)
        {
            this.connection = configuration.CreateConnection();
            return this;
        }

        public bool Connect()
        {
            try
            {
                connection.Open();
            }
            catch (MySqlException E)
            {
                return false;
            }
            return true;
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }

        public List<List<string>> GetData(string sql)
        {
            List<List<string>> ary = new List<List<string>>();
            if (Connect())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int row = 0;
                    while (rdr.Read())
                    {
                        ary.Add(new List<string>()); //add a new row
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            ary[row].Add(rdr.GetValue(i).ToString()); // add a new column
                        }
                        row++;
                    }
                }
                catch (MySqlException E)
                {
                    return null;
                }
            }
            Close();
            return ary;

        }

        // Open the database, execute a non-query, and close the database
        public int SetData(string sql)
        {
            int rc = -1; // row changed
            if (Connect())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    rc = cmd.ExecuteNonQuery();
                }
                catch (MySqlException E)
                {
                    return -1;
                }
            }
            Close();
            return rc;
        }
    }
}
