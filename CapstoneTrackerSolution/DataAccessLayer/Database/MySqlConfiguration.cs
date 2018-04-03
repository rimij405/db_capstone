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
using MySql.Data.MySqlClient;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// MySqlConfiguration class.
    /// </summary>
    public class MySqlConfiguration : IConfiguration
    {

        // Fields.

        private string server;
        private string database;
        private string username;
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
        /// 
        /// </summary>
        /// <param name="_server"></param>
        /// <param name="_database"></param>
        /// <param name="_username"></param>
        /// <param name="_password"></param>
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
            return new MySqlConnection(this.GetConnectionString());
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

        public string GetURI()
        {
            return this.server;
        }

        public string GetDatabase()
        {
            return this.database;
        }

        public string GetUsername()
        {
            return this.username;
        }

        public string GetPassword()
        {
            return this.password;
        }

        // Mutator Method(s).
        
        public IConfiguration SetURI(string _address)
        {
            this.server = _address;
            return this;
        }

        public IConfiguration SetDatabase(string _database)
        {
            this.database = _database;
            return this;
        }

        public IConfiguration SetUsername(string _username)
        {
            this.username = _username;
            return this;
        }

        public IConfiguration SetPassword(string _password)
        {
            this.password = _password;
            return this;
        }

    }
}
