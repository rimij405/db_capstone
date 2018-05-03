/*
    MySqlParameters.cs
    ---
    Ian Effendi
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using statements.
using ISTE.DAL.Database.Interfaces;
using MySql.Data.MySqlClient;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Parameters to be used with a MySqlDatabase.
    /// </summary>
    public class MySqlParameters : IParameters
    {
        //////////////////////
        // Static Method(s).
        //////////////////////

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Convert object from a parameter collection object.
        /// </summary>
        /// <param name="other">Object to cast.</param>
        public static explicit operator MySqlParameters(MySqlParameterCollection other)
        {
            MySqlParameters p = new MySqlParameters(other);
            return p;
        }

        /// <summary>
        /// Convert object from a <see cref="System.Collections.Generic.Dictionary{String, String}"/> collection object.
        /// </summary>
        /// <param name="other">Object to cast.</param>
        public static implicit operator MySqlParameters(Dictionary<string, string> other)
        {
            return new MySqlParameters(other);
        }

        /// <summary>
        /// Convert object to a <see cref="List{MySqlParameter}"/>
        /// </summary>
        /// <param name="other">Object to convert.</param>
        public static explicit operator List<MySqlParameter>(MySqlParameters other)
        {
            List<MySqlParameter> list = new List<MySqlParameter>();
            foreach (KeyValuePair<string, string> pair in other)
            {
                list.Add(new MySqlParameter(pair.Key, pair.Value));
            }
            return list;
        }

        /// <summary>
        /// Convert object to a <see cref="Dictionary{String, String}"/> object.
        /// </summary>
        /// <param name="other">Object to cast.</param>
        public static explicit operator Dictionary<string, string>(MySqlParameters other)
        {
            return (other != null) ? (Dictionary<string, string>)other.Dictionary : new Dictionary<string, string>();
        }

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Collection of dictionary terms.
        /// </summary>
        public IDictionary<string, string> internalCollection;

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Check if the parameters in the dictionary have values assigned to them.
        /// </summary>
        public bool HasValues {            
            get {
                // Check if the collection is null.
                if(this.internalCollection == null) { return false; }

                // Check if the collection size is greater than zero.
                if(this.Dictionary.Count == 0) { return false; }

                // Check if at least one value has a not-null value.
                foreach (string value in this.Dictionary.Values)
                {
                    if(value != null) { return true; }
                }

                // Return false if none of the above conditions have been met.
                return false;
            }
        }

        /// <summary>
        /// Check if the object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get { return !this.HasValues; }
        }

        /// <summary>
        /// Reference to internal key/value map.
        /// </summary>
        public IDictionary<string, string> Dictionary {
            get {
                // Check if reference is null.
                return this.internalCollection ?? (this.internalCollection = new Dictionary<string, string>());
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Collection of parameters to prepare a MySqlCommand with.
        /// </summary>
        public MySqlParameters()
        {

        }

        /// <summary>
        /// Create collection of parameters from an existing map.
        /// </summary>
        /// <param name="collection">Map storing collection values.</param>
        public MySqlParameters(IDictionary<string, string> collection)
        {
            // Validate input.
            if(collection != null && collection.Count > 0)
            {
                // Only add keys that begin with the '@' character.
                foreach(KeyValuePair<string, string> pair in collection)
                {
                    if(this.IsValidName(pair.Key))
                    {
                        this.AddWithValue(pair);
                    }
                }
            }
        }

        /// <summary>
        /// Create an object from an existing collection of MySqlParameters.
        /// </summary>
        /// <param name="parameters">Parameter collection.</param>
        public MySqlParameters(IList<MySqlParameter> parameters)
        {
            // Validate input.
            if (parameters != null && parameters.Count > 0)
            {
                // Add keys.
                foreach (MySqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        this.Add(parameter);
                    }
                }
            }
        }

        /// <summary>
        /// Creeate an object from an existing collection of <see cref="MySqlParameter"/>s.
        /// </summary>
        /// <param name="parameters"></param>
        public MySqlParameters(MySqlParameterCollection parameters)
        {
            // Validate input.
            if (parameters != null && parameters.Count > 0)
            {
                // Add keys.
                foreach (MySqlParameter parameter in parameters)
                {
                    if (parameter != null)
                    {
                        this.Add(parameter);
                    }
                }
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Display the parameters.
        /// </summary>
        /// <returns>String with metadata.</returns>
        public override string ToString()
        {
            return this.Display();
        }

        /// <summary>
        /// Get string representing list of key/value pairs.
        /// </summary>
        /// <returns>Returns parameters as a series of strings.</returns>
        public string Display(char separator = '\n')
        {
            string display = "";
            if (this.IsEmpty)
            {
                display = "This object has no values.";
            }
            else
            {
                // Enumerate over the parameters in this collection.
                int count = 0;
                foreach (KeyValuePair<string, string> pair in this)
                {
                    count++;
                    display += $"{pair.Key}: {pair.Value}";
                    if (count != this.Dictionary.Count)
                    {
                        display += separator;
                    }
                }
            }
            return display;
        }


        /// <summary>
        /// Check if the input name has no whitespace, is not null, is not empty, and contains a '@' character in the first position.
        /// </summary>
        /// <param name="key">Parameter name.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValidName(string key)
        {
            if (String.IsNullOrEmpty(key)) { return false; } // Not null / empty.
            if (key.Length == 0 || key.Contains(' ') || key.Contains('\n')) { return false; } // No whitespace.
            return (key[0] == '@');
        }

        /// <summary>
        /// Check if the parameter name is a key in the dictionary.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns true if valid.</returns>
        public bool Contains(string parameterName)
        {
            return this.IsValidName(parameterName) && this.Dictionary.ContainsKey(parameterName);
        }
        
        //////////////////////
        // Accessors.

        /// <summary>
        /// Get access to the dictionary's enumerator.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        /// <summary>
        /// Return the enumerator.
        /// </summary>
        /// <returns>Enumerator to return when cast as an IEnumerable.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.Dictionary.GetEnumerator();
        }

        //////////////////////
        // Mutators.		

        /// <summary>
        /// Remove a parameter from the collection.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Return value associated with the removed parameter. Returns an empty string if it doesn't exist.</returns>
        public string RemoveParameter(string parameterName)
        {
            if (this.Contains(parameterName))
            {
                string result = this.GetValue(parameterName);
                return (this.Dictionary.Remove(parameterName)) ? result : "";
            }
            return "";
        }

        /// <summary>
        /// Get parameter as a MySqlParameter object.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns parameter as a MySqlParameter object.</returns>
        public MySqlParameter GetParameter(string parameterName)
        {
            if (this.Contains(parameterName))
            {
                return new MySqlParameter(parameterName, this.GetValue(parameterName));
            }
            return null;
        }

        /// <summary>
        /// Return a value associated with a parameter name.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns value if it exists. Will return an empty string if it doesn't exist.</returns>
        public string GetValue(string parameterName)
        {
            if (this.Contains(parameterName))
            {
                return this.Dictionary[parameterName];
            }
            return "";
        }

        /// <summary>
        /// Attempts to set a parameter value in the collection. If it already exists, item will be overwritten.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <returns>Returns false if the parameter couldn't be found.</returns>
        public bool SetValue(string parameterName, string value)
        {
            if (this.Contains(parameterName))
            {
                this.Dictionary[parameterName] = value;
            }
            return false;
        }

        /// <summary>
        /// Attempts to set a parameter value in the collection. If it already exists, item will be overwritten.
        /// </summary>
        /// <param name="parameter">Parameter name and value pair.</param>
        /// <returns>Returns false if the parameter didn't exist before.</returns>
        public bool SetValue(KeyValuePair<string, string> parameter)
        {
            return this.SetValue(parameter.Key, parameter.Value);   
        }

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <param name="value">Value.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        public bool AddWithValue(string parameterName, string value)
        {
            if (this.Add(parameterName))
            {
                this.Dictionary[parameterName] = value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameter">Parameter name and value pair.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        public bool AddWithValue(KeyValuePair<string, string> parameter)
        {
            return this.AddWithValue(parameter.Key, parameter.Value);
        }

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameterName">Parameter name.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        public bool Add(string parameterName)
        {
            if (!this.Contains(parameterName))
            {
                this.Dictionary.Add(parameterName, null);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Attempts to add a parameter to the collection. If it already exists, nothing will happen.
        /// </summary>
        /// <param name="parameters">Parameter name.</param>
        /// <returns>Returns true if the parameter didn't exist before.</returns>
        public bool Add(MySqlParameter parameters)
        {
            return this.AddWithValue(parameters.ParameterName, parameters.Value.ToString());
        }
    }
}
