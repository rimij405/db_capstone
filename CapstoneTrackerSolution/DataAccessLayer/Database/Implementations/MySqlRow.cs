/*
    MySqlRow.cs
    Ian Effendi
    ---
    Contains the implementation for IRow, an ICollection<IEntry> interface.
 */

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
    }
}
