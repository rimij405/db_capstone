/****************************************
 *  MySqlRow.cs
 *  Ian Effendi
 *  ---
 *  Contains the implementation for IRow,
 *  an ICollection<IEntry> interface.
 ****************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTE.DAL.Database.Interfaces;
using Services.Interfaces;

namespace ISTE.DAL.Database.Implementations
{
    
    /// <summary>
    /// MySqlResultSet is a collection of MySqlRows.
    /// </summary>
    public class MySqlResultSet : IResultSet
    {

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Current state of the result set.
        /// </summary>
        private OperationStatus state;

        /// <summary>
        /// Collection of rows.
        /// </summary>
        private List<IRow> rows;

        /// <summary>
        /// Value storing the number of rows affected by a particular query.
        /// </summary>
        int rowsAffected = -1;

        /// <summary>
        /// Query associated with this result set.
        /// </summary>
        string query;

        //////////////////////
        // Properties.
        //////////////////////

        //////
        // IOperationStateMachine

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

        //////
        // IResultSet

        /// <summary>
        /// Collection of <see cref="IRow"/> rows.
        /// </summary>
        public List<IRow> Rows
        {
            get
            {
                if(this.rows == null) { this.rows = new List<IRow>(); }
                return this.rows;
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

        /// <summary>
        /// Gets a count of all fields in a result set, in the order in which they appear.
        /// </summary>
        public List<string> Fields
        {
            get
            {
                List<string> fields = new List<string>();
                if (!this.IsEmpty)
                {
                    foreach (IRow row in this)
                    {
                        if (fields.Count != row.Fields.Count) {
                            foreach (string field in row.Fields)
                            {
                                if (!fields.Contains(field)) { fields.Add(field); }
                            }
                        }
                    }
                }
                return fields;
            }
        }

        //////
        // ICollection

        /// <summary>
        /// Flag determines if result set is readonly.
        /// </summary>
        public bool IsReadOnly
        {
            get; set;
        }

        /// <summary>
        /// Number of rows in the result set.
        /// </summary>
        public int Count
        {
            get { return this.Rows.Count; }
        }

        //////
        // IEmpty

        /// <summary>
        /// Check if result set has any rows.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        public bool IsEmpty
        {
            get { return this.HasExactlyThisMany(0); }
        }

        /// <summary>
        /// Access collection of rows by index.
        /// </summary>
        /// <param name="index">Index of row to return.</param>
        /// <returns>Return the row at the input index value.</returns>
        public IRow this[int index]
        {
            get { return this.rows[index]; }
            private set { this.rows[index] = value; }
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
                if (this.HasIndex(rowIndex)) {
                    IRow row = this[rowIndex];
                    if(row != null && row.HasIndex(fieldIndex))
                    {
                        return row.GetEntry(fieldIndex);
                    }
                }
                return null;
            }
            private set
            {
                if (this.HasIndex(rowIndex))
                {
                    IRow row = this[rowIndex];
                    if (row != null && row.HasIndex(fieldIndex))
                    {
                        row.SetEntry(fieldIndex, value);
                    }
                }
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
                if (this.HasIndex(rowIndex))
                {
                    IRow row = this[rowIndex];
                    if (row != null && row.HasField(key))
                    {
                        return row.GetEntry(key);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// Indexer access all entries for a particular field by field key.
        /// </summary>
        /// <param name="key">Field to access.</param>
        /// <returns>Returns collection of entries. Returns null if there is no field.</returns>
        public List<IEntry> this[string key]
        {
            get
            {
                List<IEntry> entries = new List<IEntry>();
                if (!this.IsEmpty)
                {
                    foreach (IRow row in this)
                    {
                        if (row.HasField(key))
                        {
                            IEntry entry = row.GetEntry(key);
                            if(entry != null) { entries.Add(entry); }
                        }
                    }
                }
                return entries;
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

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
        public MySqlResultSet(string query, int rowsAffected, List<IRow> rows)
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
        /// Copy constructor.
        /// </summary>
        /// <param name="other">Other result set to create.</param>
        public MySqlResultSet(MySqlResultSet other)
            : this(other.Query, other.RowsAffected)
        {
            // Calls other constructor for copy of immutable references.
            // Deep copies rows:
            foreach (IRow row in other)
            {
                // Clones the row that requires deeper copying.
                this.Rows.Add(row.Clone() as IRow);
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        ///////
        // IOperationStateMachine

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
            if(code >= -1 && code < 2)
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

        //////
        // IComparable

        /// <summary>
        /// Check if reference is the same, or if all rows match.
        /// </summary>
        /// <param name="obj">IResultSet to be compared.</param>
        /// <returns>Returns true if equal. False, if otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is MySqlResultSet set)
            {
                if (this.Count == set.Count)
                {
                    for (int index = 0; index < this.Count; index++)
                    {
                        IRow rowA = this[index];
                        IRow rowB = set.GetRow(index);
                        if (!rowA.Equals(rowB)) { return false; }                        
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the hashcode of all the internal rows.
        /// </summary>
        /// <returns>Returns hashcode compilation of all rows.</returns>
        public override int GetHashCode()
        {
            int hash = base.GetHashCode();
            foreach (IRow row in this)
            {
                hash = hash ^ row.GetHashCode();
            }
            return hash;
        }

        /// <summary>
        /// Compare two record sets. Matches based on row-to-row comparison.
        /// </summary>
        /// <param name="obj">IResultSet to be compared.</param>
        /// <returns>Returns zero if equal. Returns greater value if more rows and unequal. Returns smaller value if less rows.</returns>
        public int CompareTo(object obj)
        {
            if (obj != null && obj is IResultSet set)
            {

                // If the same, return 0.
                if (this.Equals(obj)) { return 0; }

                // If not the same, find the difference.
                // If the row count is the same, check if rows are different.
                if (this.Count == set.Count)
                {
                    // If difference, return the compare between them.
                    for (int index = 0; index < this.Count; index++)
                    {
                        IRow rowA = this[index];
                        IRow rowB = set.GetRow(index);
                        if (rowA == null)
                        {
                            if (rowB == null) { continue; }
                            else { return -1; }
                        }
                        else
                        {
                            if (rowB == null) { return 1; }
                            int comparison = rowA.CompareTo(rowB);
                            if (comparison != 0) { return comparison; }
                        }
                    }

                    // If no conflict found, return as equal.
                    return 0;
                }
                else
                {
                    return this.Count.CompareTo(set.Count);
                }
            }

            // If not of a compatible type, return nothing.
            throw new ArgumentException("Input object is not of type IResultSet.");
        }

        //////
        // ICollectionWrapper

        /// <summary>
        /// Check if the result set has at least as many rows as the specified input.
        /// </summary>
        /// <param name="count">Amount of rows to check if result set has.</param>
        /// <returns>Returns true if count is greater than or equal to input value.</returns>
        public bool HasAtLeastThisMany(int count)
        {
            return (this.Count >= count);
        }

        /// <summary>
        /// Check if the result set has exactly as many rows as the specified input.
        /// </summary>
        /// <param name="count">Amount of rows to check if result set has.</param>
        /// <returns>Returns true if count is equal to input value.</returns>
        public bool HasExactlyThisMany(int count)
        {
            return (this.Count == count);
        }
        
        /// <summary>
        /// Check if it has a particular index.
        /// </summary>
        /// <param name="index">Index to check for.</param>
        /// <returns>Returns true if the index is in bounds. False, if otherwise.</returns>
        public bool HasIndex(int index)
        {
            return (index >= 0 && index < this.Count);
        }

        //////
        // ICollection

        /// <summary>
        /// Clear the collection.
        /// </summary>
        public void Clear()
        {
            this.Rows.Clear();
        }

        /// <summary>
        /// Check if collection contains the specified item.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Returns true if found.</returns>
        public bool Contains(IRow item)
        {
            return this.Rows.Contains(item);
        }

        /// <summary>
        /// Copy items into the input array.
        /// </summary>
        /// <param name="array">Array to copy items into.</param>
        /// <param name="arrayIndex">Starting index for array.</param>
        public void CopyTo(IRow[] array, int arrayIndex)
        {
            this.Rows.CopyTo(array, arrayIndex);
        }
        
        //////
        // IReplicate

        /// <summary>
        /// Return a cloned copy of the results.
        /// </summary>
        /// <returns>Returns a results set.</returns>
        public IReplicate Clone()
        {
            return new MySqlResultSet(this);
        }

        //////////////////////
        // Accessor(s).

        //////
        // IOperationStateMachine

        /// <summary>
        /// Return current state.
        /// </summary>
        /// <returns>Returns OperationStatus.</returns>
        public OperationStatus GetState()
        {
            return this.state;
        }

        //////
        // IResultSet

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

        /// <summary>
        /// Returns a row based on its index value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns a single row.</returns>
        public IRow GetRow(int rowIndex)
        {
            // Validate input.
            if (rowIndex < 0 || rowIndex >= this.Count) { return null; }

            // Return row at index.
            return this[rowIndex];
        }

        /// <summary>
        /// Returns a header based on the row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of row</returns>
        public List<string> GetRowHeader(int rowIndex)
        {
            List<string> header = new List<string>();
            IRow row = this.GetRow(rowIndex);
            if (row != null && row.HasFields)
            {
                foreach (string field in row.Fields)
                {
                    header.Add(field);
                }
            }
            return header;
        }

        /// <summary>
        /// Returns the collection of entries referenced by a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entries (by ref).</returns>
        public List<IEntry> GetRowEntries(int rowIndex)
        {
            List<IEntry> rowEntries = new List<IEntry>();
            IRow row = this.GetRow(rowIndex);
            if (row != null)
            {
                foreach (IEntry entry in row.Entries)
                {
                    rowEntries.Add(entry);
                }
            }
            return rowEntries;
        }

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        public List<KeyValuePair<string, string>> GetRowData(int rowIndex)
        {
            return this.GetRowMap(rowIndex).ToList<KeyValuePair<string, string>>();
        }

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        public IDictionary<string, string> GetRowMap(int rowIndex)
        {
            IDictionary<string, string> dictionary = new Dictionary<string, string>();
            IRow row = this.GetRow(rowIndex);
            if(row != null)
            {
                foreach (IEntry entry in row.Entries)
                {
                    dictionary.Add(entry.GetData());
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Return a subset of cloned rows from the current instance, using the input parameters to narrow selection.
        /// </summary>
        /// <param name="start">Start index of subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns new collection of rows.</returns>
        public List<IRow> GetRange(int start = 0, int length = -1)
        {
            List<IRow> subset = new List<IRow>();

            if (start > 0 && start < this.Count)
            {
                foreach (IRow row in this)
                {
                    if (length == -1 || subset.Count < length)
                    {
                        // Add entry (and its associated field).
                        subset.Add(row);
                    }
                }
            }

            return subset;
        }

        /// <summary>
        /// Return an entry from a given [row, col] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds.</returns>
        public IEntry GetEntry(int rowIndex, int fieldIndex)
        {
            IRow row = this.GetRow(rowIndex);
            if (row == null) { return null; }
            return row.GetEntry(fieldIndex);
        }

        /// <summary>
        /// Return an entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldKey">Field to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        public IEntry GetEntry(int rowIndex, string fieldKey)
        {
            IRow row = this.GetRow(rowIndex);
            if(row == null) { return null; }
            return row.GetEntry(fieldKey);
        }

        /// <summary>
        /// Return data from entry from a given [row, col] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds.</returns>
        public KeyValuePair<string, string> GetEntryData(int rowIndex, int fieldIndex)
        {
            IRow row = this.GetRow(rowIndex);
            if (row == null) { return new KeyValuePair<string, string>("", ""); }
            IEntry entry = row.GetEntry(rowIndex);
            return (entry == null) ? new KeyValuePair<string, string>("", "") : entry.GetData();
        }

        /// <summary>
        /// Return data from entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldKey">Field to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        public KeyValuePair<string, string> GetEntryData(int rowIndex, string fieldKey)
        {
            IRow row = this.GetRow(rowIndex);
            if(row == null) { return new KeyValuePair<string, string>("", ""); }
            IEntry entry = row.GetEntry(fieldKey);
            return (entry == null) ? new KeyValuePair<string, string>("", "") : entry.GetData();
        }

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public List<IEntry> GetEntries(int fieldIndex)
        {
            List<IEntry> entries = new List<IEntry>();
            foreach (IRow row in this)
            {
                IEntry entry = row.GetEntry(fieldIndex);
                if (entry != null) { entries.Add(entry); }
            }
            return entries;
        }

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public List<IEntry> GetEntries(string fieldname)
        {
            List<IEntry> entries = null;
            if (!this.IsEmpty)
            {
                int index = this[0].GetIndex(fieldname);
                if (index != -1) { entries = this.GetEntries(index); }
            }
            return entries ?? new List<IEntry>();
        }

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by value).</returns>
        public List<KeyValuePair<string, string>> GetEntryData(int fieldIndex)
        {
            List<KeyValuePair<string, string>> data = new List<KeyValuePair<string, string>>();

            foreach (IRow row in this)
            {
                IEntry entry = row.GetEntry(fieldIndex);
                if(entry != null) { data.Add(entry.GetData()); }
            }

            return data;
        }

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        public List<KeyValuePair<string, string>> GetEntryData(string fieldname)
        {
            List<KeyValuePair<string, string>> data = null;
            if (!this.IsEmpty)
            {
                int index = this[0].GetIndex(fieldname);
                if (index != -1) { data = this.GetEntryData(index); }
            }
            return data ?? new List<KeyValuePair<string, string>>();
        }
        
        //////
        // ICollectionWrapper<IRow>

        /// <summary>
        /// Return the index of a row if it's in the collection.
        /// </summary>
        /// <param name="row">Row to find index for.</param>
        /// <returns>Returns the index of the row. Returns -1 if no row found.</returns>
        public int GetIndex(IRow row)
        {            
            // Return a -1 if empty collection. Otherwise, attempt to find index of input row.
            return ((this.IsEmpty) ? -1 : this.Rows.IndexOf(row));
        }

        /// <summary>
        /// Return the row at the specified index.
        /// </summary>
        /// <param name="index">Index to retrieve item from.</param>
        /// <param name="item">Item to be output.</param>
        /// <returns>Returns true if successfully obtained item. Returns item if index is in bounds.</returns>
        public bool GetItem(int index, out IRow item)
        {
            // Validate input.
            if (!this.HasIndex(index))
            {
                throw new IndexOutOfRangeException($"Cannot get row from collection. Index [{index}] out of range ({this.Count} total element(s)).");
            }

            // Return reference.
            item = this[index];
            return item != null;
        }
        
        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        public IEnumerator<IRow> GetEnumerator()
        {
            return this.Rows.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>Returns an enumerator that iterates through the collection.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        //////////////////////
        // Mutator(s).

        //////
        // IOperationStateMachine

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
        
        //////
        // IResultSet

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

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="row">Row to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRow(int rowIndex, IRow row)
        {
            if(this.IsReadOnly || !this.HasIndex(rowIndex) || row == null) { return null; }
            this.Rows[rowIndex] = row;
            return this;
        }

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="entries">Row entries to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRow(int rowIndex, List<IEntry> entries)
        {
            if (this.IsReadOnly || entries == null) { return null; }
            return this.SetRow(rowIndex, new MySqlRow(entries));
        }

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="data">Row entry data to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRow(int rowIndex, List<KeyValuePair<string, string>> data)
        {
            if (this.IsReadOnly || data == null) { return null; }
            List<IEntry> entries = new List<IEntry>();
            foreach (KeyValuePair<string, string> datum in data)
            {
                entries.Add(new MySqlEntry(datum));
            }
            return this.SetRow(rowIndex, entries);
        }

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="data">Row entry data to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet SetRow(int rowIndex, IDictionary<string, string> data)
        {
            if(this.IsReadOnly || data == null) { return null; }
            return this.SetRow(rowIndex, data.ToList<KeyValuePair<string, string>>());
        }

        /// <summary>
        /// Add a row to the result set.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(IRow row)
        {
            // Add row if not null.
            if (this.IsReadOnly || row == null) { return null; }
            this.Rows.Add(row);
            return this;
        }

        /// <summary>
        /// Add a new row to the result set, containing the entries.
        /// </summary>
        /// <param name="entries">Entries to place into a new row.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(List<IEntry> entries)
        {
            if (this.IsReadOnly || entries == null) { return null; }
            return this.AddRow(new MySqlRow(entries));
        }

        /// <summary>
        /// Add a new row to the result set, containing the entries.
        /// </summary>
        /// <param name="entries">Entries to place into a new row.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(params IEntry[] entries)
        {
            if (this.IsReadOnly || entries == null) { return null; }
            return this.AddRow(entries.ToList<IEntry>());
        }

        /// <summary>
        /// Add a new row to the result set, containing the entries.
        /// </summary>
        /// <param name="data">Data to place into a new row.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(List<KeyValuePair<string, string>> data)
        {
            if (this.IsReadOnly || data == null) { return null; }
            List<IEntry> entries = new List<IEntry>();
            foreach (KeyValuePair<string, string> datum in data)
            {
                entries.Add(new MySqlEntry(datum));
            }
            return this.AddRow(entries);
        }

        /// <summary>
        /// Add a new row to the result set, containing the entries.
        /// </summary>
        /// <param name="data">Data to place into a new row.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(params KeyValuePair<string, string>[] data)
        {
            if (this.IsReadOnly || data == null) { return null; }
            return this.AddRow(data.ToList<KeyValuePair<string, string>>());
        }

        /// <summary>
        /// Add a new row to the result set, containing the entries.
        /// </summary>
        /// <param name="data">Data to place into a new row.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRow(IDictionary<string, string> data)
        {
            if (this.IsReadOnly || data == null) { return null; }
            return this.AddRow(data.ToList<KeyValuePair<string, string>>());
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="resultSet">Result set rows to concatenate.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRows(IResultSet resultSet)
        {
            if (this.IsReadOnly || resultSet == null) { return null; }
            return this.AddRows(resultSet.Rows);
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRows(List<IRow> rows)
        {
            if (this.IsReadOnly || rows == null) { return null; }
            foreach (IRow row in rows)
            {
                this.AddRow(row);
            }
            return this;
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IResultSet AddRows(params IRow[] rows)
        {
            if (this.IsReadOnly || rows == null) { return null; }
            return this.AddRows(rows.ToList<IRow>());
        }

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns removed row. Will throw exception if index out of bounds.</returns>
        public IRow RemoveRow(int rowIndex)
        {
            if (this.IsReadOnly || !this.HasIndex(rowIndex)) { return null; }

            IRow row = this.Rows[rowIndex];
            this.Rows.RemoveAt(rowIndex);
            return row;
        }

        /// <summary>
        /// Remove row based on row matching.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <returns>Returns removed row. Returns null if row is not found.</returns>
        public IRow RemoveRow(IRow row)
        {
            if (this.IsReadOnly) { return null; }
            return this.RemoveRow(this.GetIndex(row));
        }

        //////
        // ICollectionWrapper<IRow>

        /// <summary>
        /// Replace row at specified index, if index is in bounds.
        /// </summary>
        /// <param name="index">Index to replace row at.</param>
        /// <param name="row">Row to be set.</param>
        /// <returns>Return reference to row.</returns>
        public IRow SetItem(int index, IRow row)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            // Validate input.
            if (!this.HasIndex(index))
            {
                throw new IndexOutOfRangeException($"Cannot set row. Index [{index}] out of range ({this.Count} total element(s)).");
            }

            // Overwrite row at current index.
            this[index] = row;

            // Return reference to row.
            return row;
        }

        /// <summary>
        /// Add a row to the result set collection.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Return reference to row.</returns>
        public IRow AddItem(IRow row)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            this.Rows.Add(row);
            return row;
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Return reference to rows.</returns>
        public List<IRow> AddItems(List<IRow> rows)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            foreach (IRow row in rows)
            {
                this.AddItem(row);
            }
            return rows;
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Return reference to self.</returns>
        public List<IRow> AddItems(params IRow[] rows)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            return this.AddItems(rows.ToList<IRow>());
        }

        /// <summary>
        /// Add row to the row list.
        /// </summary>
        /// <param name="item"></param>
        public void Add(IRow item)
        {
            this.AddItem(item);
        }

        /// <summary>
        /// Remove row from the collection.
        /// </summary>
        /// <param name="index">Index of the row to remove.</param>
        /// <returns>Return removed row. Returns null if no row found.</returns>
        public IRow RemoveItem(int index)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            // Validate input.
            if (!this.HasIndex(index))
            {
                throw new IndexOutOfRangeException($"Cannot remove row. Index [{index}] out of range ({this.Count} total element(s)).");
            }

            // Retrieve whatever row exists at that index.
            IRow row = this[index];

            // Remove the row.
            this.Rows.RemoveAt(index);

            // Return retrieved row.
            return row;
        }

        /// <summary>
        /// Remove row if it exists in the collection.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <returns>Return removed row. Returns null if no row found.</returns>
        public IRow RemoveItem(IRow row)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return null; }

            // Remove the input row by index. This row may be null if there is no index.
            int index = this.GetIndex(row);
            return (index == -1) ? null : this.RemoveItem(index);
        }

        /// <summary>
        /// Remove row from the collection.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="row"></param>
        /// <returns></returns>
        public bool RemoveItem(int index, out IRow row)
        {
            // If read only, do not do anything.
            row = null;
            if (this.IsReadOnly) { return false; }

            row = this.RemoveItem(index);
            return (row != null);
        }
                
        /// <summary>
        /// Remove item from collection. Will return false if row doesn't exist in the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns true if operation is successful.</returns>
        public bool Remove(IRow item)
        {
            return (this.RemoveItem(item) != null);
        }
    }

}
