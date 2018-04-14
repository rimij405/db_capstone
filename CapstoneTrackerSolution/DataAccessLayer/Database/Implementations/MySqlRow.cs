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
        private List<MySqlEntry> entryCollection;

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

        /// <summary>
        /// Represents a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        public List<MySqlEntry> Entries
        {
            get
            {
                if(this.entryCollection == null) { this.entryCollection = new List<MySqlEntry>(); }
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
        /// Returns the number of fields in the row.
        /// </summary>
        public int Count
        {
            get { return this.GetFieldCount(); }
        }

        /// <summary>
        /// Flag determines whether or not collections are mutable.
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// Accessor and mutator for fieldname references at particular indices.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public string this[int index]
        {
            get { return this.GetFieldName(index); }
            set { this.SetField(index, value); }
        }

        /// <summary>
        /// Accessor and mutator for <see cref="MySqlEntry"/> entries based on field name.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public MySqlEntry this[string key]
        {
            get { return this.GetEntry(key); }
            set { this.SetEntry(key, value as IEntry); }
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
        public MySqlRow(List<string> fields) { throw new NotImplementedException("Need to create mutators."); }

        /// <summary>
        /// Construct a row with a predetermined set of fields.
        /// </summary>
        /// <param name="fields">Fields to construct object with.</param>
        public MySqlRow(params string[] fields) { throw new NotImplementedException("Need to create mutators."); }

        /// <summary>
        /// Construct a row from a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        /// <param name="entries">Entries inhabiting the row.</param>
        public MySqlRow(List<MySqlEntry> entries) { throw new NotImplementedException("Need to create mutators."); }
        
        /// <summary>
        /// Construct a row from a collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        /// <param name="entries">Entries inhabiting the row.</param>
        public MySqlRow(params MySqlEntry[] entries) { throw new NotImplementedException("Need to create mutators."); }

        /// <summary>
        /// Copy constructor.
        /// </summary>
        /// <param name="row">Other row to clone.</param>
        public MySqlRow(MySqlRow row) { throw new NotImplementedException("Need to create clone method."); }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if the row contains entries.
        /// </summary>
        /// <returns>Returns true if there is at least one entry in this row.</returns>
        public bool HasEntries()
        {
            return (this.GetEntryCount() > 0);
        }

        /// <summary>
        /// Check if row contains a particular field. If input is invalid, it will return false.
        /// </summary>
        /// <param name="field">Field to check for.</param>
        /// <returns>Returns true if field exists.</returns>
        public bool HasField(string field)
        {
            string alias = FormatField(field);
            return this.IsValidField(alias) && this.Fields.Contains(alias); 
        }

        /// <summary>
        /// Check if row contains all fields in a set of fields.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns true if ALL field exists.</returns>
        public bool HasFields(List<string> fields)
        {
            // Return false when no fields are input.
            if(fields == null || fields.Count <= 0) { return false; }

            foreach (string field in fields)
            {
                if (!this.HasField(field))
                {
                    return false;
                }
            }

            // Return true if it hasn't returned false at this point.
            return true;
        }

        /// <summary>
        /// Check if row contains all fields in a set of fields.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns true if ALL field exists.</returns>
        public bool HasFields(params string[] fields)
        {
            return this.HasFields(fields.ToList<string>());
        }

        /// <summary>
        /// Check if row contains the input index.
        /// </summary>
        /// <param name="fieldIndex">Index to accomodate.</param>
        /// <returns>Returns true if not empty and index is within bounds.</returns>
        public bool HasIndex(int fieldIndex)
        {
            if (this.IsEmpty() || fieldIndex < 0 || fieldIndex >= this.Count) { return false; }
            return true;
        }

        /// <summary>
        /// Return a clone of this row.
        /// </summary>
        /// <returns>Returns reference to this row's clone.</returns>
        public IRow Clone() {
            MySqlRow copy = new MySqlRow(this);
            return copy;
        }

        /// <summary>
        /// Check if the row has no fields.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        public bool IsEmpty() {
            return (this.Count > 0);
        }

        //////////////////////
        // Helper(s).

        /// <summary>
        /// Returns a trimmed, all caps string, based off of input.
        /// </summary>
        /// <param name="input">Input to capitalize and trim of whitespace.</param>
        /// <returns>Returns formatted string.</returns>
        private string FormatField(string input)
        {
            if (input.Length <= 0) { return ""; }
            return input.Trim().ToUpper();
        }

        /// <summary>
        /// Checks if input is not empty.
        /// </summary>
        /// <param name="input">Input to check.</param>
        /// <returns>Returns true if valid.</returns>
        private bool IsValidField(string input)
        {
            if(input.Length <= 0) { return false; }
            return true;
        }

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Find index of a particular field. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="field">Field to search for.</param>
        /// <returns>Return the index of the given field.</returns>
        int GetFieldIndex(string field) {
            // Validate input.
            if(!this.IsValidField(field)) { return -1; }

            // Find the index.
            return this.Fields.IndexOf(FormatField(field));
        }

        /// <summary>
        /// Return field alias associated with a particular index.
        /// </summary>
        /// <param name="fieldIndex">Index of field to search for.</param>
        /// <returns>Returns string holding the field alias. Will throw an index out of bounds exception when index is out of bounds.</returns>
        string GetFieldName(int fieldIndex) {
            // Validate input.
            if(!this.HasIndex(fieldIndex)) { throw new IndexOutOfRangeException(); }

            // Return the value.
            return this.Fields[fieldIndex];
        }

        /// <summary>
        /// Return field count. 
        /// </summary>
        /// <returns>Returns integer representing number of fields in a row.</returns>
        int GetFieldCount()
        {
            return this.Fields.Count;
        }

        /// <summary>
        /// Find entry associated with a field, by the field index.
        /// </summary>
        /// <param name="fieldIndex">Index of the field to search for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(int fieldIndex) {
            // Validate input.
            if (!this.HasEntries() || !this.HasIndex(fieldIndex)) { throw new IndexOutOfRangeException(); }

            // Return value from the entries collection.
            return this.Entries[fieldIndex];
        }

        /// <summary>
        /// Find first matching entry by field alias.
        /// </summary>
        /// <param name="fieldName">Field alias to look for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(string fieldName) {
            // Validate input.
            if(!this.HasEntries() || !this.IsValidField(fieldName)) { return null; }

            // Return value from the entries collection that matches.
            foreach(MySqlEntry entry in this.Entries)
            {
                if (entry.HasField(FormatField(fieldName))) {
                    return entry;
                }
            }

            // If nothing found, return null.
            return null;
        }

        /// <summary>
        /// Return entry count.
        /// </summary>
        /// <returns>Returns integer counting how many entries are in a field.</returns>
        int GetEntryCount()
        {
            return this.Entries.Count;
        }

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(List<string> fields) { throw new NotImplementedException("Need to create range methods."); }

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(params string[] fields) { throw new NotImplementedException("Need to create range methods."); }

        /// <summary>
        /// Returns a row object containing elements via a range of index values. If the start index is out of index bounds, it will trigger an error. The length will not trigger an error if not specified/if greater than the length of the row.
        /// </summary>
        /// <param name="start">Starting index to begin range at.</param>
        /// <param name="length">Length of elements to search for. -1 by default.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(int start, int length = -1) { throw new NotImplementedException("Need to create range methods."); }

        //////////////////////
        // Mutator(s).




        #region Old

        /*

        // Field(s).

        /// <summary>
        /// Collection of <see cref="MySqlEntry"/> entries.
        /// </summary>
        private List<MySqlEntry> entries;

        /// <summary>
        /// Collection of fields that are stored by the contained entries.
        /// </summary>
        private List<string> fields;

        // Properties.

        /// <summary>
        /// Access to <see cref="MySqlEntry"/> collection.
        /// </summary>
        public List<MySqlEntry> Entries {
            get {
                if (this.entries == null)
                {
                    this.entries = new List<MySqlEntry>();
                }
                return this.entries;
            }
        }

        /// <summary>
        /// Access to <see cref="string"/> field collection.
        /// </summary>
        public List<string> Fields
        {
            get {
                if (this.fields == null)
                {
                    this.fields = new List<string>();
                }
                return this.fields;
            }
        }

        /// <summary>
        /// Returns count of items in collection.
        /// </summary>
        public int Count {
            get
            {
                return this.Entries.Count;
            }
        }

        /// <summary>
        /// Returns number of fields in a row.
        /// </summary>
        public int FieldCount {
            get
            {
                return this.Fields.Count;
            }
        }

        /// <summary>
        /// Value determining if items can be added to the collection.
        /// </summary>
        public bool IsReadOnly {
            get; set;
        }

        // Constructor(s).

        /// <summary>
        /// Create collection to store entries.
        /// </summary>
        public MySqlRow()
        {
            this.entries = new List<MySqlEntry>();
            this.IsReadOnly = false;
        }

        /// <summary>
        /// Create collection to store entities and then assign the readonly flag.
        /// </summary>
        /// <param name="isReadOnly">Value to set readonly flag to.</param>
        public MySqlRow(bool isReadOnly) : this()
        {
            this.IsReadOnly = isReadOnly;
        }

        /// <summary>
        /// Create collection from another collection.
        /// </summary>
        /// <param name="collection">Collection to initialize object with.</param>
        public MySqlRow(List<MySqlEntry> collection) : this()
        {
            for (int i = 0; i < collection.Count; i++)
            {
                this.Add(collection[i]);
            }
        }

        /// <summary>
        /// Create collection from another collection.
        /// </summary>
        /// <param name="collection">Collection to initialize object with.</param>
        public MySqlRow(params MySqlEntry[] collection) : this()
        {
            for (int i = 0; i < collection.Length; i++)
            {
                this.Add(collection[i]);
            }
        }

        /// <summary>
        /// Creates a deep copy of values inside the input <see cref="MySqlRow"/> object.
        /// </summary>
        /// <param name="row">Row to copy from.</param>
        public MySqlRow(MySqlRow row) : this()
        {
            if (row.HasEntries())
            {
                for (int i = 0; i < row.Count; i++)
                {
                    this.Add(row.GetEntry(i));
                }
            }
            this.IsReadOnly = row.IsReadOnly;
        }

        // Method(s).
        
        /// <summary>
        /// Attempt to add <see cref="MySqlEntry"/> entry to collection.
        /// </summary>
        /// <param name="item">Item to attempt to add.</param>
        public void Add(IEntry item)
        {
            // This if statement is 'is' pattern-matching. It checks to see if 'item' is a particular type, and casts it to that type under the 'entry' identifier, if successful.
            if (item is MySqlEntry entry)
            {
                if (!this.Contains(entry))
                {
                    this.Entries.Add(entry);
                }
            }
        }

        /// <summary>
        /// Attempt to add unique field to the <see cref="Fields"/> collection. Will not add if a copy already exists.
        /// </summary>
        /// <param name="item">Field to add.</param>
        public void Add(string item)
        {
            string field = item.Trim().ToUpper();
            if (field.Length > 0 && !this.Contains(field))
            {
                this.Fields.Add(field);
            }
        }

        /// <summary>
        /// Clears the entire collection of all entries and their fields.
        /// </summary>
        public void Clear()
        {
            this.Entries.Clear();
            this.Fields.Clear();
        }

        /// <summary>
        /// Clones object and returns a copy.
        /// </summary>
        /// <returns>Returns cloned copy.</returns>
        public IRow Clone()
        {
            return new MySqlRow(this);
        }

        /// <summary>
        /// Check if MySqlRow already contains the input entry.
        /// </summary>
        /// <param name="item">Entry to check for.</param>
        /// <returns>Returns true if match is found.</returns>
        public bool Contains(IEntry item)
        {
            if (item is MySqlEntry entry)
            {
                return this.Entries.Contains(entry);
            }
            return false;
        }

        /// <summary>
        /// Check if MySqlRow already contains the input field.
        /// </summary>
        /// <param name="item">Input to check.</param>
        /// <returns>Returns true if match is found.</returns>
        public bool Contains(string item)
        {
            string field = item.Trim();
            if (field.Length > 0)
            {
                return this.Fields.Contains(field);
            }
            return false;
        }

        /// <summary>
        /// Copy range of entries to an input array.
        /// </summary>
        /// <param name="array">Array to copy values into.</param>
        /// <param name="arrayIndex">Represents index in which copying begins in the accompanying <paramref name="array"/>.</param>
        public void CopyTo(IEntry[] array, int arrayIndex)
        {
            if (this.HasEntries() && arrayIndex < array.Length && arrayIndex >= 0)
            {
                int index = 0;
                for (int i = arrayIndex; i < array.Length; i++)
                {
                    if(index < 0 || index >= this.Count) { return; }
                    array[i] = this.Entries[index];
                    index++;
                }
            }
        }

        /// <summary>
        /// Copy range of fields to an input array.
        /// </summary>
        /// <param name="array">Array to copy values into.</param>
        /// <param name="arrayIndex">Represents index in which copying begins in the accompanying <paramref name="array"/>.</param>
        public void CopyTo(string[] array, int arrayIndex)
        {
            if (this.HasFields() && arrayIndex < array.Length && arrayIndex >= 0)
            {
                int index = 0;
                for (int i = arrayIndex; i < array.Length; i++)
                {
                    if (index < 0 || index >= this.FieldCount) { return; }
                    array[i] = this.Fields[index];
                    index++;
                }
            }
        }

        /// <summary>
        /// Return an entry from the collection, by index.
        /// </summary>
        /// <param name="index">Index to find entry with.</param>
        /// <returns>Return the resulting entry.</returns>
        public IEntry GetEntry(int index)
        {
            if(!this.HasEntries() || index < 0 || index >= this.Count) { return null; }
            return this.Entries[index];
        }

        /// <summary>
        /// Return an entry from the collection, by field.
        /// </summary>
        /// <param name="field">Field to find entry with.</param>
        /// <returns>Return the resulting entry.</returns>
        public IEntry GetEntry(string field)
        {
            string fieldName = field.Trim().ToUpper();
            if(!this.HasEntries() || fieldName.Length <= 0 || !this.Contains(fieldName)) { return null; }

            // loop through collection and find first match.
            foreach (MySqlEntry entry in this.Entries)
            {
                if (entry.Field.ToUpper() == fieldName)
                {
                    return entry;
                }
            }

            return null;
        }

        /// <summary>
        /// Return the enumerator for the <see cref="MySqlEntry"/> collection.
        /// </summary>
        /// <returns>Returns IEnumerator.</returns>
        public IEnumerator<IEntry> GetEnumerator()
        {
            return this.Entries.GetEnumerator();
        }

        /// <summary>
        /// Get the index of a particular field/entry in the row.
        /// </summary>
        /// <param name="field">Name of the field the index would have.</param>
        /// <returns>Returns integer representing field ordinance.</returns>
        public int GetIndex(string field)
        {
            int index = -1; // error result when index not found.
            string fieldName = field.Trim().ToUpper();
            if(!this.HasEntries() || fieldName.Length <= 0 || !this.Contains(fieldName)) { return index; }
            index = this.Fields.IndexOf(fieldName);
            return index;
        }

        /// <summary>
        /// Return a row that is a subset of only the included fields, in the presented order.
        /// </summary>
        /// <param name="fields">Fields to create subset with.</param>
        /// <returns>Return subset collection.</returns>
        public IRow GetRange(List<string> fields)
        {
            throw new NotImplementedException();
        }

        public IRow GetRange(params string[] fields)
        {
            throw new NotImplementedException();
        }

        public IRow GetRange(int start, int length = -1)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Che
        /// </summary>
        /// <returns></returns>
        public bool HasEntries()
        {
            throw new NotImplementedException();
        }

        public bool HasField(string field)
        {
            throw new NotImplementedException();
        }

        public bool HasFields(List<string> fields)
        {
            throw new NotImplementedException();
        }

        public bool HasFields(params string[] fields)
        {
            throw new NotImplementedException();
        }

        public bool IsEmpty()
        {
            throw new NotImplementedException();
        }

        public bool Remove(IEntry item)
        {
            throw new NotImplementedException();
        }

        public bool Remove(string item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

    */

        #endregion

    }
}
