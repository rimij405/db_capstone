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
    /// Represents a row of data returned by MySql.
    /// </summary>
    public class MySqlRow : IRow
    {

        //////////////////////
        // Field(s).
        //////////////////////

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

        /// <summary>
        /// Represents a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        private List<IEntry> entryCollection;

        /*********************************************************
         * Developer's Note:
         * 
         * Fields stored are trimmed of whitespace and set to
         * uppercase. Input checked against the collection should
         * also be trimmed and set to uppercase as the collection
         * is both whitespace and case insensitive.
         * *******************************************************/

        /// <summary>
        /// Represents a collection of <see cref="string"/> field names/aliases associated with <see cref="MySqlEntry"/> entries.
        /// </summary>
        private List<string> fieldCollection;

        //////////////////////
        // Properties.
        //////////////////////
                
        //////
        // ICollection

        /// <summary>
        /// Returns the number of fields in the row.
        /// </summary>
        public int Count
        {
            get { return this.FieldCount; }
        }

        /// <summary>
        /// Returns the number of entries in the row.
        /// </summary>
        int ICollection<IEntry>.Count { get { return this.EntryCount; } }

        /// <summary>
        /// Flag determines whether or not collections are mutable.
        /// </summary>
        public bool IsReadOnly { get; set; }
        
        //////
        // INullable
        
        /// <summary>
        /// Check if every entry in the row is null.
        /// </summary>
        public bool IsNull
        {
            get
            {
                // If any entry is not null, return false.
                foreach (IEntry entry in this.Entries)
                {
                    if (!entry.IsNull) { return false; }
                }
                return true;
            }
        }

        //////
        // IEmpty

        /// <summary>
        /// Check if this row simply has no fields (and therefore no entries).
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return this.HasExactlyThisMany(0);
            }
        }

        //////
        // IRow

        /// <summary>
        /// Check if this collection contains any non-zero amount of fields.
        /// </summary>
        public bool HasFields
        {
            get { return this.HasAtLeastThisMany(1); }
        }

        /// <summary>
        /// Represents a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        public List<IEntry> Entries
        {
            get
            {
                if (this.entryCollection == null) { this.entryCollection = new List<IEntry>(); }
                return this.entryCollection;
            }
        }

        /// <summary>
        /// Represents a collection of <see cref="string"/> field names/aliases associated with <see cref="MySqlEntry"/> entries.
        /// </summary>
        public List<string> Fields
        {
            get
            {
                if (this.fieldCollection == null) { this.fieldCollection = new List<string>(); }
                return this.fieldCollection;
            }
        }

        /// <summary>
        /// Return the field count.
        /// </summary>
        /// <returns>Return the field count.</returns>
        public int FieldCount
        {
            get { return this.Fields.Count; }
        }

        /// <summary>
        /// Return the entry count.
        /// </summary>
        /// <returns>Return the entry count.</returns>
        public int EntryCount
        {
            get { return this.Entries.Count; }
        }

        /// <summary>
        /// Accessor and mutator for fieldname references at particular indices.
        /// </summary>
        /// <param name="index">Index to retrieve field name from.</param>
        /// <returns>Returns fieldname. Returns an empty string if it can't be found.</returns>
        public string this[int index]
        {
            get {
                return (HasFields && HasIndex(index)) ? this.Fields[index] : "";
            }
        }

        /// <summary>
        /// Accessor and mutator for <see cref="MySqlEntry"/> entries based on field name.
        /// </summary>
        /// <param name="key">Field name to retrieve entry for.</param>
        /// <returns>Returns entry. Returns null if it can't be found.</returns>
        public IEntry this[string key]
        {
            get
            {
                int fieldIndex = (key.Length == 0) ? -1 : this.GetIndex(key);
                return (fieldIndex == -1) ? null : this.Entries[fieldIndex];
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public MySqlRow() { }

        /// <summary>
        /// Construct a row with a predetermined set of fields.
        /// </summary>
        /// <param name="fields">Fields to construct object with.</param>
        public MySqlRow(List<string> fields)
        {
            // Validate input.
            if (fields == null || fields.Count <= 0) { return; }
            
            foreach(string field in fields)
            {
                this.AddField(field);
            }
        }

        /// <summary>
        /// Construct a row with a predetermined set of fields.
        /// </summary>
        /// <param name="fields">Fields to construct object with.</param>
        public MySqlRow(params string[] fields)
            : this(fields.ToList<string>())
        {
            // Call other constructor.
        }

        /// <summary>
        /// Construct a row from a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        /// <param name="entries">Entries inhabiting the row.</param>
        public MySqlRow(List<IEntry> entries)
        {
            // Validate input.
            if (entries == null || entries.Count <= 0) { return; }

            foreach (MySqlEntry entry in entries)
            {
                this.AddEntry(entry);
            }
        }

        /// <summary>
        /// Construct a row from a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        /// <param name="entries">Entries inhabiting the row.</param>
        public MySqlRow(params IEntry[] entries)
            : this(entries.ToList<IEntry>())
        {
            // Call other constructor.
        }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="row">Other row to clone.</param>
        public MySqlRow(MySqlRow row)
        {
            // Clone.
            if (row != null) {
                foreach(string field in row.Fields)
                {
                    this.Fields.Add(field);
                }

                foreach(MySqlEntry entry in row.Entries)
                {
                    this.Entries.Add((MySqlEntry)entry.Clone());
                }

                this.IsReadOnly = row.IsReadOnly;
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Format the row as a string.
        /// </summary>
        /// <returns>Return information as a formatted string.</returns>
        public override string ToString()
        {
            string format = $"  MySqlRow: {((this.Count > 0) ? "{" : "{}")}";

            for (int index = 0; index < this.Count; index++)
            {
                format += "\n    " + $"{{ Field: {this.Fields[index]}, Entry: ({this.Entries[index]}) }}{(((index + 1) < this.Count && this.Count > 1) ? "," : "\n  }")}";
            }

            return format;
        }

        //////
        // IComparable

        /// <summary>
        /// Check if reference is the same, or if all entries match.
        /// </summary>
        /// <param name="obj">IRow to be compared.</param>
        /// <returns>Returns true if equal. False, if otherwise.</returns>
        public override bool Equals(object obj)
        {
            if (obj is IRow row)
            {
                if (row.FieldCount == this.FieldCount && row.EntryCount == this.FieldCount)
                {
                    for (int index = 0; index < this.FieldCount; index++)
                    {
                        IEntry entryA = this.Entries[index];
                        IEntry entryB = row.Entries[index];
                        if (!entryA.Equals(entryB)) { return false; }
                    }
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns the hashcode of all the internal entries.
        /// </summary>
        /// <returns>Returns hashcode compilation of all entries.</returns>
        public override int GetHashCode()
        {
            return this.Fields.GetHashCode() ^ this.Entries.GetHashCode();
        }

        /// <summary>
        /// Compare two rows. Matches based on row-to-row comparison.
        /// </summary>
        /// <param name="obj">IRow to be compared.</param>
        /// <returns>Returns zero if equal. Returns greater value if more entries and unequal. Returns smaller value if less entries.</returns>
        public int CompareTo(object obj)
        {
            if (obj != null && obj is IRow row)
            {
                // If the same, return 0.
                if (this.Equals(obj)) { return 0; }

                // If not the same, find difference.
                // If field count is the same, check if fields are different.
                if (this.FieldCount == row.FieldCount)
                {
                    // If difference, return the compare between them.
                    for(int index = 0; index < this.FieldCount; index++)
                    {
                        string fieldA = this[index];
                        string fieldB = row.GetFieldName(index);
                        int comparison = fieldA.CompareTo(fieldB);
                        if(comparison != 0) { return comparison; }
                    }

                    // If no difference in fields, compare entries.
                    for (int index = 0; index < this.EntryCount; index++)
                    {
                        IEntry entryA = this.GetEntry(index);
                        IEntry entryB = row.GetEntry(index);
                        int comparison = entryA.CompareTo(entryB);
                        if(comparison != 0) { return comparison; }
                    }

                    // If there is no difference in both fields and in entries, it is the same row.
                    return 0;
                }
                else
                {
                    // Sort based on size of fields, if sizes are different.
                    return this.FieldCount.CompareTo(row.FieldCount);
                }
            }

            // If not of a compatible type, return nothing.
            throw new ArgumentException("Input object is not of type IRow.");
        }

        ///////
        // ICollectionWrapper

        /// <summary>
        /// Check if the collection has at least this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is greater than or equal to input value.</returns>
        public bool HasAtLeastThisMany(int count)
        {
            return (this.Count >= count);
        }

        /// <summary>
        /// Check if the collection has exactly this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is equal to input value.</returns>
        public bool HasExactlyThisMany(int count)
        {
            return (this.Count == count);
        }

        /// <summary>
        /// Check if row contains the input index.
        /// </summary>
        /// <param name="fieldIndex">Index to accomodate.</param>
        /// <returns>Returns true if not empty and index is within bounds.</returns>
        public bool HasIndex(int fieldIndex)
        {
            if (this.IsEmpty || fieldIndex < 0 || fieldIndex >= this.Count) { return false; }
            return true;
        }

        ///////
        // IFieldNameValidator.

        /// <summary>
        /// Check if row contains a particular field. If input is invalid, it will return false.
        /// </summary>
        /// <param name="field">Field to check for.</param>
        /// <returns>Returns true if field exists.</returns>
        public bool HasField(string field)
        {
            string alias = this.FormatField(field);
            return (this.IsValidField(alias) && this.Contains(alias));
        }

        /// <summary>
        /// Returns a trimmed, all caps string, based off of input.
        /// </summary>
        /// <param name="input">Input to capitalize and trim of whitespace.</param>
        /// <returns>Returns formatted string.</returns>
        public string FormatField(string input)
        {
            if (input.Length <= 0) { return ""; }
            return input.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if input is not empty.
        /// </summary>
        /// <param name="input">Input to check.</param>
        /// <returns>Returns true if valid.</returns>
        public bool IsValidField(string input)
        {
            if (input.Length <= 0) { return false; }
            return true;
        }

        ///////
        // ICollection<string>
        
        /// <summary>
        /// Check if fieldname is in collection.
        /// </summary>
        /// <param name="fieldname">Field to search for.</param>
        /// <returns>Returns true if found.</returns>
        public bool Contains(string fieldname)
        {
            string alias = this.FormatField(fieldname);
            return ((this.IsValidField(alias)) ? this.Fields.Contains(alias) : false);
        }

        ///////
        // ICollection<IEntry>

        /// <summary>
        /// Check if entry is in collection.
        /// </summary>
        /// <param name="entry">Entry to search for.</param>
        /// <returns>Returns true if found.</returns>
        public bool Contains(IEntry entry)
        {
            return this.Entries.Contains(entry);
        }

        ///////
        // IRow

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        public bool Contains(List<string> fieldnames)
        {
            if(fieldnames.Count <= 0) { return false; }

            foreach(string field in fieldnames)
            {
                if (!this.Contains(field)) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        public bool Contains(params string[] fieldnames)
        {
            if (fieldnames.Length <= 0) { return false; }
            return this.Contains(fieldnames.ToList<string>());
        }

        /// <summary>
        /// Check if all entries are present, with matching fields AND matching values.
        /// </summary>
        /// <param name="entries">Collection of entries to compare.</param>
        /// <returns>Returns true if ALL entries and fields are found.</returns>
        public bool Contains(List<IEntry> entries)
        {
            if (entries.Count <= 0) { return false; }

            foreach (IEntry entry in entries)
            {
                if (!this.Contains((MySqlEntry) entry)) { return false; }
            }

            return true;
        }

        /// <summary>
        /// Check if all entries are present, with matching fields AND matching values.
        /// </summary>
        /// <param name="entries">Collection of entries to compare.</param>
        /// <returns>Returns true if ALL entries and fields are found.</returns>
        public bool Contains(params IEntry[] entries)
        {
            if (entries.Length <= 0) { return false; }
            return this.Contains(entries.ToList<IEntry>());
        }

        ///////
        // IReplicate

        /// <summary>
        /// Return a clone of this row.
        /// </summary>
        /// <returns>Returns reference to this row's clone.</returns>
        public IReplicate Clone() {
            return new MySqlRow(this);
        }
        
        ///////
        // INullable

        /// <summary>
        /// Set all field entries to null entries.
        /// </summary>
        /// <returns>Makes all entries null.</returns>
        public INullable MakeNull()
        {
            foreach (IEntry e in this.Entries)
            {
                e.MakeNull();
            }
            return this;
        }

        ///////
        // ICollection

        /// <summary>
        /// Remove all items from both collections.
        /// </summary>
        public void Clear() {
            this.Fields.Clear();
            this.Entries.Clear();
        }
                        
        /// <summary>
        /// Copies elements from <see cref="Entries"/> into the supplied array, starting at a particular array index.
        /// </summary>
        /// <param name="entryArray">Array to copy into.</param>
        /// <param name="arrayIndex">Starting index.</param>
        public void CopyTo(IEntry[] entryArray, int arrayIndex = 0) {
            List<IEntry> entries = new List<IEntry>();
            foreach (IEntry e in this.Entries) {
                entries.Add(e);
            }
            entries.CopyTo(entryArray, arrayIndex);
        }

        /// <summary>
        /// Copies elements from <see cref="Fields"/> into the supplied array, starting at a particular array index.
        /// </summary>
        /// <param name="fieldArray">Array to copy into.</param>
        /// <param name="arrayIndex">Starting index.</param>
        public void CopyTo(string[] fieldArray, int arrayIndex = 0) {
            this.Fields.CopyTo(fieldArray, arrayIndex);
        }

        //////////////////////
        // Accessor(s).

        ///////
        // ICollectionWrapper<IEntry>

        /// <summary>
        /// Return the field index of the item stored in a particular collection.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Returns field index of item in the collection. Returns -1 if not found.</returns>
        public int GetIndex(IEntry item)
        {
            // Validate input.
            if (item == null || !this.Contains(item.GetField())) { return -1; }
            return this.Fields.IndexOf(item.GetField());
        }

        /// <summary>
        /// Return the entry at the specified index.
        /// </summary>
        /// <param name="index">Index to retrieve item from.</param>
        /// <param name="item">Item to be output.</param>
        /// <returns>Returns true if successfully obtained item. Returns item if index is in bounds.</returns>
        public bool GetItem(int index, out IEntry item)
        {
            // Validate input.
            item = (this.IsEmpty || !this.HasIndex(index)) ? null : this.Entries[index];
            return (item != null); // Returns true if item is not null.
        }

        ///////
        // ICollectionWrapper<string>

        /// <summary>
        /// Return the index of the item stored in a particular collection.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Returns index of item in the collection. Returns -1 if not found.</returns>
        public int GetIndex(string item)
        {
            // Validate input.
            string alias = this.FormatField(item);
            if(!this.HasField(alias)) { return -1; }
            return this.Fields.IndexOf(alias);
        }

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Index to retrieve item from.</param>
        /// <param name="item">Item to be output.</param>
        /// <returns>Returns true if successfully obtained item. Returns item if index is in bounds.</returns>
        public bool GetItem(int index, out string item)
        {
            // Validate input.
            item = (this.IsEmpty || !this.HasIndex(index)) ? null : this[index];
            return (item != null); // Returns true if item is not null.
        }

        ///////
        // IRow

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns empty pair on error.</returns>
        public KeyValuePair<string, string> GetData(int index)
        {
            KeyValuePair<string, string> data = new KeyValuePair<string, string>("", "");
            if(this.GetItem(index, out IEntry item))
            {
                data = item.GetData();
            }
            return data;
        }

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        public KeyValuePair<string, string> GetData(string fieldname)
        {
            KeyValuePair<string, string> data = new KeyValuePair<string, string>();
            IEntry item = this[fieldname];
            if (item != null)
            {
                data = item.GetData();
            }
            return data;
        }

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns formatted fieldname. Returns empty string on error.</returns>
        public string GetFieldName(int index)
        {
            if(this.GetItem(index, out string fieldname))
            {
                return fieldname;
            }
            return "";
        }

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns entry. Returns null if index doesn't exist.</returns>
        public IEntry GetEntry(int index)
        {
            return this[this[index]];
        }

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        public IEntry GetEntry(string fieldname)
        {
            return this[fieldname];
        }

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns stored value. Throws exception if entry doesn't exist.</returns>
        public string GetValue(int index)
        {
            IEntry entry = this.GetEntry(index);
            if(entry != null) { return entry.GetValue(); }
            else { throw new InvalidOperationException("Cannot find value for undefined entry."); }
        }

        /// <summary>
        /// Return entry value from the specified field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns stored value. Throws exception if entry doesn't exist.</returns>
        public string GetValue(string fieldname)
        {
            IEntry entry = this.GetEntry(fieldname);
            if (entry != null) { return entry.GetValue(); }
            else { throw new InvalidOperationException("Cannot find value for undefined entry."); }
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(int start = 0, int length = -1)
        {
            IRow subset = new MySqlRow();

            if (start > 0 && start < this.Count)
            {
                foreach (IEntry entry in this.Entries)
                {
                    if(length == -1 || subset.FieldCount < length)
                    {
                        // Add entry (and its associated field).
                        subset.AddEntry(entry.Clone() as IEntry);
                    }
                }
            }

            return subset;
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames. Duplicate fieldnames are ignored.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(List<string> fieldnames)
        {
            IRow subset = new MySqlRow();
            foreach (string field in fieldnames)
            {
                IEntry entry = this.GetEntry(field);
                if (entry != null) { subset.AddEntry(entry.Clone() as IEntry); }
                else { subset.AddField(field); }
            }
            return subset;
        }

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        public IRow GetRowRange(params string[] fieldnames)
        {
            return this.GetRowRange(fieldnames.ToList<string>());
        }

        /// <summary>
        /// Returns a subset of this row's entries, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public List<IEntry> GetRange(int start = 0, int length = -1)
        {
            List<IEntry> subset = new List<IEntry>();

            if (start > 0 && start < this.Count)
            {
                foreach (IEntry entry in this.Entries)
                {
                    if (length == -1 || subset.Count < length)
                    {
                        // Add entry (and its associated field).
                        subset.Add(entry.Clone() as IEntry);
                    }
                }
            }

            return subset;
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public List<IEntry> GetRange(List<string> fieldnames)
        {
            List<IEntry> subset = new List<IEntry>();
            foreach (string field in fieldnames)
            {
                IEntry entry = this.GetEntry(field);
                if (entry != null) { subset.Add(entry.Clone() as IEntry); }
            }
            return subset;
        }

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        public List<IEntry> GetRange(params string[] fieldnames)
        {
            return this.GetRange(fieldnames.ToList<string>());
        }
        
        ///////
        // ICollection<IEntry>

        /// <summary>
        /// Returns the entry collection's enumerator. Default enumerator.
        /// </summary>
        /// <returns>Returns enumerator for the entries.</returns>
        public IEnumerator<IEntry> GetEnumerator()
        {
            return this.Entries.GetEnumerator();
        }

        ///////
        // ICollection<string>

        /// <summary>
        /// Returns the field name collection's enumerator. Explicit enumerator.
        /// </summary>
        /// <returns>Returns enumerator for the entries.</returns>
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.Fields.GetEnumerator();
        }

        ///////
        // ICollection

        /// <summary>
        /// Returns the entry collection's enumerator. Explicit enumerator.
        /// </summary>
        /// <returns>Returns enumerator for the entries.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        //////////////////////
        // Mutator(s).

        ///////
        // ICollectionWrapper<IEntry>

        /// <summary>
        /// Replace item at specified index, if index is in bounds. Will return null if it index doesn't exist.
        /// </summary>
        /// <param name="index">Index to replace item at.</param>
        /// <param name="item">Item to be set.</param>
        /// <returns>Return reference to item.</returns>
        public IEntry SetItem(int index, IEntry item)
        {
            // Wrap implementation.
            return (this.SetEntry(index, item) == null) ? null : item;
        }

        /// <summary>
        /// Add a item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <returns>Return reference to item.</returns>
        public IEntry AddItem(IEntry item)
        {
            // Wrap implementation.
            return (this.AddEntry(item) == null) ? null : item;
        }

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to item.</returns>
        public List<IEntry> AddItems(List<IEntry> items)
        {
            // Wrap implementation.
            return (this.AddEntries(items) == null) ? new List<IEntry>() : items;
        }

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to item.</returns>
        public List<IEntry> AddItems(params IEntry[] items)
        {
            return this.AddItems(items.ToList<IEntry>());
        }

        /// <summary>
        /// Remove item from the collection.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns false if no item found.</returns>
        public bool RemoveItem(int index, out IEntry item)
        {
            // Wrap implementation.
            item = this.RemoveEntry(index);
            return (item == null) ? false : true;
        }

        /// <summary>
        /// Remove item if it exists in the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Return removed item. Returns null if no item found.</returns>
        public IEntry RemoveItem(IEntry item)
        {
            // Wrap implementation.
            return this.RemoveEntry(item);
        }

        ///////
        // ICollectionWrapper<string>

        /// <summary>
        /// Replace item at specified index, if index is in bounds.  Will return empty string if it index doesn't exist.
        /// </summary>
        /// <param name="index">Index to replace item at.</param>
        /// <param name="item">Item to be set.</param>
        /// <returns>Return reference to item.</returns>
        public string SetItem(int index, string item)
        {
            // Wrap implementation.
            return (this.SetFieldName(index, item) == null) ? "" : item;
        }

        /// <summary>
        /// Add a item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <returns>Return reference to item.</returns>
        public string AddItem(string item)
        {
            // Wrap implementation.
            IEntry entry = this.AddField(item);
            return (entry == null) ? "" : entry.GetField();
        }

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to item.</returns>
        public List<string> AddItems(List<string> items)
        {
            // Wrap implementation.
            return (this.AddFields(items) == null) ? new List<string>() : items;
        }

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to item.</returns>
        public List<string> AddItems(params string[] items)
        {
            // Wrap implementation.
            return this.AddItems(items.ToList<string>());
        }

        /// <summary>
        /// Remove item from the collection.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns false if no item found.</returns>
        public bool RemoveItem(int index, out string item)
        {
            // Wrap implementation.
            IEntry entry = this.RemoveField(index);
            item = (entry == null) ? "" : entry.GetField();
            return (item.Length == 0) ? false : true;
        }

        /// <summary>
        /// Remove item if it exists in the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Return removed item. Returns empty string if no item found.</returns>
        public string RemoveItem(string item)
        {
            // Wrap implementation.
            IEntry entry = this.RemoveField(item);            
            return (entry == null) ? "" : entry.GetField();
        }

        ///////
        // IRow

        /// <summary>
        /// Set the fieldname at a particular index, formatting the input, renaming the entry associated with the index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Field to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetFieldName(int index, string fieldname)
        {
            string alias = this.FormatField(fieldname);
            if (!this.HasIndex(index) || !this.IsValidField(alias) || this.HasField(alias)) { return null; }
            this.Fields[index] = alias;
            IEntry e = this.Entries[index];
            if(e != null) { e.SetData(alias, e.GetValue()); }
            return this;
        }

        /// <summary>
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(int index, IEntry entry)
        {
            // Validate input.
            if (entry == null || !this.HasIndex(index)) { return null; }
            // Overwrite the fieldname on the input entry.
            entry.SetData(this.GetFieldName(index), entry.GetValue());
            this.Entries[index] = entry;
            return this;
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(string fieldname, IEntry entry)
        {
            return this.SetEntry(fieldname, entry.GetValue());
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Entry value to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(string fieldname, string value)
        {
            return this.SetEntry(this.GetIndex(fieldname), new MySqlEntry(fieldname, value));
        }

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(IEntry entry)
        {
            // Find matching entry and replace it with this entry.
            return this.SetEntry(this.GetIndex(entry), entry);
        }

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        public IRow SetValue(int index, string value)
        {
            IEntry entry = this.GetEntry(index);
            if(entry != null)
            {
                entry.SetData(entry.GetField(), entry.GetValue());
            }
            return this;
        }

        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        public IRow SetValue(string fieldname, string value)
        {
            IEntry entry = this[fieldname];
            if (entry != null)
            {
                entry.SetData(entry.GetField(), entry.GetValue());
            }
            return this;
        }

        /// <summary>
        /// Insert field (and corresponding null entry) at specified index.
        /// <para>All elements from the matching index onwards are shifted to the right.</para>
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <returns>Returns null entry.</returns>
        public IEntry InsertField(int index, string fieldname)
        {
            // Validate inputs.
            string alias = this.FormatField(fieldname);
            if(!this.HasIndex(index) || this.HasField(fieldname)) { return null; }

            // Create null entry.
            IEntry entry = new MySqlEntry(alias);

            // Create field in inserted area.
            this.Fields.Insert(index, alias);
            this.Entries.Insert(index, entry);
            return entry;
        }

        /// <summary>
        /// Insert entry at specified index, using the entry's fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to create field and entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        public IEntry InsertEntry(int index, IEntry entry)
        {
            // Validate inputs.
            if (entry == null || !this.HasIndex(index) || this.HasField(entry.GetField())) { return null; }

            // Insert the field.
            this.InsertField(index, entry.GetField());
            this.SetEntry(entry);
            return entry;
        }

        /// <summary>
        /// Insert entry at specified index, using the input fieldname.
        /// <para>Entry is cloned and fieldname on entry's clone is overwritten with input.</para>
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <param name="entry">Entry to create field and entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        public IEntry InsertEntry(int index, string fieldname, IEntry entry)
        {
            // Validate inputs.
            if (entry == null) { return null; }

            // Make clone.
            return this.InsertEntry(index, fieldname, entry.GetValue());
        }

        /// <summary>
        /// Insert new entry at specified index, using the input fieldname and input value.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <param name="value">Value to create entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        public IEntry InsertEntry(int index, string fieldname, string value)
        {
            // Validate inputs.
            string alias = this.FormatField(fieldname);
            if (!this.HasIndex(index) || this.HasField(alias)) { return null; }

            // Make clone.
            return this.InsertEntry(index, new MySqlEntry(alias, value));
        }

        /// <summary>
        /// Add a unqiue fieldname, formatting the input and creating a null entry to go along with it.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <returns>Return null entry that has been associated with the field.</returns>
        public IEntry AddField(string fieldname)
        {
            // Validate input.
            string alias = this.FormatField(fieldname);
            if (!this.IsValidField(alias) || this.HasField(alias)) { return null; }

            // Create the null entry.
            IEntry entry = new MySqlEntry(alias);

            // Add the field with a new null entry.
            this.Fields.Add(alias);
            this.Entries.Add(entry);

            // Return added entry.
            return entry;
        }

        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public List<IEntry> AddFields(List<string> fieldnames)
        {
            // Validate input.
            if(fieldnames == null || fieldnames.Count <= 0) { return null; }

            List<IEntry> entries = new List<IEntry>();
            foreach (string field in fieldnames)
            {
                IEntry entry = this.AddField(field);
                if(entry != null) { entries.Add(entry); }
            }

            return entries;
        }

        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public List<IEntry> AddFields(params string[] fieldnames)
        {
            return this.AddFields(fieldnames.ToList<string>());
        }

        /// <summary>
        /// Add entry, if and only if, it has a unique fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        public IEntry AddEntry(IEntry entry)
        {
            // Validate input.
            if(entry == null || this.HasField(entry.GetField())) { return null; }

            // If entry isn't null and field doesn't already exist, create a null entry in the new field and set the value.
            this.AddField(entry.GetField());
            this.SetEntry(entry);
            return entry;
        }

        /// <summary>
        /// Add entry, using input unique fieldname. Will clone entry and overwrite clone's fieldname with input.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        public IEntry AddEntry(string fieldname, IEntry entry)
        {
            // Validate input.
            string alias = this.FormatField(fieldname);
            if (entry == null || !this.IsValidField(alias) || this.HasField(alias)) { return null; }

            // Set entry using new field.
            return this.AddEntry(((IEntry) entry.Clone()).SetData(alias, entry.GetValue()));
        }

        /// <summary>
        /// Add entry, using input unique fieldname and input value.
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="value">Value to create new entry with.</param>
        /// <returns>Returns reference to newly created entry.</returns>
        public IEntry AddEntry(string fieldname, string value)
        {
            // Validate input.
            string alias = this.FormatField(fieldname);
            if (!this.IsValidField(alias) || this.HasField(alias)) { return null; }

            // Set entry using new field.
            return this.AddEntry(new MySqlEntry(alias, value));
        }

        /// <summary>
        /// Add entries. Duplicate fields will be ignored.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public List<IEntry> AddEntries(List<IEntry> entries)
        {
            // Validate input.
            if (entries == null || entries.Count <= 0) { return null; }

            List<IEntry> result = new List<IEntry>();
            foreach (IEntry entry in entries)
            {
                IEntry e = this.AddEntry(entry);
                if (e != null) { result.Add(e); }
            }

            return result;
        }

        /// <summary>
        /// Add entries. Duplicate fields will be ignored.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        public List<IEntry> AddEntries(params IEntry[] entries)
        {
            return this.AddEntries(entries.ToList<IEntry>());
        }

        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if out of bounds.</returns>
        public IEntry RemoveField(int index)
        {
            // Validate input.
            if (!this.HasIndex(index)) { return null; }

            // Get entry at index.
            IEntry entry = this.Entries[index];

            // Remove the field and entry at the associated index.
            this.Fields.RemoveAt(index);
            this.Entries.RemoveAt(index);
            return entry;
        }

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveField(string fieldname)
        {
            // Validate input.
            string alias = this.FormatField(fieldname);
            if (!this.IsValidField(alias) || !this.HasField(alias)) { return null; }

            // Remove the field and entry at the associated index.
            return this.RemoveField(this.GetIndex(alias));
        }

        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if out of bounds.</returns>
        public IEntry RemoveEntry(int index)
        {
            // Validate input.
            if (!this.HasIndex(index)) { return null; }

            // Get entry clone to return.
            IEntry entry = this.Entries[index].Clone() as IEntry;

            // Make existing reference null.
            this.Entries[index].MakeNull();

            // Return clone.
            return entry;
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveEntry(string fieldname)
        {
            // Validate input.
            string alias = this.FormatField(fieldname);
            if (!this.IsValidField(alias) || !this.HasField(alias)) { return null; }

            return this.RemoveEntry(this.GetIndex(alias));
        }

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        public IEntry RemoveEntry(IEntry entry)
        {
            // Validate input.
            if (entry == null || !this.HasField(entry.GetField())) { return null; }

            return this.RemoveEntry(entry.GetField());
        }

        ///////
        // ICollection<IEntry>

        /// <summary>
        /// Add entry to the appropriate collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(IEntry item)
        {
            // Call the add entry method.
            this.AddItem(item);
        }

        /// <summary>
        /// Remove entry from the appropriate collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns true if operation successful. Returns false, if otherwise.</returns>
        public bool Remove(IEntry item)
        {
            // If RemoveEntry(item) returns a null object, return false. Else, true.
            return ((this.RemoveEntry(item) == null) ? false : true);            
        }
        
        ///////
        // ICollection<string>
        
        /// <summary>
        /// Add field name to the appropriate collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        public void Add(string item)
        {
            // Add field call.
            this.AddField(item);
        }

        /// <summary>
        /// Remove field name from the appropriate collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Returns true if operation successful. Returns false, if otherwise.</returns>
        public bool Remove(string item)
        {
            // If RemoveEntry(item) returns a null object, return false. Else, true.
            return ((this.RemoveField(item) == null) ? false : true);
        }
        
    }

}
