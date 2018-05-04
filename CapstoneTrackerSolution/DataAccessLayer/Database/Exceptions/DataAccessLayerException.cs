/*
    DataAccessLayerException.cs
    Allows exception class the opportunity to log results to a file. 
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
using Services;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Custom exception class that can be logged.
    /// </summary>
    public class DataAccessLayerException : Services.CustomException, Services.ILoggable
    {
        // Field(s).

        /// <summary>
        /// Name to print for this exception.
        /// </summary>
        private string name = "Data Access Layer Exception";

        // Constructor(s).

        /// <summary>
        /// Creates an exception using the default message.
        /// </summary>
        public DataAccessLayerException() : this("A data access layer exception was thrown.")
        {
            // Calls constructor below.
        }

        /// <summary>
        /// Creates an exception with only a message.
        /// </summary>
        /// <param name="message">Message to print.</param>
        public DataAccessLayerException(string message) : this(message, "Data Access Layer Exception")
        {
            // Calls constructor below.
        }

        /// <summary>
        /// Creates an exception with a custom name and only a message.
        /// </summary>
        /// <param name="name">Name to set exception to.</param>
        /// <param name="message">Message to print.</param>
        public DataAccessLayerException(string message, string name = "") : base(message)
        {
            // Calls base constructor.
            this.SetName(name);
            this.Write(Logger.Instance);
        }

        /// <summary>
        /// Creates an exception using an inner exception.
        /// </summary>
        /// <param name="name">Name to set exception to.</param>
        /// <param name="e">Exception to set as inner exception.</param>
        public DataAccessLayerException(Exception e, string name = "") : base(e.Message, e)
        {
            // Calls base constructor.
            this.SetName(name);
            this.Write(Logger.Instance);
        }

        /// <summary>
        /// Creates an exception with a custom message and an inner exception.
        /// </summary>
        /// <param name="name">Name to set exception to.</param>
        /// <param name="message">Message to print.</param>
        /// <param name="e">Exception to set as inner exception.</param>
        public DataAccessLayerException(string message, Exception e, string name = "") : base(message, e)
        {
            // Calls base constructor.
            this.SetName(name);
            this.Write(Logger.Instance);
        }

        /// <summary>
        /// Set the name of this exception.
        /// </summary>
        /// <param name="name">Name to be set to.</param>
        /// <returns>Returns self reference.</returns>
        public CustomException SetName(string name)
        {
            string value = name.Trim();
            this.name = (value.Length > 0) ? value : this.name;
            return this;
        }

        /// <summary>
        /// Returns the name of this exception class.
        /// </summary>
        /// <returns>Returns name of exception as a string.</returns>
        public override string GetExceptionName()
        {
            return this.name;
        }

        /// <summary>
        /// Writes exception messages to the log.
        /// </summary>
        /// <param name="log">Logger to use.</param>
        /// <returns>Returns reference to self.</returns>
        public ILoggable Write(Logger log)
        {
            string text = this.GetMessage().Trim();
            if(text.Length > 0) { log.Write(text); }
            return this;
        }
    }
}
