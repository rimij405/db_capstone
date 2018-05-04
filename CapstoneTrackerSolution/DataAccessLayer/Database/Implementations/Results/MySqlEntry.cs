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

        //////
        // IEntry

        /// <summary>
        /// Relational table field.
        /// </summary>
        public string Field
        {
            get { return this.field; }
            set { this.field = value; }
        }

        /// <summary>
        /// Relational table value.
        /// </summary>
        public string Value
        {
            get { return this.value; }
            private set { this.value = value; }
        }

        /// <summary>
        /// Is this entry readonly?
        /// </summary>
        public bool IsReadOnly
        {
            get; set;
        }

        //////
        // INullable

        /// <summary>
        /// Check if the currently stored value is null.
        /// </summary>
        public bool IsNull
        {
            get { return this.Field.Length == 0 || this.HasValue(MySqlEntry.NULL_VALUE); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Construct a null entry using just a field value.
        /// </summary>
        /// <param name="field">Field.</param>
        public MySqlEntry(string field)
            : this(field, MySqlEntry.NULL_VALUE)
        {
            // Default set.
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
        /// Construct a full entry from input data.
        /// </summary>
        /// <param name="data">Data field and value pair.</param>
        public MySqlEntry(KeyValuePair<string, string> data)
        {
            // Set the field and value.
            this.SetData(data);
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="entry">Entry to copy from.</param>
        public MySqlEntry(IEntry entry)
            : this(entry.GetField(), entry.GetValue())
        {
            // Set the field and value based off of the clone.
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Return a formatted string describing a single entry.
        /// </summary>
        /// <returns>Returns formatted string.</returns>
        public override string ToString()
        {
            return $"MySqlEntry {{Field: {this.Field}, Value: {this.Value}, IsNull: {this.IsNull}}}";
        }

        //////
        // IComparable
                    
        /// <summary>
        /// Compare MySqlEntry to another type of <see cref="IEntry"/> by it's value.
        /// </summary>
        /// <param name="obj">Entry to compare with.</param>
        /// <returns>Returns integer declaring sort order in relation to the object passed in.</returns>
        public int CompareTo(object obj)
        {
            if (obj != null && obj is IEntry entry)
            {

                // If the same, return 0.
                if (this.Equals(obj)) { return 0; }

                // If not the same, find the difference.
                // If the fields are the same:
                if(this.Field == entry.Field)
                {
                    // Compare values.
                    if (this.Value == entry.Value)
                    {
                        // Is same.
                        return 0;
                    }
                    else
                    {
                        return this.Value.CompareTo(entry.Value);
                    }
                }
                else
                {
                    return this.Field.CompareTo(entry.Field);
                }
            }

            // If not of a compatible type, return nothing.
            throw new ArgumentException("Input object is not of type IEntry.");
        }

        /// <summary>
        /// Checks equality between entries.
        /// </summary>
        /// <param name="obj">Object to check equality with.</param>
        /// <returns>Returns true if equal and of same IEntry type. Returns false, if otherwise.</returns>
        public override bool Equals(object obj)
        {
            return ((obj is IEntry entry) && (this.Field == entry.Field) && (this.Value == entry.Value));
        }

        /// <summary>
        /// Gets the entry hashcode.
        /// </summary>
        /// <returns>Returns hashcode.</returns>
        public override int GetHashCode()
        {
            return this.Field.GetHashCode() ^ this.Value.GetHashCode();
        }

        //////
        // IFieldNameValidator

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
        /// Returns a trimmed, all caps string, based off of input.
        /// </summary>
        /// <param name="input">Input to capitalize and trim of whitespace.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatField(string input)
        {
            if (input.Length <= 0) { return ""; }
            return input.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if input is not empty.
        /// </summary>
        /// <param name="input">Input to check.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValidField(string input)
        {
            if (input.Length <= 0) { return false; }
            return true;
        }

        //////
        // IEntry

        /// <summary>
        /// Check if the entry's value matches the input. Case sensitive. Whitespace not truncated.
        /// </summary>
        /// <param name="value">Value to check against.</param>
        /// <returns>Returns true if match found.</returns>
        public bool HasValue(string value)
        {
            return (this.Value == value);
        }
        
        //////
        // IReplicate

        /// <summary>
        /// Clone the entry and return the copy.
        /// </summary>
        /// <returns>Return clone of self.</returns>
        public IReplicate Clone()
        {
            return new MySqlEntry(this);
        }

        //////
        // INullable

        /// <summary>
        /// Sets the value to null.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public INullable MakeNull()
        {
            this.SetValue(MySqlEntry.NULL_VALUE);
            return this;
        }

        //////////////////////
        // Mutator(s).

        //////
        // IEntry

        /// <summary>
        /// Set the field.
        /// </summary>
        /// <param name="field">Field to set.</param>
        /// <returns>Return reference to self.</returns>
        public IEntry SetField(string field)
        {
            // Cannot set when readonly.
            if (this.IsReadOnly) { return this; }
            this.Field = this.FormatField(field);
            return this;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IEntry SetValue(string value)
        {
            // Cannot set when readonly.
            if (this.IsReadOnly) { return this; }
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

        //////
        // IEntry

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
