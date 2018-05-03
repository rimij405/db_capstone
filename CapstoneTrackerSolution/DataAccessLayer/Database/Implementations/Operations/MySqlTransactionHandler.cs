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
        private TransactionType transactionID;

        /// <summary>
        /// Status of the current transaction.
        /// </summary>
        private OperationStatus transactionStatus;

        /// <summary>
        /// Stores the result of the last execution.
        /// </summary>
        private IResultSet cache;
        
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Database reference.
        /// </summary>
        protected MySqlDatabase Database
        {
            get { return this.database; }
            set { this.database = value; }
        }

        /// <summary>
        /// Transaction to utilize.
        /// </summary>
        protected MySqlTransaction Transaction
        {
            get { return this.trans; }
            set { this.trans = value; }
        }

        /// <summary>
        /// Transaction ID.
        /// </summary>
        public TransactionType TransactionID
        {
            get { return this.transactionID; }
            protected set { this.transactionID = value; }
        }
        
        /// <summary>
        /// Current status of the transaction.
        /// </summary>
        public OperationStatus TransactionStatus
        {
            get { return this.transactionStatus; }
            protected set { this.transactionStatus = value; }
        }

        /// <summary>
        /// Store the result set.
        /// </summary>
        public IResultSet Results
        {
            get { return this.cache; }
            protected set { this.cache = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Create a transaction handler that will execute a single transaction from a series of operations.
        /// </summary>
        public MySqlTransactionHandler(TransactionType id, MySqlDatabase db)
        {
            this.TransactionID = id;
            this.TransactionStatus = OperationStatus.NULL; // not run yet.
            this.Database = db;
            this.cache = null;
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
        public IResultSet ExecuteTransaction()
        {
            this.cache = Execute();
            return this.cache;
        }

        /// <summary>
        /// Execute the transaction as defined by the transaction handler.
        /// </summary>
        /// <returns>Returns a result set as per the requirements of the transaction.</returns>
        protected abstract IResultSet Execute();

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
                this.TransactionStatus = OperationStatus.SUCCESS;
                return true;
            }
            this.TransactionStatus = OperationStatus.ERROR;
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
                this.TransactionStatus = OperationStatus.FAILURE;
                return true;
            }
            this.TransactionStatus = OperationStatus.ERROR;
            return false;
        }
        
    }

    /// <summary>
    /// Authenticates a transaction within the database.
    /// </summary>
    public class MySqlAuthorization : MySqlTransactionHandler
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        /// <summary>
        /// SQL query to select the appropriate user.
        /// </summary>
        private const string SELECT_USER = 
            "SELECT users.username, users.password FROM capstonedb.users"
            + " WHERE username=@username AND password=SHA2(@password, 0);";
        
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Name of user to authenticate.
        /// </summary>
        private string username = "NULL";

        /// <summary>
        /// User password to authenticate.
        /// </summary>
        private string password = "NULL";

        /// <summary>
        /// Reference to the user selection operation.
        /// </summary>
        private MySqlOperation selectUser = null;

        /// <summary>
        /// Authentication flag.
        /// </summary>
        private bool authentication = false;

        /// <summary>
        /// Check authentication flag.
        /// </summary>
        private bool authorization = false;

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Username to authenticate.
        /// </summary>
        public string Username
        {
            get { return this.username; }
            set { this.username = value; }
        }
        
        /// <summary>
        /// Username to authenticate.
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Create parameters for this transaction using internal references.
        /// </summary>
        /// <returns>Returns a collection of parameters.</returns>
        private MySqlParameters TransactionParameters
        {
            get
            {
                return new Dictionary<string, string>
                {
                    { "@username", this.username },
                    { "@password", this.password }
                };
            }
        }

        /// <summary>
        /// Return the authentication status.
        /// </summary>
        public bool IsAuthenticated
        {
            get
            {
                if (this.TransactionStatus == OperationStatus.SUCCESS)
                {
                    return this.authentication;
                }
                // Not authentic if there is any error.
                return false;
            }
        }

        /// <summary>
        /// Return the authorized status.
        /// </summary>
        public bool IsAuthorized
        {
            get
            {
                return this.IsAuthenticated && this.authorization;
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        public MySqlAuthorization(string user, string pass, MySqlDatabase db)
            : base(TransactionType.UserAuthentication, db)
        {


            selectUser = new MySqlOperation(db, true, this.Query, this.TransactionParameters);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Execute transaction using the stored operation.
        /// </summary>
        /// <returns>Returns a result set from the transaction.</returns>
        protected override IResultSet Execute()
        {
            IResultSet results = FindUser();
        }

        //////////////////////
        // Helpers.				

        /// <summary>
        /// Returns result set from the find user operation.
        /// </summary>
        /// <returns>Returns user.</returns>
        private IResultSet FindUser()
        {
            MySqlOperation findUserOperation = new MySqlOperation(this.Database, true, MySqlAuthorization.SELECT_USER, this.TransactionParameters);
            return findUserOperation.Execute();
        }
        
        //////////////////////
        // Accessors.

        //////////////////////
        // Mutators.		

    }

}
