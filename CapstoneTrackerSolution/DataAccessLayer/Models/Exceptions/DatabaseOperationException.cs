/*
    DatabaseOperationException.cs
    ---
    Ian Effendi
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements.
using ISTE.DAL.Database;

namespace ISTE.DAL.Models
{

    /// <summary>
    /// Represents an error that can be returned by the database.
    /// </summary>
    public enum DatabaseError
    {
        /// <summary>
        /// No error.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Unknown error has been thrown.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// There was an error with the SQL command syntax.
        /// </summary>
        SYNTAX = 1,

        /// <summary>
        /// An insert command could not be resolved due to a duplicate unique field value.
        /// </summary>
        DUPLICATE = 2,

        /// <summary>
        /// A record doesn't exist to perform a fetch or update operation.
        /// </summary>
        MISSING_RECORD = 3,

        /// <summary>
        /// Not enough information to perform the operation.
        /// </summary>
        MISSING_DATA = 4
    }

    /// <summary>
    /// Custom exception class that is thrown during a database close connection failure.
    /// </summary>
    public class DatabaseOperationException : DataAccessLayerException
    {

        // Field(s).

        /// <summary>
        /// Error number.
        /// </summary>
        private DatabaseError code = DatabaseError.UNKNOWN;

        // Properties.

        /// <summary>
        /// Error number.
        /// </summary>
        public DatabaseError ErrorCode
        {
            get { return this.code; }
            private set { this.code = value; }
        }
        
        // Constructor(s).

        /// <summary>
        /// Creates exception with default name and message.
        /// </summary>
        /// <param name="errorCode">Error code ID.</param>
        public DatabaseOperationException(DatabaseError errorCode = DatabaseError.UNKNOWN) 
            : this(errorCode, "There was an error when attempting to perform the operation.")
        {
            // Calls constructor below.
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates exception with default name and message.
        /// </summary>
        /// <param name="errorCode">Error code ID.</param>
        public DatabaseOperationException(DatabaseError errorCode, string message)
            : base(message, $"Database Error {errorCode}")
        {
            // Calls the base constructor.
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates exception that wraps around input Exception.
        /// </summary>
        /// <param name="errorCode">Error code ID.</param>
        /// <param name="e">Exception to be wrapped.</param>
        public DatabaseOperationException(DatabaseError errorCode, Exception e) 
            : base(e, $"Database Error {errorCode}")
        {
            // Calls the base constructor.
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Creates exception that wraps around input Exception.
        /// </summary>
        /// <param name="e">Exception to be wrapped.</param>
        public DatabaseOperationException(Exception e) : base(e, $"Database Error {DatabaseError.UNKNOWN}")
        {
            // Calls the base constructor.
        }

        /// <summary>
        /// Creates exception with custom message and wrapped Exception.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <param name="e">Exception to be wrapped.</param>
        public DatabaseOperationException(string message, Exception e) : base(message, e, $"Database Error {DatabaseError.UNKNOWN}")
        {
            // Calls the base constructor.
        }

    }
}
