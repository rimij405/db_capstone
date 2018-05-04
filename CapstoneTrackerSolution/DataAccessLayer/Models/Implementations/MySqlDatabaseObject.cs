/*
    DatabaseObject.cs
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

namespace ISTE.DAL.Models.Implementations
{

    /// <summary>
    /// Base database object class.
    /// </summary>
    public abstract class MySqlDatabaseObject : IDatabaseObject
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Holds collection of entries representing the user.
        /// </summary>
        private List<IEntry> internalModel;

        /// <summary>
        /// Collection of primary keys.
        /// </summary>
        private List<IEntry> primaryKeys;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to the collection of IEntries that make up a data object.
        /// </summary>
        public List<IEntry> Model
        {
            get {
                return this.internalModel = (this.internalModel ?? new List<IEntry>()) ;
            }
            protected set
            {
                this.internalModel = value;
            }
        }

        /// <summary>
        /// Database object reference to individual fields.
        /// </summary>
        /// <param name="field">Fieldname of field to get/set.</param>
        /// <returns>Returns entry associated with value at the particular field.</returns>
        public IEntry this[string field]
        {
            get
            {
                if (this.Model.Count > 0)
                {
                    foreach (IEntry entry in this.Model)
                    {
                        if (entry.HasField(field))
                        {
                            return entry;
                        }
                    }
                }
                return null;
            }
            set
            {
                IEntry entry = this[field];
                if (entry != null) { entry.SetValue(value.Value); }
            }
        }

        /// <summary>
        /// Storage of entry containing the primary key for the object.
        /// </summary>
        public List<IEntry> PrimaryKey
        {
            get
            {
                if (this.primaryKeys == null)
                {
                    this.primaryKeys = new List<IEntry>();
                }
                return this.primaryKeys;
            }
            set
            {
                if (this.primaryKeys == null)
                {
                    this.primaryKeys = new List<IEntry>();
                }
                foreach (IEntry key in value)
                {
                    if (this.HasField(key.Field))
                    {
                        if (!this.primaryKeys.Contains(key))
                        {
                            this.primaryKeys.Add(key);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if object is empty.
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                if (this.Model.Count == 0) { return true; }
                foreach (IEntry entry in this.Model)
                {
                    if (!entry.IsNull) { return false; }
                }
                return true;
            }
        }

        List<IEntry> IDatabaseObject.Model { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check if it has property.
        /// </summary>
        /// <param name="field">Property to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasField(string field)
        {
            foreach (IEntry entry in this.Model)
            {
                if (entry.HasField(field)) { return true; }
            }
            return false;
        }

        /// <summary>
        /// Check if it has property.
        /// </summary>
        /// <param name="field">Property to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(string field)
        {
            if (!String.IsNullOrEmpty(field))
            {
                foreach (IEntry entry in PrimaryKey)
                {
                    if (entry.HasField(field))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if it has property.
        /// </summary>
        /// <param name="field">Property to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(IEntry field)
        {
            if (field != null)
            {
                foreach (IEntry entry in PrimaryKey)
                {
                    if (entry.HasField(field.Field))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Check if it has all properties.
        /// </summary>
        /// <param name="fields">Properties to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(List<string> fields)
        {
            if (fields != null && fields.Count > 0)
            {
                int expected = fields.Count;
                foreach (string field in fields)
                {
                    if (this.HasPrimaryKey(field)) { expected--; }
                }
                return (expected <= 0);
            }
            return false;
        }

        /// <summary>
        /// Check if it has all properties.
        /// </summary>
        /// <param name="fields">Properties to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(params string[] fields)
        {
            return this.HasPrimaryKey(fields.ToList<string>());
        }

        /// <summary>
        /// Check if it has all properties.
        /// </summary>
        /// <param name="fields">Properties to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(List<IEntry> fields)
        {
            if (fields != null && fields.Count > 0)
            {
                int expected = fields.Count;
                foreach (IEntry field in fields)
                {
                    if (this.HasPrimaryKey(field)) { expected--; }
                }
                return (expected <= 0);
            }
            return false;
        }

        /// <summary>
        /// Check if it has all properties.
        /// </summary>
        /// <param name="fields">Properties to check.</param>
        /// <returns>Returns true if found.</returns>
        public bool HasPrimaryKey(params IEntry[] fields)
        {
            return this.HasPrimaryKey(fields.ToList<IEntry>());
        }

        /// <summary>
        /// Compare fields of one database object to another.
        /// </summary>
        /// <param name="other">Comparison.</param>
        /// <returns>Return result of comparison.</returns>
        public bool HasSameFields(IDatabaseObject other)
        {
            if (other != null)
            {                
                foreach (IEntry field in other.Model)
                {
                    if (!this.HasField(field.Field))
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Compare primary keys of one database object to another.
        /// </summary>
        /// <param name="other">Comparison.</param>
        /// <returns>Return result of comparison.</returns>
        public bool HasSamePrimaryKey(IDatabaseObject other)
        {
            if (this.HasSameFields(other))
            {
                return this.HasPrimaryKey(other.PrimaryKey);
            }
            return false;
        }

        /// <summary>
        /// Compare actual values of one database object to another.
        /// </summary>
        /// <param name="other">Comparison.</param>
        /// <returns>Return result of comparison.</returns>
        public bool HasSameValues(IDatabaseObject other)
        {
            if (this.HasSamePrimaryKey(other))
            {
                MySqlRow rowA = new MySqlRow(this.Model);
                MySqlRow rowB = new MySqlRow(other.Model);
                return (rowA.Equals(rowB));
            }
            return false;
        }

        //////////////////////
        // Helper method(s).

        /// <summary>
        /// Compare two database objects.
        /// </summary>
        /// <param name="other">Object to compare.</param>
        /// <returns>Returns sort integer.</returns>
        public int CompareTo(IDatabaseObject other)
        {
            if (other != null)
            {
                return new MySqlRow(this.Model).CompareTo(new MySqlRow(other.Model));
            }
            return Int32.MinValue;
        }

        //////////////////////
        // Accessor method(s).
        
        /// <summary>
        /// Retrieve entry at field.
        /// </summary>
        /// <param name="field">Field to access.</param>
        /// <returns>Returns entry at field.</returns>
        public IEntry Get(string field)
        {
            return this[field];
        }
        
        /// <summary>
        /// Get map of fields to their values as a set of MySql parameters.
        /// </summary>
        /// <returns>Returns map of fields and their parameters.</returns>
        public IDictionary<string, string> GetParameters()
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            foreach (IEntry entry in this.Model)
            {
                string field = entry.Field;
                string value = entry.Value;
                field = $"@{field.ToUpper()[0]}{field.ToLower().Substring(1)}";
                parameters.Add(field, value);
            }
            return parameters;
        }
        
        /// <summary>
        /// Get value of entry at a particular field.
        /// </summary>
        /// <param name="field">Field to access.</param>
        /// <returns>Returns value.</returns>
        public string GetValue(string field)
        {
            IEntry entry = this.Get(field);
            if (entry != null) { return entry.Value; }
            return "";
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set value of entry in given field.
        /// </summary>
        /// <param name="field">Field.</param>
        /// <param name="value">Value to set.</param>
        public void Set(string field, string value = null)
        {
            IEntry entry = this.Get(field);
            if (entry != null)
            {
                if (value == null) { entry.MakeNull(); }
                else { entry.SetValue(value); }
            }
        }

        /// <summary>
        /// Set value at input entry.
        /// </summary>
        /// <param name="field">Field to access.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Returns true if operation is successful.</returns>
        public bool SetValue(string field, string value)
        {
            if (!String.IsNullOrEmpty(field))
            {
                IEntry entry = this.Get(field);
                entry.SetData(entry.Field, value);
                return true;
            }
            return false;
        }

    }

    /// <summary>
    /// Modified base database object class.
    /// </summary>
    public abstract class MySqlDatabaseObjectModel : MySqlDatabaseObject, ICodeItem
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Model code.
        /// </summary>
        public virtual IUIDFormat Code
        {
            get { return new MySqlID(this.GetValue("code")); }
            protected set { this.SetValue("code", value.SQLValue); }
        }

        /// <summary>
        /// Model name.
        /// </summary>
        public virtual string Name
        {
            get { return this.GetValue("name"); }
            protected set { this.SetValue("name", value); }
        }

        /// <summary>
        /// Model description.
        /// </summary>
        public virtual string Description
        {
            get { return this.GetValue("description"); }
            protected set { this.SetValue("description", value); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        protected MySqlDatabaseObjectModel(IUIDFormat code)
            : this(code, null, null)
        {}

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        protected MySqlDatabaseObjectModel(string name)
            : this(null, name)
        {}

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlDatabaseObjectModel(IUIDFormat code, string name, string description)
            : this(code.SQLValue, name, description)
        {}

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlDatabaseObjectModel(string code, string name, string description = null)
        {
            // Add the entries.
            this.Model.Add(new MySqlEntry("code", code));
            this.Model.Add(new MySqlEntry("name", name));
            this.Model.Add(new MySqlEntry("description", description));
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Mutator method(s).
        
        /// <summary>
        /// Return the appropriate query to use.
        /// </summary>
        /// <returns>Return query string.</returns>
        protected abstract string GetQuery();

        /// <summary>
        /// Return the appropriate parameters to use.
        /// </summary>
        /// <returns>Return parameters object.</returns>
        protected abstract MySqlParameters GetParameters(string query);

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

            // If there is neither code nor name, we cannot fetch.
            if (String.IsNullOrWhiteSpace(this.Code.SQLValue) && String.IsNullOrWhiteSpace(this.Name))
            {
                errorCode = DatabaseError.MISSING_DATA;
                throw new DatabaseOperationException(errorCode, "Cannot fetch data - no unique identifiers present.");
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
            this.Code = (MySqlID)results[0, "code"].Value;
            this.Name = results[0, "name"].Value;
            this.Description = results[0, "description"].Value;
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
    }

}