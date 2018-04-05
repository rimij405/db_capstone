/*
    DatabaseConnectionException.cs
    Thrown when connection error occurs.
    ***
    04/04/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Custom exception class that is thrown during a database connection failure.
    /// </summary>
    public class DatabaseConnectionException : DataAccessLayerException
    {

        // Constructor(s).

        /// <summary>
        /// Creates exception with default name and message.
        /// </summary>
        public DatabaseConnectionException() : this("There was an error connecting to the database.", "Database Connection Error")
        {
            // Calls constructor below.
        }

        /// <summary>
        /// Creates exception with custom message and name.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <param name="name">Name of the exception.</param>
        public DatabaseConnectionException(string message, string name = "") : base(message, name)
        {
            // Calls base constructor.
        }

        /// <summary>
        /// Creates exception that wraps around input Exception.
        /// </summary>
        /// <param name="e">Exception to be wrapped.</param>
        public DatabaseConnectionException(Exception e) : base(e, "Database Connection Error")
        {
            // Calls the base constructor.
        }

        /// <summary>
        /// Creates exception with custom message and wrapped Exception.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <param name="e">Exception to be wrapped.</param>
        public DatabaseConnectionException(string message, Exception e) : base(message, e, "Database Connection Error")
        {
            // Calls the base constructor.
        }

    }
}
