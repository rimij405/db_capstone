/**********************************************
 *  MySqlEntry.cs
 *  Ian Effendi
 *  ---
 *  Contains the implementation for IEntry.
 *********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Represents a field-value pair to be returned from a MySqlDatabase class via a MySqlResultSet.
    /// </summary>
    public class MySqlEntry : IEntry
    {

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// The null value accepted by MySql.
        /// </summary>
        public const string NULL_VALUE = "NULL";

        /// <summary>
        /// Field value. Stored as a trimmed, all-caps value.
        /// </summary>
        private string field;

        /// <summary>
        /// Value associated with the field.
        /// </summary>
        private string value;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Relational table field.
        /// </summary>
        public string Field
        {
            get { return this.field; }
            private set { this.field = value; }
        }

        /// <summary>
        /// Relational table value.
        /// </summary>
        public string Value
        {
            get { return this.value; }
            private set { this.value = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Construct a null entry using just a field value.
        /// </summary>
        /// <param name="field">Field.</param>
        public MySqlEntry(string field)
        {
            // Default set.
            this.SetData(field, MySqlEntry.NULL_VALUE);
        }

        /// <summary>
        /// Construct a full entry.
        /// </summary>
        /// <param name="field">Field.</param>
        /// <param name="value">Value.</param>
        public MySqlEntry(string field, string value)
        {
            // Set the field and value.
            this.SetData(field, value);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="entry">Entry to copy from.</param>
        public MySqlEntry(IEntry entry)
        {
            // Set the field and value based off of the clone.
            this.SetData(entry.GetField(), entry.GetValue());
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Clone the entry and return the copy.
        /// </summary>
        /// <returns>Return clone of self.</returns>
        public IEntry Clone()
        {
            return new MySqlEntry(this);
        }
        
        /// <summary>
        /// Check if this matches the input field. Case insensitive. Whitespace trimmed.
        /// </summary>
        /// <param name="key">Field to check against.</param>
        /// <returns>Returns true if match found.</returns>
        public bool HasField(string key)
        {
            return this.IsValidField(key) && (FormatField(key) == this.Field);
        }

        /// <summary>
        /// Check if the entry's value matches the input. Case sensitive. Whitespace not truncated.
        /// </summary>
        /// <param name="value">Value to check against.</param>
        /// <returns>Returns true if match found.</returns>
        public bool HasValue(string value)
        {
            return (this.Value == value);
        }

        /// <summary>
        /// Check if the currently stored value is null.
        /// </summary>
        /// <returns>Returns true if current value is "NULL".</returns>
        public bool IsNull()
        {
            return this.HasValue(MySqlEntry.NULL_VALUE);
        }

        //////////////////////
        // Helper(s).

        /// <summary>
        /// Returns a trimmed, all caps string, based off of input.
        /// </summary>
        /// <param name="input">Input to capitalize and trim of whitespace.</param>
        /// <returns>Returns formatted string.</returns>
        private string FormatField(string input)
        {
            if (input.Length <= 0) { return ""; }
            return input.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if input is not empty.
        /// </summary>
        /// <param name="input">Input to check.</param>
        /// <returns>Returns true if valid.</returns>
        private bool IsValidField(string input)
        {
            if (input.Length <= 0) { return false; }
            return true;
        }

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Set the field.
        /// </summary>
        /// <param name="field">Field to set.</param>
        /// <returns>Return reference to self.</returns>
        private IEntry SetField(string field)
        {
            if(!this.IsValidField(field)) { throw new DataAccessLayerException("The field data member cannot be empty."); }
            this.Field = this.FormatField(field);
            return this;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        private IEntry SetValue(string value)
        {
            this.Value = value;
            return this;
        }

        /// <summary>
        /// Set the field and value at the same time.
        /// </summary>
        /// <param name="field">Field value.</param>
        /// <param name="value">Value.</param>
        /// <returns>Return reference to self.</returns>
        public IEntry SetData(string field, string value)
        {
            this.SetField(field);
            this.SetValue(value);
            return this;
        }

        /// <summary>
        /// Set the field and value at the same time.
        /// </summary>
        /// <param name="fieldValuePair">Key/value pair containing field (key) and value (value).</param>
        /// <returns>Return reference to self.</returns>
        public IEntry SetData(KeyValuePair<string, string> fieldValuePair)
        {
            this.SetData(fieldValuePair.Key, fieldValuePair.Value);
            return this;
        }

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return the field value.
        /// </summary>
        /// <returns>Return a string.</returns>
        public string GetField()
        {
            return this.Field;
        }

        /// <summary>
        /// Return the value as a string.
        /// </summary>
        /// <returns>Return a string.</returns>
        public string GetValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Create and return a key value pair linking the field to the value.
        /// </summary>
        /// <returns>Return key value pair object.</returns>
        public KeyValuePair<string, string> GetData()
        {
            return new KeyValuePair<string, string>(this.Field, this.Value);
        }

    }
}
