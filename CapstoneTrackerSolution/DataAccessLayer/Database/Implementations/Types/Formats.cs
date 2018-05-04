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

// additional using statements.
using System.Globalization;
using ISTE.DAL.Database.Interfaces;


namespace ISTE.DAL.Database.Implementations
{
    /// <summary>
    /// Allows conversion between .NET and SQL date-time formats.
    /// </summary>
    public class MySqlDateTime : IDateTimeFormat
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Format expected by MySql.
        /// </summary>
        protected const string ISOFormat = "yyyy-MM-dd HH:mm:ss";

        /// <summary>
        /// The 'zero' default handled by MySql.
        /// </summary>
        protected const string Default = "0000-00-00 00:00:00";

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Attempts to parse an input time.
        /// </summary>
        /// <param name="timestamp">Timestamp to parse.</param>
        /// <param name="result">Resulting .NET compatible value.</param>
        /// <returns>Returns true if successfully parsed.</returns>
        protected static bool Parse(string timestamp, out DateTime result)
        {
            return DateTime.TryParseExact(timestamp, MySqlDateTime.ISOFormat, new CultureInfo("en-US"), DateTimeStyles.None, out result);
        }

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Convert a DateTime struct into a MySqlDateTime object.
        /// </summary>
        /// <param name="timestamp">Time to convert.</param>
        public static implicit operator MySqlDateTime(DateTime timestamp)
        {
            return new MySqlDateTime(timestamp);
        }

        /// <summary>
        /// Convert a string timestamp into a MySqlDateTime object.
        /// </summary>
        /// <param name="timestamp">Time to convert.</param>
        public static explicit operator MySqlDateTime(string timestamp)
        {
            return new MySqlDateTime(timestamp);
        }

        /// <summary>
        /// Convert a MySqlDateTime object into a string timestamp.
        /// </summary>
        /// <param name="datetime">Time to convert.</param>
        public static implicit operator string(MySqlDateTime datetime)
        {
            return datetime.SQLValue;
        }

        /// <summary>
        /// Convert a MySqlDateTime object into a DateTime struct.
        /// </summary>
        /// <param name="datetime">Time to convert.</param>
        public static implicit operator DateTime(MySqlDateTime datetime)
        {
            return datetime.Value;
        }

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Internal value held by .NET.
        /// </summary>
        private DateTime internalTime;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Value of the MySqlDateTime object.
        /// </summary>
        public DateTime Value
        {
            get {
                if (this.internalTime == null)
                { Parse(MySqlDateTime.Default, out internalTime); }
                return this.internalTime;
            }
        }

        /// <summary>
        /// Value of the MySqlDateTime object as a MySql parsable string.
        /// </summary>
        public string SQLValue
        {
            get {
                if (this.internalTime == null) { return MySqlDateTime.Default; }
                return this.internalTime.ToString(MySqlDateTime.ISOFormat); }
        }

        /// <summary>
        /// Check if a value is currently set to the default value.
        /// </summary>
        public bool IsDefault
        {
            get {
                string value = this.SQLValue;
                return (value == MySqlDateTime.Default);
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Construct object using default values.
        /// </summary>
        public MySqlDateTime()
        {
        }

        /// <summary>
        /// Construct object with input date-time.
        /// </summary>
        /// <param name="timestamp">Date-time to assign.</param>
        public MySqlDateTime(DateTime timestamp)
        {
            this.internalTime = timestamp;
        }

        /// <summary>
        /// Construct object with input date-time.
        /// </summary>
        /// <param name="timestamp">Date-time to assign.</param>
        public MySqlDateTime(string timestamp)
        {
            Parse(timestamp, out this.internalTime);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).
        
        /// <summary>
        /// Check if this is equal to the input value.
        /// </summary>
        /// <param name="sqlValue">SQL formatted date-time to compare.</param>
        /// <returns>Returns true if there is a match.</returns>
        public bool IsEqual(string sqlValue)
        {
            return (sqlValue == this.SQLValue);
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(DateTime value)
        {
            this.internalTime = value;
        }

        /// <summary>
        /// Set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(string value)
        {
            DateTime temp = new DateTime();
            if(Parse(value, out temp)) { this.internalTime = temp; }
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
            : base()
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
            DateTime temp = new DateTime();
            if (MySqlDateTime.Parse(other, out temp))
            {
                return this.InRange(temp);
            }
            return false;
        }
        
        //////////////////////
        // Accessor method(s).

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set value, as long as it is within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns true if in bounds.</returns>
        public bool Set(DateTime value)
        {
            if (this.InRange(value))
            {
                this.SetValue(value);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set value, as long as it is within range.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns true if in bounds.</returns>
        public bool Set(string value)
        {
            DateTime temp = new DateTime();
            if (MySqlDateTime.Parse(value, out temp))
            {
                return this.Set(temp);
            }
            return false;
        }
    }

    /// <summary>
    /// Represents an ID.
    /// </summary>
    public class MySqlID : IUIDFormat
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Default ID.
        /// </summary>
        private static int Default = Int32.MaxValue;

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Attempt to parse a string.
        /// </summary>
        /// <param name="value">Value to parse.</param>
        /// <param name="identifier">Parsed value.</param>
        /// <returns>Returns true if successful.</returns>
        private static bool Parse(string value, out int identifier)
        {
            identifier = MySqlID.Default;
            if (!Int32.TryParse(value, out identifier))
            {
                identifier = MySqlID.Default;
                return false;
            }
            return true;
        }

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Converts MySqlID to a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator string(MySqlID identifier)
        {
            return identifier.SQLValue;
        }

        /// <summary>
        /// Converts MySqlID to an integer;
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator int(MySqlID identifier)
        {
            return identifier.Value;
        }

        /// <summary>
        /// Converts MySqlID from a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlID(string identifier)
        {
            return new MySqlID(identifier);
        }

        /// <summary>
        /// Converts MySqlID from an integer.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlID(int identifier)
        {
            return new MySqlID(identifier);
        }

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// ID.
        /// </summary>
        private uint internalValue;

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Identifier number.
        /// </summary>
        public int Value
        {
            get { return (int) this.internalValue; }
            private set { this.SetValue(value); }
        }

        /// <summary>
        /// MySQL compatible value.
        /// </summary>
        public string SQLValue
        {
            get { return $"{this.internalValue}"; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MySqlID()
        {
            this.SetValue(MySqlID.Default);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Identifier value.</param>
        public MySqlID(int value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Identifier value.</param>
        public MySqlID(string value)
        {
            this.SetValue(value);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check if values are equal.
        /// </summary>
        /// <param name="sqlValue">SQL value to parse.</param>
        /// <returns>Returns true if matched.</returns>
        public bool IsEqual(string sqlValue)
        {
            int temp = MySqlID.Default;
            if (Parse(sqlValue, out temp))
            {
                return (temp == this.Value);
            }
            return false;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(int value)
        {
            this.internalValue = (uint)Math.Abs(value);
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(string value)
        {
            int temp = MySqlID.Default;
            Parse(value, out temp);
            this.SetValue(temp);
        }
        
    }

    /// <summary>
    /// Represents a boolean Y/N character flag.
    /// </summary>
    public class MySqlFlag : IFlagFormat
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Default ID.
        /// </summary>
        private static bool Default = false;

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Attempt to parse a string.
        /// </summary>
        /// <param name="value">Value to parse.</param>
        /// <param name="identifier">Parsed value.</param>
        /// <returns>Returns true if successful.</returns>
        private static bool Parse(string value, out bool identifier)
        {
            identifier = MySqlFlag.Default;
            if (!Boolean.TryParse(value, out identifier))
            {
                identifier = MySqlFlag.Default;
                return false;
            }
            return true;
        }

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
        public static implicit operator int(MySqlFlag identifier)
        {
            return (identifier.Value) ? 1 : 0;
        }

        /// <summary>
        /// Converts MySqlID from a string.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlFlag(string identifier)
        {
            return new MySqlFlag(identifier);
        }

        /// <summary>
        /// Converts MySqlID from a character.
        /// </summary>
        /// <param name="identifier">Identifier to convert.</param>
        public static implicit operator MySqlFlag(char identifier)
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

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Flag.
        /// </summary>
        private bool internalValue;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Flag value.
        /// </summary>
        public bool Value
        {
            get { return this.internalValue; }
            private set { this.SetValue(value); }
        }

        /// <summary>
        /// MySQL compatible value.
        /// </summary>
        public string SQLValue
        {
            get
            {
                return this.internalValue ? "Y" : "N";
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Default constructor.
        /// </summary>
        public MySqlFlag()
        {
            this.SetValue(MySqlFlag.Default);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(bool value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(int value)
        {
            this.SetValue(value);
        }

        /// <summary>
        /// Assign a value.
        /// </summary>
        /// <param name="value">Flag value.</param>
        public MySqlFlag(string value)
        {
            this.SetValue(value);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check if values are equal.
        /// </summary>
        /// <param name="sqlValue">SQL value to parse.</param>
        /// <returns>Returns true if matched.</returns>
        public bool IsEqual(string sqlValue)
        {
            bool temp = MySqlFlag.Default;
            if (Parse(sqlValue, out temp))
            {
                return (temp == this.Value);
            }
            return false;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(bool value)
        {
            this.internalValue = value;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(int value)
        {
            if(value >= 1) { this.SetValue(true); }
            else { this.SetValue(false); }
        }

        /// <summary>
        /// Set value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        public void SetValue(string value)
        {
            bool temp = MySqlFlag.Default;
            Parse(value, out temp);
            this.SetValue(temp);
        }
    }

}
