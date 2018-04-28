/*
    MySqlConfiguration.cs
    Configuration implementation.
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
    /// MySqlConfiguration class.
    /// </summary>
    public class MySqlConfiguration : IConfiguration
    {

        // Fields.

        /// <summary>
        /// Server URI address.
        /// </summary>
        private string server;

        /// <summary>
        /// Database title.
        /// </summary>
        private string database;

        /// <summary>
        /// Database username.
        /// </summary>
        private string username;

        /// <summary>
        /// Database password.
        /// </summary>
        private string password;

        // Constructor(s).

        /// <summary>
        /// Create a configuration with the default values.
        /// </summary>
        public MySqlConfiguration() : this("127.0.0.1", "capstonedb", "root", "student")
        {
            // Empty constructor calls the constructor below.
        }

        /// <summary>
        /// Constructs a configuration file, using the input values.
        /// </summary>
        /// <param name="_server">Server URI address.</param>
        /// <param name="_database">Database name.</param>
        /// <param name="_username">Username for authentication to the database.</param>
        /// <param name="_password">Password for authentication to the database.</param>
        public MySqlConfiguration(string _server, string _database, string _username, string _password)
        {
            this.SetURI(_server).SetUsername(_username).SetPassword(_password).SetDatabase(_database);
        }

        // Service Method(s).

        /// <summary>
        /// Creates a connection object and returns it.
        /// </summary>
        /// <returns>Returns a MySqlConnection object.</returns>
        public MySqlConnection CreateConnection()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(this.GetConnectionString());
                return conn;
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectionException("Error occured when creating the connection object.", e);
            }
        }
        
        /// <summary>
        /// Returns a formatted connection string.
        /// </summary>
        /// <returns>String formatted connection info.</returns>
        public string GetConnectionString()
        {
            return "server=" + this.GetURI() + ";"
                 + "uid=" + this.GetUsername() + ";"
                 + "pwd=" + this.GetPassword() + ";"
                 + "database=" + this.GetDatabase() + ";";
        }

        /// <summary>
        /// Formats the configuration out as a string.
        /// </summary>
        /// <returns>String, formatted.</returns>
        public override string ToString()
        {
            return "[MySqlConfiguration]: '" + this.GetConnectionString() + "'";
        }

        // Accessor Method(s).

        /// <summary>
        /// Return the URI for the server.
        /// </summary>
        /// <returns>Returns the address of the server.</returns>
        public string GetURI()
        {
            return this.server;
        }

        /// <summary>
        /// Retrieves the database name.
        /// </summary>
        /// <returns>Returns the name of the database.</returns>
        public string GetDatabase()
        {
            return this.database;
        }

        /// <summary>
        /// Retrieves the stored username.
        /// </summary>
        /// <returns>Returns the username.</returns>
        public string GetUsername()
        {
            return this.username;
        }

        /// <summary>
        /// Retrieves the password.
        /// </summary>
        /// <returns>Returns the password.</returns>
        public string GetPassword()
        {
            return this.password;
        }

        // Mutator Method(s).
        
        /// <summary>
        /// Takes an input address string and stores it.
        /// </summary>
        /// <param name="_address">Address to set server to.</param>
        /// <returns>Returns reference to this instance of the configuration file.</returns>
        public IConfiguration SetURI(string _address)
        {
            this.server = _address;
            return this;
        }

        /// <summary>
        /// Takes an input database string and stores it.
        /// </summary>
        /// <param name="_database">Title to set database string to.</param>
        /// <returns>Returns reference to this instance of the configuration file.</returns>
        public IConfiguration SetDatabase(string _database)
        {
            this.database = _database;
            return this;
        }

        /// <summary>
        /// Takes an input username string and stores it.
        /// </summary>
        /// <param name="_username">Name to set the username string to.</param>
        /// <returns>Returns reference to this instance of the configuration file.</returns>
        public IConfiguration SetUsername(string _username)
        {
            this.username = _username;
            return this;
        }

        /// <summary>
        /// Takes an input password string and stores it.
        /// </summary>
        /// <param name="_password">String to set the password string to.</param>
        /// <returns>Returns reference to this instance of the configuration file.</returns>
        public IConfiguration SetPassword(string _password)
        {
            this.password = _password;
            return this;
        }

    }
}
