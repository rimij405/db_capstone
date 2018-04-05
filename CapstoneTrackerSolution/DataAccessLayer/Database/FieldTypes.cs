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

    public class MySqlIntegerRange : MySqlInteger, IClampedIntegerFieldType
    {
        public int Clamp(int value)
        {
            throw new NotImplementedException();
        }

        public IClamp<int> IsBetween(int value)
        {
            throw new NotImplementedException();
        }

        public IClamp<int> SetMaximum(int max)
        {
            throw new NotImplementedException();
        }

        public IClamp<int> SetMinimum(int min)
        {
            throw new NotImplementedException();
        }
    }

    public class MySqlDoubleRange : MySqlDouble, IClampedDoubleFieldType
    {
        public double Clamp(double value)
        {
            throw new NotImplementedException();
        }

        public IClamp<double> IsBetween(double value)
        {
            throw new NotImplementedException();
        }

        public IClamp<double> SetMaximum(double max)
        {
            throw new NotImplementedException();
        }

        public IClamp<double> SetMinimum(double min)
        {
            throw new NotImplementedException();
        }
    }

    public class MySqlDateTimeRange : MySqlDateTime, IClampedDateFieldType
    {
        public DateTime Clamp(DateTime value)
        {
            throw new NotImplementedException();
        }

        public IClamp<DateTime> IsBetween(DateTime value)
        {
            throw new NotImplementedException();
        }

        public IClamp<DateTime> SetMaximum(DateTime max)
        {
            throw new NotImplementedException();
        }

        public IClamp<DateTime> SetMinimum(DateTime min)
        {
            throw new NotImplementedException();
        }
    }

}
