/****************************************
 *  MySqlRow.cs
 *  Ian Effendi
 *  ---
 *  Contains the implementation for IRow,
 *  an ICollection<IEntry> interface.
 ****************************************/

 // using statements.
using System;
using System.Collections.Generic;
using System.Linq;

// additional using statements.
using Services.Interfaces;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database.Implementations
{

    /*********************************************************
     * Developer's Note:
     * 
     * The entry collection stores entry objects. You may
     * be wondering why we store the field name twice.
     * 
     * We trade off a slight count against performance,
     * for the sake of convienience. This enables us to
     * handle record data by passing a MySqlEntry object
     * without sacrificing the context that a field gives us.
     * 
     * We then wrap them in an IRow object, because 
     * it enables us more flexibility over when and where
     * we construct our tabular data structures.
     * *******************************************************/

    /*********************************************************
     * Developer's Note:
     * 
     * Fields stored are trimmed of whitespace and set to
     * uppercase. Input checked against the collection should
     * also be trimmed and set to uppercase as the collection
     * is both whitespace and case insensitive.
     * *******************************************************/

    /// <summary>
    /// Represents a row of data returned by MySql.
    /// </summary>
    public class MySqlRow : FieldHandler, IRow
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        #region Static Members.

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Create and return a row.
        /// </summary>
        /// <param name="entries">Entries to make row from.</param>
        /// <returns>Returns a new row.</returns>
        public static IRow CreateRow(List<IEntry> entries)
        {
            return new MySqlRow((IList<IEntry>) entries);
        }
        
        /// <summary>
        /// Create and return a row.
        /// </summary>
        /// <param name="entries">Entries to make row from.</param>
        /// <returns>Returns a new row.</returns>
        public static IRow CreateRow(params IEntry[] entries) { return MySqlRow.CreateRow(entries.ToList<IEntry>()); }

        /// <summary>
        /// Create set of rows.
        /// </summary>
        /// <param name="data">Data collection.</param>
        /// <returns>Return collection of entries.</returns>
        public static List<IRow> CreateRows(List<IDictionary<string, string>> data)
        {
            List<IRow> rows = new List<IRow>();
            foreach (Dictionary<string, string> rowData in data)
            {
                rows.Add(new MySqlRow((IDictionary<string, string>) rowData));
            }
            return rows;
        }

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Cast into a MySqlRow object.
        /// </summary>
        /// <param name="entries">Entries to cast.</param>
        public static explicit operator MySqlRow(List<IEntry> entries)
        {
            return new MySqlRow((IList<IEntry>) entries);
        }

        /// <summary>
        /// Cast into a MySqlRow object.
        /// </summary>
        /// <param name="fields">Fieldnames to cast.</param>
        public static explicit operator MySqlRow(List<string> fields)
        {
            return new MySqlRow(fields);
        }
        
        /// <summary>
        /// Cast into a MySqlRow object.
        /// </summary>
        /// <param name="data">Dataset to cast.</param>
        public static implicit operator MySqlRow(Dictionary<string, string> data)
        {
            return new MySqlRow((IDictionary<string, string>) data);
        }

        /// <summary>
        /// Cast into a collection of entries.
        /// </summary>
        /// <param name="row">Row to cast.</param>
        public static explicit operator List<IEntry>(MySqlRow row)
        {
            return row.Entries as List<IEntry>;
        }
        
        /// <summary>
        /// Cast into a collection of fields.
        /// </summary>
        /// <param name="row">Row to cast.</param>
        public static explicit operator List<string>(MySqlRow row)
        {
            return row.Fields as List<string>;
        }

        /// <summary>
        /// Cast into a collection of key/value pairs.
        /// </summary>
        /// <param name="row">Row to cast.</param>
        public static explicit operator Dictionary<string, string>(MySqlRow row)
        {
            return row.Data as Dictionary<string, string>;
        }

        #endregion

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Represents a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        private IList<IEntry> entryCollection;

        //////////////////////
        // Properties.
        //////////////////////

        #region Properties

        #region IRow

        /// <summary>
        /// Return the field collection.
        /// </summary>
        public IList<string> Fields
        {
            get
            {
                List<string> fields = new List<string>();
                foreach (IEntry entry in this)
                {
                    fields.Add(entry.Field);
                }
                return fields;
            }
        }

        /// <summary>
        /// Return the entry collection.
        /// </summary>
        public IList<IEntry> Entries
        {
            get
            {
                return this.entryCollection ?? (this.entryCollection = new List<IEntry>());
            }
        }

        /// <summary>
        /// All the data in a MySqlRow.
        /// </summary>
        public IDictionary<string, string> Data
        {
            get
            {
                Dictionary<string, string> dataSet = new Dictionary<string, string>();
                foreach (IEntry entry in this)
                {
                    dataSet.Add(entry.Field, entry.Value);
                }
                return dataSet;
            }
        }

        #endregion

        #region IListWrapper

        /// <summary>
        /// Reference to the collection in question.
        /// </summary>
        public IList<IEntry> List
        {
            get { return this.Entries; }
        }

        /// <summary>
        /// Returns the number of fields in the row.
        /// </summary>
        public int Count
        {
            get { return this.List.Count; }
        }

        #endregion

        #region IReadOnly

        /// <summary>
        /// Flag determines whether or not collections are mutable.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (this.IsEmpty) { return false; }
                foreach (IEntry entry in this)
                {
                    if (!entry.IsReadOnly) { return false; }
                }
                return true;
            }
            set
            {
                foreach (IEntry entry in this)
                {
                    entry.IsReadOnly = value;
                }
            }
        }

        #endregion

        #region IEmpty

        /// <summary>
        /// Check if this can be empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return (this.Count == 0);
            }
        }

        #endregion

        #region INullable

        /// <summary>
        /// Check if the instance is a custom version of null.
        /// </summary>
        public bool IsNull
        {
            get
            {
                if (this.IsEmpty) { return true; }
                foreach (IEntry entry in this)
                {
                    if (!entry.IsNull) { return false; }
                }
                return true;
            }
        }

        #endregion
        
        //////////////////////
        // Indexer(s).

        #region Indexer(s).

        /// <summary>
        /// Return an element from the collection.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        /// <returns>Returns object at specific index.</returns>
        public IEntry this[int index]
        {
            get
            {
                return this.GetEntry(index);
            }
            set
            {
                this.SetEntry(index, value);
            }
        }

        /// <summary>
        /// Returns index of input element, if it exists in the collection.
        /// </summary>
        /// <param name="element">Element to find index of.</param>
        /// <returns>Returns index of specific object.</returns>
        public int this[IEntry element]
        {
            get
            {
                return this.GetIndex(element);
            }
        }

        /// <summary>
        /// Return entry with a given field. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="field">Field for entry to retrieve.</param>
        /// <returns>Return entry at field.</returns>
        public IEntry this[string field]
        {
            get
            {
                return this.GetEntry(field);
            }
        }

        #endregion

        #endregion

        //////////////////////
        // Constructor(s).
        //////////////////////

        #region Constructors

        /// <summary>
        /// Empty constructor. A row with no entries.
        /// </summary>
        public MySqlRow() {
            this.IsReadOnly = false;
        }

        /// <summary>
        /// Row created from a single field.
        /// </summary>
        /// <param name="fieldname">Fields.</param>
        public MySqlRow(string fieldname) : this()
        {
            if (!String.IsNullOrWhiteSpace(fieldname))
            {
                this.AddField(fieldname);
            }
        }

        /// <summary>
        /// Row created from a collection of fields.
        /// </summary>
        /// <param name="fieldnames">Fields.</param>
        public MySqlRow(IList<string> fieldnames) : this()
        {
            if (fieldnames != null && fieldnames.Count > 0)
            {
                this.AddFields(fieldnames);
            }
        }

        /// <summary>
        /// Row created from a collection of fields.
        /// </summary>
        /// <param name="fieldnames">Fields.</param>
        public MySqlRow(params string[] fieldnames) : this()
        {
            if (fieldnames != null && fieldnames.Length > 0)
            {
                this.AddFields(fieldnames);
            }
        }

        /// <summary>
        /// Row created from a single entry.
        /// </summary>
        /// <param name="entry">Entry.</param>
        public MySqlRow(IEntry entry) : this()
        {
            if (entry != null)
            {
                this.AddEntry(entry);
            }
        }

        /// <summary>
        /// Row created from a collection of entries.
        /// </summary>
        /// <param name="entries">Entries.</param>
        public MySqlRow(IList<IEntry> entries) : this()
        {
            if (entries != null && entries.Count > 0)
            {
                this.AddEntries(entries);
            }
        }

        /// <summary>
        /// Row created from a collection of entries.
        /// </summary>
        /// <param name="entries">Entries.</param>
        public MySqlRow(params IEntry[] entries) : this()
        {
            if (entries != null && entries.Length > 0)
            {
                this.AddEntries(entries);
            }
        }

        /// <summary>
        /// Construct a row from a dictionary.
        /// </summary>
        /// <param name="entries">Data describing entries.</param>
        public MySqlRow(IDictionary<string, string> entries) : this()
        {
            if (entries != null && entries.Count > 0)
            {
                this.AddEntries(MySqlEntry.CreateEntries(entries));
            }
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Row to clone.</param>
        public MySqlRow(IRow other) : this()
        {
            if (other != null && this != other && !this.IsEqual(other))
            {
                // Clone the collection of entries.
                foreach (IEntry entry in other.Entries)
                {
                    this.entryCollection.Add(entry.Clone());
                }
            }
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Row to clone.</param>
        public MySqlRow(MySqlRow other) : this()
        {
            if (other != null && this != other && !this.IsEqual(other))
            {
                // Clone the collection of entries.
                foreach (IEntry entry in other)
                {
                    this.entryCollection.Add(entry.Clone());
                }
            }
        }

        #endregion

        //////////////////////
        // Method(s).
        //////////////////////

        #region Methods

        //////////////////////
        // Service method(s).

        #region Service methods.

        #region General Methods.

        /// <summary>
        /// Return a formatted string describing a single entry.
        /// </summary>
        /// <returns>Returns formatted string.</returns>
        public override string ToString()
        {
            string body = "";
            body += $"{{MySqlRow: Entries: {{";

            if (this.IsEmpty) { body += " No entries to print. "; }
            else
            {
                foreach (IEntry entry in this)
                {
                    body += $"\n{entry.ToString()}\n";
                }
            }

            body += $"}}, IsNull: {this.IsNull}}}.";
            return body;
        }

        #endregion

        #region FieldHandler - HasField(string)

        /// <summary>
        /// Check if a field exists.
        /// </summary>
        /// <param name="key">Key to check.</param>
        /// <returns>Returns true if found.</returns>
        public override bool HasField(string key)
        {
            foreach (IEntry entry in this)
            {
                if (entry.HasField(key)) { return true; }
            }
            return false;
        }

        #endregion
        
        #region IRowComparison - Size, Field, Value<T>, Value, and Total comparisons.

        #region Total Comparisons - Compare(object), Compare(IRow), IsEqual(IRow)

        /// <summary>
        /// Compare to current instance.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if(obj == null) { return 1; }
            if(this == obj) { return 0; }
            if (obj is IRow that)
            {
                return this.Compare(that);
            }
            return 1;
        }
        
        /// <summary>
        /// Check instances.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public int Compare(IRow other)
        {
            if (other == null) { return 1; }
            if (this == other) { return 0; }
            return this.CompareValue(this, other);
        }

        /// <summary>
        /// Check instances for equality.
        /// </summary>
        /// <param name="other">Other instance.</param>
        /// <returns>Return comparison result.</returns>
        public bool IsEqual(IRow other)
        {
            if (other == null) { return false; }
            if (this == other) { return true; }
            return (this.IsEqualValue(this, other));
        }

        #endregion

        #region Size Comparisons. - CompareSize(IRow, IRow), IsEqualSize(IRow, IRow), IsGreaterThanSize(IRow, IRow), IsLessThanSize(IRow, IRow)

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareSize(IRow left, IRow right)
        {
            return left.List.Count.CompareTo(right.List.Count);
        }

        /// <summary>
        /// Compare two collection sizes.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
       public bool IsEqualSize(IRow left, IRow right) { return (this.CompareSize(left, right) == 0); }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
       public bool IsGreaterThanSize(IRow left, IRow right) { return (this.CompareSize(left, right) >= 1); }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
       public bool IsLessThanSize(IRow left, IRow right) { return (this.CompareSize(left, right) <= -1); }

        #endregion

        #region Field comparisons. - CompareFields(IRow, IRow), IsEqualFields(IRow, IRow), IsGreaterThanFields(IRow, IRow), IsLessThanFields(IRow, IRow)

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareFields(IRow left, IRow right)
        {
            int comparison = this.CompareSize(left, right);
            if (comparison != 0) { return comparison; }

            // Same size of fields.
            IList<string> leftfields = left.Fields;
            IList<string> rightfields = right.Fields;

            // Sort the fields.
            (leftfields as List<string>).Sort();
            (rightfields as List<string>).Sort();

            // Compare each field, and, return first difference found when comparing.
            for (int i = 0; i < leftfields.Count; i++)
            {
                int fieldComparison = leftfields[i].CompareTo(rightfields[i]);
                if (fieldComparison != 0) { return fieldComparison; }
            }

            return comparison;
        }

        /// <summary>
        /// Compare two fields.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualFields(IRow left, IRow right) { return (this.CompareFields(left, right) == 0); }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanField(IRow left, IRow right) { return (this.CompareFields(left, right) >= 1); }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanField(IRow left, IRow right) { return (this.CompareFields(left, right) <= -1); }

        #endregion

        #region Value<T> comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareValue<T>(T left, T right) where T : IComparable
        {
            if (left == null && right == null) { return 0; }
            if (left == null) { return -1; }
            if (right == null) { return 1; }

            if(left is IRow leftRow && right is IRow rightRow)
            {
                return this.CompareValue(leftRow, rightRow);
            }

            if(left is IRow) { return 1; }
            if(right is IRow) { return -1; }

            return left.CompareTo(right);
        }

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualValue<T>(T left, T right) where T : IComparable { return (this.CompareValue<T>(left, right) == 0); }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanValue<T>(T left, T right) where T : IComparable { return (this.CompareValue<T>(left, right) >= 1); }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanValue<T>(T left, T right) where T : IComparable { return (this.CompareValue<T>(left, right) <= -1); }

        #endregion

        #region Value comparisons.

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareValue(IRow left, IRow right)
        {
            int comparison = 1;
            comparison = (this.IsGreaterThanValue(left, right)) ? 1 : comparison;
            comparison = (this.IsLessThanValue(left, right)) ? -1 : comparison;
            comparison = (this.IsEqualValue(left, right)) ? 0 : comparison;
            return comparison;
        }

        /// <summary>
        /// Compare two values.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualValue(IRow left, IRow right)
        {
            if(this.CompareFields(left, right) != 0) { return false; }
            
            IList<IEntry> leftEntries = left.Entries;
            IList<IEntry> rightEntries = right.Entries;

            (leftEntries as List<IEntry>).Sort();
            (rightEntries as List<IEntry>).Sort();

            for (int i = 0; i < leftEntries.Count; i++)
            {
                if (!leftEntries[i].IsEqual(rightEntries[i])) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanValue(IRow left, IRow right)
        {
            if (this.CompareFields(left, right) >= 1) { return true; }

            IList<IEntry> leftEntries = left.Entries;
            IList<IEntry> rightEntries = right.Entries;

            (leftEntries as List<IEntry>).Sort();
            (rightEntries as List<IEntry>).Sort();

            for (int i = 0; i < leftEntries.Count; i++)
            {
                int comparison = leftEntries[i].Compare(rightEntries[i]);
                if (leftEntries[i].Compare(rightEntries[i]) != 0)
                {
                    return (comparison >= 1);
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanValue(IRow left, IRow right)
        {
            if (this.CompareFields(left, right) <= -1) { return true; }

            IList<IEntry> leftEntries = left.Entries;
            IList<IEntry> rightEntries = right.Entries;

            (leftEntries as List<IEntry>).Sort();
            (rightEntries as List<IEntry>).Sort();

            for (int i = 0; i < leftEntries.Count; i++)
            {
                int comparison = leftEntries[i].Compare(rightEntries[i]);
                if (leftEntries[i].Compare(rightEntries[i]) != 0)
                {
                    return (comparison <= -1);
                }
            }

            return false;
        }

        #endregion

        #endregion

        #region IListWrapper<IEntry> - HasElement(IEntry), HasElements(IList<IEntry>), HasElements(params IEntry[]), HasAnyElements(IList<IEntry>), HasAnyElements(params IEntry[])

        /// <summary>
        /// Checks if the element is contained within the collection.
        /// </summary>
        /// <param name="element">Element to check.</param>
        /// <returns>Returns true if element is found.</returns>
        public bool HasElement(IEntry element)
        {
            if (element == null) { return false; }
            return this.List.Contains(element);
        }

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasElements(IList<IEntry> elements)
        {
            foreach (IEntry entry in elements)
            {
                if (!this.HasElement(entry)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasElements(params IEntry[] elements) { return this.HasElements(elements.ToList<IEntry>()); }

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasAnyElements(IList<IEntry> elements)
        {
            foreach (IEntry entry in elements)
            {
                if (this.HasElement(entry)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasAnyElements(params IEntry[] elements) { return this.HasElements(elements.ToList<IEntry>()); }

        #endregion

        #region IRow - HasFields(List<string>), HasFields(params string[])

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        public bool HasFields(IList<string> fieldnames)
        {
            foreach (string field in fieldnames)
            {
                if (!this.HasField(field)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        public bool HasFields(params string[] fieldnames) { return this.HasFields(fieldnames.ToList<string>()); }

        #endregion

        #region IEmpty - MakeEmpty(void)

        /// <summary>
        /// When called, turns the instance into its 'empty' form.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public IRow MakeEmpty()
        {
            this.List.Clear();
            return this;
        }

        #endregion

        #region INullable - MakeNull(void)

        /// <summary>
        /// Make all entries in the row, null.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public IRow MakeNull()
        {
            foreach (IEntry entry in this)
            {
                entry.MakeNull();
            }
            return this;
        }

        #endregion

        #region IReplicate - IRow Clone()

        /// <summary>
        /// Clone an object.
        /// </summary>
        /// <param name="obj">Object to clone.</param>
        /// <returns>Create a clone.</returns>
        public object Clone(object obj)
        {
            if (obj is IRow row)
            {
                return row.Clone();
            }
            if (obj is IReplicate copy)
            {
                return copy.Clone(copy);
            }
            return null;
        }

        /// <summary>
        /// Create a deep-copy of this row.
        /// </summary>
        /// <returns>Returns a row, cloned from this row.</returns>
        public IRow Clone()
        {
            return new MySqlRow(this);
        }

        #endregion

        #endregion

        //////////////////////
        // Helper method(s).

        #region Helper methods.

        #region IListWrapper - IsValidIndex(int)

        /// <summary>
        /// Check if index is valid.
        /// </summary>
        /// <param name="index">Index to check.</param>
        /// <returns>Returns true if in bounds.</returns>
        public bool IsValidIndex(int index)
        {
            return (index < this.List.Count && index >= 0);
        }

        #endregion

        #endregion

        //////////////////////
        // Accessor method(s).

        #region Accessor methods.

        #region IListWrapper

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerator<IEntry> GetEnumerator()
        {
            return this.Entries.GetEnumerator();
        }
        
        #region int GetIndex(IEntry), IEntry GetElement(int), bool TryGetElement(int, out IEntry)

        /// <summary>
        /// Return index of input element. Returns -1 if element doesn't exist in collection.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        public int GetIndex(IEntry element)
        {
            if (this.IsEmpty || element == null || !this.HasElement(element)) { return -1; }
            return this.List.IndexOf(element);
        }

        /// <summary>
        /// Return element at specified index. Returns null if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        public IEntry GetElement(int index)
        {
            if (!this.IsValidIndex(index)) { return null; }
            return this.List[index];
        }

        /// <summary>
        /// Return element at specified index via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="result">Element to affect.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        public bool TryGetElement(int index, out IEntry result)
        {
            result = this.GetElement(index);
            return (result != null);
        }

        #endregion
        
        #endregion

        #region IRow

        #region int GetIndex(string), string GetFieldName(int), bool TryGetFieldName(int, out string)

        /// <summary>
        /// Return index of specified fieldname. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="fieldname">Fieldname to access.</param>
        /// <returns>Returns index.</returns>
        public int GetIndex(string fieldname)
        {
            int index = -1;
            if (this.HasField(fieldname))
            {
                foreach (IEntry entry in this)
                {
                    index++;
                    if (entry.HasField(fieldname)) { return index; }
                }
            }
            return index;
        }

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns formatted fieldname. Throws exception if index out of bounds.</returns>
        public string GetFieldName(int index)
        {
            if (!IsValidIndex(index)) { return null; }
            return this.Fields[index];
        }

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to return.</param>
        /// <returns>Returns formatted fieldname. Throws exception if index out of bounds.</returns>
        public bool TryGetFieldName(int index, out string fieldname)
        {
            fieldname = this.GetFieldName(index);
            return (!String.IsNullOrEmpty(fieldname));
        }

        #endregion

        #region IEntry GetEntry(int), bool TryGetEntry(int, out IEntry), IEntry GetEntry(string), bool TryGetEntry(string, out IEntry)

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        public IEntry GetEntry(int index)
        {
            return this.GetElement(index);
        }

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        public bool TryGetEntry(int index, out IEntry entry)
        {
            entry = this.GetEntry(index);
            return (entry != null);
        }

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        public IEntry GetEntry(string fieldname)
        {
            if (!this.IsValidField(fieldname) || !this.HasField(fieldname)) { return null; }
            foreach (IEntry entry in this)
            {
                if (entry.HasField(fieldname)) { return entry; }
            }
            return null;
        }

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        public bool TryGetEntry(string fieldname, out IEntry entry)
        {
            entry = this.GetEntry(fieldname);
            return (entry != null);
        }

        #endregion

        #region KeyValuePair<string, string> GetData(int), bool TryGetData(int, KeyValuePair<string, string>),  KeyValuePair<string, string> GetData(string), bool TryGetData(string, KeyValuePair<string, string>)  

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        public KeyValuePair<string, string> GetData(int index)
        {
            if (!this.TryGetEntry(index, out IEntry entry)) { return new KeyValuePair<string, string>("", ""); }
            return entry.Data;
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        public bool TryGetData(int index, out KeyValuePair<string, string> data)
        {
            data = this.GetData(index);
            return (!String.IsNullOrWhiteSpace(data.Key));
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        public KeyValuePair<string, string> GetData(string fieldname)
        {
            if (!this.TryGetEntry(fieldname, out IEntry entry)) { return new KeyValuePair<string, string>("", ""); }
            return entry.Data;
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        public bool TryGetData(string fieldname, out KeyValuePair<string, string> data)
        {
            data = this.GetData(fieldname);
            return (!String.IsNullOrWhiteSpace(data.Key));
        }

        #endregion

        #region string GetValue(int), bool TryGetValue(int, out string), string GetValue(string), bool TryGetValue(string, out string)

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        public string GetValue(int index)
        {
            if (!this.TryGetEntry(index, out IEntry entry)) { return null; }
            return entry.Value;
        }

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to retrieve.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        public bool TryGetValue(int index, out string value)
        {
            value = this.GetValue(index);
            return (value == null); // (Note: Value can be an 'empty' string.
        }

        /// <summary>
        /// Return entry value from the specified field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns stored value. Returns a "NULL" if the field doesn't exist.</returns>
        public string GetValue(string fieldname)
        {
            if (!this.TryGetEntry(fieldname, out IEntry entry)) { return null; }
            return entry.Value;
        }

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to retrieve.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        public bool TryGetValue(string fieldname, out string value)
        {
            value = this.GetValue(fieldname);
            return (value == null); // (Note: Value can be an 'empty' string.
        }

        #endregion

        #region List<IEntry> - GetRange(int), GetRange(int, int), GetRange(List<string>), GetRange(params string[])

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IEntry> GetRange(int start = 0)
        {
            if (start == 0) { return this.Entries; }
            if (!this.IsValidIndex(start)) { return new List<IEntry>(); }
            return (this.Entries as List<IEntry>).GetRange(start, this.Count);
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IEntry> GetRange(int start = 0, int length = -1)
        {
            if (length <= -1 || length >= this.Count) { return this.GetRange(start); }
            if (length == 0 || !this.IsValidIndex(start)) { return new List<IEntry>(); }
            return (this.Entries as List<IEntry>).GetRange(start, length);
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IEntry> GetRange(IList<string> fieldnames)
        {
            List<IEntry> range = new List<IEntry>();
            foreach (string field in fieldnames)
            {
                if (this.HasField(field))
                {
                    range.Add(this.GetEntry(field));
                }
            }
            return range;
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IEntry> GetRange(params string[] fieldnames) { return this.GetRange(fieldnames.ToList<string>()); }

        #endregion

        #region IRow - GetRowRange(int), GetRowRange(int, int), GetRowRange(List<string>), GetRowRange(params string[])

        /// <summary>
        /// Returns a subset of this row, as a new row, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(int start = 0)
        {
            return new MySqlRow(this.GetRange(start));
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(int start = 0, int length = -1)
        {
            return new MySqlRow(this.GetRange(start, length));
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(IList<string> fieldnames)
        {
            return new MySqlRow(this.GetRange(fieldnames));
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(params string[] fieldnames) { return this.GetRowRange(fieldnames.ToList<string>()); }

        #endregion

        #endregion

        #endregion

        //////////////////////
        // Mutator method(s).

        #region Mutator methods.

        #region IListWrapper

        #region IEntry AddElement(IEntry), bool TryAddElement(IEntry, out IEntry), bool AddElements(IList<IEntry>), bool AddElements(params IEntry[] elements)

        /// <summary>
        /// Add element to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        public IEntry AddElement(IEntry element)
        {
            if (this.HasElement(element)) { return null; }
            if (element == null) { return null; }
            this.List.Add(element);
            return element;
        }

        /// <summary>
        /// Add element to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Resulting element.</param>
        /// <returns>Returns operation success.</returns>
        public bool TryAddElement(IEntry element, out IEntry result)
        {
            result = this.AddElement(element);
            return (result != null);
        }

        /// <summary>
        /// Add a collection of elements to the current collection. Returns false if any elements already exist in the set.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        public bool AddElements(IList<IEntry> elements)
        {
            if (this.HasAnyElements(elements))
            {
                return false;
            }

            // Validate all entries.
            foreach (IEntry entry in elements)
            {
                if (entry == null) { return false; }
            }

            // Add the elements.
            (this.Entries as List<IEntry>).AddRange(elements);
            return true;
        }

        /// <summary>
        /// Add a collection of elements to the current collection.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        public bool AddElements(params IEntry[] elements) { return this.AddElements(elements.ToList<IEntry>()); }

        #endregion

        #region IEntry SetElement(int, IEntry), bool TrySetElement(int, IEntry, out IEntry)

        /// <summary>
        /// Set existing index to reference the input element.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        public IEntry SetElement(int index, IEntry element)
        {
            if (!this.IsValidIndex(index)) { return null; }
            if (element == null) { return null; }
            this.List[index] = element;
            return this.List[index];
        }

        /// <summary>
        /// Set existing index to reference the input element.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Resulting element.</param>
        /// <returns>Returns operation success.</returns>
        public bool TrySetElement(int index, IEntry element, out IEntry result)
        {
            result = this.SetElement(index, element);
            return (result != null);
        }

        #endregion

        #region IEntry RemoveElement(IEntry), bool TryRemoveElement(IEntry, out IEntry), IEntry RemoveAt(int), bool TryRemoveAt(int, out IEntry)

        /// <summary>
        /// Remove element from the collection if it exists. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns removed element.</returns>
        public IEntry RemoveElement(IEntry element)
        {
            if (!this.HasElement(element)) { return null; }
            this.List.Remove(element);
            return element;
        }

        /// <summary>
        /// Remove element from the collection if it exists. Returns null if element was not found or could not be removed via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Element to return.</param>
        /// <returns>Returns operation success.</returns>
        public bool TryRemoveElement(IEntry element, out IEntry result)
        {
            result = this.RemoveElement(element);
            return (result != null);
        }

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns removed element.</returns>
        public IEntry RemoveAt(int index)
        {
            if (!this.IsValidIndex(index)) { return null; }
            IEntry result = this.GetElement(index);
            if (result != null) { this.List.RemoveAt(index); }
            return result;
        }

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="result">Element to return.</param>
        /// <returns>Returns operation success.</returns>
        public bool TryRemoveAt(int index, out IEntry result)
        {
            result = this.RemoveAt(index);
            return (result != null);
        }

        #endregion

        #endregion

        #region IRow
    
        ////////////////
        // SetFieldName

        #region IRow SetFieldName(int, string), bool TrySetFieldName(int, string)

        /// <summary>
        /// Set the fieldname at a particular index, formatting the input, renaming the entry associated with the index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Field to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetFieldName(int index, string fieldname)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryGetEntry(index, out IEntry entry))
            {
                if (entry.TrySetField(fieldname))
                {
                    return this;
                }
            }
            return null;
        }

        /// <summary>
        /// Set the fieldname at a particular index, formatting the input, renaming the entry associated with the index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Field to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetFieldName(int index, string fieldname)
        {
            IRow result = this.SetFieldName(index, fieldname);
            return (result != null);
        }

        #endregion
        
        ////////////////
        // SetEntry

        #region IRow SetEntry(int, IEntry), bool TrySetEntry(int, IEntry), IRow SetEntry(string, IEntry), bool TrySetEntry(string, IEntry), IRow SetEntry(string, string), bool TrySetEntry(string, string)

        /// <summary>
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(int index, IEntry entry)
        {
            if (this.IsReadOnly) { return null; }
            return (this.TrySetElement(index, entry.Clone(), out IEntry result)) ? this : null;
        }

        /// <summary>
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetEntry(int index, IEntry entry)
        {
            return (this.SetEntry(index, entry) != null);
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(string fieldname, IEntry entry)
        {
            if (this.IsReadOnly) { return null; }
            return this.SetEntry(this.GetIndex(fieldname), entry);
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetEntry(string fieldname, IEntry entry)
        {
            return (this.SetEntry(fieldname, entry) != null);
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Entry value to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(string fieldname, string value)
        {
            if (this.IsReadOnly) { return null; }
            return this.SetEntry(fieldname, new MySqlEntry(fieldname, value));
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Entry value to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetEntry(string fieldname, string value)
        {
            return (this.SetEntry(fieldname, value) != null);
        }

        #endregion
        
        ////////////////
        // SetValue

        #region IRow SetValue(IEntry), bool TrySetValue(IEntry), IRow SetValue(int, string), bool TrySetValue(int, string), IRow SetValue(string, string), bool TrySetValue(string, string)

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetValue(IEntry entry)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryGetEntry(entry.Field, out IEntry set))
            {
                set.SetValue(entry.Value);
                return this;
            }
            return null;
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetValue(IEntry entry)
        {
            return (this.SetValue(entry) != null);
        }

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        public IRow SetValue(int index, string value)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryGetEntry(index, out IEntry set))
            {
                set.SetValue(value);
                return this;
            }
            return null;
        }

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        public bool TrySetValue(int index, string value)
        {
            return (this.SetValue(index, value) != null);
        }

        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        public IRow SetValue(string fieldname, string value)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryGetEntry(fieldname, out IEntry set))
            {
                set.SetValue(value);
                return this;
            }
            return null;
        }

        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        public bool TrySetValue(string fieldname, string value)
        {
            return (this.SetValue(fieldname, value) != null);
        }

        #endregion

        ////////////////
        // AddFields

        #region bool AddField(string), bool AddFields(List<string>), bool AddFields(params string[])

        /// <summary>
        /// Add a unqiue fieldname, formatting the input and creating a null entry to go along with it.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <returns>Return null entry that has been associated with the field.</returns>
        public bool AddField(string fieldname)
        {
            if (this.IsReadOnly) { return false; }
            if (String.IsNullOrWhiteSpace(fieldname) || this.HasField(fieldname)) { return false; }
            return this.AddEntry(new MySqlEntry(fieldname));
        }

        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public bool AddFields(IList<string> fieldnames)
        {
            if (this.IsReadOnly) { return false; }
            foreach (string field in fieldnames)
            {
                if (String.IsNullOrWhiteSpace(field) || this.HasField(field)) { return false; }
            }
            foreach (string field in fieldnames)
            {
                this.AddField(field);
            }
            return true;
        }

        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public bool AddFields(params string[] fieldnames) { return this.AddFields(fieldnames.ToList<string>()); }

        #endregion

        ////////////////
        // AddEntries

        #region IEntry AddEntry(IEntry), bool AddEntry(string, IEntry), bool AddEntry(string, string), bool AddEntries(List<IEntry>), bool AddEntries(params IEntry[]), bool AddEntries(IRow row)

        /// <summary>
        /// Add entry, if and only if, it has a unique fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        public bool AddEntry(IEntry entry)
        {
            // Check if field already exists.
            if (this.IsReadOnly || entry == null || this.HasField(entry.Field)) { return false; }
            return (this.TryAddElement(entry, out IEntry result));
        }

        /// <summary>
        /// Add entry, using input unique fieldname. Will clone entry and overwrite clone's fieldname with input.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        public bool AddEntry(string fieldname, IEntry entry)
        {
            if (this.IsReadOnly || entry == null || this.HasField(fieldname)) { return false; }
            return this.AddEntry(fieldname, entry.Value);
        }

        /// <summary>
        /// Add entry, using input unique fieldname and input value.
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="value">Value to create new entry with.</param>
        /// <returns>Returns reference to newly created entry.</returns>
        public bool AddEntry(string fieldname, string value)
        {
            if (this.IsReadOnly || this.HasField(fieldname)) { return false; }
            return (this.TryAddElement(new MySqlEntry(fieldname, value), out IEntry result));
        }

        /// <summary>
        /// Add entries.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public bool AddEntries(IList<IEntry> entries)
        {
            if (this.IsReadOnly) { return false; }

            foreach (IEntry entry in entries)
            {
                if (this.HasField(entry.Field)) { return false; }
            }

            return (this.AddElements(entries));
        }

        /// <summary>
        /// Add entries.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public bool AddEntries(params IEntry[] entries)
        {
            return (this.AddEntries(entries.ToList<IEntry>()));
        }

        /// <summary>
        /// Add entries.
        /// </summary>
        /// <param name="row">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public bool AddEntries(IRow row)
        {
            return (this.AddEntries(row.Entries));
        }
        
        #endregion

        ////////////////
        // RemoveFields

        #region IEntry RemoveField(int), bool TryRemoveField(int, out IEntry), IEntry RemoveField(string), bool TryRemoveField(string, out IEntry)

        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        public IEntry RemoveField(int index)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryGetFieldName(index, out string field))
            {
                return this.RemoveField(field);
            }
            return null;
        }

        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry.</returns>
        public bool TryRemoveField(int index, out IEntry result)
        {
            result = this.RemoveField(index);
            return (result != null);
        }

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveField(string fieldname)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryRemoveEntry(fieldname, out IEntry result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry.</returns>
        public bool TryRemoveField(string fieldname, out IEntry result)
        {
            result = this.RemoveField(fieldname);
            return (result != null);
        }

        #endregion

        ////////////////
        // RemoveEntries

        #region IEntry RemoveEntry(int), bool TryRemoveEntry(int, out IEntry), IEntry RemoveField(string), bool TryRemoveField(string, out IEntry), IEntry RemoveEntry(string), bool TryRemoveEntry(string, out IEntry), IEntry RemoveEntry(IEntry), bool TryRemoveEntry(IEntry, out IEntry)

        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        public IEntry RemoveEntry(int index)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryRemoveAt(index, out IEntry result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        public bool TryRemoveEntry(int index, out IEntry result)
        {
            result = this.RemoveEntry(index);
            return (result != null);
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveEntry(string fieldname)
        {
            if (this.IsReadOnly) { return null; }
            IEntry entry = this.GetEntry(fieldname);
            if(entry == null) { return null; }
            if (this.TryRemoveElement(entry, out IEntry result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public bool TryRemoveEntry(string fieldname, out IEntry result)
        {
            result = this.RemoveEntry(fieldname);
            return (result != null);
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveEntry(IEntry entry)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryRemoveElement(entry, out IEntry result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public bool TryRemoveEntry(IEntry entry, out IEntry result)
        {
            result = this.RemoveEntry(entry);
            return (result != null);
        }

        #endregion

        #endregion

        #endregion

        #endregion
        
    }

}
