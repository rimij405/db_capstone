/*
    IResultSet.cs
    Ian Effendi
    ---
    Contains a family of cooperating interfaces:
    - IResultPrinter - Print result set items.
    - IResultSet - A collection of IRows.
    - IRow - A collection of fields, and then a collection of entries associated with each of the fields.
    - IEntry - A collection of field-data key/value pairs.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database.Interfaces
{

    /// <summary>
    /// IResultPrinters can return formatted strings out of a IRow.
    /// </summary>
    public interface IResultPrinter
    {

        //////////////////////
        // Service(s).

        /// <summary>
        /// Return a formatted header, using the field's in a particular row.
        /// </summary>
        /// <param name="header">Row to generate header from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatHeader(IRow header);

        /// <summary>
        /// Return a formatted row.
        /// </summary>
        /// <param name="row">Row to generate formatted string from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatRow(IRow row);

        /// <summary>
        /// Return a formatted string containing an entire table.
        /// </summary>
        /// <param name="results">Records to generate formatted string from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatResultSet(IResultSet results);

    }

    /// <summary>
    /// IResultSet is a collection of IRows.
    /// </summary>
    public interface IResultSet : ICollection<IRow>
    {
        
        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if the result set has at least as many rows as the specified input.
        /// </summary>
        /// <param name="count">Amount of rows to check if result set has.</param>
        /// <returns>Returns true if count is greater than or equal to input value.</returns>
        bool HasThisMany(int count);

        /// <summary>
        /// Check if result set has any rows.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        bool IsEmpty();

        /// <summary>
        /// Return a cloned copy of the results.
        /// </summary>
        /// <returns>Returns a results set.</returns>
        IResultSet Clone();

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return the number of rows affected.
        /// </summary>
        /// <returns>Returns stored number of rows.</returns>
        int GetRowsAffected();

    }

    /// <summary>
    /// IRows wrap functionality for a collection of entries.
    /// </summary>
    public interface IRow : ICollection<IEntry>, ICollection<string>
    {
        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if the row contains entries.
        /// </summary>
        /// <returns>Returns true if there is at least one entry in this row.</returns>
        bool HasEntries();

        /// <summary>
        /// Check if an entry exists where associated for a particular field.
        /// </summary>
        /// <param name="field">Field to check if entry exists for.</param>
        /// <returns>Returns true if entry AND field exists.</returns>
        bool HasEntry(string field);
        
        /// <summary>
        /// Check if row contains a particular field.
        /// </summary>
        /// <param name="field">Field to check for.</param>
        /// <returns>Returns true if field exists.</returns>
        bool HasField(string field);

        /// <summary>
        /// Check if row contains all fields in a set of fields.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns true if ALL field exists.</returns>
        bool HasFields(List<string> fields);

        /// <summary>
        /// Check if row contains all fields in a set of fields.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns true if ALL field exists.</returns>
        bool HasFields(params string[] fields);

        /// <summary>
        /// Check if row contains the input index.
        /// </summary>
        /// <param name="fieldIndex">Index to accomodate.</param>
        /// <returns>Returns true if not empty and index is within bounds.</returns>
        bool HasIndex(int fieldIndex);
        
        /// <summary>
        /// Return a clone of this row.
        /// </summary>
        /// <returns>Returns reference to this row's clone.</returns>
        IRow Clone();

        /// <summary>
        /// Check if the row has no fields.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        bool IsEmpty();
        
        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Find index of a particular field. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="field">Field to search for.</param>
        /// <returns>Return the index of the given field.</returns>
        int GetFieldIndex(string field);

        /// <summary>
        /// Return field alias associated with a particular index.
        /// </summary>
        /// <param name="fieldIndex">Index of field to search for.</param>
        /// <returns>Returns string holding the field alias. Will throw an index out of bounds exception when index is out of bounds.</returns>
        string GetFieldName(int fieldIndex);

        /// <summary>
        /// Return field count. 
        /// </summary>
        /// <returns>Returns integer representing number of fields in a row.</returns>
        int GetFieldCount();

        /// <summary>
        /// Find entry associated with a field, by the field index.
        /// </summary>
        /// <param name="fieldIndex">Index of the field to search for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(int fieldIndex);

        /// <summary>
        /// Find first matching entry by field alias.
        /// </summary>
        /// <param name="fieldName">Field alias to look for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(string fieldName);

        /// <summary>
        /// Return entry count.
        /// </summary>
        /// <returns>Returns integer counting how many entries are in a field.</returns>
        int GetEntryCount();

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(List<string> fields);

        /// <summary>
        /// Returns a row object containing entries that match with the input collection of field names. Missing fields will trigger an error.
        /// </summary>
        /// <param name="fields">Fields to check for.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(params string[] fields);

        /// <summary>
        /// Returns a row object containing elements via a range of index values. If the start index is out of index bounds, it will trigger an error. The length will not trigger an error if not specified/if greater than the length of the row.
        /// </summary>
        /// <param name="start">Starting index to begin range at.</param>
        /// <param name="length">Length of elements to search for. -1 by default.</param>
        /// <returns>Returns a new IRow object.</returns>
        IRow GetRange(int start, int length = -1);

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Set field at existing index to new value. Will change field name for associated entry. Will fail if field already exists or index is out of bounds.
        /// </summary>
        /// <param name="fieldIndex">Field index to set.</param>
        /// <param name="fieldAlias">Field alias to add.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetField(int fieldIndex, string fieldAlias);

        /// <summary>
        /// Add a field to the field name collection. 
        /// </summary>
        /// <param name="fieldAlias">Field alias to add.</param>
        /// <returns>Returns reference to self.</returns>
        IRow AddField(string fieldAlias);

        /// <summary>
        /// Removes a field alias from the collection, along with its associated entry.
        /// </summary>
        /// <param name="field">Field alias to search for.</param>
        /// <returns>Returns the removed entry.</returns>
        IEntry RemoveField(string field);

        /// <summary>
        /// Removes a field alias from the collection, based on its index, along with its associated entry.
        /// </summary>
        /// <param name="fieldIndex">Field index to check.</param>
        /// <returns>Returns the removed entry. Throws index out of bounds exception if field index is out of bounds.</returns>
        IEntry RemoveField(int fieldIndex);

        /// <summary>
        /// Set entry at existing index to new value. Will fail if field doesn't exist.
        /// </summary>
        /// <param name="fieldAlias">Field to set entry for.</param>
        /// <param name="entry">Entry to add.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(string fieldAlias, IEntry entry);

        /// <summary>
        /// Create and then add an entry to the collection, adding a field in the process. Will overwrite existing entry if the field already exists.
        /// </summary>
        /// <param name="field">Field alias to assign to new entry.</param>
        /// <param name="value">Value to give to the particular entry.</param>
        /// <returns>Returns the newly created entry.</returns>
        IEntry AddEntry(string field, string value);

        /// <summary>
        /// Adds entry to the collection, also creating a field alias if it doesn't exist. Will replace existing entries associated with that field.
        /// </summary>
        /// <param name="entry">Entry to add.</param>
        /// <returns>Returns the added entry.</returns>
        IEntry AddEntry(IEntry entry);

        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="fieldIndex">Field index of entry affected.</param>
        /// <returns>Returns the removed entry.</returns>
        IEntry RemoveEntry(int fieldIndex);
        
        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="fieldAlias">Field alias of entry affected.</param>
        /// <returns>Returns the removed entry.</returns>
        IEntry RemoveEntry(string fieldAlias);

        /// <summary>
        /// Removes entry from the collection. The associated field is not removed and instead is given a null entry.
        /// </summary>
        /// <param name="entry">Entry to move.</param>
        /// <returns>Returns the removed entry.</returns>
        IEntry RemoveEntry(IEntry entry);
        
    }
    
    /// <summary>
    /// IEntries wraps functionality for a key/value pair.
    /// </summary>
    public interface IEntry
    {
        //////////////////////
        // Service(s).
        
        /// <summary>
        /// Clone the current entry and return a new, identical one.
        /// </summary>
        /// <returns>Returns a copy of this entry.</returns>
        IEntry Clone();

        /// <summary>
        /// Checks if key matches input.
        /// </summary>
        /// <param name="key">Value to check against.</param>
        /// <returns>Returns true if matched.</returns>
        bool HasField(string key);

        /// <summary>
        /// Checks if value matches input. Returns false when value is 'NULL'.
        /// </summary>
        /// <param name="value">Value to check against.</param>
        /// <returns>Returns true if matched.</returns>
        bool HasValue(string value);

        /// <summary>
        /// Check if a given entry's value is null.
        /// </summary>
        /// <returns>Return true if value is an empty string or null.</returns>
        bool IsNull();

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return the field for a particular entry.
        /// </summary>
        /// <returns>Returns string.</returns>
        string GetField();

        /// <summary>
        /// Return the value for a particular entry.
        /// </summary>
        /// <returns>Returns string.</returns>
        string GetValue();

        /// <summary>
        /// Return combined key/value pair for a field and its value.
        /// </summary>
        /// <returns>Returns a key value pair.</returns>
        KeyValuePair<string, string> GetData();

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Sets the values for a particular entry, using individual strings for the key and value.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        IEntry SetData(string field, string value);

        /// <summary>
        /// Sets the values for a particular entry, using a composite key/value pair.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        IEntry SetData(KeyValuePair<string, string> fieldValuePair);

    }

}
