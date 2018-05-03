/****
 * IParameters.cs
 * ---
 * Ian Effendi
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using statements.
using MySql.Data.MySqlClient;

namespace ISTE.DAL.Database.Interfaces
{
    /// <summary>
    /// IParameters represents a set of SqlParameters that can be bound (and rebound) to interfaces.
    /// </summary>
    public interface IParameters : IEnumerable<KeyValuePair<string, string>>, IEmpty
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Determine if a parameters object has been given values.
        /// </summary>
        bool HasValues { get; }

        /// <summary>
        /// Return collection items as a dictionary.
        /// </summary>
        IDictionary<string, string> Dictionary { get; }
        
        //////////////////////
        // Methods.
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Checks if the parameter name that was input is valid.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns false if implementation deems the name invalid.</returns>
        bool IsValidName(string parameterName);

        /// <summary>
        /// Check if the parameter name is valid and is contained buy the internal collection.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns false if name doesn't exist in the collection or is invalid.</returns>
        bool Contains(string parameterName);
                
        //////////////////////
        // Accessors.

        /// <summary>
        /// Returns a value associated with a given parameter name. Null, means it doesn't exist.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Return value associated with given key.</returns>
        string GetValue(string parameterName);
        
        //////////////////////
        // Mutators.				

        /// <summary>
        /// Remove a parameter from the collection.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Return value associated with the removed parameter. Returns an empty string if it doesn't exist.</returns>
        string RemoveParameter(string parameterName);

        /// <summary>
        /// Attempts to set a parameter value in the collection. If it already exists, item will be overwritten.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <returns>Returns false if the parameter couldn't be found.</returns>
        bool SetValue(string parameterName, string value);

        /// <summary>
        /// Attempts to set a parameter value in the collection. If it already exists, item will be overwritten.
        /// </summary>
        /// <param name="parameter">Parameter name and value pair.</param>
        /// <returns>Returns false if the parameter didn't exist before.</returns>
        bool SetValue(KeyValuePair<string, string> parameter);

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        bool AddWithValue(string parameterName, string value);

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameter">Parameter name and value pair.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        bool AddWithValue(KeyValuePair<string, string> parameter);

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        bool Add(string parameterName);
        
    }
}
