/****
 * ITransaction.cs
 * ---
 * Ian Effendi
 * ***/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services.Interfaces;

namespace ISTE.DAL.Database.Interfaces
{

    /// <summary>
    /// Contains reference to the operation type to execute.
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// SELECT statements are queries.
        /// </summary>
        Query,

        /// <summary>
        /// Create, Update, Insert, and Delete statements are non-queries.
        /// </summary>
        NonQuery
    }

    /// <summary>
    /// A command operation that can be run.
    /// </summary>
    public interface IOperation : IOperationStateMachine
    {        
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Returns the type of query this operation is.
        /// </summary>
        OperationType QueryType { get; }

        /// <summary>
        /// Query to execute.
        /// </summary>
        string Query { get; }

        /// <summary>
        /// Parameters to apply to the command. Can be empty.
        /// </summary>
        IDictionary<string, string> Parameters { get; }

        /// <summary>
        /// Check whether or not the transaction has parameters to apply.
        /// </summary>
        bool HasParameters { get; }

        /// <summary>
        /// Check if this is a SELECT operation.
        /// </summary>
        bool IsQuery { get; }

        /// <summary>
        /// Check if this is an INSERT, UPDATE, or DELETE operation.
        /// </summary>
        bool IsNonQuery { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Execute the operation.
        /// </summary>
        /// <returns>Returns the result set.</returns>
        IResultSet Execute();

        //////////////////////
        // Accessors.

        /// <summary>
        /// Returns the SQL query to execute.
        /// </summary>
        /// <returns>Return the query.</returns>
        string GetQuery();

        /// <summary>
        /// Returns the query type.
        /// </summary>
        /// <returns>Returns the query type.</returns>
        OperationType GetQueryType();

        //////////////////////
        // Mutators.		

        /// <summary>
        /// Set the type of execution to run.
        /// </summary>
        /// <param name="type"></param>
        void SetQueryType(OperationType type);

        /// <summary>
        /// Set the query to execute.
        /// </summary>
        /// <param name="sqlQuery">SQL statement to set.</param>
        void SetQuery(string sqlQuery);
                
    }

    /// <summary>
    /// The type of transaction that is occuring.
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// Authentication transaction checks if a username and a password matches in the database.
        /// </summary>
        Authentication
    }

    /// <summary>
    /// Blueprint for making transactions.
    /// </summary>
    public interface ITransaction
    {
        /// <summary>
        /// ID number given to a transaction.
        /// </summary>
        TransactionType TransactionID { get; }

        /// <summary>
        /// Execute the operations in the transaction.
        /// </summary>
        /// <returns>Returns result set from executed operation chain.</returns>
        IResultSet ExecuteTransaction();

        /// <summary>
        /// Commit the transaction to the database.
        /// </summary>
        /// <returns>Return operation success.</returns>
        bool Commit();
        
        /// <summary>
        /// Rollback the transaction from the database.
        /// </summary>
        /// <returns>Return operation success.</returns>
        bool Rollback();
        
        /// <summary>
        /// Check if this is the transaction associated with a particular id.
        /// </summary>
        /// <param name="id">ID of transaction to check.</param>
        /// <returns>Returns true if IDs are the same.</returns>
        bool IsTransaction(TransactionType id);
        
    }
        
}
