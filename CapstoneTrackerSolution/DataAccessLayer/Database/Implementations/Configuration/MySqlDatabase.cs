/*
    MySqlDatabase.cs
    Database connector.
    ***
    04/03/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// using statements.
using MySql.Data.MySqlClient;
using ISTE.DAL.Database.Interfaces;
using Services;
using System.Data;

namespace ISTE.DAL.Database.Implementations
{
    /// <summary>
    /// Connect to a MySqlDatabase.
    /// </summary>
    public class MySqlDatabase : IDatabase, IReadable, IWritable
    {

        /// <summary>
        /// Instance of the MySqlDatabase logger.
        /// </summary>
        private static Logger loggerInstance = null;

        /// <summary>
        /// Logger for exceptions within the MySqlDatabase class.
        /// </summary>
        public static Logger MySqlDatabaseLogger
        {
            get
            {
                if (loggerInstance == null)
                {
                    loggerInstance = new Logger("", "mysqldb", "log");
                }
                return loggerInstance;
            }
        }

        //////////////////////
        // Field(s).
        //////////////////////
        
        /// <summary>
        /// Connection object for connecting to the database.
        /// </summary>
        private MySqlConnection connection = null;
        
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Checks the connection state to see if it's connected to the database.
        /// </summary>
        /// <returns>Returns true if connection is currently open.</returns>
        public bool IsConnected
        {
            get
            {
                if (this.connection == null) { return false; }
                if (this.connection.State == System.Data.ConnectionState.Closed) { return false; }
                if (this.connection.State == System.Data.ConnectionState.Open) { return true; }
                return false;
            }
        }
        
        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Constructor, configured by a configuration object.
        /// </summary>
        /// <param name="_config">Configuration object.</param>
        public MySqlDatabase(MySqlConfiguration _config)
        {
            this.Configure(_config);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Configures the database connection string.
        /// </summary>
        /// <param name="configuration">A configuration object that will create the connection string.</param>
        /// <returns>Returns reference to this <see cref="IDatabase"/> object.</returns>
        public MySqlDatabase Configure(MySqlConfiguration configuration)
        {
            try
            {
                this.connection = configuration.CreateConnection();
                return this;
            }
            catch (DataAccessLayerException e)
            {
                // Pass any exception that was thrown up from the configuration method.
                throw new DatabaseConnectionException("Error occured in the connection configuration process.", e);
            }
        }

        /// <summary>
        /// Attempts to connect to the databse using the connection object.
        /// </summary>
        /// <returns>Returns true if successfully connected.</returns>
        public bool Connect()
        {
            try
            {
                connection.Open();
                return true;
            }
            catch (MySqlException e)
            {
                throw new DatabaseConnectionException("Error occured in connection.", e);
            }
        }

        /// <summary>
        /// Attempt to close the connection.
        /// </summary>
        /// <returns>Returns true if operation was successful.</returns>
        public bool Close()
        {
            try
            {
                // Check if it needs to be closed at all.
                if (this.IsConnected)
                {
                    connection.Close();
                }

                // Database either was already closed or it closed successfully.
                return true;
            }
            catch(MySqlException e)
            {
                throw new DatabaseCloseException("Error occured when attempting to close the database.", e);
            }

            // Error.
            throw new DatabaseCloseException("Unreachable code triggered during database close operation.");
        }

        //////////////////////
        // Helpers.						

        /// <summary>
        /// Create a MySqlCommand that can be used.
        /// </summary>
        /// <param name="mysqlQuery">Query to initialize the command with.</param>
        /// <returns>Returns command object.</returns>
        private MySqlCommand CreateCommand(string mysqlQuery)
        {
            if (String.IsNullOrEmpty(mysqlQuery)) { throw new DataAccessLayerException("Cannot execute an empty query on the database."); }
            return new MySqlCommand(mysqlQuery, this.connection);
        }

        /// <summary>
        /// Create a MySqlCommand and prepare the statement. Applies parameters if any to apply.
        /// </summary>
        /// <param name="mysqlQuery">Query to prepare statement with.</param>
        /// <param name="parameters">Parameters to assign to the prepared statement.</param>
        /// <returns>Returns prepared command object.</returns>
        private MySqlCommand Prepare(string mysqlQuery, IDictionary<string, string> parameters = null)
        {
            MySqlCommand command = this.CreateCommand(mysqlQuery);
            command.Prepare();
            return Assign(command, parameters);
        }

        /// <summary>
        /// Assign parameters to a prepared statement.
        /// </summary>
        /// <param name="preparedStatement">Prepared statement.</param>
        /// <param name="parameters">Parameters to assign to the prepared statement.</param>
        /// <returns>Returns prepared command object.</returns>
        private MySqlCommand Assign(MySqlCommand preparedStatement, IDictionary<string, string> parameters = null)
        {
            if (preparedStatement != null)
            {
                if (parameters != null)
                {
                    foreach (KeyValuePair<string, string> pair in parameters)
                    {
                        if (preparedStatement.Parameters.Contains(pair.Key))
                        {
                            preparedStatement.Parameters[pair.Key].Value = pair.Value;
                        }
                        else
                        {
                            preparedStatement.Parameters.AddWithValue(pair.Key, pair.Value);
                        }
                    }
                }
            }
            else
            {
                throw new DataAccessLayerException("Cannot assign parameters to null or unprepared command statement.");
            }

            return preparedStatement;
        }

        //////////////////////
        // Accessors.
        
        /// <summary>
        /// Returns a transaction object from the server that must be handled.
        /// </summary>
        /// <returns>Returns a MySqlTransaction object.</returns>
        public MySqlTransaction BeginTransaction()
        {
            return this.connection.BeginTransaction();
        }

        /// <summary>
        /// Return collection of data from the database, applying any parameters if necessary. (No parameters will assume it is a statement that needs no preparation).
        /// </summary>
        /// <param name="sqlQuery">Query to execute on the data reader.</param>
        /// <param name="parameters">Parameters to assign to the command.</param>
        /// <returns>Returns result set containing response from database.</returns>
        public IResultSet GetData(string sqlQuery, MySqlParameters parameters)
        {
            return GetData(sqlQuery, parameters.Dictionary);
        }

        /// <summary>
        /// Return collection of data from the database, applying any parameters if necessary. (No parameters will assume it is a statement that needs no preparation).
        /// </summary>
        /// <param name="sqlQuery">Query to execute on the data reader.</param>
        /// <param name="parameters">Parameters to assign to the command.</param>
        /// <returns>Returns result set containing response from database.</returns>
        public IResultSet GetData(string sqlQuery, IDictionary<string, string> parameters = null)
        {
            // Create default response, to return on failure.
            IResultSet set = new MySqlResultSet(sqlQuery, -1); // -1 rowsAffected means that no query has been processed.
            set.Fail(); // Default set to failure.

            // Create a reference to the command statement.
            MySqlCommand statement = null;

            // Create the MySql command statement:
            if (parameters != null && parameters.Count > 0)
            {
                // If the input parameter collection is greater than zero:
                try
                {
                    // Prepare the statement if necessary.
                    statement = Prepare(sqlQuery, parameters);
                }
                catch (DataAccessLayerException dale)
                {
                    dale.Write(MySqlDatabaseLogger);
                    set.Error();
                }
            }
            else
            {
                // If there is no need for a prepared statement:
                try
                {
                    statement = CreateCommand(sqlQuery);
                }
                catch (DataAccessLayerException dale)
                {
                    dale.Write(MySqlDatabaseLogger);
                    set.Error();
                }
            }

            // Attempt to execute the statement.
            try
            {
                // Create reference to the reader.
                MySqlDataReader reader = null;

                // Set the rows affected to zero, since this is a read query.
                set.SetRowsAffected(0);
                
                // Using a MySqlDataReader to get the values.
                using (reader = statement.ExecuteReader())
                {
                    // If the statement executed properly, set can be true.
                    set.Pass();

                    // Get the fields.
                    List<string> fields = new List<string>();
                    using (DataTable dt = reader.GetSchemaTable())
                    {
                        // The column name property is in index 0 of the data table's columns.
                        DataColumn prop = dt.Columns[0];
                        foreach (DataRow field in dt.Rows)
                        {   
                            fields.Add(field[prop].ToString());
                            MySqlDatabaseLogger.Write(prop.ColumnName + " = " + field[prop].ToString());
                        }
                    }

                    // Loop through the resulting set.
                    while (reader.Read())
                    {
                        // Create the row to handle entries.
                        IRow row = new MySqlRow();

                        // Loop through the entries.
                        for (int index = 0; index < reader.FieldCount; index++)
                        {
                            string field = fields[index];
                            string value = reader.GetValue(index).ToString();
                            row.AddEntry(new MySqlEntry(field, value));
                        }

                        // Add the row to the result set.
                        set.AddRow(row);
                    }
                }
            }
            catch (DataAccessLayerException dale)
            {
                dale.Write(MySqlDatabaseLogger);
                set.Error();
            }

            return set;            
        }

        //////////////////////
        // Mutators.				

        /// <summary>
        /// Return metadata from database, after executing input statement, applying any parameters if necessary. (No parameters will assume it is a statement that needs no preparation).
        /// </summary>
        /// <param name="sqlQuery">Query to execute on the data reader.</param>
        /// <param name="parameters">Parameters to assign to the command.</param>
        /// <returns>Returns result set containing response from database.</returns>
        public IResultSet SetData(string sqlQuery, MySqlParameters parameters)
        {
            return this.SetData(sqlQuery, parameters.Dictionary);
        }

        /// <summary>
        /// Return metadata from database, after executing input statement, applying any parameters if necessary. (No parameters will assume it is a statement that needs no preparation).
        /// </summary>
        /// <param name="sqlQuery">Query to execute on the data reader.</param>
        /// <param name="parameters">Parameters to assign to the command.</param>
        /// <returns>Returns result set containing response from database.</returns>
        public IResultSet SetData(string sqlQuery, IDictionary<string, string> parameters = null)
        {
            // Create default response, to return on failure.
            IResultSet set = new MySqlResultSet(sqlQuery, -1); // -1 rowsAffected means that no query has been processed.
            set.Fail(); // Default set to failure.

            // Create a reference to the command statement.
            MySqlCommand statement = null;

            // Create the MySql command statement:
            if (parameters != null && parameters.Count > 0)
            {
                // If the input parameter collection is greater than zero:
                try
                {
                    // Prepare the statement if necessary.
                    statement = Prepare(sqlQuery, parameters);
                }
                catch (DataAccessLayerException dale)
                {
                    dale.Write(MySqlDatabaseLogger);
                    set.Error();
                }
            }
            else
            {
                // If there is no need for a prepared statement:
                try
                {
                    statement = CreateCommand(sqlQuery);
                }
                catch (DataAccessLayerException dale)
                {
                    dale.Write(MySqlDatabaseLogger);
                    set.Error();
                }
            }

            // Attempt to execute the statement.
            try
            {
                // Get the affected rows, if possible. (We don't need to add any rows or entries to this result set).
                int rowsAffected = statement.ExecuteNonQuery();
                set.SetRowsAffected(rowsAffected);

                // Set the state of the result set, depending on outcome.
                if(rowsAffected <= -1) { set.Error(); }
                else if(rowsAffected == 0) { set.Fail(); }
                else if(rowsAffected > 0) { set.Pass(); }                
            }
            catch (DataAccessLayerException dale)
            {
                dale.Write(MySqlDatabaseLogger);
                set.Error();
            }

            // Return the result set.
            return set;
        }
        
       /* public List<List<string>> GetData(string sql)
        {
            List<List<string>> ary = new List<List<string>>();
            if (Connect())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    MySqlDataReader rdr = cmd.ExecuteReader();
                    int row = 0;
                    while (rdr.Read())
                    {
                        ary.Add(new List<string>()); //add a new row
                        for (int i = 0; i < rdr.FieldCount; i++)
                        {
                            ary[row].Add(rdr.GetValue(i).ToString()); // add a new column
                        }
                        row++;
                    }
                }
                catch (MySqlException E)
                {
                    return null;
                }
            }
            Close();
            return ary;

        }

        // Open the database, execute a non-query, and close the database
        public int SetData(string sql)
        {
            int rc = -1; // row changed
            if (Connect())
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(sql, connection);
                    rc = cmd.ExecuteNonQuery();
                }
                catch (MySqlException E)
                {
                    return -1;
                }
            }
            Close();
            return rc;
        }*/
    }
}
