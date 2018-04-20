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
            TestMethods += Test_MySqlDatabaseConnect;

            TestMethods += Test_MySqlEntry_SingleField;
            TestMethods += Test_MySqlEntry_FieldValue;
            TestMethods += Test_MySqlEntry_KeyValuePair;
            TestMethods += Test_MySqlEntry_Clone;

            // TESTING MySqlDatabase: Connect
            // TESTING MySqlEntry object.
            // TESTING MySqlRow object.
            // TESTING MySqlResultSet object.
            
            try
            {
                r.Log("------------------------", $"Starting {r.Title}");

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
                    currentTest++;

                    r.Log("------------------------", $"-- Invoking test {currentTest} out of {totalTests} test(s).");

                    // Get the result from test.
                    Func<TestResults> t = (Func<TestResults>)test;
                    TestResults output = t.Invoke();
                    
                    if (output == null)
                    {
                        errors++;
                        r.Log(TestResults.CreateError($"Test #{currentTest}", "No result object returned.")[TestResults.ErrorState]);
                        console.Pause($"Error thrown during test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                        continue;
                    }

                    // If it isn't null, we can cast it.
                    TestResults result = ((TestResults)output);

                    r.Log($"Test #{currentTest}: \"{result.Title}\"");
                    if (verbose) { console.Write(result.StackTrace); }

                    // If it failed, log and continue to next test.
                    if (result.IsFailure)
                    {
                        r.Log(result[TestStatus.FAILURE], result.StackTrace);
                        console.Pause($"Failed test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                        continue;
                    }

                    // Increment counter.
                    if (result.IsSuccessful)
                    {
                        r.Log(result[TestStatus.SUCCESS]);
                        successfulTests++;
                        console.Pause($"Completed test {currentTest} out of {totalTests} test(s). Press any key to continue...");
                    }
                }
                
                r.Pass($"Successfully executed {totalTests} test(s).");
            }
            catch (TestException te)
            {
                r.Fail($"Exception \"{te.GetExceptionName()}\" has been thrown.");
                r.Log(te.Results.StackTrace, $"-- {te.Results.GetCurrentMessage()}");
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
                if (verbose) { console.Write(r.StackTrace); }

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
                results.Log($"-- -- -- {results.Title}");                
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

        #region Database Configuration and Connection

        /// <summary>
        /// Test database configuration object constructor.
        /// </summary>
        private static TestResults Test_MySqlConfiguration() {

            // Create the results object for this test.
            TestResults results = TestResults.Create("Testing MySqlConfiguration");

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- {results.Title}");

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
        private static TestResults Test_MySqlDatabaseConnect() {

            // Create the results object for this test.
            TestResults results = TestResults.Create("Testing MySqlDatabase");

            try
            {
                // Make divisor and log the title for the test.
                results.Log($"-- -- -- {results.Title}");

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

        #endregion

        /// <summary>
        /// Assertion keys are linked to assertion maps, allowing for checking of particular values with a reference.
        /// </summary>
        private enum AssertKey
        {
            Total,
            IsNullReference,
            HasNullValue,
            HasField,
            HasValue,
            IsEmpty
        }

        #region MySqlEntry
        
        /// <summary>
        /// Tests the single field constructor of MySqlEntry.
        /// </summary>
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
                results.Log($"-- -- -- {results.Title}");                
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
                results.Log($"{entry}");

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
                results.Log($"-- -- -- {results.Title}");
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
                results.Log($"{entry}");

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
                results.Log($"-- -- -- {results.Title}");
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
                results.Log($"{entry}");

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
                results.Log($"-- -- -- {results.Title}");
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
                results.Log($"{entry}");

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

        #endregion

        #region MySqlRow
        
        /// <summary>
        /// Tests empty constructor for MySqlRow.
        /// </summary>
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
                results.Log($"-- -- -- {results.Title}");
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
                results.Log($"{row}");

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

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.IsEmpty] &&
                    !assertions[AssertKey.HasField];
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
        /// Tests empty constructor for MySqlRow.
        /// </summary>
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
                results.Log($"-- -- -- {results.Title}");
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
                results.Log($"{row}");

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
                // results.Log($"-- What is the first field of the row")

                // assertions[AssertKey.HasEntry] = 

                // Evaluate assertions.
                assertions[AssertKey.Total] =
                    !assertions[AssertKey.IsNullReference] &&
                    assertions[AssertKey.HasNullValue] &&
                    assertions[AssertKey.IsEmpty] &&
                    !assertions[AssertKey.HasField];
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
        /// Test row methods.
        /// </summary>
        private static void TestMySqlRow() {
            try
            {

                console.Write("Testing MySqlRow");
                MySqlRow row = new MySqlRow();

                console.Write("Testing empty constructor.");
                console.Write($"{row}");
                if (!row.IsEmpty) { throw new Exception("Should be empty."); } else { console.Write("Test passed. Empty row."); }

                row = new MySqlRow(new List<string>() {
                    "FirstName",
                    "LastName",
                    "Age"                    
                });
                row.AddEntry("Birthday", "1997-12-09 00:00:00");
                row.AddEntry("UserID", "1");
                console.Write("Testing field constructor.");
                console.Write($"{row}");
                if (row.IsEmpty) { throw new Exception("Should not be empty."); } else { console.Write("Test passed. Row with fields."); }
                if (row[0].Length == 0) { throw new Exception("Empty string in place of field."); } else { console.Write($"Test passed. Field \"{row[0]}\" found."); }
                if (!row["LastName"].IsNull) { throw new Exception("New entry should be null when only field is instantiated."); } else { console.Write($"Test passed. Entry \"{row["LastName"]}\" found."); }
                if (row["Birthday"].IsNull) { throw new Exception("New entry should have value."); } else { console.Write($"Test passed. Entry \"{row["Birthday"]}\" found."); }
                console.Write("Overwriting entry value.");
                row.SetEntry("Birthday", "TEST-REWRITE");
                if (row["Birthday"].IsNull) { throw new Exception("New entry should have value."); } else { console.Write($"Test passed. Entry \"{row["Birthday"]}\" overwritten."); }
                console.Write("Removing field UserID.");
                row.RemoveField("UserID");
                console.Write($"{row}");
                MySqlRow copy = new MySqlRow(row);
                console.Write("Testing copy constructor.");
                console.Write($"{copy}");
                console.Write($"{row}");
                console.Write("Clearing row.");
                row.Clear();
                console.Write($"{copy}");
                console.Write($"{row}");

                MySqlResultPrinter formatter = new MySqlResultPrinter(150, 1);
                console.Write(formatter.GenerateDivisor(10));
                console.Write(formatter.GenerateDivisor(new List<int> { 7, 10 }));
                console.Write(formatter.GenerateDivisor(new List<int> { 10, 5, 5, 7, 10 }));

                console.Write("Testing format with value: \"Ape\" at total length of 10. Expected: \"| Ape      |\"");
                console.Write(formatter.FormatTextSegment("Ape", 10));
                console.Write("Testing format with value: \"ApeIsLongerThan10\" at total length of 10. Expected: \"| ApIsLo...  |\"");
                console.Write(formatter.FormatTextSegment("ApeIsLongerThan10", 10));
                console.Write("Testing format with value: \"JoelMikcheklson\" at total length of 10. Expected: \"| JoelMik...  |\"");
                console.Write(formatter.FormatTextSegment("JoelMikcheklson", 10));

                console.Write("Testing entries: {Tom, Bobby, Bill, JoelMikcheklson}, { 5, 5, 10, 10 } ");
                console.Write(
                    formatter.FormatText(
                        new List<string>() {
                            "Tom",
                            "Bobby",
                            "Bill",
                            "JoelMikcheklson"
                        },
                        new List<int>() {
                            5, 5, 10, 10
                        }
                    ));

                MySqlEntry entry = new MySqlEntry("Format Example", "This is how we print entries.");
                console.Write(formatter.FormatEntry(entry));

                MySqlEntry entry2 = new MySqlEntry("Field Print Example", "This is an example entry.");
                console.Write(formatter.FormatHeader(entry, entry2));

                MySqlEntry entry3 = new MySqlEntry("Second Example", "This is an different example entry.");
                MySqlRow row2 = new MySqlRow(entry2, entry3);
                console.Write(formatter.FormatHeader(row2));
                console.Write(formatter.FormatRow(row2));

                MySqlRow row3 = new MySqlRow(entry2, entry3);
                MySqlResultSet set = new MySqlResultSet("[Test query.]", 0, row2, row3);
                console.Write(formatter.FormatResultSet(set));

            }
            catch (Exception e)
            {
                console.Debug("Failed to create the MySqlRow.\n" + e.Message);
                console.Pause();
                // throw new DataAccessLayerException("Failed to create the MySqlRow object.", e);
            }
        }

        /// <summary>
        /// Test result set methods.
        /// </summary>
        public static void TestMySqlResultSet()
        {

        }

    }
}
