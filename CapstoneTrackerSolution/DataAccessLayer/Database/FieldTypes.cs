/*
    FieldTypes.cs
    Contains a series of implementations for various field types.
    ***
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

namespace ISTE.DAL.Database
{
    /// <summary>
    /// A boolean type for MySQL.
    /// </summary>
    public class MySqlBoolean : IBooleanFieldType
    {
        /// <summary>
        /// Convert a string into a boolean value for MySql.
        /// </summary>
        /// <param name="obj">Object to convert.</param>
        /// <param name="value">Value at other end of conversion.</param>
        /// <returns>Returns a flag indicating operation success.</returns>
        public bool Convert(string obj, out bool value)
        {
            value = false;
            string val = obj.Trim().ToLower();
            if (val.Length > 0)
            {
                switch (val)
                {
                    case "1":
                    case "true":
                        value = true;
                        return true;
                    case "0":
                    case "false":
                        value = false;
                        return true;               
                }
            }

            // Operation unsuccessful.
            return false;
        }

        /// <summary>
        /// Returns the name of this field type.
        /// </summary>
        /// <returns>Returns name formatted as string.</returns>
        public string GetFieldTypeName()
        {
            return "BOOLEAN";
        }

        /// <summary>
        /// Always returns true for any type of boolean that's been passed in.
        /// </summary>
        /// <param name="obj">Checks if value is valid for this type.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValid(bool obj)
        {
            return true;
        }
        
        /// <summary>
        /// Parse a value into a string.
        /// </summary>
        /// <param name="obj">Value to parse out.</param>
        /// <param name="value">String value to output.</param>
        /// <returns>Returns true if operation was successful.</returns>
        public bool Parse(bool obj, out string value)
        {
            value = obj.ToString();
            return true;
        }
    }

    /// <summary>
    /// An integer type for MySQL.
    /// </summary>
    public class MySqlInteger : IIntegerFieldType
    {
        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="obj">String to convert.</param>
        /// <param name="value">Value output.</param>
        /// <returns>Returns flag on operation success.</returns>
        public bool Convert(string obj, out int value)
        {
            return Int32.TryParse(obj.Trim(), out value);
        }

        /// <summary>
        /// Returns the name of this field type.
        /// </summary>
        /// <returns>Returns name formatted as string.</returns>
        public string GetFieldTypeName()
        {
            return "INT";
        }

        /// <summary>
        /// Always returns true for when there isn't clamping involved.
        /// </summary>
        /// <param name="obj">Checks if value is valid for this type.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValid(int obj)
        {
            return true;
        }

        /// <summary>
        /// Parse value into a string.
        /// </summary>
        /// <param name="obj">Value to parse into a string.</param>
        /// <param name="value">Value to return.</param>
        /// <returns>Returns flag based on operation success.</returns>
        public bool Parse(int obj, out string value)
        {
            value = obj.ToString();
            return true;
        }
    }

    /// <summary>
    /// A double type for MySQL.
    /// </summary>
    public class MySqlDouble : IDoubleFieldType {

        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="obj">String to convert.</param>
        /// <param name="value">Value output.</param>
        /// <returns>Returns flag on operation success.</returns>
        public bool Convert(string obj, out double value)
        {
            return Double.TryParse(obj.Trim(), out value);
        }

        /// <summary>
        /// Returns the name of this field type.
        /// </summary>
        /// <returns>Returns name formatted as string.</returns>
        public string GetFieldTypeName()
        {
            return "DECIMAL";
        }

        /// <summary>
        /// Always returns true for when there isn't clamping involved.
        /// </summary>
        /// <param name="obj">Checks if value is valid for this type.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValid(double obj)
        {
            return true;
        }

        /// <summary>
        /// Parse value into a string.
        /// </summary>
        /// <param name="obj">Value to parse into a string.</param>
        /// <param name="value">Value to return.</param>
        /// <returns>Returns flag based on operation success.</returns>
        public bool Parse(double obj, out string value)
        {
            value = obj.ToString();
            return true;
        }

    }

    /// <summary>
    /// A date type for MySQL.
    /// </summary>
    public class MySqlDateTime : IDateFieldType
    {
        /// <summary>
        /// Convert value.
        /// </summary>
        /// <param name="obj">String to convert.</param>
        /// <param name="value">Value output.</param>
        /// <returns>Returns flag on operation success.</returns>
        public bool Convert(string obj, out DateTime value)
        {
            return DateTime.TryParse(obj, out value);
        }

        /// <summary>
        /// Returns the name of this field type.
        /// </summary>
        /// <returns>Returns name formatted as string.</returns>
        public string GetFieldTypeName()
        {
            return "DATETIME";
        }

        /// <summary>
        /// Always returns true for when there isn't clamping involved.
        /// </summary>
        /// <param name="obj">Checks if value is valid for this type.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValid(DateTime obj)
        {
            return true;
        }

        /// <summary>
        /// Parse value into a string.
        /// </summary>
        /// <param name="obj">Value to parse into a string.</param>
        /// <param name="value">Value to return.</param>
        /// <returns>Returns flag based on operation success.</returns>
        public bool Parse(DateTime obj, out string value)
        {
            value = obj.ToString();
            return true;
        }
    }

    /// <summary>
    /// Provides an inclusive range of integers.
    /// </summary>
    public class MySqlIntegerRange : MySqlInteger, IClampedIntegerFieldType
    {
        // Field(s).

        /// <summary>
        /// Inclusive maximum value in this range.
        /// </summary>
        private int maximum = 0;

        /// <summary>
        /// Inclusive minimum value in this range.
        /// </summary>
        private int minimum = 0;

        /// <summary>
        /// Clamp value of input between minimum and maximum.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>Return clamped value.</returns>
        public int Clamp(int value)
        {
            if (IsBetween(value)) { return value; }
            else
            {
                if(value > maximum) { return maximum; }
                return minimum;
            }
        }

        /// <summary>
        /// Checks if input is between extents.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Return true if value is between extents.</returns>
        public bool IsBetween(int value)
        {
            return ((value <= maximum) && (value >= minimum));
        }

        /// <summary>
        /// Sets inclusive maximum extent.
        /// </summary>
        /// <param name="max">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<int> SetMaximum(int max)
        {
            this.maximum = max;
            return this;
        }

        /// <summary>
        /// Sets inclusive minimum extent.
        /// </summary>
        /// <param name="min">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<int> SetMinimum(int min)
        {
            this.minimum = min;
            return this;
        }
    }

    /// <summary>
    /// Provides an inclusive range of doubles.
    /// </summary>
    public class MySqlDoubleRange : MySqlDouble, IClampedDoubleFieldType
    {
        // Field(s).

        /// <summary>
        /// Inclusive maximum value in this range.
        /// </summary>
        private double maximum = 0;

        /// <summary>
        /// Inclusive minimum value in this range.
        /// </summary>
        private double minimum = 0;

        /// <summary>
        /// Clamp value of input between minimum and maximum.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>Return clamped value.</returns>
        public double Clamp(double value)
        {
            if (IsBetween(value)) { return value; }
            else
            {
                if (value > maximum) { return maximum; }
                return minimum;
            }
        }

        /// <summary>
        /// Checks if input is between extents.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Return true if value is between extents.</returns>
        public bool IsBetween(double value)
        {
            return ((value <= maximum) && (value >= minimum));
        }

        /// <summary>
        /// Sets inclusive maximum extent.
        /// </summary>
        /// <param name="max">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<double> SetMaximum(double max)
        {
            this.maximum = max;
            return this;
        }

        /// <summary>
        /// Sets inclusive minimum extent.
        /// </summary>
        /// <param name="min">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<double> SetMinimum(double min)
        {
            this.minimum = min;
            return this;
        }
    }

    /// <summary>
    /// Provides an inclusive range of DateTime objects.
    /// </summary>
    public class MySqlDateTimeRange : MySqlDateTime, IClampedDateFieldType
    {
        // Field(s).

        /// <summary>
        /// Inclusive maximum value in this range.
        /// </summary>
        private DateTime maximum = new DateTime();

        /// <summary>
        /// Inclusive minimum value in this range.
        /// </summary>
        private DateTime minimum = new DateTime();

        /// <summary>
        /// Clamp value of input between minimum and maximum.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>Return clamped value.</returns>
        public DateTime Clamp(DateTime value)
        {
            if (IsBetween(value)) { return value; }
            else
            {
                if (value > maximum) { return maximum; }
                return minimum;
            }
        }

        /// <summary>
        /// Checks if input is between extents.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Return true if value is between extents.</returns>
        public bool IsBetween(DateTime value)
        {
            return ((value <= maximum) && (value >= minimum));
        }

        /// <summary>
        /// Sets inclusive maximum extent.
        /// </summary>
        /// <param name="max">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<DateTime> SetMaximum(DateTime max)
        {
            this.maximum = max;
            return this;
        }

        /// <summary>
        /// Sets inclusive minimum extent.
        /// </summary>
        /// <param name="min">Value to set.</param>
        /// <returns>Return reference to self.</returns>
        public IClamp<DateTime> SetMinimum(DateTime min)
        {
            this.minimum = min;
            return this;
        }
    }

}
