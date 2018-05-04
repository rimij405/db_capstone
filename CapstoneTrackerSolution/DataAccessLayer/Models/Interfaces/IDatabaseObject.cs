/*
    IDatabaseObject.cs
    DBO interfaces. 
    ***
    04/03/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

// General using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Custom using statements.
using ISTE.DAL.Database.Interfaces;

// This namespace contains all interfaces
namespace ISTE.DAL.Models.Interfaces
{
    /// <summary>
    /// Comparable interface that will allow DBOs to be compared.
    /// </summary>
    public interface IDatabaseObjectComparable : IComparable, IComparable<IDatabaseObject>
    {
        /// <summary>
        /// Check if two database objects have the same primary key.
        /// </summary>
        /// <param name="other">Other object to compare.</param>
        /// <returns>Returns true if the primary key (even composite) is the same.</returns>
        bool HasSamePrimaryKey(IDatabaseObject other);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="field">Field to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(string field);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="field">Field to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(IEntry field);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="fields">Fields to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(List<string> fields);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="fields">Fields to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(params string[] fields);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="fields">Fields to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(List<IEntry> fields);

        /// <summary>
        /// Check if primary key matches input.
        /// </summary>
        /// <param name="fields">Fields to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasPrimaryKey(params IEntry[] fields);

        /// <summary>
        /// Check if the object has the same fields as one another.
        /// </summary>
        /// <param name="other">Object to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasSameFields(IDatabaseObject other);

        /// <summary>
        /// Check if the object has the same fields and same values as one another.
        /// </summary>
        /// <param name="other">Object to check.</param>
        /// <returns>Primary key match.</returns>
        bool HasSameValues(IDatabaseObject other);
    }

    /// <summary>
    /// Interface representing objects that model data.
    /// </summary>
    public interface IDatabaseObject : IEmpty, IDatabaseObjectComparable {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to the collection of IEntries that make up a data object.
        /// </summary>
        IRow Model {
            get;
            set;
        }
        
        /// <summary>
        /// Database object reference to individual fields.
        /// </summary>
        /// <param name="field">Fieldname of field to get/set.</param>
        /// <returns>Returns entry associated with value at the particular field.</returns>
        IEntry this[string field] {
            get;
            set;
        }

        /// <summary>
        /// Storage of entry containing the primary key for the object.
        /// </summary>
        List<IEntry> PrimaryKey
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
        /// Determine if database object contains a particular field.
        /// </summary>
        /// <param name="field">Fieldname to access.</param>
        /// <returns>Returns true if this object contains a reference to the field.</returns>
        bool HasField(string field);

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a set of key/value pairs containing field parameters and their values.
        /// </summary>
        /// <returns>Returns a dictionary containing a set of parameters.</returns>
        IDictionary<string, string> GetParameters();

        /// <summary>
        /// Return index of a particular entry in the object. Returns -1 if field doesn't exist.
        /// </summary>
        /// <param name="field">Fieldname to access.</param>
        /// <returns>Returns index of the entry.</returns>
        int GetIndex(string field);

        /// <summary>
        /// Return entry at particular field.
        /// </summary>
        /// <param name="fieldIndex">Field to access.</param>
        /// <returns>Returns entry containing field and value, if possible.</returns>
        IEntry Get(int fieldIndex);

        /// <summary>
        /// Return entry at particular field.
        /// </summary>
        /// <param name="field">Fieldname to access.</param>
        /// <returns>Returns entry containing field and value, if possible.</returns>
        IEntry Get(string field);

        /// <summary>
        /// Returns value at a particular field.
        /// </summary>
        /// <param name="fieldIndex">Field to access.</param>
        /// <returns>Returns value of particular field.</returns>
        string GetValue(int fieldIndex);

        /// <summary>
        /// Returns value at a particular field.
        /// </summary>
        /// <param name="field">Fieldname to access.</param>
        /// <returns>Returns value of particular field.</returns>
        string GetValue(string field);

        //////////////////////
        // Mutator method(s).      

        /// <summary>
        /// Attempts to set value at a particular entry.
        /// </summary>
        /// <param name="field">Fieldname to access.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns value of particular field.</returns>
        bool SetValue(string field, string value);

        /// <summary>
        /// Attempts to set value at a particular entry.
        /// </summary>
        /// <param name="fieldIndex">Field to access.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns value of particular field.</returns>
        bool SetValue(int fieldIndex, string value);

    }

    /// <summary>
    /// Represents an error that can be returned by the database.
    /// </summary>
    public enum DatabaseError
    {
        /// <summary>
        /// No error.
        /// </summary>
        NONE = 0,

        /// <summary>
        /// Unknown error has been thrown.
        /// </summary>
        UNKNOWN = -1,

        /// <summary>
        /// There was an error with the SQL command syntax.
        /// </summary>
        SYNTAX = 1,

        /// <summary>
        /// An insert command could not be resolved due to a duplicate unique field value.
        /// </summary>
        DUPLICATE = 2,

        /// <summary>
        /// A record doesn't exist to perform a fetch or update operation.
        /// </summary>
        MISSING = 3
    }
    
    /// <summary>
    /// Fetch data for a particular database object.
    /// </summary>
    public interface IDatabaseObjectReader
    {

        /// <summary>
        /// Obtains information associated with the database object from a readable database. Applies results of fetch to the current object.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns a result set.</returns>
        IResultSet Fetch(IReadable database, out DatabaseError errorCode);

        /// <summary>
        /// Attempts to fetch data from the database, catching exceptions and returning the proper error code on failure.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="results">Results returned by the Fetch method.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns true if operation was a success.</returns>
        bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode);
            
    }

    /// <summary>
    /// Delete a particular database object from the database.
    /// </summary>
    public interface IDatabaseObjectDeleter
    {

        /// <summary>
        /// Deletes record that matches this object's primary key from the database.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns a result set.</returns>
        IResultSet Delete(IWritable database, out DatabaseError errorCode);

        /// <summary>
        /// Attempts to delete data from the database, catching exceptions and returning the proper error code on failure.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="results">Results returned by the Fetch method.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns true if operation was a success.</returns>
        bool TryDelete(IWritable database, out IResultSet results, out DatabaseError errorCode);

    }

    /// <summary>
    /// Insert a particular database object into the database.
    /// </summary>
    public interface IDatabaseObjectInserter {

        /// <summary>
        /// Inserts the DBO's current data into the database, as a new entry. If the primary key already exists (or other unique key information matches) this can result in an error.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns a result set.</returns>
        IResultSet Insert(IWritable database, out DatabaseError errorCode);

        /// <summary>
        /// Attempts to insert data into the database, catching exceptions and returning the proper error code on failure.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="results">Results returned by the Fetch method.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns true if operation was a success.</returns>
        bool TryInsert(IWritable database, out IResultSet results, out DatabaseError errorCode);

    }
    
    /// <summary>
    /// Update a particular database object in the database.
    /// </summary>
    public interface IDatabaseObjectUpdater
    {

        /// <summary>
        /// Updates the DBO's current data into the database, as is.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns a result set.</returns>
        IResultSet Update(IWritable database, out DatabaseError errorCode);

        /// <summary>
        /// Attempts to update data in the database, catching exceptions and returning the proper error code on failure.
        /// </summary>
        /// <param name="database">Database that will be accessed.</param>
        /// <param name="results">Results returned by the Fetch method.</param>
        /// <param name="errorCode">Error code associated with a particular exception.</param>
        /// <returns>Returns true if operation was a success.</returns>
        bool TryUpdate(IWritable database, out IResultSet results, out DatabaseError errorCode);

    }
}
