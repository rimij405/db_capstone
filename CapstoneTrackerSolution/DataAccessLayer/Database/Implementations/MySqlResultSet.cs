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

namespace ISTE.DAL.Database
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
        /// Return a collection of rows from the specified range.
        /// </summary>
        /// <param name="index">Starting index.</param>
        /// <param name="length">From starting index, how many other elements should be returned.</param>
        /// <returns>Return ranged collection of rows.</returns>
        public List<IRow> this[int index, int length]
        {
            get
            {
                List<IRow> range = new List<IRow>();
                for (int i = index; i < this.Count; i++)
                {
                    if (range.Count < length)
                    {
                        range.Add(this[i]);
                    }
                }
                return range;
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
            foreach (IRow row in other.Rows)
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
        /// Return row at specified index.
        /// </summary>
        /// <param name="index">Index of the row to return.</param>
        /// <returns>Return row at index.</returns>
        public IRow GetItem(int index)
        {
            // Validate input.
            if (!this.HasIndex(index))
            {
                throw new IndexOutOfRangeException($"Cannot get row from collection. Index [{index}] out of range ({this.Count} total element(s)).");
            }

            // Return reference.
            return this[index];
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
        /// Replace row at specified index, if index is in bounds.
        /// </summary>
        /// <param name="index">Index to replace row at.</param>
        /// <param name="row">Row to be set.</param>
        /// <returns>Return reference to self.</returns>
        public IResultSet SetItem(int index, IRow row)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return this; }

            // Validate input.
            if (!this.HasIndex(index))
            {
                throw new IndexOutOfRangeException($"Cannot set row. Index [{index}] out of range ({this.Count} total element(s)).");
            }

            // Overwrite row at current index.
            this[index] = row;

            // Return reference to self.
            return this;
        }

        /// <summary>
        /// Add a row to the result set collection.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Return reference to self.</returns>
        public IResultSet AddItem(IRow row)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return this; }

            this.Rows.Add(row);
            return this;
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Return reference to self.</returns>
        public IResultSet AddItems(List<IRow> rows)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return this; }

            foreach (IRow row in rows)
            {
                this.AddItem(row);
            }
            return this;
        }

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Return reference to self.</returns>
        public IResultSet AddItems(params IRow[] rows)
        {
            // If read only, do not do anything.
            if (this.IsReadOnly) { return this; }

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
        /// Remove item from collection. Will return false if row doesn't exist in the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns true if operation is successful.</returns>
        public bool Remove(IRow item)
        {
            return (this.RemoveItem(item) == null) ? false : true;
        }
    }

}
