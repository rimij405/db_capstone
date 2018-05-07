/*
    IFormat.cs
    ---
    Ian Effendi
 */

 // using statements.
using System;

namespace ISTE.DAL.Database.Interfaces
{    
    /// <summary>
    /// Interface representing type comparisons.
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    public interface IComparison<T> where T : IComparable
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareValue(T left, T right);

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualValue(T left, T right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanValue(T left, T right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanValue(T left, T right);
    }
    
    /// <summary>
    /// Helper format for converting between two types.
    /// </summary>
    /// <typeparam name="T">Data type for conversion.</typeparam>
    public interface IFormat<T> where T : IComparable
    {
        /// <summary>
        /// Return the value as the .NET compatible type.
        /// </summary>
        T Value
        {
            get;
        }

        /// <summary>
        /// Get the value as the SQL format.
        /// </summary>
        string SQLValue
        {
            get;
        }

        /// <summary>
        /// Return the value as its .NET compatible type.
        /// </summary>
        /// <returns>Return value.</returns>
        T GetValue();

        /// <summary>
        /// Return the value as its SQL string.
        /// </summary>
        /// <returns>Return value.</returns>
        string GetSQL();

        /// <summary>
        /// Attempt to set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        void SetValue(T value);

        /// <summary>
        /// Attempt to set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        void SetSQL(string value);
    }

    /// <summary>
    /// Represent a range between value types.
    /// /// </summary>
    /// <typeparam name="T">Data type for conversion.</typeparam>
    public interface IRange<T> where T : IComparable
    {
        /// <summary>
        /// The maximum.
        /// </summary>
        T Max
        {
            get;
        }

        /// <summary>
        /// Return the value as the .NET compatible type.
        /// </summary>
        T Value
        {
            get;
        }

        /// <summary>
        /// The minimum.
        /// </summary>
        T Min
        {
            get;
        }

        /// <summary>
        /// Attempt to set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        bool TrySetValue(T value);

        /// <summary>
        /// Attempt to set the value.
        /// </summary>
        /// <param name="value">Value to set.</param>
        bool TrySetSQL(string value);

        /// <summary>
        /// Check if a particular value is within the range.
        /// </summary>
        /// <param name="other">Value to check.</param>
        /// <returns>Value to compare.</returns>
        bool InRange(T other);

        /// <summary>
        /// Check if a particular value is within the range.
        /// </summary>
        /// <param name="other">Value to check.</param>
        /// <returns>Value to compare.</returns>
        bool InRange(string other);
    }

    /// <summary>
    /// Represents a date-time format for .NET.
    /// </summary>
    public interface IDateTimeFormat : IFormat<DateTime> { }

    /// <summary>
    /// Represents a date-time range for .NET.
    /// </summary>
    public interface IDateTimeRange : IRange<DateTime> { }    

    /// <summary>
    /// Represents a unique identifier.
    /// </summary>
    public interface IUIDFormat : IFormat<int> { }

    /// <summary>
    /// Represents a yes/no flag.
    /// </summary>
    public interface IFlagFormat : IFormat<bool> { }
}
