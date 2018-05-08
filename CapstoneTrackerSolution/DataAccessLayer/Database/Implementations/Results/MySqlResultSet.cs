/****************************************
 *  MySqlResultSet.cs
 *  Ian Effendi
 *  ---
 *  Contains the implementation for IResultSet.
 ****************************************/

// using statements.
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements.
using Services.Interfaces;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database.Implementations
{
    
    /// <summary>
    /// MySqlResultSet is a collection of MySqlRows.
    /// </summary>
    public class MySqlResultSet : FieldHandler, IResultSet
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        #region Static Members

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Create and return a result set.
        /// </summary>
        /// <param name="rows">Rows to make result set from.</param>
        /// <returns>Returns a new row.</returns>
        public static IResultSet CreateResultSet(IList<IRow> rows)
        {
            return new MySqlResultSet(rows as List<IRow>);
        }

        /// <summary>
        /// Create and return a result set.
        /// </summary>
        /// <param name="rows">Rows to make result set from.</param>
        /// <returns>Returns a new row.</returns>
        public static IResultSet CreateResultSet(params IRow[] rows) { return MySqlResultSet.CreateResultSet(rows.ToList<IRow>()); }

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Cast into a MySqlResultSet object.
        /// </summary>
        /// <param name="rows">Rows to cast.</param>
        public static implicit operator MySqlResultSet(List<IRow> rows)
        {
            return new MySqlResultSet(rows);
        }
        
        /// <summary>
        /// Cast into a MySqlRow object.
        /// </summary>
        /// <param name="data">Dataset to cast.</param>
        public static implicit operator MySqlResultSet(List<IDictionary<string, string>> data)
        {
            return new MySqlResultSet(MySqlRow.CreateRows(data));
        }

        /// <summary>
        /// Cast into a collection of a collection of entries.
        /// </summary>
        /// <param name="results">Result set to cast.</param>
        public static explicit operator List<IList<IEntry>>(MySqlResultSet results)
        {
            List<IList<IEntry>> list = new List<IList<IEntry>>();
            foreach (IRow row in results)
            {
                list.Add(row.Entries);
            }
            return list;
        }

        /// <summary>
        /// Cast into a collection of a collection of fields.
        /// </summary>
        /// <param name="results">Result set to cast.</param>
        public static explicit operator List<IList<string>>(MySqlResultSet results)
        {
            List<IList<string>> rowFields = new List<IList<string>>();
            foreach (IRow row in results)
            {
                rowFields.Add(row.Fields);
            }
            return rowFields;
        }

        /// <summary>
        /// Cast into a collection of a collection of key/value pairs.
        /// </summary>
        /// <param name="results">Result set to cast.</param>
        public static explicit operator List<IDictionary<string, string>>(MySqlResultSet results)
        {
            List<IDictionary<string, string>> collection = new List<IDictionary<string, string>>();
            foreach (IRow row in results)
            {
                collection.Add(row.Data);
            }
            return collection;
        }

        #endregion

        //////////////////////
        // Field(s).
        //////////////////////

        #region Fields

        /// <summary>
        /// Collection of rows.
        /// </summary>
        private IList<IRow> rows;

        /// <summary>
        /// Current state of the result set.
        /// </summary>
        private OperationStatus state;

        /// <summary>
        /// Value storing the number of rows affected by a particular query.
        /// </summary>
        int rowsAffected = -1;

        /// <summary>
        /// Query associated with this result set.
        /// </summary>
        string query;

        #endregion

        //////////////////////
        // Properties.
        //////////////////////

        #region Properties

        #region IResultSet

        /// <summary>
        /// Collection of <see cref="IRow"/> rows.
        /// </summary>
        public IList<IRow> Rows
        {
            get
            {
                if (this.rows == null) { this.rows = new List<IRow>(); }
                return this.rows;
            }
        }

        /// <summary>
        /// Gets a count of all fields in a result set, in the order in which they appear.
        /// </summary>
        public IList<string> Fields
        {
            get
            {
                return this.GetRowHeader(0);
            }
        }

        /// <summary>
        /// All the data in a MySqlResultSet.
        /// </summary>
        public IList<IDictionary<string, string>> Data
        {
            get
            {
                List<IDictionary<string, string>> dataSet = new List<IDictionary<string, string>>();
                foreach (IRow row in this)
                {
                    dataSet.Add(row.Data);
                }
                return dataSet;
            }
        }

        /// <summary>
        /// Number of rows affected by an SQL query.
        /// </summary>
        public int RowsAffected
        {
            get { return this.rowsAffected; }
            private set { this.rowsAffected = value; }
        }

        /// <summary>
        /// SQL query associated with a result set.
        /// </summary>
        public string Query
        {
            get { return this.query; }
            private set { this.query = value.Trim().Replace('\n', ' '); }
        }

        #endregion

        #region IListWrapper.

        /// <summary>
        /// IListWrapper reference.
        /// </summary>
        public IList<IRow> List
        {
            get { return this.Rows; }
        }

        /// <summary>
        /// Number of rows in the result set.
        /// </summary>
        public int Count
        {
            get { return this.List.Count; }
        }

        #endregion

        #region IReadOnly

        /// <summary>
        /// Flag determines if result set is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                if (this.IsEmpty) { return false; }
                foreach (IRow row in this)
                {
                    if (!row.IsReadOnly) { return false; }
                }
                return true;
            }
            set
            {
                foreach (IRow row in this)
                {
                    row.IsReadOnly = value;
                }
            }
        }

        #endregion

        #region IEmpty

        /// <summary>
        /// Check if result set has any rows.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        public bool IsEmpty
        {
            get { return (this.Count == 0); }
        }

        #endregion

        #region IOperationStateMachine

        /// <summary>
        /// Reference to state of the result set.
        /// </summary>
        public OperationStatus State
        {
            get { return this.state; }
        }

        /// <summary>
        /// Check if in error state.
        /// </summary>
        public bool IsError
        {
            get { return this.IsState(OperationStatus.ERROR); }
        }

        /// <summary>
        /// Check if in failure state.
        /// </summary>
        public bool IsFailure
        {
            get { return this.IsState(OperationStatus.FAILURE); }
        }

        /// <summary>
        /// Check if in success state.
        /// </summary>
        public bool IsSuccess
        {
            get { return this.IsState(OperationStatus.SUCCESS); }
        }

        #endregion

        //////////////////////
        // Indexer(s).

        #region Indexers.

        /// <summary>
        /// Return an element from the collection.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        /// <returns>Returns object at specific index.</returns>
        public IRow this[int index]
        {
            get
            {
                return this.GetRow(index);
            }
            set
            {
                this.SetRow(index, value);
            }
        }

        /// <summary>
        /// Returns index of input element, if it exists in the collection.
        /// </summary>
        /// <param name="element">Element to find index of.</param>
        /// <returns>Returns index of specific object.</returns>
        public int this[IRow element]
        {
            get
            {
                return this.GetIndex(element);
            }
        }

        /// <summary>
        /// Return an entry from the result set.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <returns>Return entry at coordinate.</returns>
        public IEntry this[int rowIndex, int fieldIndex]
        {
            get
            {
                if (this.TryGetRow(rowIndex, out IRow row))
                {
                    return row.GetEntry(fieldIndex);
                }
                return null;
            }
        }

        /// <summary>
        /// Indexer access to entry, by row and fieldname.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="key">Field to access.</param>
        /// <returns>Returns entry if it exists. Returns null if there is no entry or no row.</returns>
        public IEntry this[int rowIndex, string key]
        {
            get
            {
                if (this.TryGetRow(rowIndex, out IRow row)) {
                    return row.GetEntry(key);
                }
                return null;
            }
        }

        /// <summary>
        /// Indexer access all entries for a particular field by field key.
        /// </summary>
        /// <param name="key">Field to access.</param>
        /// <returns>Returns collection of entries. Returns null if there is no field.</returns>
        public IList<IEntry> this[string key]
        {
            get
            {
                IList<IEntry> entries = new List<IEntry>();
                foreach (IRow row in this)
                {
                    if (row.TryGetEntry(key, out IEntry entry))
                    {
                        entries.Add(entry);
                    }
                }
                return entries;
            }
        }

        #endregion

        #endregion

        //////////////////////
        // Constructor(s).
        //////////////////////

        #region Constructor(s).

        /// <summary>
        /// Construct an empty <see cref="MySqlResultSet"/> <see cref="IResultSet"/>.
        /// </summary>
        public MySqlResultSet()
            : this("", -1)
        {
            // Call other constructor. Unknown amount of rows affected. (-1 means no query has been processed).
        }

        /// <summary>
        /// Construct a result set using just the rows affected and the query.
        /// </summary>
        /// <param name="query">SQL query associated with result set.</param>
        /// <param name="rowsAffected">Rows affected by execution of query. (Can be zero).</param>
        public MySqlResultSet(string query, int rowsAffected)
        {
            // Set the initial state.
            this.SetState(OperationStatus.NULL);

            // Set the query.
            this.Query = query;

            // Set the amount of rows affected.
            this.RowsAffected = rowsAffected;
        }

        /// <summary>
        /// Construct a result set using the query, the affected row count, and the actual rows filled with data.
        /// </summary>
        /// <param name="query">SQL query associated with result set.</param>
        /// <param name="rowsAffected">Rows affected by execution of query. (Can be zero).</param>
        /// <param name="rows">Rows to add to the result set.</param>
        public MySqlResultSet(string query, int rowsAffected, IList<IRow> rows)
            : this(query, rowsAffected)
        {
            // Calls other constructor.
            // Makes a shallow copy. This means the reference is the same and the collection is NOT duplicated.
            this.rows = rows;
        }

        /// <summary>
        /// Construct a result set using the query, the affected row count, and the actual rows filled with data.
        /// </summary>
        /// <param name="query">SQL query associated with result set.</param>
        /// <param name="rowsAffected">Rows affected by execution of query. (Can be zero).</param>
        /// <param name="rows">Rows to add to the result set.</param>
        public MySqlResultSet(string query, int rowsAffected, params IRow[] rows)
            : this(query, rowsAffected, rows.ToList<IRow>())
        {
            // Calls other constructor.
        }

        /// <summary>
        /// Construct a result set from a collection of maps.
        /// </summary>
        /// <param name="rowData">Data per row.</param>
        public MySqlResultSet(IList<IDictionary<string, string>> rowData)
        {
            foreach (Dictionary<string, string> data in rowData)
            {
                this.AddRow(new MySqlRow((IDictionary<string, string>) data));
            }
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Other result set to create.</param>
        public MySqlResultSet(MySqlResultSet other)
            : this(other.Query, other.RowsAffected)
        {
            // Copy.
            if (other != null && this != other && !this.IsEqual(other))
            {
                // Clone the collection of entries.
                foreach (IRow row in other)
                {
                    this.rows.Add(row.Clone());
                }
            }
        }

        #endregion

        //////////////////////
        // Method(s).
        //////////////////////

        #region Methods

        //////////////////////
        // Service(s).

        #region Service methods.

        #region General methods.

        /// <summary>
        /// Return this result set's metadata as a string.
        /// </summary>
        /// <returns>Returns the result set as a formatted string.</returns>
        public override string ToString()
        {
            // Example:
            // [MySqlResultSet] {(Empty set). 'SELECT * FROM capstonedb.users' - 0 rows affected.}
            return $"[MySqlResultSet] {{({((this.IsEmpty) ? "Empty set" : $"{this.Count} total elements")}). '{((this.Query.Length == 0) ? "No query set" : this.Query)}' - {this.RowsAffected} rows affected.}}";
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
            if (this.IsEmpty) { return false; }
            IRow row = this.GetRow(0);
            return (row != null) ? this.GetRow(0).HasField(key) : false;
        }

        #endregion
        
        #region IResultSetComparison - Size, Field, Value<T>, Value, and Total comparisons.

        #region Total Comparisons - Compare(object), Compare(IRow), IsEqual(IRow)

        /// <summary>
        /// Compare to current instance.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns></returns>
        public int CompareTo(object obj)
        {
            if (obj == null) { return 1; }
            if (this == obj) { return 0; }
            if (obj is IResultSet that)
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
        public int Compare(IResultSet other)
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
        public bool IsEqual(IResultSet other)
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
        public int CompareSize(IResultSet left, IResultSet right)
        {
            return left.List.Count.CompareTo(right.List.Count);
        }

        /// <summary>
        /// Compare two collection sizes.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        public bool IsEqualSize(IResultSet left, IResultSet right) { return (this.CompareSize(left, right) == 0); }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanSize(IResultSet left, IResultSet right) { return (this.CompareSize(left, right) >= 1); }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanSize(IResultSet left, IResultSet right) { return (this.CompareSize(left, right) <= -1); }

        #endregion

        #region Field comparisons. - CompareFields(IRow, IRow), IsEqualFields(IRow, IRow), IsGreaterThanFields(IRow, IRow), IsLessThanFields(IRow, IRow)

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        public int CompareFields(IResultSet left, IResultSet right)
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
        public bool IsEqualFields(IResultSet left, IResultSet right) { return (this.CompareFields(left, right) == 0); }

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        public bool IsGreaterThanField(IResultSet left, IResultSet right) { return (this.CompareFields(left, right) >= 1); }

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        public bool IsLessThanField(IResultSet left, IResultSet right) { return (this.CompareFields(left, right) <= -1); }

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

            if (left is IResultSet leftSet && right is IResultSet rightSet)
            {
                return this.CompareValue(left, right);
            }

            if (left is IResultSet) { return 1; }
            if (right is IResultSet) { return -1; }

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
        public int CompareValue(IResultSet left, IResultSet right)
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
        public bool IsEqualValue(IResultSet left, IResultSet right)
        {
            if (this.CompareFields(left, right) != 0) { return false; }

            IList<IRow> leftEntries = left.Rows;
            IList<IRow> rightEntries = right.Rows;

            (leftEntries as List<IRow>).Sort();
            (rightEntries as List<IRow>).Sort();

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
        public bool IsGreaterThanValue(IResultSet left, IResultSet right)
        {
            if (this.CompareFields(left, right) >= 1) { return true; }

            IList<IRow> leftEntries = left.Rows;
            IList<IRow> rightEntries = right.Rows;

            (leftEntries as List<IRow>).Sort();
            (rightEntries as List<IRow>).Sort();

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
        public bool IsLessThanValue(IResultSet left, IResultSet right)
        {
            if (this.CompareFields(left, right) <= -1) { return true; }

            IList<IRow> leftEntries = left.Rows;
            IList<IRow> rightEntries = right.Rows;

            (leftEntries as List<IRow>).Sort();
            (rightEntries as List<IRow>).Sort();

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
        public bool HasElement(IRow element)
        {
            if (element == null) { return false; }
            return this.List.Contains(element);
        }

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasElements(IList<IRow> elements)
        {
            foreach (IRow row in elements)
            {
                if (!this.HasElement(row)) { return false; }
            }
            return true;
        }

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasElements(params IRow[] elements) { return this.HasElements(elements.ToList<IRow>()); }

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasAnyElements(IList<IRow> elements)
        {
            foreach (IRow row in elements)
            {
                if (this.HasElement(row)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        public bool HasAnyElements(params IRow[] elements) { return this.HasElements(elements.ToList<IRow>()); }

        #endregion

        #region IResultSet - HasFields(List<string>), HasFields(params string[])

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
        public IResultSet MakeEmpty()
        {
            this.List.Clear();
            return this;
        }

        #endregion

        #region IReplicate - IResultSet Clone()

        /// <summary>
        /// Clone an object.
        /// </summary>
        /// <param name="obj">Object to clone.</param>
        /// <returns>Create a clone.</returns>
        public object Clone(object obj)
        {
            if (obj is IResultSet row)
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
        public IResultSet Clone()
        {
            return new MySqlResultSet(this);
        }

        #endregion

        #region IOperationStateMachine - IsState(OperationStatus), IsState(int), Error(), Fail(), Pass()

        /// <summary>
        /// Check if matches input state.
        /// </summary>
        /// <param name="other">State to compare.</param>
        /// <returns>Returns true if match made.</returns>
        public bool IsState(OperationStatus other)
        {
            return this.state == other;
        }

        /// <summary>
        /// Check if matches input state, by integer code.
        /// </summary>
        /// <param name="code">Code to find comparison state.</param>
        /// <returns>Returns true if match made.</returns>
        public bool IsState(int code)
        {
            if (code >= -1 && code < 2)
            {
                return this.state == (OperationStatus)code;
            }
            return false;
        }

        /// <summary>
        /// Set to the error state.
        /// </summary>
        public void Error()
        {
            this.SetState(OperationStatus.ERROR);
        }

        /// <summary>
        /// Set to the fail state.
        /// </summary>
        public void Fail()
        {
            this.SetState(OperationStatus.FAILURE);
        }

        /// <summary>
        /// Set to the pass state.
        /// </summary>
        public void Pass()
        {
            this.SetState(OperationStatus.SUCCESS);
        }

        #endregion

        #endregion

        //////////////////////
        // Helper(s).

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
        // Accessor(s).

        #region Accessor methods.

        #region IListWrapper

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerator<IRow> GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        #region int GetIndex(IRow), IEntry GetElement(int), bool TryGetElement(int, out IRow)

        /// <summary>
        /// Return the index of a row if it's in the collection.
        /// </summary>
        /// <param name="row">Row to find index for.</param>
        /// <returns>Returns the index of the row. Returns -1 if no row found.</returns>
        public int GetIndex(IRow row)
        {
            if (this.IsEmpty || row == null || !this.HasElement(row)) { return -1; }
            return this.List.IndexOf(row);
        }
        
        /// <summary>
        /// Return element at specified index. Returns null if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        public IRow GetElement(int index)
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
        public bool TryGetElement(int index, out IRow result)
        {
            result = this.GetElement(index);
            return (result != null);
        }

        #endregion

        #endregion

        #region IResultSet

        #region int GetRowsAffected(void), string GetQuery(void)

        /// <summary>
        /// Return the number of rows affected.
        /// </summary>
        /// <returns>Returns stored number of rows.</returns>
        public int GetRowsAffected()
        {
            return this.RowsAffected;
        }

        /// <summary>
        /// Query associated with this result set.
        /// </summary>
        /// <returns>Returns the stored query.</returns>
        public string GetQuery()
        {
            return this.Query;
        }

        #endregion
        
        #region int GetIndex(string), string GetFieldName(int), bool TryGetFieldName(int, out string)

        /// <summary>
        /// Return index of specified fieldname. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="fieldname">Fieldname to access.</param>
        /// <returns>Returns index.</returns>
        public int GetIndex(string fieldname)
        {
            int index = -1;
            if (this.TryGetRow(0, out IRow row))
            {
                index = row.GetIndex(fieldname);
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

        #region IRow GetRow(int), bool TryGetRow(int, out IRow)

        /// <summary>
        /// Return row at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns row. Throws exception if index out of bounds.</returns>
        public IRow GetRow(int index)
        {
            return this.GetElement(index);
        }

        /// <summary>
        /// Return row at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="row">Entry to retrieve.</param>
        /// <returns>Returns row. Throws exception if index out of bounds.</returns>
        public bool TryGetRow(int index, out IRow row)
        {
            row = this.GetRow(index);
            return (row != null);
        }

        #endregion

        #region IList<string> GetRowHeader(int), IList<IEntry> GetRowEntries(int), IDictionary<string, string> GetRowMap(int), IList<KeyValuePair<string, string>> GetRowData(int)

        /// <summary>
        /// Returns a header based on the row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of row</returns>
        public IList<string> GetRowHeader(int rowIndex)
        {
            if (this.TryGetRow(rowIndex, out IRow row))
            {
                return row.Fields;
            }
            return new List<string>();
        }

        /// <summary>
        /// Returns the collection of entries referenced by a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entries (by ref).</returns>
        public IList<IEntry> GetRowEntries(int rowIndex)
        {
            if (this.TryGetRow(rowIndex, out IRow row))
            {
                return row.Entries;
            }
            return new List<IEntry>();
        }

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        public IDictionary<string, string> GetRowMap(int rowIndex)
        {
            if (this.TryGetRow(rowIndex, out IRow row))
            {
                return row.Data;
            }
            return new Dictionary<string, string>();
        }

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        public IList<KeyValuePair<string, string>> GetRowData(int rowIndex)
        {
            return this.GetRowMap(rowIndex).ToList<KeyValuePair<string, string>>();
        }

        #endregion
        
        #region IEntry GetEntry(int, int), bool TryGetEntry(int, int, out IEntry), IEntry GetEntry(int, string), bool TryGetEntry(int, string, out IEntry)

        /// <summary>
        /// Return row index referenced row's entry at field index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Field index to access.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        public IEntry GetEntry(int rowIndex, int colIndex)
        {
            if (this.TryGetRow(rowIndex, out IRow row))
            {
                return row.GetEntry(colIndex);
            }
            return null;
        }

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Field index to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        public bool TryGetEntry(int rowIndex, int colIndex, out IEntry entry)
        {
            entry = this.GetEntry(rowIndex, colIndex);
            return (entry != null);
        }

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        public IEntry GetEntry(int rowIndex, string fieldname)
        {
            if (this.TryGetRow(rowIndex, out IRow row))
            {
                return row.GetEntry(fieldname);
            }
            return null;
        }

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        public bool TryGetEntry(int rowIndex, string fieldname, out IEntry entry)
        {
            entry = this.GetEntry(rowIndex, fieldname);
            return (entry != null);
        }

        #endregion

        #region IList<IRow> - GetRange(int), GetRange(int, int)

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IRow> GetRange(int start = 0)
        {
            if (start == 0) { return this.Rows; }
            if (!this.IsValidIndex(start)) { return new List<IRow>(); }
            return (this.Rows as List<IRow>).GetRange(start, this.Count);
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public IList<IRow> GetRange(int start = 0, int length = -1)
        {
            if (length <= -1 || length >= this.Count) { return this.GetRange(start); }
            if (length == 0 || !this.IsValidIndex(start)) { return new List<IRow>(); }
            return (this.Rows as List<IRow>).GetRange(start, length);
        }

        #endregion

        #region KeyValuePair<string, string> GetData(int, int), bool TryGetData(int, int, KeyValuePair<string, string>),  KeyValuePair<string, string> GetData(int, string), bool TryGetData(int, string, KeyValuePair<string, string>)  

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Field index to get data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        public KeyValuePair<string, string> GetData(int rowIndex, int colIndex)
        {
            if (!this.TryGetEntry(rowIndex, colIndex, out IEntry entry)) { return new KeyValuePair<string, string>("", ""); }
            return entry.Data;
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Field index to get data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        public bool TryGetData(int rowIndex, int colIndex, out KeyValuePair<string, string> data)
        {
            data = this.GetData(rowIndex, colIndex);
            return (!String.IsNullOrWhiteSpace(data.Key));
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        public KeyValuePair<string, string> GetData(int rowIndex, string fieldname)
        {
            if (!this.TryGetEntry(rowIndex, fieldname, out IEntry entry)) { return new KeyValuePair<string, string>("", ""); }
            return entry.Data;
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        public bool TryGetData(int rowIndex, string fieldname, out KeyValuePair<string, string> data)
        {
            data = this.GetData(rowIndex, fieldname);
            return (!String.IsNullOrWhiteSpace(data.Key));
        }

        #endregion

        #region IList<IEntry> GetEntries(int), IList<IEntry> GetEntries(string), IList<KeyValuePair<string, string>> GetData(int), IList<KeyValuePair<string, string>> GetData(string) 

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public IList<IEntry> GetEntries(int fieldIndex)
        {
            List<IEntry> entries = new List<IEntry>();
            foreach (IRow row in this)
            {
                if(row.TryGetEntry(fieldIndex, out IEntry entry))
                {
                    entries.Add(entry);
                }
            }
            return entries;
        }

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public IList<IEntry> GetEntries(string fieldname)
        {
            List<IEntry> entries = new List<IEntry>();
            foreach (IRow row in this)
            {
                if (row.TryGetEntry(fieldname, out IEntry entry))
                {
                    entries.Add(entry);
                }
            }
            return entries;
        }

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by value).</returns>
        public IList<KeyValuePair<string, string>> GetData(int fieldIndex)
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            foreach (IRow row in this)
            {
                if (row.TryGetEntry(fieldIndex, out IEntry entry))
                {
                    data.Add(entry.Data);
                }
            }
            return data;
        }

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public IList<KeyValuePair<string, string>> GetData(string fieldname)
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();
            foreach (IRow row in this)
            {
                if (row.TryGetEntry(fieldname, out IEntry entry))
                {
                    data.Add(entry.Data);
                }
            }
            return data;
        }

        #endregion
        
        #endregion

        #region IOperationStateMachine - GetState()

        /// <summary>
        /// Return current state.
        /// </summary>
        /// <returns>Returns OperationStatus.</returns>
        public OperationStatus GetState()
        {
            return this.state;
        }

        #endregion

        #endregion

        //////////////////////
        // Mutator(s).

        #region Mutator methods.

        #region IListWrapper

        #region IRow AddElement(IRow), bool TryAddElement(IRow, out IRow), bool AddElements(IList<IRow>), bool AddElements(params IRow[] elements)

        /// <summary>
        /// Add element to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        public IRow AddElement(IRow element)
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
        public bool TryAddElement(IRow element, out IRow result)
        {
            result = this.AddElement(element);
            return (result != null);
        }

        /// <summary>
        /// Add a collection of elements to the current collection. Returns false if any elements already exist in the set.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        public bool AddElements(IList<IRow> elements)
        {
            if (this.HasAnyElements(elements))
            {
                return false;
            }

            // Validate all entries.
            foreach (IRow entry in elements)
            {
                if (entry == null) { return false; }
            }

            // Add the elements.
            (this.Rows as List<IRow>).AddRange(elements);
            return true;
        }

        /// <summary>
        /// Add a collection of elements to the current collection.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        public bool AddElements(params IRow[] elements) { return this.AddElements(elements.ToList<IRow>()); }

        #endregion

        #region IRow SetElement(int, IRow), bool TrySetElement(int, IRow, out IRow)

        /// <summary>
        /// Set existing index to reference the input element.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        public IRow SetElement(int index, IRow element)
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
        public bool TrySetElement(int index, IRow element, out IRow result)
        {
            result = this.SetElement(index, element);
            return (result != null);
        }

        #endregion

        #region IRow RemoveElement(IRow), bool TryRemoveElement(IRow, out IRow), IRow RemoveAt(int), bool TryRemoveAt(int, out IRow)

        /// <summary>
        /// Remove element from the collection if it exists. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns removed element.</returns>
        public IRow RemoveElement(IRow element)
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
        public bool TryRemoveElement(IRow element, out IRow result)
        {
            result = this.RemoveElement(element);
            return (result != null);
        }

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns removed element.</returns>
        public IRow RemoveAt(int index)
        {
            if (!this.IsValidIndex(index)) { return null; }
            IRow result = this.GetElement(index);
            if (result != null) { this.List.RemoveAt(index); }
            return result;
        }

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="result">Element to return.</param>
        /// <returns>Returns operation success.</returns>
        public bool TryRemoveAt(int index, out IRow result)
        {
            result = this.RemoveAt(index);
            return (result != null);
        }

        #endregion

        #endregion

        #region IResultSet

        #region IResultSet SetRowsAffected(int), IResultSet SetQuery(string)

        /// <summary>
        /// Set the value of rows affected due to query execution.
        /// </summary>
        /// <param name="rowsAffected">Value to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRowsAffected(int rowsAffected)
        {            
            // Can only be -1 or higher. -1 means unknown. 0 and up are valid responses for a query execution.
            this.RowsAffected = (rowsAffected >= -1) ? rowsAffected : -1;
            return this;
        }

        /// <summary>
        /// Set the SQL query associated with this result set.
        /// </summary>
        /// <param name="query">Query to set.</param>
        /// <returns>Returns the stored query.</returns>
        public IResultSet SetQuery(string query)
        {
            this.Query = query;
            return this;
        }

        #endregion

        #region IResultSet SetRow(int, IRow), bool TrySetRow(int, IRow, out IRow)

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="index">Row index to access.</param>
        /// <param name="row">Row to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRow(int index, IRow row)
        {
            return (this.TrySetElement(index, row.Clone(), out IRow result)) ? this : null;
        }

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="index">Row index to access.</param>
        /// <param name="row">Row to set.</param>
        /// <returns>Returns reference to self.</returns>
        public bool TrySetRow(int index, IRow row)
        {
            return (this.SetRow(index, row) != null);
        }

        #endregion

        #region IResultSet AddRow(IRow), bool TryAddRow(IRow row), bool AddRows(IList<IRow>), bool AddRows(params IRow[]), bool AddRows(IResultSet)

        /// <summary>
        /// Add a row to the result set.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(IRow row)
        {
            // Add row if not null.
            if (this.IsReadOnly || row == null) { return null; }
            return (this.TryAddElement(row, out IRow result)) ? this : null;
        }

        /// <summary>
        /// Add a row to the result set.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Operation success.</returns>
        public bool TryAddRow(IRow row)
        {
            return (this.AddRow(row) != null);
        }
        
        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        public bool AddRows(IList<IRow> rows)
        {
            if (this.IsReadOnly || rows == null) { return false; }
            foreach (IRow row in rows)
            {
                this.AddRow(row);
            }
            return true;
        }
        
        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        public bool AddRows(params IRow[] rows)
        {
            return this.AddRows(rows.ToList<IRow>());
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="resultSet">Result set rows to concatenate.</param>
        /// <returns>Returns reference to self.</returns>
        public bool AddRows(IResultSet resultSet)
        {
            return this.AddRows(resultSet.Rows);
        }

        #endregion

        #region IRow RemoveRow(int), bool TryRemoveRow(int, out IRow), IRow RemoveRow(IRow), IRow TryRemoveRow(IRow, out IRow)

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns removed row. Will throw exception if index out of bounds.</returns>
        public IRow RemoveRow(int rowIndex)
        {
            if (this.TryRemoveAt(rowIndex, out IRow result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="results">Results to return.</param>
        /// <returns>Operation success.</returns>
        public bool TryRemoveRow(int rowIndex, out IRow results)
        {
            results = this.RemoveRow(rowIndex);
            return (results != null);
        }

        /// <summary>
        /// Remove row based on row matching.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <returns>Returns removed row. Returns null if row is not found.</returns>
        public IRow RemoveRow(IRow row)
        {
            if (this.IsReadOnly) { return null; }
            if (this.TryRemoveElement(row, out IRow result))
            {
                return result;
            }
            return null;
        }

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <param name="results">Results to return.</param>
        /// <returns>Operation success.</returns>
        public bool TryRemoveRow(IRow row, out IRow results)
        {
            results = this.RemoveRow(row);
            return (results != null);
        }

        #endregion
                
        #endregion

        #region IOperationStateMachine - SetState(OperationStatus), bool SetState(int)

        /// <summary>
        /// Set the current state to input value.
        /// </summary>
        /// <param name="other">Value to assign.</param>
        public void SetState(OperationStatus other)
        {
            this.state = other;
        }

        /// <summary>
        /// Set the current state based on input integer code.
        /// </summary>
        /// <param name="code">Reference to value to assign.</param>
        public bool SetState(int code)
        {
            if (code >= -1 && code < 2)
            {
                OperationStatus status = (OperationStatus)code;
                this.SetState(status);
                return true;
            }
            return false;
        }

        #endregion

        #endregion

        #endregion

    }

}
