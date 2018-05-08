/*
    Formats.cs
    ---
    Ian Effendi
 */

 // using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

// additional using statements.
using Services.Interfaces;
using ISTE.DAL.Database.Interfaces;


namespace ISTE.DAL.Database.Implementations
{
    /// <summary>
    /// Represents shared functionality between formats of a given type.
    /// </summary>
    /// <typeparam name="TData">Type in general.</typeparam>
    public abstract class MySqlData<TData> : IFormat<TData>, IComparison<TData>, IComparable where TData : IComparable
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Internal explicitly-typed value. The SQL value is derived from this value.
        /// </summary>
        private TData internalValue = default(TData);

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Data as its .NET compatible format.
        /// </summary>
        public TData Value
        {
            get { return this.GetValue(); }
            set { this.SetValue(value); }
        }

        /// <summary>
        /// Data as its equivalent SQL string.
        /// </summary>
        public string SQLValue
        {
            get { return this.GetSQL(); }
            set { this.SetSQL(value); }
        }

        /// <summary>
        /// Returns the default value.
        /// </summary>
        public TData Default
        {
            get { return this.GetDefault(); }
        }

        /// <summary>
        /// Check if this is set to the default value.
        /// </summary>
        public bool IsDefault
        {
            get
            {
                TData data = this.GetDefault();
                if(data == null) { return (this.Value == null); }
                else { return (this.IsEqualValue(data)); }
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Empty constructor for the class.
        /// </summary>
        private MySqlData()
        {
            // Empty SQL assignment.
            this.SetSQL("");
        }

        /// <summary>
        /// Create a data wrapper for the explicit data passed into the constructor.
        /// </summary>
        /// <param name="value">Value to set.</param>
        protected MySqlData(TData value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Create a data wrapper for the SQL data passed into the constructor.
        /// </summary>
        /// <param name="sqlValue">Value to set.</param>
        protected MySqlData(string sqlValue)
        {
            this.SetSQL(sqlValue);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).	

        #region Value type conversions.

        /// <summary>
        /// Parse a .NET compatible datatype value into its equivalent SQL version.
        /// </summary>
        /// <param name="value">.NET value to convert into its SQL equivalent.</param>
        /// <returns>Returns value as a string.</returns>
        public abstract string ConvertToSQL(TData value);

        /// <summary>
        /// Parse an SQL string representing data into its equivalent .NET compatible version.
        /// </summary>
        /// <param name="value">SQL string to convert into its .NET equivalent.</param>
        /// <returns>Returns value as the data format.</returns>
        public abstract TData Parse(string value);

        #endregion

        #region Class comparison methods.

        /// <summary>
        /// Check if an object is equal to this instance.
        /// </summary>
        /// <param name="obj">O</param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if(obj == null) { return false; }
            if(obj == this) { return true; }
            if (obj is MySqlData<TData> || obj is string || obj is TData)
            {
                return (this.CompareTo(obj) == 0);
            }

            // Not equal if nothing has been met.
            return false;
        }

        /// <summary>
        /// This object's hash code is the join of its value and SQL value.
        /// </summary>
        /// <returns>Returns a hash index.<returns>
        public override int GetHashCode()
        {
            return this.Value.GetHashCode() ^ this.SQLValue.GetHashCode();
        }

        /// <summary>
        /// Compare the stored value of an input object to this instance's stored value.
        /// </summary>
        /// <param name="obj">Object or value to compare.</param>
        /// <returns>Returns a comparison value.</returns>
        public virtual int CompareTo(object obj)
        {
            if (obj == null) { return 1; } // If null reference, this is greater.
            if (this == obj) { return 0; } // If same reference, this is equal to that.
            if (obj is MySqlData<TData> that)
            {
                // If object is the same type as this, compare by equality first.
                if (this.IsEqual(that)) { return 0; }

                // Then, if not equal, compare if greater or less than.
                if (this.IsGreaterThan(that)) { return 1; }
                if (this.IsLessThan(that)) { return -1; }
            }
            if (obj is TData value)
            {
                // If comparing direct values, compare for inequality index.
                return this.CompareValue(value);
            }
            if (obj is string sqlValue)
            {
                // If comparing SQL values, compare for inequality index.
                return this.CompareSQL(sqlValue);
            }
            // If no case is found, throw exception. This will take precedence over the other.
            throw new DataAccessLayerException("Not a comparable type.");
        }

        /// <summary>
        /// Check if other data is equal to this.
        /// </summary>
        /// <param name="other">Data to compare for equality.</param>
        /// <returns>Returns true if equal.</returns>
        public bool IsEqual(object other)
        {
            if(other == null) { return false; }
            if(this == other) { return true; }
            if (other is MySqlData<TData> that)
            {
                return (this.Value.CompareTo(that.Value) == 0);
            }
            if (other is TData value)
            {
                return (this.CompareValue(value) == 0);
            }
            if (other is string sqlValue)
            {
                return (this.CompareSQL(sqlValue) == 0);
            }
            return false;
        }

        /// <summary>
        /// Check if this instance's data is greater than the input data's value.
        /// </summary>
        /// <param name="other">Data to compare.</param>
        /// <returns>Returns true if this is greater than the data.</returns>
        public bool IsGreaterThan(object other)
        {
            if (other == null || this == other) { return false; }
            if (other is MySqlData<TData> that)
            {
                return (this.Value.CompareTo(that.Value) >= 1);
            }
            if (other is TData value)
            {
                return (this.CompareValue(value) >= 1);
            }
            if (other is string sqlValue)
            {
                return (this.CompareSQL(sqlValue) >= 1);
            }
            return false;
        }

        /// <summary>
        /// Check if this instance's data is less than the input data's value.
        /// </summary>
        /// <param name="other">Data to compare.</param>
        /// <returns>Returns true if this is less than the data.</returns>
        public bool IsLessThan(object other)
        {
            if (other == null || this == other) { return false; }
            if (other is MySqlData<TData> that)
            {
                return (this.Value.CompareTo(that.Value) <= -1);
            }
            if (other is TData value)
            {
                return (this.CompareValue(value) <= -1);
            }
            if (other is string sqlValue)
            {
                return (this.CompareSQL(sqlValue) <= -1);
            }
            return false;
        }

        #endregion

        //////////////////////
        // Helper method(s).	

        /// <summary>
        /// Returns the default value for this type. Can be overloaded.
        /// </summary>
        /// <returns>Returns default value.</returns>
        protected virtual TData GetDefault()
        {
            return default(TData);
        }

        #region Internal value comparison methods.

        /// <summary>
        /// Compare internal value to input value.
        /// </summary>
        /// <param name="thatValue">String representing data.</param>
        /// <returns>Returns comparison sort index value.</returns>
        public int CompareValue(TData thatValue)
        {
            return this.CompareValue(this.Value, thatValue);
        }

        /// <summary>
        /// Compare internal value to input value.
        /// </summary>
        /// <param name="thatValue">String representing data.</param>
        /// <returns>Returns true if equal.</returns>
        public bool IsEqualValue(TData thatValue)
        {
            return this.IsEqualValue(this.Value, thatValue);
        }

        /// <summary>
        /// Compare internal value to input value.
        /// </summary>
        /// <param name="thatValue">String representing data.</param>
        /// <returns>Returns true if this internal value is greater than the input.</returns>
        public bool IsGreaterThanValue(TData thatValue)
        {
            return this.IsGreaterThanValue(this.Value, thatValue);
        }

        /// <summary>
        /// Compare internal value to input value.
        /// </summary>
        /// <param name="thatValue">String representing data.</param>
        /// <returns>Returns true if this internal value is greater than the input.</returns>
        public bool IsLessThanValue(TData thatValue)
        {
            return this.IsLessThanValue(this.Value, thatValue);
        }

        #endregion

        #region Internal SQL comparison methods.

        /// <summary>
        /// Compare internal SQL value to input value.
        /// </summary>
        /// <param name="sqlValue">String representing data.</param>
        /// <returns>Returns comparison sort index value.</returns>
        public int CompareSQL(string sqlValue)
        {
            return this.CompareSQL(this.SQLValue, sqlValue);
        }

        /// <summary>
        /// Compare internal SQL value to input value.
        /// </summary>
        /// <param name="sqlValue">String representing data.</param>
        /// <returns>Returns true if equal.</returns>
        public bool IsEqualSQL(string sqlValue)
        {
            return this.IsEqualSQL(this.SQLValue, sqlValue);
        }

        /// <summary>
        /// Compare internal SQL value to input value.
        /// </summary>
        /// <param name="sqlValue">String representing data.</param>
        /// <returns>Returns true if this internal value is greater than the input.</returns>
        public bool IsGreaterThanSQL(string sqlValue)
        {
            return this.IsGreaterThanSQL(this.SQLValue, sqlValue);
        }

        /// <summary>
        /// Compare internal SQL value to input value.
        /// </summary>
        /// <param name="sqlValue">String representing data.</param>
        /// <returns>Returns true if this internal value is greater than the input.</returns>
        public bool IsLessThanSQL(string sqlValue)
        {
            return this.IsLessThanSQL(this.SQLValue, sqlValue);
        }

        #endregion

        #region Explicit value comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareValue(TData left, TData right)
        {
            return left.CompareTo(right);
        }

        /// <summary>
        /// Check if the explicit data types are equal to each other.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if values of data types are equal.</returns>
        public bool IsEqualValue(TData left, TData right)
        {
            return (this.CompareValue(left, right) == 0);
        }

        /// <summary>
        /// Check if the explicit-typed left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanValue(TData left, TData right)
        {
            return (this.CompareValue(left, right) >= 1);
        }

        /// <summary>
        /// Check if the explicit-typed left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanValue(TData left, TData right)
        {
            return (this.CompareValue(left, right) <= -1);
        }

        #endregion

        #region SQL value comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left SQL value.</param>
        /// <param name="right">Right SQL value.</param>
        /// <returns>Return comparison sort index.</returns>
        public abstract int CompareSQL(string left, string right);

        /// <summary>
        /// Check if the explicit data types are equal to each other.
        /// </summary>
        /// <param name="left">Left SQL value.</param>
        /// <param name="right">Right SQL value.</param>
        /// <returns>Returns true if values of data types are equal.</returns>
        public bool IsEqualSQL(string left, string right)
        {
            return (this.CompareSQL(left, right) == 0);
        }

        /// <summary>
        /// Check if the explicit-typed left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left SQL value.</param>
        /// <param name="right">Right SQL value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanSQL(string left, string right)
        {
            return (this.CompareSQL(left, right) >= 1);
        }

        /// <summary>
        /// Check if the explicit-typed left value is less than the right value.
        /// </summary>
        /// <param name="left">Left SQL value.</param>
        /// <param name="right">Right SQL value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanSQL(string left, string right)
        {
            return (this.CompareSQL(left, right) <= -1);
        }

        #endregion

        //////////////////////
        // Accessor method(s).	

        /// <summary>
        /// Return the internal value.
        /// </summary>
        /// <returns>Return the value.</returns>
        public virtual TData GetValue()
        {
            return this.internalValue;
        }

        /// <summary>
        /// Return SQL format string from the internal value.
        /// </summary>
        /// <returns>Return the value.</returns>
        public virtual string GetSQL()
        {
            return this.ConvertToSQL(this.internalValue);
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set the internal value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public virtual void SetValue(TData value)
        {
            this.internalValue = value;
        }

        /// <summary>
        /// Set the internal value using an SQL string.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public virtual void SetSQL(string value)
        {
            this.internalValue = this.Parse(value);
        }

        /// <summary>
        /// Compare.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int Compare(TData other)
        {
            return CompareTo(other);
        }

        /// <summary>
        /// Check instances for equality.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public bool IsEqual(TData other)
        {
            return IsEqualValue(other);
        }

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareValue<T>(T left, T right) where T : IComparable
        {
            return left.CompareTo(right);
        }

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) == 0);
        }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) >= 1);
        }
        
        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) <= -1);
        }
    }

    /// <summary>
    /// Allows conversion between .NET and SQL date-time formats.
    /// </summary>
    public class MySqlDateTime : MySqlData<DateTime>, IDateTimeFormat
    {
        //////////////////////
        // Static member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Date-Time format (SQL).
        /// </summary>
        private const string SQL_FORMAT = "yyyy-MM-dd HH:mm:ss"; // SQL format.

        /// <summary>
        /// Date-Time format (.NET).
        /// </summary>
        private const string ISO_FORMAT = "MM/dd/yyyy HH:mm:ss"; // .NET format.

        /// <summary>
        /// Default string representing an 'empty' date.
        /// </summary>
        private static readonly DateTime DEFAULT = new DateTime(1, 1, 1, 0, 0, 0);

        //////////////////////
        // Operator(s).

        /// <summary>
        /// Add a Timespan to a MySqlDateTime object's value.
        /// </summary>
        /// <param name="datetime">DateTime being added to.</param>
        /// <param name="duration">Duration to add to the date time.</param>
        /// <returns>Returns existing datetime object.</returns>
        public static MySqlDateTime operator +(MySqlDateTime datetime, TimeSpan duration)
        {
            datetime.Value = datetime.Value + duration;
            return datetime;
        }

        /// <summary>
        /// Subtracts a Timespan from a MySqlDateTime object's value.
        /// </summary>
        /// <param name="datetime">DateTime being subtracted from.</param>
        /// <param name="duration">Duration to subtract from the date time.</param>
        /// <returns>Returns existing datetime object.</returns>
        public static MySqlDateTime operator -(MySqlDateTime datetime, TimeSpan duration)
        {
            datetime.Value = datetime.Value - duration;
            return datetime;
        }

        /// <summary>
        /// Cast MySqlDateTime into a DateTime object.
        /// </summary>
        /// <param name="instance">Value to cast.</param>
        public static implicit operator DateTime(MySqlDateTime instance)
        {
            return instance.Value;
        }

        /// <summary>
        /// Cast MySqlDateTime into a string.
        /// </summary>
        /// <param name="instance">Value to cast.</param>
        public static explicit operator string(MySqlDateTime instance)
        {
            return instance.SQLValue;
        }

        /// <summary>
        /// Cast DateTime into a MySqlDateTime object.
        /// </summary>
        /// <param name="value">Value to cast.</param>
        public static implicit operator MySqlDateTime(DateTime value)
        {
            return new MySqlDateTime(value);
        }

        /// <summary>
        /// Cast string into a MySqlDateTime object.
        /// </summary>
        /// <param name="value">Value to cast.</param>
        public static explicit operator MySqlDateTime(string value)
        {
            MySqlDateTime dt = new MySqlDateTime(value);
            if (String.IsNullOrWhiteSpace(value) || dt.IsDefault) { throw new InvalidCastException("Input string cannot be cast to DateTime object."); }
            return dt;
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a MySqlDateTime object from an input value.
        /// </summary>
        /// <param name="value">Value to assign.</param>
        public MySqlDateTime(DateTime value) : base(value) {}

        /// <summary>
        /// Create a MySqlDateTime object from an input SQL value.
        /// </summary>
        /// <param name="value">Value to assign.</param>
        public MySqlDateTime(string value) : base(value) {}

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Comparison of two, individual SQL statements.
        /// </summary>
        /// <param name="left">Left SQL statement.</param>
        /// <param name="right">Right SQL statement.</param>
        /// <returns>Returns comparison index.</returns>
        public override int CompareSQL(string left, string right)
        {
            DateTime leftDate = this.Parse(left);
            DateTime rightDate = this.Parse(right);
            return this.CompareValue(leftDate, rightDate);
        }

        //////////////////////
        // Helpers.						

        /// <summary>
        /// Print the data value in its .NET incarnation.
        /// </summary>
        /// <returns>Returns a formatted representation of the data.</returns>
        public override string ToString()
        {
            return $"{this.Value.ToString(MySqlDateTime.ISO_FORMAT)}";
        }

        /// <summary>
        /// Returns the default date-time (01/01/0001 00:00:00).
        /// </summary>
        /// <returns>Returns a DateTime object.</returns>
        protected override DateTime GetDefault()
        {
            return MySqlDateTime.DEFAULT;
        }

        /// <summary>
        /// Convert the object into the SQL string format.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns SQL string representation of the data.</returns>
        public override string ConvertToSQL(DateTime value)
        {
            return value.ToString(MySqlDateTime.SQL_FORMAT);
        }

        /// <summary>
        /// Convert the SQL string format into the object data type.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns data.</returns>
        public override DateTime Parse(string value)
        {
            return (DateTime.TryParse(value, out DateTime result)) ? result : this.GetDefault();
        }	

    }
    
    /// <summary>
    /// Ranged version of the MySqlDateTime object.
    /// </summary>
    public class MySqlDateRange : MySqlDateTime, IRange<DateTime>
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Minimum date that can be in the range.
        /// </summary>
        private DateTime minimum;

        /// <summary>
        /// Maximum date that can be in the range.
        /// </summary>
        private DateTime maximum;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Maximum date-time value.
        /// </summary>
        public DateTime Max
        {
            get { return this.maximum; }
            set { this.maximum = value; }
        }

        /// <summary>
        /// Minimum date-time value.
        /// </summary>
        public DateTime Min
        {
            get { return this.minimum; }
            set { this.minimum = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a range.
        /// </summary>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        public MySqlDateRange(DateTime minimum, DateTime maximum)
            : base(minimum)
        {
            this.Min = minimum;
            this.Max = maximum;
        }

        /// <summary>
        /// Create a range.
        /// </summary>
        /// <param name="minimum">Minimum value.</param>
        /// <param name="maximum">Maximum value.</param>
        /// <param name="timestamp">Current value.</param>
        public MySqlDateRange(DateTime minimum, DateTime maximum, DateTime timestamp)
            : base(timestamp)
        {
            this.Min = minimum;
            this.Max = maximum;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Print the data value in its .NET incarnation.
        /// </summary>
        /// <returns>Returns a formatted representation of the data.</returns>
        public override string ToString()
        {
            return $"{base.ToString()} between dates '{this.Min.ToString()}' and '{this.Max.ToString()}'.";
        }

        /// <summary>
        /// Check if date is between two dates. (Inclusive max and inclusive min).
        /// </summary>
        /// <param name="other">Date to check.</param>
        /// <returns>Returns true if in range.</returns>
        public bool InRange(DateTime other)
        {
            return ((other <= this.Max) && (other >= this.Min));
        }

        /// <summary>
        /// Check if date is between two dates. (Inclusive max and inclusive min).
        /// </summary>
        /// <param name="other">Date to check.</param>
        /// <returns>Returns true if in range.</returns>
        public bool InRange(string other)
        {
            return this.InRange(this.Parse(other));
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set the value, ensuring it stays within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns true if successful.</returns>
        public bool TrySetValue(DateTime value)
        {
            if (this.InRange(value))
            {
                base.SetValue(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the value, ensuring it stays within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns true if successful.</returns>
        public bool TrySetSQL(string value)
        {
            if (this.InRange(value))
            {
                base.SetSQL(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the value, ensuring it stays within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public override void SetValue(DateTime value)
        {
            TrySetValue(value);
        }

        /// <summary>
        /// Set the value, ensuring it stays within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public override void SetSQL(string value)
        {
            this.TrySetSQL(value);
        }

    }

    /// <summary>
    /// Represents a unique identifier, as a string and as an integer. Will trim leading zeroes.
    /// </summary>
    public class MySqlID : MySqlData<int>, IUIDFormat
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Operator(s).
        
        /// <summary>
        /// Converts MySqlID to an integer;
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator int(MySqlID identifier)
        {
            return identifier.Value;
        }

        /// <summary>
        /// Converts MySqlID to a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static explicit operator string(MySqlID identifier)
        {
            return identifier.SQLValue;
        }
                
        /// <summary>
        /// Converts MySqlID from an integer.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlID(int identifier)
        {
            return new MySqlID(identifier);
        }

        /// <summary>
        /// Converts MySqlID from a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static explicit operator MySqlID(string identifier)
        {
            return new MySqlID(identifier);
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Identifier value.</param>
        public MySqlID(int value) : base(value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Identifier value.</param>
        public MySqlID(string value) : base(value)
        {
            this.SetSQL(value);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Return as a formatted representation of the data.
        /// </summary>
        /// <returns>Returns formatted string.</returns>
        public override string ToString()
        {
            return $"ID: [{this.Value}]";
        }
        
        /// <summary>
        /// Comparison of SQL data strings.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns comparison index.</returns>
        public override int CompareSQL(string left, string right)
        {
            if (String.IsNullOrWhiteSpace(left) && String.IsNullOrWhiteSpace(right)) { return 0; }
            if (String.IsNullOrWhiteSpace(left)) { return -1; }
            if (String.IsNullOrWhiteSpace(right)) { return 1; }
            if (left == right) { return 0; }

            int leftID = this.Parse(left);
            int rightID = this.Parse(right);
            return this.CompareValue(leftID, rightID);
        }

        //////////////////////
        // Helper method(s).

        /// <summary>
        /// Returns the default value.
        /// </summary>
        /// <returns>Returns minimum possible value.</returns>
        protected override int GetDefault()
        {
            return Int32.MinValue;
        }

        /// <summary>
        /// Convert the integer to an SQL string.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns the value formatted.</returns>
        public override string ConvertToSQL(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Convert the SQL to an integer.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns the value formatted.</returns>
        public override int Parse(string value)
        {
            if (Int32.TryParse(value, out int result))
            {
                return result;
            }
            return this.GetDefault();
        }        
    }

    /// <summary>
    /// Represents a boolean Y/N character flag.
    /// </summary>
    public class MySqlFlag : MySqlData<bool>, IFlagFormat
    {
        //////////////////////
        // Static Member(s).
        //////////////////////
        
        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Converts MySqlID to a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator string(MySqlFlag identifier)
        {
            return identifier.SQLValue;
        }

        /// <summary>
        /// Converts MySqlID to a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator char(MySqlFlag identifier)
        {
            return identifier.SQLValue[0];
        }

        /// <summary>
        /// Converts MySqlID to an integer;
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static explicit operator int(MySqlFlag identifier)
        {
            return (identifier.Value) ? 1 : 0;
        }

        /// <summary>
        /// Converts MySqlID to an integer;
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator bool(MySqlFlag identifier)
        {
            return identifier.Value;
        }

        /// <summary>
        /// Converts MySqlID from a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static explicit operator MySqlFlag(string identifier)
        {
            return new MySqlFlag(identifier);
        }

        /// <summary>
        /// Converts MySqlID from a character.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static explicit operator MySqlFlag(char identifier)
        {
            return new MySqlFlag($"{identifier}");
        }

        /// <summary>
        /// Converts MySqlID from an integer.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlFlag(int identifier)
        {
            return new MySqlFlag(identifier);
        }

        /// <summary>
        /// Converts MySqlID from a boolean.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlFlag(bool identifier)
        {
            return new MySqlFlag(identifier);
        }
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(bool value) : base(value)
        {
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(int value) : base((value == 1) ? true : false)
        {
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(string value) : base(value)
        {
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Compares two separate SQL values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Comparison index.</returns>
        public override int CompareSQL(string left, string right)
        {
            if (String.IsNullOrWhiteSpace(left + right)) { return 0; }
            if (String.IsNullOrWhiteSpace(right)) { return 1; }
            if (string.IsNullOrWhiteSpace(left)) { return -1; }

            bool leftBoolean = this.Parse(left);
            bool rightBoolean = this.Parse(right);
            return this.CompareValue(leftBoolean, rightBoolean);
        }

        //////////////////////
        // Helper method(s).

        /// <summary>
        /// Formats .NET value.
        /// </summary>
        /// <returns>Return the value.</returns>
        public override string ToString()
        {
            return $"Flag: {this.Value}";
        }

        /// <summary>
        /// Return the default value.
        /// </summary>
        /// <returns>Returns flag.</returns>
        public override int CompareTo(object other)
        {
            if (other == null) { return 1; }
            if (this == other) { return 0; }
            if (other is MySqlData<bool> that)
            {
                return this.CompareTo(that.Value);
            }
            if (other is bool boolean)
            {
                return this.CompareTo(boolean ? 1 : 0);
            }
            if (other is string sqlBoolean)
            {
                return this.CompareTo(this.Parse(sqlBoolean));
            }
            if (other is int integer)
            {
                switch (integer)
                {
                    case 0:
                        return (this.Value) ? 1 : 0;
                    case 1:
                        return (this.Value) ? 0 : -1;
                    default:
                        return (this.Value) ? 1 : 1;
                }
            }
            return 1;
        }

        /// <summary>
        /// Return the default value.
        /// </summary>
        /// <returns>Returns flag.</returns>
        protected override bool GetDefault()
        {
            return false;
        }

        /// <summary>
        /// Convert values to SQL.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns a string.</returns>
        public override string ConvertToSQL(bool value)
        {
            return (value == true) ? "Y" : "N";
        }

        /// <summary>
        /// Convert value from SQL. Empty string is also treated as 'false'.
        /// </summary>
        /// <param name="value">Value to convert.</param>
        /// <returns>Returns a boolean.</returns>
        public override bool Parse(string value)
        {
            if (!String.IsNullOrWhiteSpace(value))
            {
                string result = value.Trim().ToUpper();
                if (result == "Y" || result == "YES" || result == "TRUE" || result == "1" || result == "T" || result == "ON")
                {
                    return true;
                }
                else if (result == "N" || result == "NO" || result == "FALSE" || result == "0" || result == "F" || result == "OFF")
                {
                    return false;
                }
                else
                {
                    throw new InvalidCastException("Cannot parse the input string as a boolean.");
                }
            }
            return this.GetDefault();
        }

    }

}
