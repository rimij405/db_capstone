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
using Services.Interfaces;
using ISTE.DAL.Database.Interfaces;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Models;

// This namespace contains all interfaces
namespace ISTE.DAL.Models.Interfaces
{
    /// <summary>
    /// Represents type of database operation.
    /// </summary>
    public enum DatabaseOperation
    {
        /// <summary>
        /// Create/Insert operation.
        /// </summary>
        Create,

        /// <summary>
        /// Select/Read operation.
        /// </summary>
        Read,

        /// <summary>
        /// Update operation.
        /// </summary>
        Update,

        /// <summary>
        /// Delete operation.
        /// </summary>
        Delete
    }

    /// <summary>
    /// Interface representing objects that model data.
    /// </summary>
    public interface IDatabaseObjectModel : IReplicate<IDatabaseObjectModel>, IComparable {

        //////////////////////
        // Properties.
        //////////////////////
                
        /// <summary>
        /// Peek at the last saved snapshot.
        /// </summary>
        IDatabaseObjectMemento Snapshot
        {
            get;
        }

        /// <summary>
        /// Model's memento stack.
        /// </summary>
        Stack<IDatabaseObjectMemento> History
        {
            get;
        }

        /// <summary>
        /// Reference to the collection of IEntries that make up a data object.
        /// </summary>
        IRow Model {
            get;
        }

        /// <summary>
        /// Reference to primary key fields defining the model identifier.
        /// </summary>
        IList<IPrimaryKey> PrimaryKeys
        {
            get;
        }

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return entry at index reference, if one exists.
        /// </summary>
        /// <param name="index">Field index to reference.</param>
        /// <returns>Return entry at index.</returns>
        IEntry this[int index] { get; }

        /// <summary>
        /// Return entry with matching fieldname, if one exists.
        /// </summary>
        /// <param name="fieldname">Field to reference.</param>
        /// <returns>Return entry at field.</returns>
        IEntry this[string fieldname] { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Save the current version of the model.
        /// </summary>
        /// <returns>Returns the new momento.</returns>
        IDatabaseObjectMemento Save();
        
        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Sets a primary key using an individual key. Overwrites existing.
        /// </summary>
        /// <param name="key">Key to set.</param>
        void SetPrimaryKey(IPrimaryKey key);

        /// <summary>
        /// Sets a composite primary key using a set of keys. Overwrites existing.
        /// </summary>
        /// <param name="compositeKey">Keys to set.</param>
        void SetCompositeKey(IList<IPrimaryKey> compositeKey);

        /// <summary>
        /// Restore the data from the last saved memento from the stack and also return a reference to it.
        /// </summary>
        /// <returns>Returns the new momento.</returns>
        IDatabaseObjectMemento Restore();

        /// <summary>
        /// Load data from an input memento.
        /// </summary>
        /// <param name="memento">Memento to load the data from.</param>
        /// <returns>Returns reference to the model.</returns>
        IDatabaseObjectModel Load(IDatabaseObjectMemento memento);

    }

    /// <summary>
    /// A memento is a snapshot of a model.
    /// </summary>
    public interface IDatabaseObjectMemento
    {
        /// <summary>
        /// Reference to the collection of IEntries that make up a data object.
        /// </summary>
        IRow Data
        {
            get;
        }

        /// <summary>
        /// Model's memento stack.
        /// </summary>
        Stack<IDatabaseObjectMemento> History
        {
            get;
        }

        /// <summary>
        /// Reference to primary key fields defining the model identifier.
        /// </summary>
        IList<IPrimaryKey> PrimaryKeys
        {
            get;
        }

        /// <summary>
        /// Compare a snapshot to a model, to see if there has been any change.
        /// </summary>
        /// <param name="model">Model to compare.</param>
        /// <returns>Return true if different from the snapshot.</returns>
        int Compare(IDatabaseObjectModel model);
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
