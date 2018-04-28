/**
 * MySqlOperation.cs
 * ---
 * Ian Effendi
 * */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MySql.Data.MySqlClient;
using Services.Interfaces;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Database.Implementations
{
    /// <summary>
    /// Represents an operation that can be completed as part of a transaction.
    /// </summary>
    public class MySqlOperation : IOperation
    {

        //////////////////////
        // Field(s).
        //////////////////////
        
        /// <summary>
        /// Reference to the database that can create the commands.
        /// </summary>
        private MySqlDatabase mysqldb;

        /// <summary>
        /// Determines type of result set that will be returned.
        /// </summary>
        private OperationType operationType;

        /// <summary>
        /// The current state of the operation.
        /// </summary>
        private OperationStatus operationState;

        /// <summary>
        /// SQL to execute.
        /// </summary>
        private string query;

        /// <summary>
        /// Parameters to apply to the command.
        /// </summary>
        private IDictionary<string, string> parameters;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Database for command statement preparation and creation.
        /// </summary>
        private MySqlDatabase Database
        {
            get { return this.mysqldb; }
            set { this.mysqldb = value; }
        }

        /// <summary>
        /// Returns the type of query this operation is.
        /// </summary>
        public OperationType QueryType {
            get
            {
                return this.operationType;    
            }
            private set
            {
                this.operationType = value;
            }
        }

        /// <summary>
        /// Returns the state of the operation.
        /// </summary>
        public OperationStatus State
        {
            get { return this.operationState; }
            private set { this.operationState = value; }
        }

        /// <summary>
        /// Query to execute.
        /// </summary>
        public string Query {
            get
            {
                return this.query;
            }
            private set
            {
                this.query = value;
            }
        }

        /// <summary>
        /// Parameters to apply to the command. Can be empty.
        /// </summary>
        public IDictionary<string, string> Parameters {
            get
            {
                if(this.parameters == null)
                {
                    this.parameters = new Dictionary<string, string>();
                }
                return this.parameters;
            }
            private set
            {
                this.parameters = value;
            }
        }

        /// <summary>
        /// Check whether or not the transaction has parameters to apply.
        /// </summary>
        public bool HasParameters {
            get
            {
                return (this.Parameters.Count > 0);
            }
        }

        /// <summary>
        /// Check if this is a SELECT operation.
        /// </summary>
        public bool IsQuery { get { return this.QueryType == OperationType.Query; } }

        /// <summary>
        /// Check if this is an INSERT, UPDATE, or DELETE operation.
        /// </summary>
        public bool IsNonQuery { get { return this.QueryType == OperationType.NonQuery; } }

        /// <summary>
        /// Check if this has entered an error state.
        /// </summary>
        public bool IsError { get { return this.State == OperationStatus.ERROR; } }
        
        /// <summary>
        /// Check if this has entered a failure state.
        /// </summary>
        public bool IsFailure { get { return this.State == OperationStatus.FAILURE; } }

        /// <summary>
        /// Check if this has entered a success state.
        /// </summary>
        public bool IsSuccess { get { return this.State == OperationStatus.SUCCESS; } }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create an operation that can be executed at a later point in time.
        /// </summary>
        /// <param name="db">Database to set.</param>
        /// <param name="isQuery">If set to true, will return a set of data. If false, will expect rows to be affected.</param>
        /// <param name="sql">SQL to execute.</param>
        /// <param name="parameters">Parameters to bind to the statement.</param>
        public MySqlOperation(MySqlDatabase db, bool isQuery, string sql, IDictionary<string, string> parameters)
        {
            // Set reference to the database.
            this.Database = db;
            this.QueryType = (isQuery) ? OperationType.Query : OperationType.NonQuery;
            this.Query = sql;
            this.Parameters = parameters;
            this.State = OperationStatus.NULL;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Execute statement based on query type.
        /// </summary>
        /// <returns>Returns result set.</returns>
        public IResultSet Execute()
        {
            IResultSet results = (IsQuery) ? this.ExecuteQuery() : this.ExecuteNonQuery();
            this.SetState(results.State);
            return results;
        }

        /// <summary>
        /// Set to error state.
        /// </summary>
        public void Error()
        {
            this.State = OperationStatus.ERROR;
        }

        /// <summary>
        /// Set to fail state.
        /// </summary>
        public void Fail()
        {
            this.State = OperationStatus.FAILURE;
        }

        /// <summary>
        /// Set to success state.
        /// </summary>
        public void Pass()
        {
            this.State = OperationStatus.SUCCESS;
        }

        /// <summary>
        /// Check the operation status.
        /// </summary>
        /// <param name="other">State to check.</param>
        /// <returns>Returns true if match.</returns>
        public bool IsState(OperationStatus other)
        {
            return (this.State == other);
        }

        /// <summary>
        /// Check the operation status, by integer code.
        /// </summary>
        /// <param name="code">Code to check.</param>
        /// <returns>Returns true if match.</returns>
        public bool IsState(int code)
        {
            if (code >= -1 && code < 2)
            {
                return this.State == (OperationStatus)code;
            }
            return false;
        }

        //////////////////////
        // Helpers.

        /// <summary>
        /// Execute the read command.
        /// </summary>
        /// <returns>Returns result set.</returns>
        private IResultSet ExecuteQuery()
        {
            return this.Database.GetData(this.Query, this.Parameters);
        }

        /// <summary>
        /// Execute the create, update, or delete command.
        /// </summary>
        /// <returns>Returns result set.</returns>
        private IResultSet ExecuteNonQuery()
        {
            return this.Database.SetData(this.Query, this.Parameters);
        }

        //////////////////////
        // Accessors.

        /// <summary>
        /// Return the query type.
        /// </summary>
        /// <returns>Returns type of operation.</returns>
        public OperationType GetQueryType()
        {
            return this.QueryType;
        }

        /// <summary>
        /// Return the state.
        /// </summary>
        /// <returns>Returns status of the operation.</returns>
        public OperationStatus GetState()
        {
            return this.State;
        }

        /// <summary>
        /// Return the SQL query.
        /// </summary>
        /// <returns>Returns SQL.</returns>
        public string GetQuery()
        {
            return this.Query;
        }

        //////////////////////
        // Mutators.				
        
        /// <summary>
        /// Set query type.
        /// </summary>
        /// <param name="other">Value to set.</param>
        public void SetQueryType(OperationType other)
        {
            this.QueryType = other;
        }

        /// <summary>
        /// Set state.
        /// </summary>
        /// <param name="other">Value to set.</param>
        public void SetState(OperationStatus other)
        {
            this.State = other;
        }

        /// <summary>
        /// Set state, by integer code.
        /// </summary>
        /// <param name="code">Value to set.</param>
        public bool SetState(int code)
        {
            if (code >= -1 && code < 2)
            {
                this.State = (OperationStatus)code;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Set the SQL query.
        /// </summary>
        /// <param name="sql">Query to set.</param>
        public void SetQuery(string sql)
        {
            this.Query = sql;
        }
    }
}
