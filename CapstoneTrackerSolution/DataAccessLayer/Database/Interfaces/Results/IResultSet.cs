﻿/*
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
        // Method(s).
        //////////////////////

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
    /// Interface for allowing clonable collections/items.
    /// </summary>
    public interface IReplicate
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Return a cloned deep-copy of this object.
        /// </summary>
        /// <returns>Returns a clone of this object.</returns>
        IReplicate Clone();
    }

    /// <summary>
    /// Implementations of this interface can have a 'null' value.
    /// </summary>
    public interface INullable
    {
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Check if the instance is a custom version of null.
        /// </summary>
        bool IsNull { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// When called, turns the instance into its 'null' form.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        INullable MakeNull();
    }

    /// <summary>
    /// Implementations of this interface can be empty.
    /// </summary>
    public interface IEmpty
    {
        /// <summary>
        /// Check if this can be empty.
        /// </summary>
        bool IsEmpty { get; }
    }

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
    /// Custom collection wrapper functions.
    /// </summary>
    /// <typeparam name="TItem">Type of object stored by this collection.</typeparam>
    public interface ICollectionWrapper<TItem> : ICollection<TItem>, IReplicate, IEmpty where TItem : IComparable
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Check if the collection has at least this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is greater than or equal to input value.</returns>
        bool HasAtLeastThisMany(int count);

        /// <summary>
        /// Check if the collection has exactly this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is equal to input value.</returns>
        bool HasExactlyThisMany(int count);

        /// <summary>
        /// Check if it has a particular index.
        /// </summary>
        /// <param name="index">Index to check for.</param>
        /// <returns>Returns true if the index is in bounds. False, if otherwise.</returns>
        bool HasIndex(int index);

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Return the index of the item stored in a particular collection.
        /// </summary>
        /// <param name="item">Item to search for.</param>
        /// <returns>Returns index of item in the collection. Returns -1 if not found.</returns>
        int GetIndex(TItem item);

        /// <summary>
        /// Return the item at the specified index.
        /// </summary>
        /// <param name="index">Index to retrieve item from.</param>
        /// <returns>Returns item if index is in bounds. Throws exception if out of bounds.</returns>
        TItem GetItem(int index);

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Replace item at specified index, if index is in bounds.
        /// </summary>
        /// <param name="index">Index to replace item at.</param>
        /// <param name="item">Item to be set.</param>
        /// <returns>Return reference to self.</returns>
        IResultSet SetItem(int index, TItem item);

        /// <summary>
        /// Add a item to the collection.
        /// </summary>
        /// <param name="item">Item to add.</param>
        /// <returns>Return reference to self.</returns>
        IResultSet AddItem(TItem item);

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to self.</returns>
        IResultSet AddItems(List<TItem> items);

        /// <summary>
        /// Add a collection of items to the collection.
        /// </summary>
        /// <param name="items">Items to add.</param>
        /// <returns>Return reference to self.</returns>
        IResultSet AddItems(params TItem[] items);

        /// <summary>
        /// Remove item from the collection.
        /// </summary>
        /// <param name="index">Index of the item to remove.</param>
        /// <returns>Return removed item. Returns null if no item found.</returns>
        TItem RemoveItem(int index);

        /// <summary>
        /// Remove item if it exists in the collection.
        /// </summary>
        /// <param name="item">Item to remove.</param>
        /// <returns>Return removed item. Returns null if no item found.</returns>
        TItem RemoveItem(TItem item);
    }

    /// <summary>
    /// IResultSet is a collection of IRows.
    /// </summary>
    public interface IResultSet : ICollectionWrapper<IRow>
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Collection of <see cref="IRow"/> rows.
        /// </summary>
        List<IRow> Rows { get; }

        /// <summary>
        /// Number of rows affected by an SQL query.
        /// </summary>
        int RowsAffected { get; }

        /// <summary>
        /// SQL query associated with a result set.
        /// </summary>
        string Query { get; }
        
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
        /// Return a subset of cloned rows from the current instance, using the input parameters to narrow selection.
        /// </summary>
        /// <param name="start">Start index of subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns new collection of rows.</returns>
        List<IRow> GetRange(int start = 0, int length = -1);

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
        /// <param name="fieldKey">Field to access.</param>
        /// <returns>Returns entry. Will throw exception if index out of bounds. Returns null if field cannot be found.</returns>
        IEntry GetEntry(int rowIndex, string fieldKey);

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldIndex">Field index to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        List<IEntry> GetEntries(int fieldIndex);

        /// <summary>
        /// Creates a list of entries corresponding to the input fieldindex from all rows.
        /// </summary>
        /// <param name="fieldname">Field to access entries from.</param>
        /// <returns>Returns collection of entries (by reference).</returns>
        List<IEntry> GetEntries(string fieldname);
        
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
                
    }

    /// <summary>
    /// IRows wrap functionality for a collection of entries.
    /// </summary>
    public interface IRow : ICollectionWrapper<IEntry>, ICollectionWrapper<string>, INullable, IComparable, IFieldNameValidator
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Check if this collection contains any non-zero amount of fields.
        /// </summary>
        bool HasFields { get; }

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
        bool Contains(List<string> fieldnames);

        /// <summary>
        /// Check if all fields are present in a row.
        /// </summary>
        /// <param name="fieldnames">Collection of field names to look for.</param>
        /// <returns>Returns true if ALL names are found.</returns>
        bool Contains(params string[] fieldnames);

        /// <summary>
        /// Check if all entries are present, with matching fields AND matching values.
        /// </summary>
        /// <param name="entries">Collection of entries to compare.</param>
        /// <returns>Returns true if ALL entries and fields are found.</returns>
        bool Contains(List<IEntry> entries);

        /// <summary>
        /// Check if all entries are present, with matching fields AND matching values.
        /// </summary>
        /// <param name="entries">Collection of entries to compare.</param>
        /// <returns>Returns true if ALL entries and fields are found.</returns>
        bool Contains(params IEntry[] entries);

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="index">Field index to get data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Throws exception if index out of bounds.</returns>
        KeyValuePair<string, string> GetData(int index);

        /// <summary>
        /// Get tabular data linking a fieldname to a value.
        /// </summary>
        /// <param name="fieldname">Fieldname to search data from.</param>
        /// <returns>Returns key/value pair if field exists at index. Returns null if fieldname doesn't exist.</returns>
        KeyValuePair<string, string> GetData(string fieldname);

        /// <summary>
        /// Return the fieldname at the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns formatted fieldname. Throws exception if index out of bounds.</returns>
        string GetFieldName(int index);

        /// <summary>
        /// Return entry at field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns entry. Throws exception if index out of bounds.</returns>
        IEntry GetEntry(int index);

        /// <summary>
        /// Return entry associated with the field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns entry. Returns null if field doesn't exist.</returns>
        IEntry GetEntry(string fieldname);

        /// <summary>
        /// Return entry value from the specified index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns stored value. Throws exception if index out of bounds.</returns>
        string GetValue(int index);

        /// <summary>
        /// Return entry value from the specified field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns stored value. Returns a "NULL" if the field doesn't exist.</returns>
        string GetValue(string fieldname);

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
        IRow GetRowRange(List<string> fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a new row, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will have null entries in the returned value.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new row.</param>
        /// <returns>Returns a new row.</returns>
        IRow GetRowRange(params string[] fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, using the input values.
        /// </summary>
        /// <param name="start">Starting index to begin subset parsing.</param>
        /// <param name="length">Length of subset parsing.</param>
        /// <returns>Returns a new collection of entries.</returns>
        List<IEntry> GetRange(int start = 0, int length = -1);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        List<IEntry> GetRange(List<string> fieldnames);

        /// <summary>
        /// Returns a subset of this row, as a list of entries, containing all the input fieldnames.
        /// <para>Fieldnames that don't exist in the current row will be ignored.</para>
        /// </summary>
        /// <param name="fieldnames">Collection of fieldnames that should be in the new collection.</param>
        /// <returns>Returns a new collection of entries.</returns>
        List<IEntry> GetRange(params string[] fieldnames);

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
        /// Set entry at field index to the input entry reference.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(int index, IEntry entry);

        /// <summary>
        /// Set entry with matching fieldname to the input entry clone. Will overwrite fieldname on cloned entry.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(string fieldname, IEntry entry);

        /// <summary>
        /// Set entry with matching fieldname to the input entry reference. Will only add if the entry's field exists in the row.
        /// </summary>
        /// <param name="entry">Entry to set.</param>
        /// <returns>Returns reference to self.</returns>
        IRow SetEntry(IEntry entry);

        /// <summary>
        /// Set entry value at a particular field index.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Throws exception if index out of bounds.</returns>
        IRow SetValue(int index, string value);

        /// <summary>
        /// Set entry value at a particular field.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <param name="value">Value to set on entry.</param>
        /// <returns>Returns reference to self. Returns null if field doesn't exist.</returns>
        IRow SetValue(string fieldname, string value);

        /// <summary>
        /// Insert field (and corresponding null entry) at specified index.
        /// <para>All elements from the matching index onwards are shifted to the right.</para>
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <returns>Returns null entry.</returns>
        IEntry InsertField(int index, string fieldname);

        /// <summary>
        /// Insert entry at specified index, using the entry's fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="entry">Entry to create field and entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        IEntry InsertEntry(int index, IEntry entry);

        /// <summary>
        /// Insert entry at specified index, using the input fieldname.
        /// <para>Entry is cloned and fieldname on entry's clone is overwritten with input.</para>
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <param name="entry">Entry to create field and entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        IEntry InsertEntry(int index, string fieldname, IEntry entry);

        /// <summary>
        /// Insert new entry at specified index, using the input fieldname and input value.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <param name="fieldname">Fieldname to create field with.</param>
        /// <param name="value">Value to create entry with.</param>
        /// <returns>Returns reference to inserted entry.</returns>
        IEntry InsertEntry(int index, string fieldname, string value);

        /// <summary>
        /// Add a unqiue fieldname, formatting the input and creating a null entry to go along with it.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <returns>Return null entry that has been associated with the field.</returns>
        IEntry AddField(string fieldname);

        /// <summary>
        /// Add entry, if and only if, it has a unique fieldname.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        IEntry AddEntry(IEntry entry);

        /// <summary>
        /// Add entry, using input unique fieldname. Will clone entry and overwrite clone's fieldname with input.
        /// <para>This operation will return null if the field already exists.</para>
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="entry">Entry to add to the collection.</param>
        /// <returns>Returns reference to added entry.</returns>
        IEntry AddEntry(string fieldname, IEntry entry);

        /// <summary>
        /// Add entry, using input unique fieldname and input value.
        /// </summary>
        /// <param name="fieldname">Unique field to add.</param>
        /// <param name="value">Value to create new entry with.</param>
        /// <returns>Returns reference to newly created entry.</returns>
        IEntry AddEntry(string fieldname, string value);

        /// <summary>
        /// Remove field at input index and associated entry from the collection.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        IEntry RemoveField(int index);

        /// <summary>
        /// Remove field and associated entry from the collection.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveField(string fieldname);
        
        /// <summary>
        /// Will make entry associated with the field index null.
        /// </summary>
        /// <param name="index">Field index to access.</param>
        /// <returns>Returns clone of removed entry. Will throw exception if index out of bounds.</returns>
        IEntry RemoveEntry(int index);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="fieldname">Field to access.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveEntry(string fieldname);

        /// <summary>
        /// Will make entry associated with the field null.
        /// </summary>
        /// <param name="entry">Entry to attempt to remove.</param>
        /// <returns>Returns clone of removed entry. Returns null if field does not exist.</returns>
        IEntry RemoveEntry(IEntry entry);                
    }

    /// <summary>
    /// IEntries wraps functionality for a key/value pair.
    /// </summary>
    public interface IEntry : IReplicate, INullable, IFieldNameValidator, IComparable
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Field associated with an entry.
        /// </summary>
        string Field { get; }

        /// <summary>
        /// Value associated with a field.
        /// </summary>
        string Value { get; }

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
