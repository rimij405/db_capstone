/**********************************************
 *  MySqlEntry.cs
 *  Ian Effendi
 *  ---
 *  Contains the implementation for IEntry.
 *********************************************/

// using statements.
using System;
using System.Collections.Generic;

// additional using statements.
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database.Implementations
{
    /// <summary>
    /// Represents a field-value pair to be returned from a MySqlDatabase class via a MySqlResultSet.
    /// </summary>
    public class MySqlEntry : FieldHandler, IEntry
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        #region Static Members.

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Create and return an entry.
        /// </summary>
        /// <param name="field">Field of new entry.</param>
        /// <param name="value">Value of new entry.</param>
        /// <returns>Returns a new entry.</returns>
        public static IEntry CreateEntry(string field, string value)
        {
            return new MySqlEntry(field, value);
        }

        /// <summary>
        /// Create and return an entry.
        /// </summary>
        /// <param name="data">Field/value of new entry.</param>
        /// <returns>Returns a new entry.</returns>
        public static IEntry CreateEntry(KeyValuePair<string, string> data) { return MySqlEntry.CreateEntry(data.Key, data.Value); }

        /// <summary>
        /// Create set of entries.
        /// </summary>
        /// <param name="entryData">Data collection.</param>
        /// <returns>Return collection of entries.</returns>
        public static List<IEntry> CreateEntries(IDictionary<string, string> entryData)
        {
            List<IEntry> entries = new List<IEntry>();
            foreach (KeyValuePair<string, string> datum in entryData)
            {
                entries.Add(MySqlEntry.CreateEntry(datum));
            }
            return entries;
        }
        
        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Cast into a MySqlEntry object.
        /// </summary>
        /// <param name="data">Data pair to cast.</param>
        public static implicit operator MySqlEntry(KeyValuePair<string, string> data)
        {
            return new MySqlEntry(data);
        }

        /// <summary>
        /// Cast into a pair.
        /// </summary>
        /// <param name="entry">Entry to cast.</param>
        public static explicit operator KeyValuePair<string, string>(MySqlEntry entry)
        {
            return entry.Data;
        }
        
        #endregion

        //////////////////////
        // Field(s).
        //////////////////////

        #region Fields.

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

        #endregion

        //////////////////////
        // Properties.
        //////////////////////

        #region Properties.

        //////
        // IEntry
        
        /// <summary>
        /// Field/value pair.
        /// </summary>
        public KeyValuePair<string, string> Data
        {
            get { return this.GetData(); }
            set { this.SetData(value); }
        }

        /// <summary>
        /// Relational table field.
        /// </summary>
        public string Field
        {
            get { return this.GetField(); }
            set { this.SetField(value); }
        }

        /// <summary>
        /// Relational table value.
        /// </summary>
        public string Value
        {
            get { return this.GetValue(); }
            private set { this.SetValue(value); }
        }

        /// <summary>
        /// Is this entry readonly?
        /// </summary>
        public bool IsReadOnly { get; set; } = false; // Auto-implemented as false.

        //////
        // Indexer(s).

        /// <summary>
        /// Return field or value based on input of 0 (field) or 1 (value).
        /// </summary>
        /// <param name="index">Accessor.</param>
        /// <returns>Returns string containing value.</returns>
        public string this[int index]
        {
            get
            {
                if(index == 0) { return this.Field; }
                if(index == 1) { return this.Value; }
                throw new IndexOutOfRangeException($"Index value of {index} references position that is out of range for entry: (0-1).");
            }
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

        #endregion

        //////////////////////
        // Constructor(s).
        //////////////////////

        #region Constructors.

        /// <summary>
        /// Construct a null entry using just a field value.
        /// </summary>
        /// <param name="field">Field.</param>
        public MySqlEntry(string field)
            : this(field, MySqlEntry.NULL_VALUE)
        {
            this.IsReadOnly = false;
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
            this.IsReadOnly = false;
        }

        /// <summary>
        /// Construct a full entry from input data.
        /// </summary>
        /// <param name="data">Data field and value pair.</param>
        public MySqlEntry(KeyValuePair<string, string> data)
        {
            // Set the field and value.
            this.SetData(data);
            this.IsReadOnly = false;
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="entry">Entry to copy from.</param>
        public MySqlEntry(IEntry entry)
            : this(entry.GetField(), entry.GetValue())
        {
            // Set the field and value based off of the clone.
            this.IsReadOnly = false;
        }

        #endregion

        //////////////////////
        // Method(s).
        //////////////////////

        #region Methods.

        //////////////////////
        // Service(s).

        #region IEntry

        /// <summary>
        /// Return a formatted string describing a single entry.
        /// </summary>
        /// <returns>Returns formatted string.</returns>
        public override string ToString()
        {
            return $"MySqlEntry {{Field: {this.Field}, Value: {this.Value}, IsNull: {this.IsNull}}}";
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

        #endregion

        #region IEntryComparison

        #region Field Comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareFields(IEntry left, IEntry right)
        {
            if(left == null && right == null) { return 0; }
            if(left == null) { return -1; }
            if(right == null) { return 1; }
            if(this.IsEqualFields(left, right)) { return 0; }
            if(this.IsGreaterThanField(left, right)) { return 1; }
            if(this.IsLessThanField(left, right)) { return -1; }
            return 1;
        }

        /// <summary>
        /// Compare two fields.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualFields(IEntry left, IEntry right)
        {
            return (left.Field.CompareTo(right.Field) == 0);
        }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanField(IEntry left, IEntry right)
        {
            return (left.Field.CompareTo(right.Field) >= 1);
        }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanField(IEntry left, IEntry right)
        {
            return (left.Field.CompareTo(right.Field) <= -1);
        }

        #endregion
        
        #region Value<T> Comparisons.

        /// <summary>
        /// Compare values of entries.
        /// </summary>
        /// <typeparam name="T">Type of entry being passed in. (Could be a derived class).</typeparam>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns result of comparison.</returns>
        public int CompareValue<T>(T left, T right) where T : IComparable
        {
            if(left == null && right == null) { return 0; }
            if(left == null) { return -1; }
            if(right == null) { return 1; }

            if (left is IEntry leftEntry && right is IEntry rightEntry)
            {
                return this.CompareValue(leftEntry, rightEntry);
            }

            if (left is IEntry) { return 1; }
            if (right is IEntry) { return -1; }
            return left.CompareTo(right);
        }

        /// <summary>
        /// Compare values of entries.
        /// </summary>
        /// <typeparam name="T">Type of entry being passed in. (Could be a derived class).</typeparam>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns result of comparison.</returns>
        public bool IsEqualValue<T>(T left, T right) where T : IComparable
        {
            if (left is IEntry leftEntry && right is IEntry rightEntry)
            {
                return this.IsEqualValue(leftEntry, rightEntry);
            }
            return false;
        }

        /// <summary>
        /// Compare values of entries.
        /// </summary>
        /// <typeparam name="T">Type of entry being passed in. (Could be a derived class).</typeparam>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns result of comparison.</returns>
        public bool IsGreaterThanValue<T>(T left, T right) where T : IComparable
        {
            if (left is IEntry leftEntry && right is IEntry rightEntry)
            {
                return this.IsGreaterThanValue(leftEntry, rightEntry);
            }
            return true;
        }

        /// <summary>
        /// Compare values of entries.
        /// </summary>
        /// <typeparam name="T">Type of entry being passed in. (Could be a derived class).</typeparam>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns result of comparison.</returns>
        public bool IsLessThanValue<T>(T left, T right) where T : IComparable
        {
            if (left is IEntry leftEntry && right is IEntry rightEntry)
            {
                return this.IsLessThanValue(leftEntry, rightEntry);
            }
            return false;
        }

        #endregion

        #region Value Comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareValue(IEntry left, IEntry right)
        {
            int comparison = 1;
            comparison = (this.IsGreaterThanValue(left, right)) ? 1 : comparison;
            comparison = (this.IsLessThanValue(left, right)) ? -1 : comparison;
            comparison = (this.IsEqualValue(left, right)) ? 0 : comparison;
            return comparison;
        }

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualValue(IEntry left, IEntry right)
        {
            return (left.Value.CompareTo(right.Value) == 0);
        }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanValue(IEntry left, IEntry right)
        {
            return (left.Value.CompareTo(right.Value) >= 1);
        }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanValue(IEntry left, IEntry right)
        {
            return (left.Value.CompareTo(right.Value) <= -1);
        }

        #endregion

        #region Total Comparisons.
               
        /// <summary>
        /// Check instances.
        /// </summary>
        /// <param name="obj">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public int CompareTo(object obj)
        {
            if(obj == null) { return 1; }
            if(this == obj) { return 0; }
            if (obj is IEntry other)
            {
                return this.Compare(other);
            }
            return 1;
        }

        /// <summary>
        /// Check instances.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public int Compare(IEntry other)
        {
            if (other == null) { return 1; }
            if (this == other) { return 0; }
            int fieldComparison = this.CompareFields(this, other);
            int valueComparison = this.CompareValue(this, other);
            return (fieldComparison == 0) ? valueComparison : fieldComparison;
        }

        /// <summary>
        /// Check instances for equality.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public bool IsEqual(IEntry other)
        {
            if(other == null) { return false; }
            if(this == other) { return true; }
            return (this.IsEqualFields(this, other) && this.IsEqualValue(this, other));
        }

        #endregion

        #endregion

        #region FieldHandler

        /// <summary>
        /// Check if this matches the input field. Case insensitive. Whitespace trimmed.
        /// </summary>
        /// <param name="key">Field to check against.</param>
        /// <returns>Returns true if match found.</returns>
        public override bool HasField(string key)
        {
            return this.IsValidField(key) && (this.FormatField(key) == this.Field);
        }

        #endregion
                
        #region IReplicate

        //////
        // IReplicate

        /// <summary>
        /// Clone the entry and return the copy.
        /// </summary>
        /// <returns>Return clone of self.</returns>
        public IEntry Clone()
        {
            return new MySqlEntry(this);
        }

        /// <summary>
        /// Clone the input object, if (and only if), it's an entry.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public object Clone(object obj)
        {
            if (obj is IEntry objectToClone)
            {
                return objectToClone.Clone();
            }
            return null;
        }

        #endregion

        #region INullable

        //////
        // INullable

        /// <summary>
        /// Sets the value to null.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public IEntry MakeNull()
        {
            this.SetValue(MySqlEntry.NULL_VALUE);
            return this;
        }

        #endregion

        //////////////////////
        // Accessor(s).

        #region Accessors.

        //////
        // IEntry

        /// <summary>
        /// Return the field value.
        /// </summary>
        /// <returns>Return a string.</returns>
        public string GetField()
        {
            return this.field;
        }

        /// <summary>
        /// Return the value as a string.
        /// </summary>
        /// <returns>Return a string.</returns>
        public string GetValue()
        {
            return this.value;
        }

        /// <summary>
        /// Create and return a key value pair linking the field to the value.
        /// </summary>
        /// <returns>Return key value pair object.</returns>
        public KeyValuePair<string, string> GetData()
        {
            return new KeyValuePair<string, string>(this.GetField(), this.GetValue());
        }

        #endregion

        //////////////////////
        // Mutator(s).

        #region Mutators.

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
            this.field = this.FormatField(field);
            return this;
        }

        /// <summary>
        /// Attempt to set field.
        /// </summary>
        /// <param name="field">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        public bool TrySetField(string field)
        {
            if (this.IsReadOnly) { return false; }
            this.SetField(field);
            return true;
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
            this.value = value;
            return this;
        }

        /// <summary>
        /// Attempt to set value.
        /// </summary>
        /// <param name="value">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        public bool TrySetValue(string value)
        {
            if (this.IsReadOnly) { return false; }
            this.SetValue(value);
            return true;
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

        /// <summary>
        /// Attempt to set data.
        /// </summary>
        /// <param name="data">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        public bool TrySetData(KeyValuePair<string, string> data)
        {
            if (this.IsReadOnly) { return false; }
            return this.TrySetField(data.Key) && this.TrySetValue(data.Value);
        }

        #endregion

        #endregion

    }
        
    /// <summary>
    /// Represents a primary key entry.
    /// </summary>
    public class MySqlPrimaryKeyEntry : MySqlEntry, IPrimaryKey
    {

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Primary key flag.
        /// </summary>
        private bool pkeyStatus;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Primary key flag.
        /// </summary>
        public bool PrimaryKey
        {
            get { return this.pkeyStatus; }
            set { this.pkeyStatus = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a primary key.
        /// </summary>
        /// <param name="isPKey">Primary key flag.</param>
        /// <param name="field">Fieldname.</param>
        /// <param name="value">Value.</param>
        public MySqlPrimaryKeyEntry(bool isPKey, string field, string value)
            : base(field, value)
        {
            this.PrimaryKey = isPKey;
        }

        /// <summary>
        /// Create a primary key.
        /// </summary>
        /// <param name="isPKey">Primary key flag.</param>
        /// <param name="field">Fieldname.</param>
        public MySqlPrimaryKeyEntry(bool isPKey, string field)
            : base(field)
        {
            this.PrimaryKey = isPKey;
        }

        /// <summary>
        /// Create a primary key.
        /// </summary>
        /// <param name="isPKey">Primary key flag.</param>
        /// <param name="entry">Entry to get values from.</param>
        public MySqlPrimaryKeyEntry(bool isPKey, IEntry entry)
            : this(isPKey, entry.Field, entry.Value)
        {
        }

        /// <summary>
        /// Create a primary key.
        /// </summary>
        /// <param name="isPKey">Primary key flag.</param>
        /// <param name="data">Data to get values from.</param>
        public MySqlPrimaryKeyEntry(bool isPKey, KeyValuePair<string, string> data)
            : this(isPKey, data.Key, data.Value)
        {
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Format string to display entry.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"MySqlEntry {{PRIMARY KEY {this.PrimaryKey}, Field: {this.Field}, Value: {this.Value}, IsNull: {this.IsNull}}}"; ;
        }

        /// <summary>
        /// Toggle primary key status.
        /// </summary>
        public void TogglePrimaryKey()
        {
            this.PrimaryKey = !this.PrimaryKey;
        }

    }
}
