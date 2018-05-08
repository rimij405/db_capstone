/*
    IComparison.cs
    Contains the IComparison and generic IComparison<T> interfaces.
    ---
    Ian Effendi
 */

 // using statements.
using System;

/*
    The Services.Interfaces namespace
    is a collection of services that can be
    used in implementations for any project.
     */
namespace Services.Interfaces
{

    /// <summary>
    /// Represents generic type comparisons between items of the same type. 
    /// </summary>
    public interface IComparison
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
        int CompareValue<T>(T left, T right) where T : IComparable;

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualValue<T>(T left, T right) where T : IComparable;

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanValue<T>(T left, T right) where T : IComparable;

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanValue<T>(T left, T right) where T : IComparable;
    }

    /// <summary>
    /// Interface representing generic type comparisons.
    /// </summary>
    /// <typeparam name="T">Type to compare.</typeparam>
    public interface IComparison<T> : IComparison where T : IComparable
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Check instances.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        int Compare(T other);
        
        /// <summary>
        /// Check instances for equality.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        bool IsEqual(T other);

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

}
