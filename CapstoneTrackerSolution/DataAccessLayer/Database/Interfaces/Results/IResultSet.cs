/*
    IResultSet.cs
    Ian Effendi
    ---
    Contains the interfaces for the results.
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
        /// Return the number of rows affected.
        /// </summary>
        /// <returns>Returns stored number of rows.</returns>
        int GetRowsAffected();

        /// <summary>
        /// Return a cloned copy of the results.
        /// </summary>
        /// <returns>Returns a results set.</returns>
        IResultSet Clone();

    }

    /// <summary>
    /// IRows wrap functionality for a collection of entries.
    /// </summary>
    public interface IRow : ICollection<IEntry>, ICollection<string>
    {
        /// <summary>
        /// Check if the row contains fields.
        /// </summary>
        /// <returns>Returns true if there is at least one field in this row.</returns>
        bool HasEntries();

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
        /// Find index of a particular field/entry. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="field">Field to search for.</param>
        /// <returns>Return the index of the given field.</returns>
        int GetIndex(string field);
                
        /// <summary>
        /// Find entry by index.
        /// </summary>
        /// <param name="index">Index of the field to search for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(int index);

        /// <summary>
        /// Find first matching entry by field alias.
        /// </summary>
        /// <param name="field">Field alias to look for.</param>
        /// <returns>Returns the IEntry object.</returns>
        IEntry GetEntry(string field);

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

        /// <summary>
        /// Return a clone of this row.
        /// </summary>
        /// <returns>Returns reference to this row.</returns>
        IRow Clone();

        /// <summary>
        /// Check if the row has no fields.
        /// </summary>
        /// <returns>Returns true if empty.</returns>
        bool IsEmpty();
        
        /// <summary>
        /// Return field count. 
        /// </summary>
        /// <returns>Returns integer representing number of fields in a row.</returns>
        int GetFieldCount();

    }

    /// <summary>
    /// IEntries wraps functionality for a key/value pair.
    /// </summary>
    public interface IEntry
    {
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
        KeyValuePair<string, string> Get();
        
        /// <summary>
        /// Sets the values for a particular entry, using individual strings for the key and value.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        IEntry Set(string field, string value);

        /// <summary>
        /// Sets the values for a particular entry, using a composite key/value pair.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        IEntry Set(KeyValuePair<string, string> fieldValuePair);

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
    }

}
