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
            throw new NotImplementedException();
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
            // TODO: Implement this.
            // TODO: Implement the exceptions.

            throw new NotImplementedException();
        }

        public bool IsConnected()
        {
            throw new NotImplementedException();
        }
    }
}
