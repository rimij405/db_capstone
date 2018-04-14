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
                if (this.entryCollection == null) { this.entryCollection = new List<MySqlEntry>(); }
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
            get { return this.GetEntry(key) as MySqlEntry; }
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
        /// Check if an entry exists where associated for a particular field.
        /// </summary>
        /// <param name="field">Field to check if entry exists for.</param>
        /// <returns>Returns true if entry AND field exists.</returns>
        public bool HasEntry(string field) {
            string alias = this.FormatField(field);
            return (this.HasEntries() && this.HasField(alias) && (this.GetEntry(alias) != null));
        }

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
        /// Check if row contains all fields in a set of fields.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns true if ALL field exists.</returns>
        public bool HasFields(List<string> fields)
        {
            // Return false when no fields are input.
            if (fields == null || fields.Count <= 0) { return false; }

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

        /// <summary>
        /// Remove all items from both collections.
        /// </summary>
        public void Clear() {
            this.Fields.Clear();
            this.Entries.Clear();
        }

        /// <summary>
        /// Determines if collection has a specific entry.
        /// </summary>
        /// <param name="entry">Entry to check for.</param>
        /// <returns>Returns true if match is made.</returns>
        public bool Contains(IEntry entry) {
            return this.Entries.Contains(entry);
        }

        /// <summary>
        /// Determines if collection has a specific field.
        /// </summary>
        /// <param name="field">Field to check for.</param>
        /// <returns>Returns true if match is made.</returns>
        public bool Contains(string field) {
            return this.Fields.Contains(field);
        }

        /// <summary>
        /// Copies elements from <see cref="Entries"/> into the supplied array, starting at a particular array index.
        /// </summary>
        /// <param name="entryArray">Array to copy into.</param>
        /// <param name="arrayIndex">Starting index.</param>
        public void CopyTo(IEntry[] entryArray, int arrayIndex = 0) {
            MySqlEntry[] array = new MySqlEntry[this.GetEntryCount()];
            this.Entries.CopyTo(array);
            int index = 0;
            for (int i = arrayIndex; i < entryArray.Length; i++)
            {
                if (index <= 0 || index >= array.Length) { return; }
                entryArray[i] = array[index];
                index++;
            }
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
            if (input.Length <= 0) { return false; }
            return true;
        }

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Find index of a particular field. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="field">Field to search for.</param>
        /// <returns>Return the index of the given field.</returns>
        public int GetFieldIndex(string field) {
            // Validate input.
            if (!this.IsValidField(field)) { return -1; }

            // Find the index.
            return this.Fields.IndexOf(FormatField(field));
        }

        /// <summary>
        /// Return field alias associated with a particular index.
        /// </summary>
        /// <param name="fieldIndex">Index of field to search for.</param>
        /// <returns>Returns string holding the field alias. Will throw an index out of bounds exception when index is out of bounds.</returns>
        public string GetFieldName(int fieldIndex) {
            // Validate input.
            if (!this.HasIndex(fieldIndex)) { throw new IndexOutOfRangeException(); }

            // Return the value.
            return this.Fields[fieldIndex];
        }

        /// <summary>
        /// Return field count. 
        /// </summary>
        /// <returns>Returns integer representing number of fields in a row.</returns>
        public int GetFieldCount()
        {
            return this.Fields.Count;
        }

        /// <summary>
        /// Find entry associated with a field, by the field index.
        /// </summary>
        /// <param name="fieldIndex">Index of the field to search for.</param>
        /// <returns>Returns the IEntry object.</returns>
        public IEntry GetEntry(int fieldIndex) {
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
        public IEntry GetEntry(string fieldName) {
            // Validate input.
            string alias = this.FormatField(fieldName);
            if (!this.HasEntries() || !this.IsValidField(alias)) { return null; }
            
            // Return value from the entries collection that matches.
            foreach (MySqlEntry entry in this.Entries)
            {
                if (entry.HasField(alias)) {
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
        public int GetEntryCount()
        {
            return this.Entries.Count;
        }

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        public IRow GetRange(List<string> fields) { throw new NotImplementedException("Need to create range methods."); }

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        public IRow GetRange(params string[] fields) { throw new NotImplementedException("Need to create range methods."); }

        /// <summary>
        /// Returns a row object containing elements via a range of index values. If the start index is out of index bounds, it will trigger an error. The length will not trigger an error if not specified/if greater than the length of the row.
        /// </summary>
        /// <param name="start">Starting index to begin range at.</param>
        /// <param name="length">Length of elements to search for. -1 by default.</param>
        /// <returns>Returns a new IRow object.</returns>
        public IRow GetRange(int start, int length = -1) { throw new NotImplementedException("Need to create range methods."); }

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Add entry to collection.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        public void Add(IEntry entry) {
            if (entry != null)
            {
                if (entry is MySqlEntry e)
                {
                    this.Entries.Add(e);
                }
            }
        }

        /// <summary>
        /// Set entry at existing index to new value. Will fail if field doesn't exist.
        /// </summary>
        /// <param name="fieldAlias">Field to set entry for.</param>
        /// <param name="entry">Entry to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetEntry(string fieldAlias, IEntry entry)
        {
            // Validate the inputs.
            string alias = this.FormatField(fieldAlias);
            if(!this.IsValidField(alias) || !this.HasField(alias) || !this.HasEntry(alias)) { return this; }

            // Overwrite existing field's entry.
            this.Entries[this.GetFieldIndex(alias)] = entry as MySqlEntry;

            // Reference to self.
            return this;
        }

        /// <summary>
        /// Create and then add an entry to the collection, adding a field in the process. Will overwrite existing entry if the field already exists.
        /// </summary>
        /// <param name="field">Field alias to assign to new entry.</param>
        /// <param name="value">Value to give to the particular entry.</param>
        /// <returns>Returns the newly created entry.</returns>
        public IEntry AddEntry(string field, string value)
        {
            // Validate the inputs.
            string alias = this.FormatField(field);
            if(!this.IsValidField(alias)) { return null; }

            // Create the entry object.
            MySqlEntry entry = new MySqlEntry(alias, value);
            
            // Add entry, overwriting if there are conflicts.
            this.AddEntry(entry);

            // Return the added object.
            return entry;
        }

        /// <summary>
        /// Adds entry to the collection, also creating a field alias if it doesn't exist. Will replace existing entries associated with that field.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        /// <returns>Returns the added entry.</returns>
        public IEntry AddEntry(IEntry entry)
        {
            // Validate the inputs.
            if(entry != null) {
                // Get field.
                if (!this.HasField(entry.GetField()))
                {
                    this.AddField(entry.GetField());
                }

                // Set entry.
                this.SetEntry(entry.GetField(), entry);
            }

            return entry;
        }

        /// <summary>
        /// Add field to collection.
        /// </summary>
        /// <param name="field">Field to add.</param>
        public void Add(string field) {
            string alias = this.FormatField(field);
            if (this.IsValidField(alias)) {
                this.Fields.Add(alias);
            }
        }

        /// <summary>
        /// Set field at existing index to new value. Will change field name for associated entry. Will fail if field already exists or index is out of bounds.
        /// </summary>
        /// <param name="fieldIndex">Field index to set.</param>
        /// <param name="fieldAlias">Field alias to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow SetField(int fieldIndex, string fieldAlias) {
            // Validate index. If invalid, throw index out of bounds exception.
            if (!this.HasIndex(fieldIndex)) { throw new IndexOutOfRangeException(); }

            // Validate field alias. If invalid, do nothing.
            string alias = this.FormatField(fieldAlias);
            if (!this.IsValidField(alias) || !this.HasField(alias)) { return this; }

            // Set the field at a particular index and change the field for the entry at that index.
            this.Fields[fieldIndex] = alias;

            // Overwrite field in the associated entry.
            MySqlEntry e = (this.GetEntry(fieldIndex) as MySqlEntry);
            if (e != null) { e.Field = alias; }
            
            // Return reference to self.
            return this;
        }

        /// <summary>
        /// Add a field to the field name collection, if unique, and add a corresponding empty entry.
        /// </summary>
        /// <param name="fieldAlias">Field alias to add.</param>
        /// <returns>Returns reference to self.</returns>
        public IRow AddField(string fieldAlias) {
            // Validate field alias. If invalid - or it already exists - do nothing.
            string alias = this.FormatField(fieldAlias);
            if (!this.IsValidField(alias) || this.HasField(alias)) { return this; }

            // Add the field.
            this.Add(alias);

            // Add empty entry.
            this.Add(new MySqlEntry(alias));

            // Return reference to self.
            return this;
        }

        /// <summary>
        /// Remove entry from collection.
        /// </summary>
        /// <param name="entry">Entry to remove.</param>
        /// <returns>Returns true if removed successfully.</returns>
        public bool Remove(IEntry entry) {            
            return (entry is MySqlEntry e) && (this.Entries.Remove(e));
        }

        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="fieldIndex">Field index of entry affected.</param>
        /// <returns>Returns the removed entry.</returns>
        public IEntry RemoveEntry(int fieldIndex) {
            // Validate input.
            if (this.HasIndex(fieldIndex)) {
                IEntry e = this.Entries[fieldIndex] as IEntry;
                return (this.Remove(e) ? e : null); // return null if it fails to remove any entry.
            }

            // Return null.
            return null;
        }

        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="fieldAlias">Field alias of entry affected.</param>
        /// <returns>Returns the removed entry.</returns>
        public IEntry RemoveEntry(string fieldAlias) {
            // Validate input.
            string alias = this.FormatField(fieldAlias);
            if(!this.IsValidField(alias) || !this.HasField(alias)) { return null; }
            return RemoveEntry(this.GetFieldIndex(alias));
        }

        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="entry">Entry to move.</param>
        /// <returns>Returns the removed entry.</returns>
        public IEntry RemoveEntry(IEntry entry) {
            return ((entry is MySqlEntry e) ? RemoveEntry(e.GetField()) : null);
        }

        /// <summary>
        /// Remove field from collection.
        /// </summary>
        /// <param name="field">Field to remove.</param>
        /// <returns>Returns true if removed successfully.</returns>
        public bool Remove(string field) {
            return this.Fields.Remove(field);
        }

        /// <summary>
        /// Removes a field alias from the collection, along with its associated entry.
        /// </summary>
        /// <param name="field">Field alias to search for.</param>
        /// <returns>Returns the removed entry.</returns>
        public IEntry RemoveField(string field) {
            // Validate input.
            string alias = this.FormatField(field);
            if(!this.IsValidField(alias) || !this.HasField(alias)) { return null; } // field doesn't exist.
            
            // if field exists, we need to remove the entry first.
            IEntry entry = this.GetEntry(alias);

            // only remove field and entry if entry is not null.
            if (entry != null) {
                this.RemoveEntry(alias);
                return (this.Remove(alias) ? entry : null);
            }

            return null;
        }

        /// <summary>
        /// Removes a field alias from the collection, based on its index, along with its associated entry.
        /// </summary>
        /// <param name="fieldIndex">Field index to check.</param>
        /// <returns>Returns the removed entry. Throws index out of bounds exception if field index is out of bounds.</returns>
        public IEntry RemoveField(int fieldIndex)
        {
            // Validate input.
            if (this.HasIndex(fieldIndex)) {
                return this.RemoveField(this.GetFieldName(fieldIndex));
            }

            // Only return null when index is invalid.
            return null;
        }

        /// <summary>
        /// Implementation wrapper.
        /// </summary>
        /// <returns>Returns enumerator.</returns>
        public IEnumerator<IEntry> GetEnumerator()
        {
            return this.Entries.GetEnumerator();
        }

        /// <summary>
        /// Implementation wrapper.
        /// </summary>
        /// <returns>Returns enumerator.</returns>
        IEnumerator<string> IEnumerable<string>.GetEnumerator()
        {
            return this.Fields.GetEnumerator();
        }

        /// <summary>
        /// Implementation wrapper.
        /// </summary>
        /// <returns>Returns enumerator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
