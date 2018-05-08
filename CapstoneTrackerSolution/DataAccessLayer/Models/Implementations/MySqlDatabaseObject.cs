/*
    MySqlDatabaseObject.cs
    ---
    Ian Effendi
 */

// using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ISTE.DAL.Database.Interfaces;

// additional using statements.
using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Models.Interfaces;
using Services.Interfaces;

namespace ISTE.DAL.Models.Implementations
{

    /// <summary>
    /// Represents a MySqlDatabaseObjectMemento.
    /// </summary>
    public struct MySqlDatabaseObjectMemento : IDatabaseObjectMemento
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Cast to MySqlRow.
        /// </summary>
        /// <param name="memento">Memento to cast.</param>
        public static explicit operator MySqlRow(MySqlDatabaseObjectMemento memento)
        {
            return memento.Data as MySqlRow;
        }

        /// <summary>
        /// Cast to MySqlDatabaseObjectMemento.
        /// </summary>
        /// <param name="row">Row to cast.</param>
        public static explicit operator MySqlDatabaseObjectMemento(MySqlRow row)
        {
            return new MySqlDatabaseObjectMemento(row, new List<IPrimaryKey>(), new Stack<IDatabaseObjectMemento>());
        }
        
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// MySqlRow data.
        /// </summary>
        private MySqlRow internalData;

        /// <summary>
        /// Internal memento stack.
        /// </summary>
        private Stack<IDatabaseObjectMemento> internalHistory;

        /// <summary>
        /// Internal storage of primary keys.
        /// </summary>
        private List<IPrimaryKey> internalKeys;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to internal model.
        /// </summary>
        public IRow Data
        {
            get
            {
                return this.internalData;
            }
        }

        /// <summary>
        /// Reference to history at time of creation.
        /// </summary>
        public Stack<IDatabaseObjectMemento> History
        {
            get
            {
                return this.internalHistory;
            }
        }

        /// <summary>
        /// Reference to the primary keys.
        /// </summary>
        public IList<IPrimaryKey> PrimaryKeys
        {
            get
            {
                return this.internalKeys;
            }
        }

        //////////////////////
        // Indexer(s).
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a memento with the input data values.
        /// </summary>
        /// <param name="data">Record data to store.</param>
        /// <param name="keys">Primary keys of the data.</param>
        /// <param name="history">History.</param>
        public MySqlDatabaseObjectMemento(IRow data, IList<IPrimaryKey> keys, Stack<IDatabaseObjectMemento> history)
        {
            // Clone the data.
            this.internalData = data.Clone() as MySqlRow;
            this.internalData.IsReadOnly = true;

            // Clone the list.
            this.internalKeys = new List<IPrimaryKey>(keys);

            // Clones the stack.
            IDatabaseObjectMemento[] stack = history.ToArray<IDatabaseObjectMemento>();
            stack.Reverse<IDatabaseObjectMemento>();
            this.internalHistory = new Stack<IDatabaseObjectMemento>(stack);          
        }

        /// <summary>
        /// Clone a memento.
        /// </summary>
        /// <param name="memento">Memento to clone.</param>
        public MySqlDatabaseObjectMemento(MySqlDatabaseObjectMemento memento)
        {
            // Clone the data.
            this.internalData = memento.Data.Clone() as MySqlRow;
            this.internalData.IsReadOnly = true;

            // Clone the list.
            this.internalKeys = new List<IPrimaryKey>(memento.PrimaryKeys);

            // Clones the stack.
            IDatabaseObjectMemento[] stack = memento.History.ToArray<IDatabaseObjectMemento>();
            stack.Reverse<IDatabaseObjectMemento>();
            this.internalHistory = new Stack<IDatabaseObjectMemento>(stack);
        }

        //////////////////////
        // Method(s).
        //////////////////////
        
        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Compare internal data for the memento against the model.
        /// </summary>
        /// <param name="model">Model to compare.</param>
        /// <returns>Returns the comparison index value.</returns>
        public int Compare(IDatabaseObjectModel model)
        {
            return this.Data.Compare(model.Model);
        }
    }

    /// <summary>
    /// Base database object class.
    /// </summary>
    public abstract class MySqlDatabaseObjectModel : IDatabaseObjectModel, IComparison<IDatabaseObjectModel>
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Conversion operators.

        /// <summary>
        /// Cast to MySqlRow.
        /// </summary>
        /// <param name="model">Cast to MySqlRow.</param>
        public static implicit operator MySqlRow(MySqlDatabaseObjectModel model)
        {
            return model.Model.Clone() as MySqlRow;
        }

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Internal data structure storing the database model's data.
        /// </summary>
        private MySqlRow internalModel;

        /// <summary>
        /// Internal data structure storing the primary keys.
        /// </summary>
        private List<IPrimaryKey> internalKeys;

        /// <summary>
        /// Stack for the memento history.
        /// </summary>
        private Stack<IDatabaseObjectMemento> internalHistory;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Stack of the model's mementos.
        /// </summary>
        public Stack<IDatabaseObjectMemento> History
        {
            get
            {
                if (this.internalHistory == null)
                {
                    this.internalHistory = new Stack<IDatabaseObjectMemento>();
                }
                return this.internalHistory;
            }
        }

        /// <summary>
        /// Peek at the top-most memento.
        /// </summary>
        public IDatabaseObjectMemento Snapshot
        {
            get
            {
                return this.History.Peek();
            }
        }

        /// <summary>
        /// Internal model to store database model data.
        /// </summary>
        public IRow Model
        {
            get
            {
                if (this.internalModel == null)
                {
                    this.internalModel = new MySqlRow();
                }
                return this.internalModel;
            }

            protected set
            {
                this.internalModel = value as MySqlRow;
            }
        }

        /// <summary>
        /// Collection of primary keys.
        /// </summary>
        public IList<IPrimaryKey> PrimaryKeys
        {
            get
            {
                if (this.internalKeys == null)
                {
                    this.internalKeys = new List<IPrimaryKey>();
                }
                return this.internalKeys;
            }
        }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Return entry at index reference, if one exists.
        /// </summary>
        /// <param name="index">Field index to reference.</param>
        /// <returns>Return entry at index.</returns>
        public IEntry this[int index]
        {
            get { return this.Model[index]; }
        }

        /// <summary>
        /// Return entry with matching fieldname, if one exists.
        /// </summary>
        /// <param name="fieldname">Field to reference.</param>
        /// <returns>Return entry at field.</returns>
        public IEntry this[string fieldname]
        {
            get { return this.Model[fieldname]; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Empty model constructor.
        /// </summary>
        protected MySqlDatabaseObjectModel() { }

        /// <summary>
        /// Creates a model from an input row.
        /// </summary>
        /// <param name="model">Model data.</param>
        protected MySqlDatabaseObjectModel(MySqlRow model)
        {
            this.internalModel = new MySqlRow(model);
        }
        
        /// <summary>
        /// Create a model from an input memento.
        /// </summary>
        /// <param name="memento">Stored model data.</param>
        protected MySqlDatabaseObjectModel(IDatabaseObjectMemento memento)
        {
            this.Load(memento);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        #region IReplicate

        /// <summary>
        /// Clone object.
        /// </summary>
        /// <returns>Return clone.</returns>
        public abstract IDatabaseObjectModel Clone();

        /// <summary>
        /// Clone object.
        /// </summary>
        /// <returns>Return clone.</returns>
        public object Clone(object objectToClone)
        {
            if (objectToClone is MySqlDatabaseObjectModel obj)
            {
                return obj.Clone();
            }
            if (objectToClone is IReplicate replicant)
            {
                return replicant.Clone(replicant);
            }
            return null;
        }

        #endregion

        #region IComparison

        #endregion

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return the query to be used with the fetch request.
        /// </summary>
        /// <returns>Returns SQL query string.</returns>
        protected abstract string GetQuery();

        /// <summary>
        /// Return the parameters to be used with the fetch request.
        /// </summary>
        /// <returns>Returns SQL parameters.</returns>
        protected abstract MySqlParameters GetParameters(string query);

        /// <summary>
        /// Save current snapshot.
        /// </summary>
        /// <returns>Return reference to the saved memento.</returns>
        public IDatabaseObjectMemento Save()
        { 
            // Create new snapshot.
            IDatabaseObjectMemento memento
                = new MySqlDatabaseObjectMemento(this.Model, this.PrimaryKeys, this.History);

            // Push onto the history.
            this.History.Push(memento);

            // Return snapshot.
            return memento;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Assign a composite primary key made up of separate keys.
        /// </summary>
        /// <param name="compositeKey">Primary keys of model.</param>
        public void SetCompositeKey(IList<IPrimaryKey> compositeKey)
        {
            if (compositeKey != null && compositeKey.Count > 0)
            {
                PrimaryKeys.Clear();
                foreach (IPrimaryKey key in compositeKey)
                {
                    this.PrimaryKeys.Add(key);
                }
            }
        }

        /// <summary>
        /// Assign a single primary key.
        /// </summary>
        /// <param name="key">Primary key of model.</param>
        public void SetPrimaryKey(IPrimaryKey key)
        {
            if (key != null)
            {
                this.PrimaryKeys.Clear();
                this.PrimaryKeys.Add(key);
            }
        }

        /// <summary>
        /// Restore the top-most memento data in the stack.
        /// </summary>
        /// <returns>Returns reference to the memento.</returns>
        public IDatabaseObjectMemento Restore()
        {
            IDatabaseObjectMemento memento = this.Snapshot;
            if (this.History.Count > 1)
            {
                memento = this.History.Pop();
                this.Load(memento);
            }
            return memento;
        }

        /// <summary>
        /// Load the data from the input memento. Overwrites the stack history.
        /// </summary>
        /// <param name="memento">Memento data to load.</param>
        /// <returns>Returns reference to self.</returns>
        public IDatabaseObjectModel Load(IDatabaseObjectMemento memento)
        {
            if (memento != null)
            {
                this.internalKeys = (memento.PrimaryKeys as List<IPrimaryKey>);
                this.internalModel = memento.Data as MySqlRow;
                this.internalHistory = memento.History;
            }
            return this;
        }

        /// <summary>
        /// Fetch all relevant codes from the database.
        /// </summary>
        /// <param name="database">Database to read from.</param>
        /// <param name="errorCode">Error code returned.</param>
        /// <returns>Return the result set.</returns>
        public virtual IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            // Results to return.
            MySqlResultSet results = new MySqlResultSet();
            results.Error(); // Set to error state by default.
            errorCode = DatabaseError.NONE;

            // Check primary keys
            foreach (IPrimaryKey key in this.PrimaryKeys)
            {
                if (key.PrimaryKey)
                {
                    if (key.IsNull || String.IsNullOrWhiteSpace(key.Value))
                    {
                        errorCode = DatabaseError.MISSING_DATA;
                        throw new DatabaseOperationException(errorCode, $"Cannot fetch data - unique identifier {key.Value} is missing.");
                    }
                }
            }

            // Determine the query.
            // Defer to code when possible, but, if null, attempt selection by name.
            string query = GetQuery();

            // Create the parameters.
            MySqlParameters parameters = GetParameters(query);

            // If there is no database we cannot fetch.
            if (database == null || !(database is MySqlDatabase))
            {
                errorCode = DatabaseError.MISSING_DATA;
                throw new DatabaseOperationException(errorCode, "Cannot fetch data from non-MySQL database.");
            }

            // Using the database, fetch the data.
            if (database is MySqlDatabase mysqldb)
            {
                // Cast IResultSet to new form.
                results = mysqldb.GetData(query, parameters) as MySqlResultSet;

                if (results == null || results.IsEmpty) { results.Fail(); }
                else
                {
                    this.SetResults(results);
                    results.Pass();
                }
            }

            return results;
        }

        /// <summary>
        /// Set the results from the query.
        /// </summary>
        /// <param name="results">Results to set.</param>
        protected virtual void SetResults(IResultSet results)
        {
            foreach (IEntry entry in this.Model.Entries)
            {
                if (entry is IPrimaryKey primaryKey && primaryKey.PrimaryKey)
                {
                    // Do NOT overwrite values for primary key!
                    continue;
                }
                else
                {
                    if(results.TryGetRow(0, out IRow row))
                    {
                        if (row.TryGetEntry(entry.Field, out IEntry resultEntry))
                        {
                            this.Model[entry.Field].SetValue(resultEntry.Value);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Attempt to fetch the data.
        /// </summary>
        /// <param name="database">Database to read from.</param>
        /// <param name="results">Result set containing response.</param>
        /// <param name="errorCode">Error code returned.</param>
        /// <returns>Return the result set.</returns>
        public virtual bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode)
        {
            results = new MySqlResultSet();
            results.Error();
            errorCode = DatabaseError.UNKNOWN;

            try
            {
                // Complete the fetch command.
                results = this.Fetch(database, out errorCode);

                if (results == null || results.IsFailure)
                {
                    return false;
                }
                else if (results.IsError || errorCode != DatabaseError.NONE)
                {
                    throw new DatabaseOperationException(errorCode);
                }
                else
                {
                    return true;
                }
            }
            catch (DatabaseOperationException doe)
            {
                results.Error();
                errorCode = doe.ErrorCode;
                return false;
            }
        }

        /// <summary>
        /// Compare individual entries in the models.
        /// </summary>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public abstract int CompareModels(IDatabaseObjectModel left, IDatabaseObjectModel right);

        /// <summary>
        /// Compare values.
        /// </summary>
        /// <param name="other">object to compare.</param>
        /// <returns>Returns comparison value.</returns>
        public int Compare(IDatabaseObjectModel other)
        {
            return this.CompareValue(this, other);
        }

        /// <summary>
        /// Compare for equality.
        /// </summary>
        /// <param name="other">object to compare.</param>
        /// <returns>Returns comparison value.</returns>
        public bool IsEqual(IDatabaseObjectModel other)
        {
            return (this.Compare(other) == 0);
        }

        /// <summary>
        /// Comparison of values.
        /// </summary>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public virtual int CompareValue(IDatabaseObjectModel left, IDatabaseObjectModel right)
        {
            if (left == null && right == null) { return 0; }
            if (left == null) { return -1; }
            if (right == null) { return 1; }
            if (left.Model.IsEmpty && right.Model.IsEmpty) { return 0; }
            if (left.Model.IsEmpty) { return -1; }
            if (right.Model.IsEmpty) { return 1; }
            if (left == right) { return 0; }
            return CompareModels(left, right);
        }

        /// <summary>
        /// Find equality value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsEqualValue(IDatabaseObjectModel left, IDatabaseObjectModel right)
        {
            return (this.CompareValue(left, right) == 0);
        }

        /// <summary>
        /// Find greater than value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsGreaterThanValue(IDatabaseObjectModel left, IDatabaseObjectModel right)
        {
            return (this.CompareValue(left, right) >= 1);
        }

        /// <summary>
        /// Find less than value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsLessThanValue(IDatabaseObjectModel left, IDatabaseObjectModel right)
        {
            return (this.CompareValue(left, right) <= -1);
        }

        /// <summary>
        /// Find comparison value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public int CompareValue<T>(T left, T right) where T : IComparable
        {
            return left.CompareTo(right);
        }

        /// <summary>
        /// Find if equal than value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsEqualValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) == 0);
        }

        /// <summary>
        /// Find greater than value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsGreaterThanValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) >= 1);
        }

        /// <summary>
        /// Find less than value.
        /// </summary>
        /// <typeparam name="T">Data to compare.</typeparam>
        /// <param name="left">Left model.</param>
        /// <param name="right">Right model.</param>
        /// <returns>Returns comparison value</returns>
        public bool IsLessThanValue<T>(T left, T right) where T : IComparable
        {
            return (this.CompareValue<T>(left, right) <= 1);
        }

        /// <summary>
        /// Comparison.
        /// </summary>
        /// <param name="obj">Other object.</param>
        /// <returns>Returns value.</returns>
        public int CompareTo(object obj)
        {
            if (obj is IDatabaseObjectModel model)
            {
                this.Compare(model);
            }
            return 1;
        }
    }

    /// <summary>
    /// Base code model database class.
    /// </summary>
    public abstract class MySqlDatabaseCodeModel : MySqlDatabaseObjectModel
    {
        
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Code.
        /// </summary>
        public IUIDFormat Code
        {
            get { return new MySqlID(this["code"].Value); }
            set { this["code"].SetValue(value.SQLValue); }
        }

        /// <summary>
        /// Code name.
        /// </summary>
        public string Name
        {
            get { return this["name"].Value; }
            set { this["name"].SetValue(value); }
        }

        /// <summary>
        /// Code description.
        /// </summary>
        public string Description
        {
            get { return this["description"].Value; }
            set { this["description"].SetValue(value); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Empty constructor.
        /// </summary>
        protected MySqlDatabaseCodeModel() { }

        /// <summary>
        /// Construct model.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlDatabaseCodeModel(IUIDFormat code, string name, string description)
            : base()
        {
            // Add entries to the data model.
            this.Model.AddEntry(new MySqlPrimaryKeyEntry(true, "code", code.SQLValue));
            this.Model.AddEntry(new MySqlPrimaryKeyEntry(false, "name", name));
            this.Model.AddEntry(new MySqlEntry("description", description));

            // Set the primary key.
            this.SetCompositeKey(new List<IPrimaryKey>() { this["code"] as IPrimaryKey, this["name"] as IPrimaryKey });

            // Save the model's snapshot.
            this.Save();
        }

        //////////////////////
        // Method(s).
        //////////////////////

        /// <summary>
        /// Compare the term values.
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public override int CompareModels(IDatabaseObjectModel left, IDatabaseObjectModel right)
        {
            if (left == null && right == null) { return 0; }
            if (left == null) { return -1; }
            if (right == null) { return 1; }
            if (left == right) { return 0; }
            if (left.Model == right.Model) { return 0; }

            string leftValues = "";
            string rightValues = "";

            if (left is MySqlDatabaseCodeModel leftTerm && right is MySqlDatabaseCodeModel rightTerm)
            {
                leftValues += leftTerm.Code.SQLValue;
                leftValues += leftTerm.Name;
                leftValues += leftTerm.Description;

                rightValues += rightTerm.Code.SQLValue;
                rightValues += rightTerm.Name;
                rightValues += rightTerm.Description;

                return leftValues.CompareTo(rightValues);
            }

            if (left is MySqlDatabaseCodeModel) { return 1; }
            if (right is MySqlDatabaseCodeModel) { return -1; }
            return left.Model.CompareTo(right.Model);
        }

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            if ((this["code"] as IPrimaryKey).PrimaryKey)
            {
                return GetCodeQuery();
            }

            if ((this["name"] as IPrimaryKey).PrimaryKey)
            {
                return GetNameQuery();
            }

            return "";
        }
        
        /// <summary>
        /// Find data using code.
        /// </summary>
        /// <returns>Returns SQL query string.</returns>
        protected abstract string GetCodeQuery();

        /// <summary>
        /// Find data using name.
        /// </summary>
        /// <returns>Returns SQL query string.</returns>
        protected abstract string GetNameQuery();
    }

}