/*
    Program.cs
    Entry point for the testing the DAL library.    
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
using ISTE.DAL.Database;
using ISTE.DAL.Database.Interfaces;

// Project using statements.
using Services;
using Services.Interfaces;

namespace TestDataAccessLayer
{
    /// <summary>
    /// Entry point for the <see cref="TestDataAccessLayer"/> program.
    /// </summary>
    public class Program
    {

        /// <summary>
        /// Printer handles special console functionality.
        /// </summary>
        private static Printer console;

        /// <summary>
        /// Result printer.
        /// </summary>
        private static MySqlResultPrinter printer = new MySqlResultPrinter(150, 1);

        /// <summary>
        /// Configuration implementation.
        /// </summary>
        private static IConfiguration configuration;

        /// <summary>
        /// Database implementation.
        /// </summary>
        private static IDatabase database;
        
        /// <summary>
        /// Entry point for the <see cref="TestDataAccessLayer"/> <see cref="Program"/>.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Create the printer service.
            console = new Printer("Test Data Access Layer Library", true);
            console.Debug("Printer initialized.");

            // Set up values for testing.
            int currentTest = 0;
            int successfulTests = 0;
            int errors = 0;
            int totalTests = 0;
            bool verbose = true;

            // Create the TestMethods invocation list delegate.
            TestResults r = TestResults.Create("Test Method");
            Func<TestResults> TestMethods = null;

            // << ADD TESTING METHODS HERE | THEY EXECUTE IN THE ORDER THEY ARE ADDED >>
            TestMethods += Test_MySqlConfiguration;

            TestMethods += Test_MySqlDatabase_Connect;
            TestMethods += Test_MySqlDatabase_Select;
            TestMethods += Test_MySqlDatabase_PreparedSelect;

            TestMethods += Test_MySqlEntry_SingleField;
            TestMethods += Test_MySqlEntry_FieldValue;
            TestMethods += Test_MySqlEntry_KeyValuePair;
            TestMethods += Test_MySqlEntry_Clone;
            TestMethods += Test_MySqlEntry_Equality;

            TestMethods += Test_MySqlRow_Empty;
            TestMethods += Test_MySqlRow_Fields;
            TestMethods += Test_MySqlRow_Mutators;

            TestMethods += Test_MySqlResultSet_Empty;
            TestMethods += Test_MySqlResultSet_Mutators;

            TestMethods += Test_Logger_Empty;

            Logger testLogger = new Logger("", "test", "log");
            testLogger.Clear();
                        
            try
            {
                r.Log("", "------------------------", $"Trace of {r.Title}");

                // If null, throw a new error.
                if (TestMethods == null)
                {
                    r.Fail("No test methods to invoke.");
                    throw new TestException(r);
                }

                // If not null, we can run the tests.
                Delegate[] methods = TestMethods.GetInvocationList();
                totalTests = methods.Length;

                foreach (Delegate test in methods)
                {
                    // Print a newline.
                    console.Write("");
                    testLogger.Write("\n\n");
                    currentTest++;

                    r.Log("------------------------", $"-- Invoking test {currentTest} out of {totalTests} test(s).");

                    // Get the result from test.
                    Func<TestResults> t = (Func<TestResults>)test;
                    TestResults output = t.Invoke();
                    
                    if (output == null)
                    {
                        errors++;
                        r.Log(TestResults.CreateError($"Test #{currentTest}", "No result object returned.")[TestResults.ErrorState]);
                        testLogger.Write($"\nError thrown during test {currentTest} out of {totalTests} test(s).\n");
                        console.Pause($"Error thrown during test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                        continue;
                    }

                    // If it isn't null, we can cast it.
                    TestResults result = ((TestResults)output);

                    r.Log($"Test #{currentTest}: \"{result.Title}\"");
                    if (verbose) {
                        console.Write(result.StackTrace);
                        testLogger.Write("\n" + result.StackTrace);
                    }

                    // If it failed, log and continue to next test.
                    if (result.IsFailure)
                    {
                        r.Log(result[OperationStatus.FAILURE], result.StackTrace);
                        testLogger.Write($"\nFailed test {currentTest} out of {totalTests} test(s).\n");
                        console.Pause($"Failed test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                        continue;
                    }

                    // Increment counter.
                    if (result.IsSuccessful)
                    {
                        r.Log(result[OperationStatus.SUCCESS]);
                        successfulTests++;
                        testLogger.Write($"\nCompleted test {currentTest} out of {totalTests} test(s).\n");
                        console.Pause($"Completed test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                    }
                }
                
                r.Pass($"Successfully executed {totalTests} test(s).");
            }
            catch (TestException te)
            {
                r.Fail($"Exception \"{te.GetExceptionName()}\" has been thrown.");
                r.Log(te.Results.StackTrace, $"-- {te.Results.GetCurrentMessage()}");
                testLogger.Write("\n" + te.Results.StackTrace);   
            }
            finally
            {
                // Print log.
                r.Log("------------------------", $"{r.Title} Results");
                int failedTests = totalTests - successfulTests;
                r.Log(
                    $"\t{successfulTests} test(s) completed successfully.",
                    $"\t{failedTests} test(s) completed with failure.",
                    $"\t{errors} test(s) interrupted by errors."
                    );

                // Log the current state message as well.
                r.Log($"-- {r.GetCurrentMessage()}", "------------------------");

                // Print results from all tests.
                if (verbose) {
                    console.Write(r.StackTrace);
                    testLogger.Write("\n" + r.StackTrace);
                }

                // Wait for user input.
                console.Pause("Press any key to exit the program...");
            }
        }

        #region Test Method Template.

        /*
         * Template method for creating new tests.
         * 
        private static TestResults Test__()
        {
            // Set values and dependencies here.
            string subject = "<<Functionality being tested>>";
            
            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");                
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }
            
            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Operation description.");
                // << Replace line with the operation to be completed >>
                               
                // << IF ON FAIL, USE FAIL() METHOD >>
                // results.Fail("Overwrite fail message if necessary.");
                
                // << IF ON PASS, USE PASS() METHOD >>
                // results.Pass("Overwrite success message if necessary.");

                // << IF A USE-CASE ERROR OCCURS, USE ERROR() METHOD >>
                // results.Error("Overwrite error message, without exception.");
                // -- OR -- USE THROW THROW() METHOD TO TRIGGER AN EXCEPTION:
                // throw results.Throw("Overwrite error message, without exception.");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{exceptionMessage} {e.Message}", e);
            }

            // Return the test results.
            return results;
        }
        */

        #endregion

        #region Database Tests

        /// <summary>
        /// Test database configuration object constructor.
        /// </summary>
        /// <returns>Returns result from test.</returns>
        private static TestResults Test_MySqlConfiguration() {

            // Create the results object for this test.
            TestResults results = TestResults.Create("Testing MySqlConfiguration");

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");

                // Create the configuration object.
                results.Log("Create the configuration object: ");
                configuration = new MySqlConfiguration();

                // If passed, test has passed successfully.
                results.Log($"{configuration}").Pass("Created the configuration object successfully.");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw("Failed to create the configuration. " + e.Message, e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Test database and its connection method.
        /// </summary>
        /// <returns>Returns result from test.</returns>
        private static TestResults Test_MySqlDatabase_Connect() {

            // Create the results object for this test.
            TestResults results = TestResults.Create("Testing MySqlDatabase");

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");

                // Create the database object.
                results.Log("Creating the MySqlDatabase object.");
                database = new MySqlDatabase(configuration as MySqlConfiguration);

                // Connecting to the database object.
                results.Log("Connecting to the database object.");
                database.Connect();

                // If passed, test has passed successfully.
                results.Log($"{database}").Pass("Created and connected to the database object successfully.");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw("Failed to create the database. " + e.Message, e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Test the select statement in the mysql database.
        /// </summary>
        /// <returns>Returns result from test.</returns>
        private static TestResults Test_MySqlDatabase_Select()
        {

            // Set values and dependencies here.
            string subject = "MySqlDatabase GetData() - Email Types";
            string query = "SELECT * FROM emailtypes;";
            
            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
                results.Log($"Executing \"{query}\"");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }
            
            try
            {
                if (configuration == null)
                {
                    results.Log($"Creating the configuration object.");
                    configuration = new MySqlConfiguration();
                    results.Log($"{configuration.GetConnectionString()}");
                }    

                if(database == null)
                {
                    results.Log($"Creating the database object.");
                    database = new MySqlDatabase(configuration as MySqlConfiguration);
                    results.Log($"{database.ToString()}");
                }

                if (!database.IsConnected)
                {
                    results.Log($"Connecting to the database.");
                    database.Connect();
                }

                // Call the GetData method.
                MySqlDatabase mysqldb = database as MySqlDatabase;
                MySqlResultSet set = mysqldb.GetData(query, null) as MySqlResultSet;
                results.Log($"{printer.FormatResultSet(set)}");
                
                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (set == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is the set empty? {assertions[AssertKey.IsEmpty]}");

                bool hasAffectedRows = set.RowsAffected > 0;
                results.Log($"-- Any affected rows? {hasAffectedRows}, {set.RowsAffected} rows");

                bool hasError = set.IsError;
                bool hasFailure = set.IsFailure;
                bool hasSuccess = set.IsSuccess;
                results.Log($"-- What is the operation status? Error [{hasError}], Failure [{hasFailure}], Success [{hasSuccess}]");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.IsEmpty] &&
                    !hasAffectedRows &&
                    !hasError && !hasFailure && hasSuccess;
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Test prepared select statement in the mysql database.
        /// </summary>
        /// <returns>Returns result from test.</returns>
        private static TestResults Test_MySqlDatabase_PreparedSelect()
        {

            // Set values and dependencies here.
            string subject = "MySqlDatabase GetData() - Student Info via Prepared Statement.";
            string query = "SELECT * FROM capstonedb.users"
                            + " INNER JOIN capstonedb.students USING(userID)"
                            + " WHERE userID = @userID;";           
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@userID", "3"}
            };

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
                results.Log(
                    $"Executing \"{query}\"",
                    $"with values [\"@emailtypes\" : \"{parameters["@userID"]}\"]."
                    );
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            try
            {
                if (configuration == null)
                {
                    results.Log($"Creating the configuration object.");
                    configuration = new MySqlConfiguration();
                    results.Log($"{configuration.GetConnectionString()}");
                }

                if (database == null)
                {
                    results.Log($"Creating the database object.");
                    database = new MySqlDatabase(configuration as MySqlConfiguration);
                    results.Log($"{database.ToString()}");
                }

                if (!database.IsConnected)
                {
                    results.Log($"Connecting to the database.");
                    database.Connect();
                }

                // Call the GetData method.
                MySqlDatabase mysqldb = database as MySqlDatabase;
                MySqlResultSet set = mysqldb.GetData(query, parameters) as MySqlResultSet;
                results.Log($"{printer.FormatResultSet(set)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (set == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is the set empty? {assertions[AssertKey.IsEmpty]}");

                bool hasAffectedRows = set.RowsAffected > 0;
                results.Log($"-- Any affected rows? {hasAffectedRows}, {set.RowsAffected} rows");

                bool hasError = set.IsError;
                bool hasFailure = set.IsFailure;
                bool hasSuccess = set.IsSuccess;
                results.Log($"-- What is the operation status? Error [{hasError}], Failure [{hasFailure}], Success [{hasSuccess}]");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.IsEmpty] &&
                    !hasAffectedRows &&
                    !hasError && !hasFailure && hasSuccess;
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Test create operation in the mysql database.
        /// </summary>
        /// <returns>Returns result from test.</returns>
        private static TestResults Test_MySqlDatabase_Authenticate()
        {

            // Set values and dependencies here.
            string subject = "MySqlDatabase Attempting authentication with default password and .";

            // Set up the values to authenticiate.
            string usernameCheck = "jxd1236";
            string passwordCheck = "PASSWORD";

            // Set up the hash query (for the password).
            string hashQuery = "SELECT SHA2(@password) as 'Hashed Value';";
            Dictionary<string, string> hashParameters = new Dictionary<string, string> {
                {"@password", passwordCheck}
            };

            // 
            string query = "SELECT username, password FROM capstonedb.users WHERE userID = @userID;";
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                {"@userID", "3"}
            };

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
                results.Log(
                    $"Executing \"{query}\"",
                    $"with values [\"@emailtypes\" : \"{parameters["@userID"]}\"]."
                    );
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            try
            {
                if (configuration == null)
                {
                    results.Log($"Creating the configuration object.");
                    configuration = new MySqlConfiguration();
                    results.Log($"{configuration.GetConnectionString()}");
                }

                if (database == null)
                {
                    results.Log($"Creating the database object.");
                    database = new MySqlDatabase(configuration as MySqlConfiguration);
                    results.Log($"{database.ToString()}");
                }

                if (!database.IsConnected)
                {
                    results.Log($"Connecting to the database.");
                    database.Connect();
                }

                // Call the GetData method.
                MySqlDatabase mysqldb = database as MySqlDatabase;
                MySqlResultSet set = mysqldb.GetData(query, parameters) as MySqlResultSet;
                results.Log($"{printer.FormatResultSet(set)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (set == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is the set empty? {assertions[AssertKey.IsEmpty]}");

                bool hasAffectedRows = set.RowsAffected > 0;
                results.Log($"-- Any affected rows? {hasAffectedRows}, {set.RowsAffected} rows");

                bool hasError = set.IsError;
                bool hasFailure = set.IsFailure;
                bool hasSuccess = set.IsSuccess;
                results.Log($"-- What is the operation status? Error [{hasError}], Failure [{hasFailure}], Success [{hasSuccess}]");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.IsEmpty] &&
                    !hasAffectedRows &&
                    !hasError && !hasFailure && hasSuccess;
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        #endregion

        /// <summary>
        /// Assertion keys are linked to assertion maps, allowing for checking of particular values with a reference.
        /// </summary>
        private enum AssertKey
        {
            /// <summary>
            /// Use to store the "total" pass/fail evaluation of an operation.
            /// </summary>
            Total,

            /// <summary>
            /// Assertion reference for when something is a null reference.
            /// </summary>
            IsNullReference,

            /// <summary>
            /// Assertion reference for when something is null.
            /// </summary>
            HasNullValue,

            /// <summary>
            /// Store the result of the HasField(string) method.
            /// </summary>
            HasField,

            /// <summary>
            /// Store the result of the HasValue(string) method.
            /// </summary>
            HasValue,

            /// <summary>
            /// Determine if the row still contains an entry object.
            /// </summary>
            HasEntry,
            
            /// <summary>
            /// Assertion reference for when something is empty.
            /// </summary>
            IsEmpty,

            /// <summary>
            /// Equality check.
            /// </summary>
            AreSame,

            /// <summary>
            /// Inequality check.
            /// </summary>
            AreDifferent
        }

        #region MySqlEntry

        /// <summary>
        /// Tests the single field constructor of MySqlEntry.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlEntry_SingleField()
        {
            // Set values and dependencies here.
            string subject = "MySqlEntry (Single Field)";
            MySqlEntry entry = null;
            
            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");                
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }
            
            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the single field constructor.");
                entry = new MySqlEntry("Single Field");
                results.Log($"{printer.FormatEntry(entry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (entry == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = entry.IsNull;
                results.Log($"-- Does this entry have the NULL_VALUE? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.HasField] = entry.HasField("Single Field");
                results.Log($"-- Does the entry field match 'Single Field'? {assertions[AssertKey.HasField]}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.HasField];
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass("Single field constructor operation passed.");
                }                                              
                else
                {
                    results.Fail("Single field constructor operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of MySqlEntry. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests the field and value constructor of MySqlEntry.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlEntry_FieldValue()
        {
            // Set values and dependencies here.
            string subject = "MySqlEntry (Field, Value)";
            MySqlEntry entry = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the field/value constructor.");
                entry = new MySqlEntry("Field", "Value");
                results.Log($"{printer.FormatEntry(entry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (entry == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = entry.IsNull;
                results.Log($"-- Does this entry have the NULL_VALUE? {assertions[AssertKey.HasNullValue]}");
                results.Log($"-- entry.GetValue()? {entry.GetValue()}");

                assertions[AssertKey.HasField] = entry.HasField("Field");
                results.Log($"-- Does the entry field match 'Field'? {assertions[AssertKey.HasField]}");

                assertions[AssertKey.HasValue] = entry.HasValue("Value");
                results.Log($"-- Does the entry value match 'Value'? {assertions[AssertKey.HasValue]}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.HasField];
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass("Field, value constructor operation passed.");
                }
                else
                {
                    results.Fail("Field, value constructor operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of MySqlEntry. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests the key/value pair constructor of MySqlEntry.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlEntry_KeyValuePair()
        {
            // Set values and dependencies here.
            string subject = "MySqlEntry (KeyValuePair<string, string>)";
            MySqlEntry entry = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the specified constructor.");
                entry = new MySqlEntry(new KeyValuePair<string, string>("Field", "Value"));
                results.Log($"{printer.FormatEntry(entry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (entry == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = entry.IsNull;
                results.Log($"-- Does this entry have the NULL_VALUE? {assertions[AssertKey.HasNullValue]}");
                results.Log($"-- entry.GetValue()? {entry.GetValue()}");

                assertions[AssertKey.HasField] = entry.HasField("Field");
                results.Log($"-- Does the entry field match 'Field'? {assertions[AssertKey.HasField]}");

                assertions[AssertKey.HasValue] = entry.HasValue("Value");
                results.Log($"-- Does the entry value match 'Value'? {assertions[AssertKey.HasValue]}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.HasField];
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass("Constructor operation passed.");
                }
                else
                {
                    results.Fail("Constructor operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of MySqlEntry. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests the clone constructor of MySqlEntry.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlEntry_Clone()
        {
            // Set values and dependencies here.
            string subject = "MySqlEntry (Clone())";
            MySqlEntry entry = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the specified constructor.");
                entry = new MySqlEntry(new KeyValuePair<string, string>("Field", "Value")).Clone() as MySqlEntry;
                results.Log($"{printer.FormatEntry(entry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (entry == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = entry.IsNull;
                results.Log($"-- Does this entry have the NULL_VALUE? {assertions[AssertKey.HasNullValue]}");
                results.Log($"-- entry.GetValue()? {entry.GetValue()}");

                assertions[AssertKey.HasField] = entry.HasField("Field");
                results.Log($"-- Does the entry field match 'Field'? {assertions[AssertKey.HasField]}");

                assertions[AssertKey.HasValue] = entry.HasValue("Value");
                results.Log($"-- Does the entry value match 'Value'? {assertions[AssertKey.HasValue]}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.HasField];
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass("Clone operation passed.");
                }
                else
                {
                    results.Fail("Clone operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during cloning of MySqlEntry. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests the clone constructor of MySqlEntry.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlEntry_Equality()
        {
            // Set values and dependencies here.
            string subject = "MySqlEntry (Equality)";
            MySqlEntry entry = null;
            MySqlEntry entryA = null;
            MySqlEntry entryB = null;
            MySqlEntry entryC = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the specified constructor.");
                entry = new MySqlEntry(new KeyValuePair<string, string>("Field", "Value")).Clone() as MySqlEntry;
                entryA = entry.Clone() as MySqlEntry;
                entryB = new MySqlEntry("Field", "Different Value");
                entryC = new MySqlEntry("Different", "Value");

                results.Log("Base:", $"{printer.FormatEntry(entry)}", 
                    "A:", $"{printer.FormatEntry(entryA)}", 
                    "B:", $"{printer.FormatEntry(entryB)}", 
                    "C:", $"{printer.FormatEntry(entryC)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");
                
                assertions[AssertKey.AreSame] = (entry.Equals(entryA));
                results.Log($"-- Does the Base entry match A (true)? {assertions[AssertKey.AreSame]}");

                assertions[AssertKey.AreDifferent] = !(entry.Equals(entryB));
                results.Log($"-- Does the Base entry match B (false)? {!assertions[AssertKey.AreDifferent]}");

                assertions[AssertKey.AreDifferent] = assertions[AssertKey.AreDifferent] && !(entryA.Equals(entryC));
                results.Log($"-- Does the A entry match C (false)? {entryA.Equals(entryC)}");
                
                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    assertions[AssertKey.AreSame] &&
                    assertions[AssertKey.AreDifferent];
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass("Clone operation passed.");
                }
                else
                {
                    results.Fail("Clone operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during cloning of MySqlEntry. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        #endregion

        #region MySqlRow

        /// <summary>
        /// Tests empty constructor for MySqlRow.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlRow_Empty()
        {
            // Set values and dependencies here.
            string subject = "MySqlRow ()";
            MySqlRow row = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the single field constructor.");
                row = new MySqlRow();
                results.Log($"{printer.FormatRow(row)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (row == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");
                
                assertions[AssertKey.HasNullValue] = row.IsNull;
                results.Log($"-- Are all entries within this row null? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.IsEmpty] = row.IsEmpty;
                results.Log($"-- Is this row empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- row.Count? {row.Count}");
                results.Log($"-- row.FieldCount? {row.FieldCount}");
                results.Log($"-- row.EntryCount? {row.EntryCount}");

                assertions[AssertKey.HasField] = row.HasField("Field");
                results.Log($"-- Does any entry inside this row match 'Field'? {assertions[AssertKey.HasField]}");

                int fieldIndex = row.GetIndex("Field");
                results.Log($"-- Index of the \"Field\" entry? {fieldIndex}"); // Should be -1!

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.IsEmpty] &&
                    !assertions[AssertKey.HasField] &&
                    fieldIndex == -1;
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests field collection constructor for MySqlRow.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlRow_Fields()
        {
            // Set values and dependencies here.
            string subject = "MySqlRow (params string[])";
            MySqlRow row = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlEntry object using the single field constructor.");
                row = new MySqlRow(
                    new MySqlEntry("Field", "Column 1 Value"),
                    new MySqlEntry("Field 2", "Column 2 Value"),
                    new MySqlEntry("Field 3", "Column 3 Value")
                    );
                results.Log($"{printer.FormatHeader(row)}",
                    $"{printer.FormatRow(row)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (row == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = row.IsNull;
                results.Log($"-- Are all entries within this row null? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.IsEmpty] = row.IsEmpty;
                results.Log($"-- Is this row empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- row.Count? {row.Count}");
                results.Log($"-- row.FieldCount? {row.FieldCount}");
                results.Log($"-- row.EntryCount? {row.EntryCount}");

                assertions[AssertKey.HasField] = row.HasField("Field");
                results.Log($"-- Does any field inside this row match 'Field'? {assertions[AssertKey.HasField]}");
                
                int fieldIndex = row.GetIndex("Field");
                results.Log($"-- Index of the \"Field\" entry? {fieldIndex}"); // Should not be -1.

                MySqlEntry secondIndexEntry = row.GetEntry(1) as MySqlEntry; // zero-based so second index is 1.
                results.Log($"-- Entry at second index? {secondIndexEntry}");

                MySqlEntry field3Entry = row.GetEntry("Field 3") as MySqlEntry;
                results.Log($"-- Entry with field \"Field 3\"? {field3Entry}");
                                
                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] && // not null reference
                    !assertions[AssertKey.HasNullValue] && // not null entries
                    !assertions[AssertKey.IsEmpty] && // not empty
                    assertions[AssertKey.HasField] && // HasField("Field")
                    fieldIndex >= 0 && // Retrieve field index is not == -1
                    secondIndexEntry != null && // Second index entry is not null. (Index 1)
                    field3Entry != null; // "Field 3" entry is not null.
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Tests accessor/mutator methods in MySqlRow.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlRow_Mutators()
        {
            // Set values and dependencies here.
            string subject = "MySqlRow (Mutators)";
            MySqlRow row = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << CREATE >>
            try
            {
                // Perform the correct operations.
                results.Log("", "-- Operation #1", "Creating the MySqlEntry object using the single field constructor.");
                row = new MySqlRow(
                    new MySqlEntry("Field", "Column 1 Value"),
                    new MySqlEntry("Field 2", "Column 2 Value"),
                    new MySqlEntry("Field 3", "Column 3 Value")
                    );
                results.Log($"{printer.FormatHeader(row)}",
                    $"{printer.FormatRow(row)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (row == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = row.IsNull;
                results.Log($"-- Are all entries within this row null? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.IsEmpty] = row.IsEmpty;
                results.Log($"-- Is this row empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- row.Count? {row.Count}");
                results.Log($"-- row.FieldCount? {row.FieldCount}");
                results.Log($"-- row.EntryCount? {row.EntryCount}");

                assertions[AssertKey.HasField] = row.HasField("Field");
                results.Log($"-- Does any field inside this row match 'Field'? {assertions[AssertKey.HasField]}");

                int fieldIndex = row.GetIndex("Field");
                results.Log($"-- Index of the \"Field\" entry? {fieldIndex}"); // Should not be -1.

                MySqlEntry secondIndexEntry = row.GetEntry(1) as MySqlEntry; // zero-based so second index is 1.
                results.Log($"-- Entry at second index? {secondIndexEntry}");

                MySqlEntry field3Entry = row.GetEntry("Field 3") as MySqlEntry;
                results.Log($"-- Entry with field \"Field 3\"? {field3Entry}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] && // not null reference
                    !assertions[AssertKey.HasNullValue] && // not null entries
                    !assertions[AssertKey.IsEmpty] && // not empty
                    assertions[AssertKey.HasField] && // HasField("Field")
                    fieldIndex >= 0 && // Retrieve field index is not == -1
                    secondIndexEntry != null && // Second index entry is not null. (Index 1)
                    field3Entry != null; // "Field 3" entry is not null.
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (!assertions[AssertKey.Total])
                {
                    results.Fail($"{subject} operation failed.");
                    return results;
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }
            
            // << REMOVE ENTRY >>
            try
            {
                // Perform the correct operations.
                results.Log("", "-- Operation #2", "Removing entry at the second index (index 1).");
                MySqlEntry removedEntry = row.RemoveEntry(1) as MySqlEntry;
                results.Log($"{printer.FormatHeader(row)}",
                    $"{printer.FormatRow(row)}",                     
                    "Removed Entry:", $"{printer.FormatEntry(removedEntry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.HasEntry] = (row.Contains(removedEntry));
                results.Log($"-- Does the row still contain the removed entry? {assertions[AssertKey.HasEntry]}");
                
                assertions[AssertKey.IsNullReference] = (removedEntry == null);
                results.Log($"-- Is the removed entry object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = row[removedEntry.GetField()].IsNull;
                results.Log($"-- Is the entry in the old position in the row where an entry was removed from, null? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.IsEmpty] = row.IsEmpty;
                results.Log($"-- Is this row empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- row.Count? {row.Count}");
                results.Log($"-- row.FieldCount? {row.FieldCount}");
                results.Log($"-- row.EntryCount? {row.EntryCount}");

                assertions[AssertKey.HasField] = row.HasField(removedEntry.GetField());
                results.Log($"-- Does any field inside this row match the removed entry's? {assertions[AssertKey.HasField]}");

                int fieldIndex = row.GetIndex(removedEntry.GetField());
                results.Log($"-- Index of the field belonging to the removed entry (Expect 1)? {fieldIndex}"); // Should not be -1.

                MySqlEntry secondIndexEntry = row.GetEntry(1) as MySqlEntry; // zero-based so second index is 1.
                results.Log($"-- Entry at second index? {secondIndexEntry}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] && // not null reference
                    !assertions[AssertKey.HasEntry] && // should not have entry.
                    assertions[AssertKey.HasNullValue] && // has null value at the index.
                    !assertions[AssertKey.IsEmpty] && // not empty
                    assertions[AssertKey.HasField] && // HasField("Field") (true)
                    fieldIndex >= 0 && // Retrieve field index is not == -1                   
                    secondIndexEntry != null &&
                    secondIndexEntry.IsNull; // Second index entry is not null. (Index 1)
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (!assertions[AssertKey.Total])
                {
                    results.Fail($"{subject} operation failed.");
                    return results;
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }
            
            // << REMOVE FIELD >>
            try
            {
                // Perform the correct operations.
                results.Log("", "-- Operation #3", "Removing field at the second index (index 1).");
                MySqlEntry removedEntry = row.RemoveField(1) as MySqlEntry;
                results.Log($"{printer.FormatHeader(row)}", 
                    $"{printer.FormatRow(row)}",
                    "Removed Entry:", $"{printer.FormatEntry(removedEntry)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.HasEntry] = (row.Contains(removedEntry));
                results.Log($"-- Does the row still contain the removed entry? {assertions[AssertKey.HasEntry]}");

                assertions[AssertKey.IsNullReference] = (removedEntry == null);
                results.Log($"-- Is the removed entry object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.HasNullValue] = row[removedEntry.GetField()] == null;
                results.Log($"-- Does GetEntry() on the non-existent field return null? {assertions[AssertKey.HasNullValue]}");

                assertions[AssertKey.IsEmpty] = row.IsEmpty;
                results.Log($"-- Is this row empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- row.Count? {row.Count}");
                results.Log($"-- row.FieldCount? {row.FieldCount}");
                results.Log($"-- row.EntryCount? {row.EntryCount}");

                assertions[AssertKey.HasField] = row.HasField(removedEntry.GetField());
                results.Log($"-- Does the removed field still show? {assertions[AssertKey.HasField]}");

                int fieldIndex = row.GetIndex(removedEntry.GetField());
                results.Log($"-- Index of the field belonging to the removed entry (Expect -1)? {fieldIndex}");

                MySqlEntry secondIndexEntry = row.GetEntry(1) as MySqlEntry; // zero-based so second index is 1.
                results.Log($"-- Entry at second index? {secondIndexEntry}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] && // not null reference
                    !assertions[AssertKey.HasEntry] && // should not have entry.
                    assertions[AssertKey.HasNullValue] && // has null value at the index.
                    !assertions[AssertKey.IsEmpty] && // not empty
                    !assertions[AssertKey.HasField] && // HasField("Field") (true)
                    fieldIndex < 0 && // Retrieve field index is not == -1                   
                    secondIndexEntry != null;
                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (!assertions[AssertKey.Total])
                {
                    results.Fail($"{subject} operation failed.");
                    return results;
                }
                else
                {
                    results.Pass($"{subject} operation passed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }
            
            // Return the test results.
            return results;
        }

        #endregion

        #region MySqlResultSet

        /// <summary>
        /// Test empty constructor for a result set.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlResultSet_Empty()
        {
            // Set values and dependencies here.
            string subject = "MySqlResultSet ()";
            MySqlResultSet set = null;
            
            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << REPEAT FOR EVERY OPERATION >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlResultSet object using the single field constructor.");
                set = new MySqlResultSet();
                results.Log($"{printer.FormatResultSet(set)}");
                
                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (set == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");
                
                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is this result set empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- Rows in set (set.Count)? {set.Count}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    assertions[AssertKey.IsEmpty];

                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        /// <summary>
        /// Test mutators for ResultSets.
        /// </summary>
        /// <returns>Return results from test.</returns>
        private static TestResults Test_MySqlResultSet_Mutators()
        {
            // Set values and dependencies here.
            string subject = "MySqlResultSet ()";
            MySqlResultSet set = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            // << ADD 10 ROWS >>
            try
            {
                // Perform the correct operations.
                results.Log("Creating the MySqlResultSet object using the single field constructor.");
                set = new MySqlResultSet();

                // Add the rows to the result set.
                results.Log("Add example row to the result set.");

                // Add 10 rows to the collection.
                for (int i = 0; i < 10; i++)
                {
                    set.Add(new MySqlRow(
                            new List<IEntry> {
                            new MySqlEntry("First Name", "Ian"),
                            new MySqlEntry("Last Name", "Effendi"),
                            new MySqlEntry("Email", $"example{i}@google.com")
                            }
                        ));
                }

                // Print the result set.
                results.Log($"{printer.FormatResultSet(set)}");

                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (set == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is this result set empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- Rows in set (set.Count)? {set.Count}");

                results.Log($"-- Grab first row entry with field, 'Email'.");
                IEntry example = set[0]["Email"];
                assertions[AssertKey.HasValue] = example != null;
                results.Log($"-- Is there an entry with the 'Email' field in the set? { assertions[AssertKey.HasValue] }");

                // Print the example entry.
                results.Log($"{printer.FormatEntry(example)}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.IsEmpty] &&
                    assertions[AssertKey.HasValue];

                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }
            
            // << REMOVE ROW >>
            try
            {
                // Perform the correct operations.
                results.Log("Get row from index 5.");
                IRow row = set[5];
                // Print the row.
                results.Log($"{printer.FormatRow(row)}");

                // Remove the item.
                results.Log("Remove retrieved row from set.");                
                set.Remove(row);
                // Print the result set.
                results.Log($"{printer.FormatResultSet(set)}");
                
                // If entry is null, fail the test.
                results.Log("Checking assertions...");

                assertions[AssertKey.IsNullReference] = (row == null);
                results.Log($"-- Is the object reference null? {assertions[AssertKey.IsNullReference]}");

                assertions[AssertKey.IsEmpty] = set.IsEmpty;
                results.Log($"-- Is this result set empty? { assertions[AssertKey.IsEmpty] }");
                results.Log($"-- Rows in set (set.Count)? {set.Count}");

                results.Log($"-- Grab new row that exists at index 5.");
                row = set[5];
                assertions[AssertKey.HasValue] = row != null && !row.IsNull;
                results.Log($"-- Retrieved row exists? { assertions[AssertKey.HasValue] }");
                // Print the retrieved row.
                results.Log($"{printer.FormatRow(row)}");

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    !assertions[AssertKey.IsEmpty] &&
                    assertions[AssertKey.HasValue];

                results.Log($"All assertions passed? {assertions[AssertKey.Total]}");

                if (assertions[AssertKey.Total])
                {
                    results.Pass($"{subject} operation passed.");
                }
                else
                {
                    results.Fail($"{subject} operation failed.");
                }
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            // Return the test results.
            return results;
        }

        #endregion

        #region Exception Tests.

        /// <summary>
        /// Test the logger by writing something to a file.
        /// </summary>
        /// <returns>Returns result from a test.</returns>
        private static TestResults Test_Logger_Empty()
        {
            // Set values and dependencies here.
            string subject = "Logger ()";
            Logger logger = null;

            // Create the results object for this test.
            TestResults results = TestResults.Create($"Testing {subject}");

            // Create the assertion map.
            Dictionary<AssertKey, bool> assertions = new Dictionary<AssertKey, bool>();

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- -- -- -- --\n-- -- -- {results.Title}");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"{e.Message}", e);
            }

            try
            {
                results.Log("Creating the logger object.");
                logger = new Logger("", "log-test");
                logger.Clear();
                results.Log($"{logger}");
                
                if (logger == null)
                {
                    results.Fail("Logger not constructed properly.");
                    return results;
                }

                results.Log("Writing to a file.");
                logger.Write("Writing this to a file, test.");

                results.Log("Appending to file.");
                logger.Write("Appending text to same file.");

                try
                {
                    results.Log("Testing data access layer exception logging.");
                    throw new DataAccessLayerException("Testing data access layer exception logging.");
                }
                catch (DataAccessLayerException dale)
                {
                    dale.Write(logger);
                }

                try
                {
                    results.Log("Testing database connection exception logging.");
                    throw new DatabaseConnectionException("Testing DBConnection exception logging.");
                }
                catch (DatabaseConnectionException dbce)
                {
                    dbce.Write(logger);
                }

                try
                {
                    results.Log("Testing database close exception logging.");
                    throw new DatabaseCloseException("Testing DBClose exception logging.");
                }
                catch (DatabaseCloseException dce)
                {
                    dce.Write(logger);
                }

                results.Pass("Logger constructed properly.");
            }
            catch (Exception e)
            {
                // Wraps exception for the results.
                throw results.Throw($"Exception occurred during construction of {subject}. {e.Message}", e);
            }

            return results;
        }

        #endregion

    }
}
