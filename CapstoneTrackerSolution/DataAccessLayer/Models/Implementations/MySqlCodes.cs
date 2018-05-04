/*
    MySqlCodes.cs
    ---
    Ian Effendi
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements;
using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Database.Interfaces;
using ISTE.DAL.Models.Interfaces;

namespace ISTE.DAL.Models.Implementations
{

    /// <summary>
    /// Abstract implementation of the CodeGlossary class.
    /// </summary>
    /// <typeparam name="TCodeItem">Generic type.</typeparam>
    public abstract class CodeGlossary<TCodeItem> : ICodeGlossary<TCodeItem> where TCodeItem : ICodeItem
    {

        //////////////////////
        // Field(s).
        //////////////////////

        private List<ICodeItem> items;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to collection of items.
        /// </summary>
        public IList<ICodeItem> Glossary
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<ICodeItem>();
                }
                return this.items;
            }
        }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Find item in collection matching a particular code.
        /// </summary>
        /// <param name="code">Code to match.</param>
        /// <returns>Item to return.</returns>
        public ICodeItem this[string code]
        {
            get
            {
                foreach (ICodeItem item in this.Glossary)
                {
                    if (item.Code.SQLValue == code)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="code">Code to find model for.</param>
        /// <returns>Returns the code model.</returns>
        public TCodeItem Find(string code)
        {
            return (TCodeItem)this[code];
        }

        /// <summary>
        /// Returns a code model associated with the input name. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="name">Name of model to find.</param>
        /// <returns>Returns the code model.</returns>
        public TCodeItem FindByName(string name)
        {
            foreach (ICodeItem item in this.Glossary)
            {
                if (item is TCodeItem model)
                {
                    if (model.Name.Trim().ToUpper() == name.Trim().ToUpper())
                    {
                        return model;
                    }
                }
            }
            return default(TCodeItem);
        }

        /// <summary>
        /// Return query to execute.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected abstract string GetQuery();

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected abstract void AddItem(string code, string name, string description);

        /// <summary>
        /// Fetch data.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public virtual IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            // Results to return.
            MySqlResultSet results = new MySqlResultSet();
            results.Error(); // Set to error state by default.
            errorCode = DatabaseError.NONE;

            // Select all the roles.
            string query = this.GetQuery();

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
                results = mysqldb.GetData(query) as MySqlResultSet;

                if (results == null || results.IsEmpty) { results.Fail(); }
                else
                {
                    // Create a role for every record in the result set.
                    foreach (MySqlRow row in results.Rows)
                    {
                        string code = row["code"].Value;
                        string name = row["name"].Value;
                        string description = row["description"].Value;
                        this.AddItem(code, name, description);
                    }
                    results.Pass();
                }
            }

            return results;
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

    /// <summary>
    /// Collection of MySqlStatus models.
    /// </summary>
    public class MySqlTerms : ITerms
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all terms.
        /// </summary>
        private const string SELECT_ALL = "SELECT code, termStart, termEnd, gradeDeadline, addDropDeadline FROM capstonedb.term;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlTerms instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlTerms GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlTerms(database);
            }
            return instance;
        }
        //////////////////////
        // Field(s).
        //////////////////////

        private List<ITermModel> items;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to collection of items.
        /// </summary>
        public IList<ITermModel> Glossary
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<ITermModel>();
                }
                return this.items;
            }
        }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Find item in collection matching a particular code.
        /// </summary>
        /// <param name="code">Code to match.</param>
        /// <returns>Item to return.</returns>
        public ITermModel this[string code]
        {
            get
            {
                foreach (ITermModel item in this.Glossary)
                {
                    if (item.Code.SQLValue == code)
                    {
                        return item;
                    }
                }
                return null;
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="code">Code to find model for.</param>
        /// <returns>Returns the code model.</returns>
        public ITermModel Find(string code)
        {
            return (ITermModel)this[code];
        }
        
        /// <summary>
        /// Fetch data.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            // Results to return.
            MySqlResultSet results = new MySqlResultSet();
            results.Error(); // Set to error state by default.
            errorCode = DatabaseError.NONE;

            // Select all the roles.
            string query = this.GetQuery();

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
                results = mysqldb.GetData(query) as MySqlResultSet;

                if (results == null || results.IsEmpty) { results.Fail(); }
                else
                {
                    // Create a role for every record in the result set.
                    foreach (MySqlRow row in results.Rows)
                    {
                        string code = row["code"].Value;
                        string termStart = row["termStart"].Value;
                        string termEnd = row["termEnd"].Value;
                        string gradeDeadline = row["gradeDeadline"].Value;
                        string addDropDeadline = row["addDropDeadline"].Value;
                        this.AddItem(code, termStart, termEnd, gradeDeadline, addDropDeadline);
                    }
                    results.Pass();
                }
            }

            return results;
        }

        /// <summary>
        /// Attempt to fetch the data.
        /// </summary>
        /// <param name="database">Database to read from.</param>
        /// <param name="results">Result set containing response.</param>
        /// <param name="errorCode">Error code returned.</param>
        /// <returns>Return the result set.</returns>
        public bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode)
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
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlTerms(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected string GetQuery()
        {
            return SELECT_ALL;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="termStart">Start.</param>
        /// <param name="termEnd">End.</param>
        /// <param name="gradeDeadline">Deadline.</param>
        /// <param name="addDropDeadline">Deadline.</param>
        protected void AddItem(string code, string termStart, string termEnd, string gradeDeadline, string addDropDeadline)
        {
            this.Glossary.Add(MySqlTerm.Create(code, termStart, termEnd, gradeDeadline, addDropDeadline));
        }
    }
    
    /// <summary>
    /// Representation of the status code-name-description tuple.
    /// </summary>
    public class MySqlTerm : MySqlDatabaseObjectModel, ITermModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT = "SELECT code, termStart, termEnd, gradeDeadline, addDropDeadline FROM capstonedb.term WHERE code=@termCode;";
        
        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="code">Code to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlTerm GetTerm(IReadable database, MySqlID code)
        {
            MySqlTerm type = new MySqlTerm(code as IUIDFormat);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }
        
        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="termStart">Start.</param>
        /// <param name="termEnd">End.</param>
        /// <param name="grade">Deadline</param>
        /// <param name="addDrop">Deadline</param>
        /// <returns>Return the model.</returns>
        public static MySqlTerm Create(string code, string termStart, string termEnd, string grade, string addDrop)
        {
            return new MySqlTerm(new MySqlID(code), termStart, termEnd, grade, addDrop);
        }

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Return the range.
        /// </summary>
        public IDateTimeRange TermRange
        {
            get { return new MySqlDateRange(this.TermStart.Value, this.TermEnd.Value) as IDateTimeRange; }
        }
        
        /// <summary>
        /// Start of term.
        /// </summary>
        public IDateTimeFormat TermStart
        {
            get { return new MySqlDateTime(this.GetValue("termStart")); }
            set { this.SetValue("termStart", value.SQLValue); }
        }

        /// <summary>
        /// End of term.
        /// </summary>
        public IDateTimeFormat TermEnd
        {
            get { return new MySqlDateTime(this.GetValue("termEnd")); }
            set { this.SetValue("termEnd", value.SQLValue); }
        }

        /// <summary>
        /// Grade deadline.
        /// </summary>
        public IDateTimeFormat GradeDeadline
        {
            get { return new MySqlDateTime(this.GetValue("gradeDeadline")); }
            set { this.SetValue("gradeDeadline", value.SQLValue); }
        }

        /// <summary>
        /// Add drop deadline.
        /// </summary>
        public IDateTimeFormat AddDropDeadline
        {
            get { return new MySqlDateTime(this.GetValue("addDropDeadline")); }
            set { this.SetValue("addDropDeadline", value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        protected MySqlTerm(IUIDFormat code)
            : base(code, null, null)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="start">Start.</param>
        /// <param name="end">End.</param>
        /// <param name="grade">Deadline</param>
        /// <param name="addDrop">Deadline</param>
        protected MySqlTerm(IUIDFormat code, string start, string end, string grade, string addDrop)
            : base(code.SQLValue, "Term", "A term.")
        {
            this.Model.Add(new MySqlEntry("termStart", start));
            this.Model.Add(new MySqlEntry("termEnd", end));
            this.Model.Add(new MySqlEntry("gradeDeadline", grade));
            this.Model.Add(new MySqlEntry("addDropDeadline", addDrop));
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return SELECT;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>() {
                { "@termCode", this.Code.SQLValue }
            };
            return parameters;
        }

        /// <summary>
        /// Set the results from the query.
        /// </summary>
        /// <param name="results">Results to set.</param>
        protected override void SetResults(IResultSet results)
        {
            this.Code = (MySqlID)results[0, "code"].Value;
            this.TermStart = new MySqlDateTime(results[0, "termStart"].Value);
            this.TermEnd = new MySqlDateTime(results[0, "termEnd"].Value);
            this.GradeDeadline = new MySqlDateTime(results[0, "gradeDeadline"].Value);
            this.AddDropDeadline = new MySqlDateTime(results[0, "addDropDeadline"].Value);
        }

    }

    /// <summary>
    /// Collection of MySqlStatus models.
    /// </summary>
    public class MySqlStatuses : CodeGlossary<IStatusModel>, IStatuses
    { 
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all statuses.
        /// </summary>
        private const string SELECT_ALL_STATUSES = "SELECT code, name, description FROM capstonedb.statuses;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlStatuses instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlStatuses GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlStatuses(database);
            }
            return instance;
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlStatuses(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected override string GetQuery()
        {
            return SELECT_ALL_STATUSES;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected override void AddItem(string code, string name, string description)
        {
            this.Glossary.Add(MySqlStatus.Create(code, name, description));
        }

    }

    /// <summary>
    /// Representation of the status code-name-description tuple.
    /// </summary>
    public class MySqlStatus : MySqlDatabaseObjectModel, IStatusModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT_BY_CODE = "SELECT code, name, description FROM capstonedb.statuses WHERE code=@statusCode;";

        /// <summary>
        /// Query used to select role type by role name.
        /// </summary>
        private const string SELECT_BY_NAME = "SELECT code, name, description FROM capstonedb.statuses WHERE name=@statusName;";
      
        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="code">Code to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlStatus GetStatusType(IReadable database, MySqlID code)
        {
            MySqlStatus type = new MySqlStatus(code as IUIDFormat);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="name">Name to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlStatus GetStatusType(IReadable database, string name)
        {
            MySqlStatus type = new MySqlStatus(name);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <returns>Return the model.</returns>
        public static MySqlStatus Create(string code, string name, string description)
        {
            return new MySqlStatus(new MySqlID(code), name, description);
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        protected MySqlStatus(IUIDFormat code)
            : base(code, null, null)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        protected MySqlStatus(string name)
            : base(null, name)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlStatus(IUIDFormat code, string name, string description)
            : base(code.SQLValue, name, description)
        { }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return (String.IsNullOrWhiteSpace(this.Code.SQLValue)) ? SELECT_BY_NAME : SELECT_BY_CODE;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            if (query == SELECT_BY_CODE) { parameters.AddWithValue("@statusCode", this.Code.SQLValue); }
            else { parameters.AddWithValue("@statusName", this.Name); }
            return parameters;
        }
    }

    /// <summary>
    /// Represents a collection of status history events.
    /// </summary>
    public class MySqlStatusEventHistory : IStatusEvents
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all terms.
        /// </summary>
        private const string SELECT_ALL = "SELECT capstoneID, statusCode, timeStamp FROM capstonedb.statushistoryevent;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlStatusEventHistory instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlStatusEventHistory GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlStatusEventHistory(database);
            }
            return instance;
        }
        //////////////////////
        // Field(s).
        //////////////////////

        private List<IStatusEventModel> items;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to collection of items.
        /// </summary>
        public IList<IStatusEventModel> Glossary
        {
            get
            {
                if (this.items == null)
                {
                    this.items = new List<IStatusEventModel>();
                }
                return this.items;
            }
        }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Find item in collection matching a particular code.
        /// </summary>
        /// <param name="capstoneID">Capstone association.</param>
        /// <param name="statusCode">Event status.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        /// <returns>Item to return.</returns>
        public IStatusEventModel this[string capstoneID, string statusCode, string timestamp]
        {
            get
            {
                foreach (IStatusEventModel item in this.Glossary)
                {
                    if (item is MySqlStatusEvent evt)
                    {
                        if (evt.CapstoneID.SQLValue == capstoneID && evt.StatusCode.SQLValue == statusCode && evt.Timestamp.SQLValue == timestamp)
                        {
                            return item;
                        }
                    }
                }
                return null;
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="capstoneID">Capstone association.</param>
        /// <param name="statusCode">Event status.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        /// <returns>Returns the code model.</returns>
        public IStatusEventModel Find(string capstoneID, string statusCode, string timestamp)
        {
            return (IStatusEventModel)this[capstoneID, statusCode, timestamp];
        }

        /// <summary>
        /// Fetch data.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            // Results to return.
            MySqlResultSet results = new MySqlResultSet();
            results.Error(); // Set to error state by default.
            errorCode = DatabaseError.NONE;

            // Select all the roles.
            string query = this.GetQuery();

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
                results = mysqldb.GetData(query) as MySqlResultSet;

                if (results == null || results.IsEmpty) { results.Fail(); }
                else
                {
                    // Create a role for every record in the result set.
                    foreach (MySqlRow row in results.Rows)
                    {
                        string capstoneID = row["capstoneID"].Value;
                        string statusCode = row["statusCode"].Value;
                        string timestamp = row["timeStamp"].Value;
                        this.AddItem(capstoneID, statusCode, timestamp);
                    }
                    results.Pass();
                }
            }

            return results;
        }

        /// <summary>
        /// Attempt to fetch the data.
        /// </summary>
        /// <param name="database">Database to read from.</param>
        /// <param name="results">Result set containing response.</param>
        /// <param name="errorCode">Error code returned.</param>
        /// <returns>Return the result set.</returns>
        public bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode)
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

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlStatusEventHistory(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected string GetQuery()
        {
            return SELECT_ALL;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="capstoneID">Capstone association.</param>
        /// <param name="statusCode">Event status.</param>
        /// <param name="timeStamp">Timestamp of event.</param>
        protected void AddItem(string capstoneID, string statusCode, string timeStamp)
        {
            this.Glossary.Add(MySqlStatusEvent.Create(capstoneID, statusCode, timeStamp));
        }
    }

    /// <summary>
    /// Representation of a single status event.
    /// </summary>
    public class MySqlStatusEvent : MySqlDatabaseObjectModel, IStatusEventModel
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT = "SELECT capstoneID, statusCode, timeStamp FROM capstonedb.statushistoryevent WHERE capstoneID=@capstoneID AND statusCode=@statusCode AND timeStamp=@timeStamp;";

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="capstoneID">Capstone ID.</param>
        /// <param name="statusCode">Status code.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlStatusEvent GetStatusEvent(IReadable database, MySqlID capstoneID, MySqlID statusCode, IDateTimeFormat timestamp)
        {
            MySqlStatusEvent type = new MySqlStatusEvent(capstoneID, statusCode, timestamp);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="capstoneID">Capstone affiliation.</param>
        /// <param name="statusCode">Status of event.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        /// <returns>Return the model.</returns>
        public static MySqlStatusEvent Create(string capstoneID, string statusCode, string timestamp)
        {
            return new MySqlStatusEvent(new MySqlID(capstoneID), new MySqlID(statusCode), timestamp);
        }

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Capstone ID.
        /// </summary>
        public IUIDFormat CapstoneID
        {
            get { return new MySqlID(this.GetValue("capstoneID")); }
            set { this.SetValue("capstoneID", value.SQLValue); }
        }

        /// <summary>
        /// Status.
        /// </summary>
        public IUIDFormat StatusCode
        {
            get { return new MySqlID(this.GetValue("statusCode")); }
            set { this.SetValue("statusCode", value.SQLValue); }
        }

        /// <summary>
        /// Timestamp.
        /// </summary>
        public IDateTimeFormat Timestamp
        {
            get { return new MySqlDateTime(this.GetValue("timeStamp")); }
            set { this.SetValue("timeStamp", value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="capstoneID">Capstone ID.</param>
        /// <param name="statusCode">Status code.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        protected MySqlStatusEvent(IUIDFormat capstoneID, IUIDFormat statusCode, IDateTimeFormat timestamp)
            : base(capstoneID.SQLValue, "Status Event", "A timestampped status event.")
        {
            this.Model.Add(new MySqlEntry("capstoneID", capstoneID.SQLValue));
            this.Model.Add(new MySqlEntry("statusCode", statusCode.SQLValue));
            this.Model.Add(new MySqlEntry("timeStamp", timestamp.SQLValue));
        }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="capstoneID">Capstone ID.</param>
        /// <param name="statusCode">Status code.</param>
        /// <param name="timestamp">Timestamp of event.</param>
        protected MySqlStatusEvent(IUIDFormat capstoneID, IUIDFormat statusCode, string timestamp)
            : base(capstoneID.SQLValue, "Status Event", "A timestampped status event.")
        {
            this.Model.Add(new MySqlEntry("capstoneID", capstoneID.SQLValue));
            this.Model.Add(new MySqlEntry("statusCode", statusCode.SQLValue));
            this.Model.Add(new MySqlEntry("timeStamp", timestamp));
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return SELECT;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>() {
                { "@capstoneID", this.CapstoneID.SQLValue },
                { "@statusCode", this.StatusCode.SQLValue },
                { "@timeStamp", this.Timestamp.SQLValue }
            };
            return parameters;
        }

        /// <summary>
        /// Set the results from the query.
        /// </summary>
        /// <param name="results">Results to set.</param>
        protected override void SetResults(IResultSet results)
        {
            this.CapstoneID = (MySqlID)results[0, "capstoneID"].Value;
            this.StatusCode = (MySqlID)results[0, "statusCode"].Value;
            this.Timestamp = new MySqlDateTime(results[0, "timeStamp"].Value);
        }
    }

    /// <summary>
    /// Collection of email type models.
    /// </summary>
    public class MySqlEmailTypes : CodeGlossary<IEmailTypeModel>, IEmailTypes
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all statuses.
        /// </summary>
        private const string SELECT_ALL = "SELECT code, name, description FROM capstonedb.emailTypes;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlEmailTypes instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlEmailTypes GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlEmailTypes(database);
            }
            return instance;
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlEmailTypes(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected override string GetQuery()
        {
            return SELECT_ALL;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected override void AddItem(string code, string name, string description)
        {
            this.Glossary.Add(MySqlEmailType.Create(code, name, description));
        }

    }

    /// <summary>
    /// Representation of the status code-name-description tuple.
    /// </summary>
    public class MySqlEmailType : MySqlDatabaseObjectModel, IEmailTypeModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT_BY_CODE = "SELECT code, name, description FROM capstonedb.emailTypes WHERE code=@emailTypeCode;";

        /// <summary>
        /// Query used to select role type by role name.
        /// </summary>
        private const string SELECT_BY_NAME = "SELECT code, name, description FROM capstonedb.emailTypes WHERE name=@emailTypeName;";

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="code">Code to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlEmailType GetEmailType(IReadable database, MySqlID code)
        {
            MySqlEmailType type = new MySqlEmailType(code as IUIDFormat);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="name">Name to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlEmailType GetEmailType(IReadable database, string name)
        {
            MySqlEmailType type = new MySqlEmailType(name);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <returns>Return the model.</returns>
        public static MySqlEmailType Create(string code, string name, string description)
        {
            return new MySqlEmailType(new MySqlID(code), name, description);
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        protected MySqlEmailType(IUIDFormat code)
            : base(code, null, null)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        protected MySqlEmailType(string name)
            : base(null, name)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlEmailType(IUIDFormat code, string name, string description)
            : base(code.SQLValue, name, description)
        { }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return (String.IsNullOrWhiteSpace(this.Code.SQLValue)) ? SELECT_BY_NAME : SELECT_BY_CODE;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            if (query == SELECT_BY_CODE) { parameters.AddWithValue("@emailTypeCode", this.Code.SQLValue); }
            else { parameters.AddWithValue("@emailTypeName", this.Name); }
            return parameters;
        }
    }
    
    /// <summary>
    /// Collection of phone type models.
    /// </summary>
    public class MySqlPhoneTypes : CodeGlossary<IPhoneTypeModel>, IPhoneTypes
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all statuses.
        /// </summary>
        private const string SELECT_ALL = "SELECT code, name, description FROM capstonedb.phoneTypes;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlPhoneTypes instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlPhoneTypes GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlPhoneTypes(database);
            }
            return instance;
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlPhoneTypes(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected override string GetQuery()
        {
            return SELECT_ALL;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected override void AddItem(string code, string name, string description)
        {
            this.Glossary.Add(MySqlPhoneType.Create(code, name, description));
        }

    }

    /// <summary>
    /// Representation of the status code-name-description tuple.
    /// </summary>
    public class MySqlPhoneType : MySqlDatabaseObjectModel, IPhoneTypeModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT_BY_CODE = "SELECT code, name, description FROM capstonedb.phoneTypes WHERE code=@phoneTypeCode;";

        /// <summary>
        /// Query used to select role type by role name.
        /// </summary>
        private const string SELECT_BY_NAME = "SELECT code, name, description FROM capstonedb.phoneTypes WHERE name=@phoneTypeName;";

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="code">Code to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlPhoneType GetPhoneType(IReadable database, MySqlID code)
        {
            MySqlPhoneType type = new MySqlPhoneType(code as IUIDFormat);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Returns model type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="name">Name to get type from.</param>
        /// <returns>Returns type associated with the code.</returns>
        public static MySqlPhoneType GetPhoneType(IReadable database, string name)
        {
            MySqlPhoneType type = new MySqlPhoneType(name);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Create a model.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        /// <returns>Return the model.</returns>
        public static MySqlPhoneType Create(string code, string name, string description)
        {
            return new MySqlPhoneType(new MySqlID(code), name, description);
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        protected MySqlPhoneType(IUIDFormat code)
            : base(code, null, null)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="name">Name.</param>
        protected MySqlPhoneType(string name)
            : base(null, name)
        { }

        /// <summary>
        /// Code item constructor.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected MySqlPhoneType(IUIDFormat code, string name, string description)
            : base(code.SQLValue, name, description)
        { }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return (String.IsNullOrWhiteSpace(this.Code.SQLValue)) ? SELECT_BY_NAME : SELECT_BY_CODE;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            if (query == SELECT_BY_CODE) { parameters.AddWithValue("@phoneTypeCode", this.Code.SQLValue); }
            else { parameters.AddWithValue("@phoneTypeName", this.Name); }
            return parameters;
        }
    }

    /// <summary>
    /// Collection of all possible role types in the database.
    /// </summary>
    public class MySqlRoleTypes : CodeGlossary<IRoleTypeModel>, IRoleTypes
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select all roles.
        /// </summary>
        private const string SELECT_ALL_ROLES = "SELECT code, name, description FROM capstonedb.roles;";

        /// <summary>
        /// Instance for singleton pattern.
        /// </summary>
        private static MySqlRoleTypes instance = null;

        //////////////////////
        // Static properties.

        /// <summary>
        /// Singleton pattern.
        /// </summary>
        public static MySqlRoleTypes GetInstance(IReadable database)
        {
            if (instance == null)
            {
                instance = new MySqlRoleTypes(database);
            }
            return instance;
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Private, empty constructor.
        /// </summary>
        private MySqlRoleTypes(IReadable database)
        {
            this.TryFetch(database, out IResultSet results, out DatabaseError error);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the query for this fetch.
        /// </summary>
        /// <returns>Returns query string.</returns>
        protected override string GetQuery()
        {
            return SELECT_ALL_ROLES;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add item to the item glossary.
        /// </summary>
        /// <param name="code">Code.</param>
        /// <param name="name">Name.</param>
        /// <param name="description">Description.</param>
        protected override void AddItem(string code, string name, string description)
        {
            this.Glossary.Add(MySqlRoleType.Create(code, name, description));
        }

    }

    /// <summary>
    /// Represents a role associated with a code.
    /// </summary>
    public class MySqlRoleType : MySqlDatabaseObjectModel, IRoleTypeModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        /// <summary>
        /// Query used to select role type by role code.
        /// </summary>
        private const string SELECT_BY_CODE = "SELECT code, name, description FROM capstonedb.roles WHERE code=@roleCode;";

        /// <summary>
        /// Query used to select role type by role name.
        /// </summary>
        private const string SELECT_BY_NAME = "SELECT code, name, description FROM capstonedb.roles WHERE name=@roleName;";

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Returns role type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="roleCode">Code to get role type from.</param>
        /// <returns>Returns role type associated with the code.</returns>
        public static MySqlRoleType GetRoleType(IReadable database, MySqlID roleCode)
        {
            MySqlRoleType type = new MySqlRoleType(roleCode);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch role type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Returns role type from database containing the information needed.
        /// </summary>
        /// <param name="database">Database for which the query will be executed.</param>
        /// <param name="roleName">Name to get role type from.</param>
        /// <returns>Returns role type associated with the code.</returns>
        public static MySqlRoleType GetRoleType(IReadable database, string roleName)
        {
            MySqlRoleType type = new MySqlRoleType(roleName);
            DatabaseError errorCode = DatabaseError.NONE;
            IResultSet results = new MySqlResultSet();
            if (!type.TryFetch(database, out results, out errorCode))
            {
                throw new DatabaseOperationException(errorCode, $"Failed to fetch role type. - {results.Query}");
            }
            return type;
        }

        /// <summary>
        /// Create a role type.
        /// </summary>
        /// <param name="code">Role code.</param>
        /// <param name="name">Role name.</param>
        /// <param name="description">Role description.</param>
        /// <returns>Return the role type.</returns>
        public static MySqlRoleType Create(string code, string name, string description)
        {
            return new MySqlRoleType(code, name, description);
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Role type from role code.
        /// </summary>
        /// <param name="code">Role code.</param>
        private MySqlRoleType(MySqlID code)
            : base(code as IUIDFormat)
        { }

        /// <summary>
        /// Role type from role name.
        /// </summary>
        /// <param name="roleName">Role name.</param>
        private MySqlRoleType(string roleName)
            : base(roleName)
        { }

        /// <summary>
        /// Creation of a role from all three settings.
        /// </summary>
        /// <param name="code">Role code.</param>
        /// <param name="roleName">Role name.</param>
        /// <param name="description">Role description.</param>
        private MySqlRoleType(MySqlID code, string roleName, string description)
            : base(code as IUIDFormat, roleName, description)
        { }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns query to execute.
        /// </summary>
        /// <returns></returns>
        protected override string GetQuery()
        {
            return (String.IsNullOrWhiteSpace(this.Code.SQLValue)) ? SELECT_BY_NAME : SELECT_BY_CODE;
        }

        /// <summary>
        /// Get the parameters for the query.
        /// </summary>
        /// <param name="query">Query to execute.</param>
        /// <returns>Parameters to bind.</returns>
        protected override MySqlParameters GetParameters(string query)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            if (query == SELECT_BY_CODE) { parameters.AddWithValue("@roleCode", this.Code.SQLValue); }
            else { parameters.AddWithValue("@roleName", this.Name); }
            return parameters;
        }
    }

}
