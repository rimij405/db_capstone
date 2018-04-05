/*
    IField.cs
    Field in a result set.
    ***
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database
{

    /// <summary>
    /// A field represented by results in the database.
    /// </summary>
    public interface IField
    {

        /// <summary>
        /// Return the field name.
        /// </summary>
        /// <returns>Returns formatted string field name.</returns>
        string GetFieldName();

        /// <summary>
        /// Check if the field is nullable.
        /// </summary>
        /// <returns>Returns true if field can contain a null value.</returns>
        bool IsNullable();

    }
}
