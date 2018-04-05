/*
    CustomException.cs
    Allows for the creation of custom exceptions.
    ***
    04/03/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

 // Using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{

    /// <summary>
    /// Custom exception handling class that can be requested.
    /// </summary>
    public abstract class CustomException : Exception
    {

        // Constructor(s).

        /// <summary>
        /// Exception custom built with custom message.
        /// </summary>
        /// <param name="message">Message to give to a new exception.</param>
        public CustomException(string message) : base(message)
        {   
        }

        /// <summary>
        /// Exception constructor wraps an existing exception to handle with custom functionality.
        /// </summary>
        /// <param name="e">Exception to inherit.</param>
        public CustomException(Exception e) : base(e.Message, e)
        {
        }

        /// <summary>
        /// Exception constructor wraps an existing exception to handle with custom functionality.
        /// </summary>
        /// <param name="message">Exception message to print.</param>
        /// <param name="e">Exception to inherit.</param>
        public CustomException(string message, Exception e) : base(message + " (" + e.Message + ")", e)
        {
        }

        /// <summary>
        /// Return formatted exception message that is printed and logged.
        /// </summary>
        /// <returns>Returns the message.</returns>
        public string GetMessage()
        {
            string innerExceptionMessage = GetInnerExceptionMessage().Trim();

            return "[" + this.GetExceptionName() + "]: " 
                + this.Message 
                + ((innerExceptionMessage.Length <= 0) ? "" : ". " + innerExceptionMessage);
        }

        /// <summary>
        /// Returns inner exception message if the exception exists; returns an empty string if otherwise.
        /// </summary>
        /// <returns>Returns inner exception message.</returns>
        private string GetInnerExceptionMessage()
        {
            return (this.InnerException == null) ? "" : this.InnerException.Message;
        }

        /// <summary>
        /// Returns custom name for this exception.
        /// </summary>
        /// <returns>Returns name for this exception.</returns>
        public abstract string GetExceptionName();

    }
}
