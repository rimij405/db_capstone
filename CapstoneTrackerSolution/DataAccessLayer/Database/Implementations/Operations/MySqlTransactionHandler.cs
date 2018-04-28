/**
 * MySqlTransactionHandler.cs
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
    /// Handles a transaction.
    /// </summary>
    public abstract class MySqlTransactionHandler : ITransaction
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Database transaction will take place in.
        /// </summary>
        private MySqlDatabase database;

        /// <summary>
        /// Transaction associated with this handler.
        /// </summary>
        private MySqlTransaction trans;

        /// <summary>
        /// Transaction ID defining the type of transaction that is being executed.
        /// </summary>
        private TransactionType id;
        
        /// <summary>
        /// Operations to execute.
        /// </summary>
        private List<IOperation> operations;
        
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Database reference.
        /// </summary>
        private MySqlDatabase Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// Transaction to utilize.
        /// </summary>
        private MySqlTransaction Transaction
        {
            get { return this.trans; }
            set { this.trans = value; }
        }

        /// <summary>
        /// Transaction ID.
        /// </summary>
        public TransactionType TransactionID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Reference to collection of operations.
        /// </summary>
        public List<IOperation> Operations
        {
            get {
                if(this.operations == null)
                {
                    this.operations = new List<IOperation>();
                }
                return this.operations;
            }
        }
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a transaction handler that will execute a single transaction from a series of operations.
        /// </summary>
        public MySqlTransactionHandler(int id, MySqlDatabase db)
        {
            this.Database = db;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.

        /// <summary>
        /// Execute the transaction as defined by the transaction handler.
        /// </summary>
        /// <returns>Returns a result set as per the requirements of the transaction.</returns>
        public abstract IResultSet ExecuteTransaction();

        /// <summary>
        /// Check if this transaction matches?
        /// </summary>
        /// <param name="id">ID to check.</param>
        /// <returns>Returns true if transaction matches.</returns>
        public bool IsTransaction(TransactionType id)
        {
            return (this.TransactionID == id);
        }

        /// <summary>
        /// Commit the transaction.
        /// </summary>
        /// <returns>Returns true if successfully committed.</returns>
        public bool Commit()
        {
            if(this.Transaction != null)
            {
                this.Transaction.Commit();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        /// <returns>Returns true if successfully rolled back.</returns>
        public bool Rollback()
        {
            if (this.Transaction != null)
            {
                this.Transaction.Rollback();
                return true;
            }
            return false;
        }
        
    }

    /// <summary>
    /// Authenticates a transaction within the database.
    /// </summary>
    public class MySqlAuthorization : MySqlTransactionHandler
    {


        public override IResultSet ExecuteTransaction()
        {
            throw new NotImplementedException();
        }
    }

}
