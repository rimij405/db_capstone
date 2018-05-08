/*
    IResultSet.cs
    Contains a family of interfaces that work together to handle results from SQL queries.
    ---
    Ian Effendi
 */

 // using statements.
using System;
using System.Collections.Generic;
using Services.Interfaces;

namespace ISTE.DAL.Database.Interfaces
{

    #region Formatters

    /// <summary>
    /// Implementations of this interface can validate the field name.
    /// </summary>
    public interface IFieldNameValidator
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if implementation has the given input field.
        /// </summary>
        /// <param name="fieldname">Field to check for.</param>
        /// <returns>Returns true if implementation has matching fieldname.</returns>
        bool HasField(string fieldname);

        /// <summary>
        /// Formats a particular field using this function.
        /// </summary>
        /// <param name="input">Input string to check.</param>
        /// <returns>Returns a formatted fieldname string.</returns>
        string FormatField(string input);

        /// <summary>
        /// Checks if the input string is a valid match.
        /// </summary>
        /// <param name="input">Input string to check.</param>
        /// <returns>Returns true if input can be a valid field name.</returns>
        bool IsValidField(string input);
    }

    /// <summary>
    /// FieldHandler class, inherited by relevant field name validators.
    /// </summary>
    public abstract class FieldHandler : IFieldNameValidator
    {
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
        /// Check if this matches the input field. Case insensitive. Whitespace trimmed.
        /// </summary>
        /// <param name="key">Field to check against.</param>
        /// <returns>Returns true if match found.</returns>
        public abstract bool HasField(string key);

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
    }
    
    /// <summary>
    /// IResultPrinters can return formatted strings out of a IRow.
    /// </summary>
    public interface IResultPrinter
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Return a formatted entry.
        /// </summary>
        /// <param name="entry">Entry to generate formatted string from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatEntry(IEntry entry);


        /// <summary>
        /// Return a formatted header, using the field's in a particular row.
        /// </summary>
        /// <param name="entries">Collection to generate header from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatHeader(List<IEntry> entries);

        /// <summary>
        /// Return a formatted header, using the field's in a particular row.
        /// </summary>
        /// <param name="entries">Collection to generate header from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatHeader(params IEntry[] entries);

        /// <summary>
        /// Return a formatted header, using the field's in a particular row.
        /// </summary>
        /// <param name="row">Row to generate header from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatHeader(IRow row);
        
        /// <summary>
        /// Return a formatted row.
        /// </summary>
        /// <param name="entries">Entries to generate row formatted string from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatRow(List<IEntry> entries);

        /// <summary>
        /// Return a formatted row.
        /// </summary>
        /// <param name="entries">Entries to generate row formatted string from.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatRow(params IEntry[] entries);

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
        
        /// <summary>
        /// Return a formatted string containing an entire table.
        /// </summary>
        /// <param name="results">Records to generate formatted string from.</param>
        /// <param name="offset">Amount of records to offset pagination of. (Zero-based).</param>
        /// <param name="amount">Amount of records to display.</param>
        /// <returns>Returns formatted string.</returns>
        string FormatResultSet(IResultSet results, int offset, int amount = 10);

    }

    #endregion

    #region Comparison Interfaces.

    /// <summary>
    /// Allow comparison of result sets.
    /// </summary>
    public interface IResultSetComparison : IComparison<IResultSet>
    {   
        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareSize(IResultSet left, IResultSet right);

        /// <summary>
        /// Compare two collection sizes.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualSize(IResultSet left, IResultSet right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanSize(IResultSet left, IResultSet right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanSize(IResultSet left, IResultSet right);

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareFields(IResultSet left, IResultSet right);

        /// <summary>
        /// Compare two sets of fields.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualFields(IResultSet left, IResultSet right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanField(IResultSet left, IResultSet right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanField(IResultSet left, IResultSet right);
    }

    /// <summary>
    /// Allow comparison of rows.
    /// </summary>
    public interface IRowComparison : IComparison<IRow>
    {
        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareSize(IRow left, IRow right);

        /// <summary>
        /// Compare two collection sizes.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualSize(IRow left, IRow right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanSize(IRow left, IRow right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanSize(IRow left, IRow right);

        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareFields(IRow left, IRow right);

        /// <summary>
        /// Compare two fields.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualFields(IRow left, IRow right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanField(IRow left, IRow right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanField(IRow left, IRow right);

    }

    /// <summary>
    /// Allow comparison of entries.
    /// </summary>
    public interface IEntryComparison : IComparison<IEntry>
    {
        /// <summary>
        /// Return a 1, 0, or -1 representing the inequality (or equality) between the left and right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Return comparison sort index.</returns>
        int CompareFields(IEntry left, IEntry right);

        /// <summary>
        /// Compare two fields.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if both values are equal.</returns>
        bool IsEqualFields(IEntry left, IEntry right);

        /// <summary>
        /// Returns true if the left value is greater than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is greater than the right value.</returns>
        bool IsGreaterThanField(IEntry left, IEntry right);

        /// <summary>
        /// Returns true if the left value is less than the right value.
        /// </summary>
        /// <param name="left">Left value.</param>
        /// <param name="right">Right value.</param>
        /// <returns>Returns true if the left value is less than the right value.</returns>
        bool IsLessThanField(IEntry left, IEntry right);        
    }

    #endregion
    
    /// <summary>
    /// IResultSet is a collection of IRows.
    /// </summary>
    public interface IResultSet : IListWrapper<IRow>, IReadOnly, IReplicate<IResultSet>, IEmpty<IResultSet>, IComparable, IResultSetComparison, IOperationStateMachine
    {
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Collection of <see cref="IRow"/> rows.
        /// </summary>
        IList<IRow> Rows { get; }
        
        /// <summary>
        /// Number of rows affected by an SQL query.
        /// </summary>
        int RowsAffected { get; }

        /// <summary>
        /// SQL query associated with a result set.
        /// </summary>
        string Query { get; }
        
        /// <summary>
        /// Gets a count of all fields in a result set, in the order in which they appear.
        /// </summary>
        IList<string> Fields { get; }

        /// <summary>
        /// Indexer access to entry, by row and field index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Column index to access.</param>
        /// <returns>Returns entry if it exists. Returns null if there is no entry or no row.</returns>
        IEntry this[int rowIndex, int colIndex] { get; }

        /// <summary>
        /// Indexer access to entry, by row and fieldname.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="key">Field to access.</param>
        /// <returns>Returns entry if it exists. Returns null if there is no entry or no row.</returns>
        IEntry this[int rowIndex, string key] { get; }

        /// <summary>
        /// Indexer access all entries for a particular field by field key.
        /// </summary>
        /// <param name="key">Field to access.</param>
        /// <returns>Returns collection of entries. Returns null if there is no field.</returns>
        IList<IEntry> this[string key] { get; }

        //////////////////////
        // Method(s).
        //////////////////////
        
        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return the number of rows affected.
        /// </summary>
        /// <returns>Returns stored number of rows.</returns>
        int GetRowsAffected();

        /// <summary>
        /// Query associated with this result set.
        /// </summary>
        /// <returns>Returns the stored query.</returns>
        string GetQuery();

        /// <summary>
        /// Returns a row based on its index value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns a single row.</returns>
        IRow GetRow(int rowIndex);

        /// <summary>
        /// Returns a row based on its index value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="row">Row to return.</param>
        /// <returns>Returns a single row.</returns>
        bool TryGetRow(int rowIndex, out IRow row);

        /// <summary>
        /// Returns a header based on the row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of row</returns>
        IList<string> GetRowHeader(int rowIndex);

        /// <summary>
        /// Returns the collection of entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entries.</returns>
        IList<IEntry> GetRowEntries(int rowIndex);

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        IList<KeyValuePair<string, string>> GetRowData(int rowIndex);

        /// <summary>
        /// Returns collection of data entries in a single row.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns collection of entry data pairs.</returns>
        IDictionary<string, string> GetRowMap(int rowIndex);
        
        /// <summary>
        /// Return a subset of cloned rows from the current instance, using the input parameters to narrow selection.
        /// </summary>
        /// <param name="start">Start index of subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns new collection of rows.</returns>
        IList<IRow> GetRange(int start = 0, int length = -1);

        /// <summary>
        /// Return an entry from a given [row, col] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds.</returns>
        IEntry GetEntry(int rowIndex, int fieldIndex);

        /// <summary>
        /// Return an entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        bool TryGetEntry(int rowIndex, int fieldIndex, out IEntry entry);

        /// <summary>
        /// Return an entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldKey">Field to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        IEntry GetEntry(int rowIndex, string fieldKey);
        
        /// <summary>
        /// Return an entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldKey">Field to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        bool TryGetEntry(int rowIndex, string fieldKey, out IEntry entry);

        /// <summary>
        /// Return data from entry from a given [row, col] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldIndex">Field index to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds.</returns>
        KeyValuePair<string, string> GetData(int rowIndex, int fieldIndex);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="colIndex">Field index to get data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        bool TryGetData(int rowIndex, int colIndex, out KeyValuePair<string, string> data);

        /// <summary>
        /// Return data from entry from a given [row, key] indexing system.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldKey">Field to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        KeyValuePair<string, string> GetData(int rowIndex, string fieldKey);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        bool TryGetData(int rowIndex, string fieldname, out KeyValuePair<string, string> data);

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        IList<IEntry> GetEntries(int fieldIndex);

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        IList<IEntry> GetEntries(string fieldname);

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by value).</returns>
        IList<KeyValuePair<string, string>> GetData(int fieldIndex);

        /// <summary>
        /// Creates a collection of data pairs corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        IList<KeyValuePair<string, string>> GetData(string fieldname);
        
        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Set the value of rows affected due to query execution.
        /// </summary>
        /// <param name="rowsAffected">Value to set.</param>
        /// <returns>Returns reference to self.</returns>
        IResultSet SetRowsAffected(int rowsAffected);

        /// <summary>
        /// Set the SQL query associated with this result set.
        /// </summary>
        /// <param name="query">Query to set.</param>
        /// <returns>Returns reference to self.</returns>
        IResultSet SetQuery(string query);

        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="row">Row to set.</param>
        /// <returns>Returns reference to self.</returns>
        IResultSet SetRow(int rowIndex, IRow row);
        
        /// <summary>
        /// Set row at a particular index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="row">Row to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetRow(int rowIndex, IRow row);

        /// <summary>
        /// Add a row to the result set.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Returns reference to self.</returns>
        IResultSet AddRow(IRow row);

        /// <summary>
        /// Add a row to the result set.
        /// </summary>
        /// <param name="row">Row to add.</param>
        /// <returns>Returns reference to self.</returns>
        bool TryAddRow(IRow row);
              
        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        bool AddRows(IList<IRow> rows);

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="rows">Rows to add.</param>
        /// <returns>Returns reference to self.</returns>
        bool AddRows(params IRow[] rows);

        /// <summary>
        /// Add a collection of rows to the result set.
        /// </summary>
        /// <param name="resultSet">Result set rows to concatenate.</param>
        /// <returns>Returns reference to self.</returns>
        bool AddRows(IResultSet resultSet);

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <returns>Returns removed row. Will throw exception if index out of bounds.</returns>
        IRow RemoveRow(int rowIndex);

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="rowIndex">Row index to access.</param>
        /// <param name="results">Row to remove.</param>
        /// <returns>Returns removed row. Will throw exception if index out of bounds.</returns>
        bool TryRemoveRow(int rowIndex, out IRow results);

        /// <summary>
        /// Remove row based on row matching.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <returns>Returns removed row. Returns null if row is not found.</returns>
        IRow RemoveRow(IRow row);

        /// <summary>
        /// Remove row based on row index.
        /// </summary>
        /// <param name="row">Row to remove.</param>
        /// <param name="results">Row to remove.</param>
        /// <returns>Returns removed row. Will throw exception if index out of bounds.</returns>
        bool TryRemoveRow(IRow row, out IRow results);
    }

    /// <summary>
    /// IRows wrap functionality for a collection of entries.
    /// </summary>
    public interface IRow : IListWrapper<IEntry>, INullable<IRow>, IReplicate<IRow>, IEmpty<IRow>, IReadOnly, IComparable, IRowComparison, IFieldNameValidator
    {

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Return the field collection.
        /// </summary>
        IList<string> Fields { get; }

        /// <summary>
        /// Return the entry collection.
        /// </summary>
        IList<IEntry> Entries { get; }

        /// <summary>
        /// Return the collection of data.
        /// </summary>
        IDictionary<string, string> Data { get; }
        
        //////////////////////
        // Indexer(s).
        
        /// <summary>
        /// Return entry with a given field. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="field">Field for entry to retrieve.</param>
        /// <returns>Return entry at field.</returns>
        IEntry this[string field]
        {
            get;
        }
        
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        bool HasFields(IList<string> fieldnames);

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        bool HasFields(params string[] fieldnames);

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return index of specified fieldname. Returns -1 if it doesn't exist.
        /// </summary>
        /// <param name="fieldname">Fieldname to access.</param>
        /// <returns>Returns index.</returns>
        int GetIndex(string fieldname);

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns formatted fieldname. Throws exception if index out of bounds.</returns>
        string GetFieldName(int index);

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to return.</param>
        /// <returns>Returns formatted fieldname. Throws exception if index out of bounds.</returns>
        bool TryGetFieldName(int index, out string fieldname);
        
        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        IEntry GetEntry(int index);

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        bool TryGetEntry(int index, out IEntry entry);

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        IEntry GetEntry(string fieldname);

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to retrieve.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        bool TryGetEntry(string fieldname, out IEntry entry);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        KeyValuePair<string, string> GetData(int index);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        bool TryGetData(int index, out KeyValuePair<string, string> data);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        KeyValuePair<string, string> GetData(string fieldname);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <param name="data">Data to retrieve.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        bool TryGetData(string fieldname, out KeyValuePair<string, string> data);
        
        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        string GetValue(int index);

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to retrieve.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        bool TryGetValue(int index, out string value);

        /// <summary>
        /// Return entry value from the specified field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns stored value. Returns a "NULL" if the field doesn't exist.</returns>
        string GetValue(string fieldname);

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to retrieve.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        bool TryGetValue(string fieldname, out string value);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        IList<IEntry> GetRange(int start = 0);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        IList<IEntry> GetRange(int start = 0, int length = -1);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        IList<IEntry> GetRange(IList<string> fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        IList<IEntry> GetRange(params string[] fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a new row, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <returns>Returns a new row.</returns>
        IRow GetRowRange(int start = 0);

        /// <summary>
        /// Returns a subset of this row, as a new row, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new row.</returns>
        IRow GetRowRange(int start = 0, int length = -1);

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        IRow GetRowRange(IList<string> fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        IRow GetRowRange(params string[] fieldnames);
        
        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Set the fieldname at a particular index, formatting the input, renaming the entry associated with the index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Field to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetFieldName(int index, string fieldname);

        /// <summary>
        /// Set the fieldname at a particular index, formatting the input, renaming the entry associated with the index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Field to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetFieldName(int index, string fieldname);

        /// <summary>
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(int index, IEntry entry);

        /// <summary>
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetEntry(int index, IEntry entry);

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(string fieldname, IEntry entry);
        
        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetEntry(string fieldname, IEntry entry);

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Entry value to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(string fieldname, string value);

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Entry value to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetEntry(string fieldname, string value);

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetValue(IEntry entry);

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        bool TrySetValue(IEntry entry);

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        IRow SetValue(int index, string value);

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        bool TrySetValue(int index, string value);

        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        IRow SetValue(string fieldname, string value);
        
        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        bool TrySetValue(string fieldname, string value);

        /// <summary>
        /// Add a unqiue fieldname, formatting the input and creating a null entry to go along with it.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <returns>Return null entry that has been associated with the field.</returns>
        bool AddField(string fieldname);
        
        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        bool AddFields(IList<string> fieldnames);

        /// <summary>
        /// Add set of fieldnames, creating null entries to go with it. Duplicates are ignored.
        /// </summary>
        /// <param name="fieldnames">Fieldnames to add.</param>
        /// <returns>Return reference to added entries.</returns>
        bool AddFields(params string[] fieldnames);
        
        /// <summary>
        /// Add entry, if and only if, it has a unique fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        bool AddEntry(IEntry entry);
        
        /// <summary>
        /// Add entry, using input unique fieldname. Will clone entry and overwrite clone's fieldname with input.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        bool AddEntry(string fieldname, IEntry entry);
        
        /// <summary>
        /// Add entry, using input unique fieldname and input value.
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="value">Value to create new entry with.</param>
        /// <returns>Returns reference to newly created entry.</returns>
        bool AddEntry(string fieldname, string value);
        
        /// <summary>
        /// Add entries. Duplicate fields will be ignored.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        bool AddEntries(IList<IEntry> entries);

        /// <summary>
        /// Add entries. Duplicate fields will be ignored.
        /// </summary>
        /// <param name="entries">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        bool AddEntries(params IEntry[] entries);

        /// <summary>
        /// Add entries.
        /// </summary>
        /// <param name="row">Entries to add.</param>
        /// <returns>Return reference to added entries.</returns>
        bool AddEntries(IRow row);
        
        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        IEntry RemoveField(int index);

        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns clone of removed entry.</returns>
        bool TryRemoveField(int index, out IEntry entry);

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveField(string fieldname);

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        bool TryRemoveField(string fieldname, out IEntry entry);
        
        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        IEntry RemoveEntry(int index);

        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        bool TryRemoveEntry(int index, out IEntry entry);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveEntry(string fieldname);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        bool TryRemoveEntry(string fieldname, out IEntry entry);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveEntry(IEntry entry);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <param name="result">Entry to return.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        bool TryRemoveEntry(IEntry entry, out IEntry result);
    }
    
    /// <summary>
    /// IEntries wraps functionality for a key/value pair.
    /// </summary>
    public interface IEntry : IReplicate<IEntry>, INullable<IEntry>, IEntryComparison, IComparable, IReadOnly, IFieldNameValidator
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Field/value pair.
        /// </summary>
        KeyValuePair<string, string> Data { get; }

        /// <summary>
        /// Field associated with an entry.
        /// </summary>
        string Field { get; }

        /// <summary>
        /// Value associated with a field.
        /// </summary>
        string Value { get; }
                
        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Return field or value, using 0 or 1. All other values will throw an index out of bounds exception.
        /// </summary>
        /// <param name="index">Accessor for field or value reference.</param>
        /// <returns>Returns a field or value.</returns>
        string this[int index]
        {
            get;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Checks if value matches input. Returns false when value is 'NULL'.
        /// </summary>
        /// <param name="value">Value to check against.</param>
        /// <returns>Returns true if matched.</returns>
        bool HasValue(string value);
        
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
        /// Set the field.
        /// </summary>
        /// <param name="field">Field to set.</param>
        /// <returns>Return reference to self.</returns>
        IEntry SetField(string field);

        /// <summary>
        /// Attempt to set field.
        /// </summary>
        /// <param name="field">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        bool TrySetField(string field);

        /// <summary>
        /// Sets value for entry.
        /// </summary>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns reference to self.</returns>
        IEntry SetValue(string value);
        
        /// <summary>
        /// Attempt to set value.
        /// </summary>
        /// <param name="value">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        bool TrySetValue(string value);

        /// <summary>
        /// Sets the values for a particular entry, using individual strings for the key and value.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        IEntry SetData(string field, string value);

        /// <summary>
        /// Sets the values for a particular entry, using a composite key/value pair.
        /// </summary>
        /// <param name="data">Data to set.</param>
        /// <returns>Returns reference to self.</returns>
        IEntry SetData(KeyValuePair<string, string> data);

        /// <summary>
        /// Attempt to set data.
        /// </summary>
        /// <param name="data">Data to set.</param>
        /// <returns>Returns operation success.</returns>
        bool TrySetData(KeyValuePair<string, string> data);

    }

    /// <summary>
    /// Reference to primary (and partials of composite) keys.
    /// </summary>
    public interface IPrimaryKey : IEntry
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Primary key flag.
        /// </summary>
        bool PrimaryKey
        {
            get;
            set;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Toggle primary key status.
        /// </summary>
        void TogglePrimaryKey();

    }

}
