/*
    IConfiguration.cs
    Interface describing configurations for connecting to a database. 
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

namespace ISTE.DAL.Database.Interfaces
{
    /// <summary>
    /// Database connection configuration methods.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Set the URI of the server.
        /// </summary>
        /// <param name="_address">URI address to set the server URI to.</param>
        /// <returns>Returns reference to this <see cref="IConfiguration"/> object.</returns>
        IConfiguration SetURI(string _address);
        
        /// <summary>
        /// Set the database identity for the connection string.
        /// </summary>
        /// <param name="_database">Database to connect with.</param>
        /// <returns>Returns reference to this <see cref="IConfiguration"/> object.</returns>
        IConfiguration SetDatabase(string _database);

        /// <summary>
        /// Set the username of the connection string.
        /// </summary>
        /// <param name="_username">Username to connect with.</param>
        /// <returns>Returns reference to this <see cref="IConfiguration"/> object.</returns>
        IConfiguration SetUsername(string _username);

        /// <summary>
        /// Set the password of the connection string.
        /// </summary>
        /// <param name="_password">Password to connect with.</param>
        /// <returns>Returns reference to this <see cref="IConfiguration"/> object.</returns>
        IConfiguration SetPassword(string _password);

        /// <summary>
        /// Returns the URI.
        /// </summary>
        /// <returns>Value is returned as string.</returns>
        string GetURI();
        
        /// <summary>
        /// Returns the database.
        /// </summary>
        /// <returns>Value is returned as string.</returns>
        string GetDatabase();

        /// <summary>
        /// Returns the username.
        /// </summary>
        /// <returns>Value is returned as string.</returns>
        string GetUsername();

        /// <summary>
        /// Returns the password.
        /// </summary>
        /// <returns>Value is returned as string.</returns>
        string GetPassword();

        /// <summary>
        /// Returns the formatted connection string.
        /// </summary>
        /// <returns>Value is returned as string.</returns>
        string GetConnectionString();

    }
}
