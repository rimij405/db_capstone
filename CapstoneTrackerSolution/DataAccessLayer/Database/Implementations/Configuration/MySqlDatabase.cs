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

// using statements.
using MySql.Data.MySqlClient;
using ISTE.DAL.Database.Interfaces;
using Services;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Connect to a MySqlDatabase.
    /// </summary>
    public class MySqlDatabase : IDatabase, IReadable, IWritable
    {
        //////////////////////
        // Field(s).
        //////////////////////
        
        /// <summary>
        /// Connection object for connecting to the database.
        /// </summary>
        private MySqlConnection connection = null;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Checks the connection state to see if it's connected to the database.
        /// </summary>
        /// <returns>Returns true if connection is currently open.</returns>
        public bool IsConnected
        {
            get
            {
                if (this.connection == null) { return false; }
                if (this.connection.State == System.Data.ConnectionState.Closed) { return false; }
                if (this.connection.State == System.Data.ConnectionState.Open) { return true; }
                return false;
            }
        }
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Constructor, configured by a configuration object.
        /// </summary>
        /// <param name="_config">Configuration object.</param>
        public MySqlDatabase(MySqlConfiguration _config)
        {
            this.Configure(_config);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Configures the database connection string.
        /// </summary>
        /// <param name="configuration">A configuration object that will create the connection string.</param>
        /// <returns>Returns reference to this <see cref="IDatabase"/> object.</returns>
        public MySqlDatabase Configure(MySqlConfiguration configuration)
        {
            try
            {
                this.connection = configuration.CreateConnection();
                return this;
            }
            catch (DataAccessLayerException e)
            {
                // Pass any exception that was thrown up from the configuration method.
                throw new DatabaseConnectionException("Error occured in the connection configuration process.", e);
            }
        }

        /// <summary>
        /// Attempts to connect to the databse using the connection object.
        /// </summary>
        /// <returns>Returns true if successfully connected.</returns>
        public bool Connect()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectionException("Error occured in connection.", e);
            }
        }

        /// <summary>
        /// Attempt to close the connection.
        /// </summary>
        /// <returns>Returns true if operation was successful.</returns>
        public bool Close()
        {
            try
            {
                // Check if it needs to be closed at all.
                if (this.IsConnected)
                {
                    connection.Close();
                }

                // Database either was already closed or it closed successfully.
                return true;
            }
            catch(MySqlException e)
            {
                throw new DatabaseCloseException("Error occured when attempting to close the database.", e);
            }

            // Error.
            throw new DatabaseCloseException("Unreachable code triggered during database close operation.");
        }

        //////////////////////
        // Helpers.						

        /// <summary>
        /// Create a MySqlCommand that can be used.
        /// </summary>
        /// <param name="mysqlQuery">Query to initialize the command with.</param>
        /// <returns>Returns command object.</returns>
        private MySqlCommand CreateCommand(string mysqlQuery)
        {
            return new MySqlCommand(mysqlQuery, this.connection);
        }

        /// <summary>
        /// Create a MySqlCommand and prepare the statement. Applies parameters if any to apply.
        /// </summary>
        /// <param name="mysqlQuery">Query to prepare statement with.</param>
        /// <param name="parameters">Parameters to assign to the prepared statement.</param>
        /// <returns>Returns prepared command object.</returns>
        private MySqlCommand Prepare(string mysqlQuery, IDictionary<string, string> parameters = null)
        {
            MySqlCommand command = this.CreateCommand(mysqlQuery);
            command.Prepare();
            return Assign(command, parameters);
        }

        /// <summary>
        /// Assign parameters to a prepared statement.
        /// </summary>
        /// <param name="preparedStatement">Prepared statement.</param>
        /// <param name="parameters">Parameters to assign to the prepared statement.</param>
        /// <returns>Returns prepared command object.</returns>
        private MySqlCommand Assign(MySqlCommand preparedStatement, IDictionary<string, string> parameters = null)
        {
            if (preparedStatement.IsPrepared)
            {
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, string> pair in parameters)
                    {
                        if (preparedStatement.Parameters.Contains(pair.Key))
                        {
                            preparedStatement.Parameters[pair.Key].Value = pair.Value;
                        }
                        else
                        {
                            preparedStatement.Parameters.AddWithValue(pair.Key, pair.Value);
                        }
                    }
                }
            }
            else
            {
                throw new DataAccessLayerException("Cannot assign parameters to null or unprepared command statement.");
            }

            return preparedStatement;
        }
        
        //////////////////////
        // Mutators.				

        //////////////////////
        // Accessors.





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
