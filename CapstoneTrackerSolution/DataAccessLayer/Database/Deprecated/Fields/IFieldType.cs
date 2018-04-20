/*
    IFieldType.cs
    The field type in a result set.
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
    /// Represents a field type converter/parser; does not hold value.
    /// </summary>
    public interface IFieldType<TValue>
    {
        /// <summary>
        /// Return string formatted version of the field type.
        /// </summary>
        /// <returns>Return field name.</returns>
        string GetFieldTypeName();

        /// <summary>
        /// Parse an input value of the generic data type into its string format.
        /// </summary>
        /// <param name="obj">Value to be parsed.</param>
        /// <param name="value">Value to be sent out.</param>
        /// <returns>Return value as string.</returns>
        bool Parse(TValue obj, out string value);

        /// <summary>
        /// Convert an input string into the value, if possible.
        /// </summary>
        /// <param name="obj">Value to be converted.</param>
        /// <param name="value">Value to be sent out.</param>
        /// <returns>Return value as generic type.</returns>
        bool Convert(string obj, out TValue value);

        /// <summary>
        /// Check if the input value is 'valid' for a particular implementation.
        /// </summary>
        /// <param name="obj">Value to validate.</param>
        /// <returns>Returns true if valid; false, if otherwise.</returns>
        bool IsValid(TValue obj);
    }
}
