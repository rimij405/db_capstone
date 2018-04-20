/*
    Field.cs
    Abstract class to inherit from.
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
    /// Abstract field.
    /// </summary>
    /// <typeparam name="TValue">Value to associate with the field type.</typeparam>
    public abstract class Field<TValue> : IField
    {

        // Field(s).
        
        /// <summary>
        /// Field type of the field.
        /// </summary>
        private IFieldType<TValue> fieldType;

        /// <summary>
        /// Name of the field.
        /// </summary>
        private string fieldName;

        // Properties.

        /// <summary>
        /// Reference to the field's type.
        /// </summary>
        public IFieldType<TValue> FieldType
        {
            get { return this.fieldType; }
            set { this.fieldType = value; }
        }

        // Constructor(s).

        /// <summary>
        /// Create a nullable field (of a particular type).
        /// </summary>
        public Field(string name, IFieldType<TValue> fieldtype)
        {
            this.fieldName = name.Trim();
            this.fieldType = fieldtype;
        }

        /// <summary>
        /// Create a copy of the input field.
        /// </summary>
        /// <param name="field"></param>
        public Field(Field<TValue> field)
        {
            this.fieldName = field.GetFieldName();
            this.fieldType = field.GetFieldType();
        }

        // Service methods.

        /// <summary>
        /// Return this field's field name.
        /// </summary>
        /// <returns>Returns string formatted name.</returns>
        public string GetFieldName()
        {
            return this.fieldName;
        }

        /// <summary>
        /// Returns this field's field type.
        /// </summary>
        /// <returns>Returns this field's associated field type.</returns>
        public IFieldType<TValue> GetFieldType()
        {
            return this.fieldType;
        }

        /// <summary>
        /// This is a nullable implementation of IField.
        /// </summary>
        /// <returns>Returns false.</returns>
        public abstract bool IsNullable();

    }
}
