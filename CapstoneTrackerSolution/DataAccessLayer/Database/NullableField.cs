/*
    NullableField.cs
    A nullable field in a result set.
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
    /// Nullable field.
    /// </summary>
    /// <typeparam name="TValue">Type of the field type value.</typeparam>
    public class NullableField<TValue> : Field<TValue>
    {

        // Field(s).

        /// <summary>
        /// Is null flag.
        /// </summary>
        private bool isNull;
        
        // Constructor(s).
        
        /// <summary>
        /// Create a nullable field (of a particular type).
        /// </summary>
        /// <param name="name">Name of the field.</param>
        /// <param name="fieldtype">Type associated with the field.</param>
        public NullableField(string name, IFieldType<TValue> fieldtype) : base(name, fieldtype)
        {
            // Calls base constructor.
        }

        /// <summary>
        /// Create a nullable field from a copy.
        /// </summary>
        /// <param name="field">Field to create a copy from.</param>
        public NullableField(Field<TValue> field) : base(field)
        {
            // Calls base constructor.
        }

        // Service methods.
        
        /// <summary>
        /// Set the null value for the field.
        /// </summary>
        /// <param name="isNull">Value to make null flag.</param>
        /// <returns>Returns reference to self.</returns>
        public IField SetNull(bool isNull)
        {
            this.isNull = isNull;
            return this;
        }

        /// <summary>
        /// Returns the flag indicating if the value is currently null.
        /// </summary>
        /// <returns>Returns isNull flag.</returns>
        public bool IsNull()
        {
            return this.isNull;
        }

        /// <summary>
        /// This is a nullable implementation of IField.
        /// </summary>
        /// <returns>Returns true.</returns>
        public override bool IsNullable()
        {
            return true;
        }
    }
}
