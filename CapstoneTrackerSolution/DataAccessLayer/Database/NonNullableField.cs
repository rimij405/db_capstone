/*
    NonNullableField.cs
    A non-nullable field in a result set.
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
    /// A non-nullable field.
    /// </summary>
    /// <typeparam name="TValue">Type of the field type value.</typeparam>
    public class NonNullableField<TValue> : Field<TValue>
    {

        // Constructor(s).

        /// <summary>
        /// Create a non-nullable field (of a particular type).
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="fieldtype">Type associated with the field.</param>
        public NonNullableField(string name, IFieldType<TValue> fieldtype) : base(name, fieldtype)
        {
            // Calls base constructor.
        }

        /// <summary>
        /// Create a non-nullable field from a copy.
        /// </summary>
        /// <param name="field">Field to create a copy from.</param>
        public NonNullableField(Field<TValue> field) : base(field)
        {
            // Calls base constructor.
        }

        // Service methods.
        
        /// <summary>
        /// This is a non-nullable implementation of IField.
        /// </summary>
        /// <returns>Returns true.</returns>
        public override bool IsNullable()
        {
            return false;
        }
    }
}
