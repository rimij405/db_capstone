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
        /// Entry implementation.
        /// </summary>
        private static IEntry entry;

        /// <summary>
        /// Entry point for the <see cref="TestDataAccessLayer"/> <see cref="Program"/>.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Create the printer service.
            console = new Printer("Test Data Access Layer Library", true);
            console.Debug("Printer initialized.");

            // TESTING MySqlConfiguration.
            // TestConfiguration();

            // TESTING MySqlDatabase: Connect
            // TestConnect();

            // TESTING MySqlEntry object.
            // TestMySqlEntry();

            // TESTING MySqlRow object.
            // TestMySqlRow();

            // Wait for user input.
            console.Pause("Press any key to exit the program...");
        }
                
        /// <summary>
        /// Test database configuration object.
        /// </summary>
        public static void TestConfiguration() {
            try
            {
                console.Debug("Testing: Creating MySqlConfiguration.");
                configuration = new MySqlConfiguration();
                console.Debug("Created the configuration successfully:\n" + configuration.ToString());
                console.Pause();
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the configuration.\nError: " + e.Message);
                console.Pause();
                // throw new DataAccessLayerException("Failed to create the configuration.", e);
            }
        }

        /// <summary>
        /// Test database connection object.
        /// </summary>
        public static void TestConnect() {
            try
            {
                console.Debug("Testing: Creating the MySqlDatabase.");
                database = new MySqlDatabase(configuration as MySqlConfiguration);
                database.Connect();
                console.Debug("Created the database successfully:\n" + database.ToString());
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the database.\n" + e.Message);
                console.Pause();
                // throw new DataAccessLayerException("Failed to create the database.", e);
            }
        }

        /// <summary>
        /// Tests methods of Entry.
        /// </summary>
        public static void TestMySqlEntry()
        {
            try
            {
                console.Write("Testing: MySqlEntry object.");
                MySqlEntry test;

                console.Write("Testing empty constructor.\n");
                test = new MySqlEntry("Test");
                console.Write($"MySqlEntry Test:\n{test}\n---------");                
                if (!test.IsNull()) { throw new Exception("Test failed. (IsNull())"); } else { console.Write("Test passed. (IsNull())."); }
                if (test.GetField() != "TEST") { throw new Exception("Test failed. (GetField())"); } else { console.Write("Test passed. (GetField())."); }
                if (!test.HasField("Test")) { throw new Exception("Test failed. (HasField(\"Test\"))"); } else { console.Write("Test passed. (HasField(\"Test\"))."); }
                if (test.GetValue() != MySqlEntry.NULL_VALUE) { throw new Exception("Test failed. (GetValue())"); } else { console.Write("Test passed. (GetValue())."); }
                if (!test.HasValue("NULL")) { throw new Exception("Test failed. (HasValue(\"NULL\"))"); } else { console.Write("Test passed. (HasValue(\"NULL\"))."); }
                if (test.GetData().Key != "TEST") { throw new Exception("Test failed. (GetData().Key)"); } else { console.Write("Test passed. (GetData().Key)."); }
                if (test.GetData().Value != MySqlEntry.NULL_VALUE) { throw new Exception("Test failed. (GetData().Value)"); } else { console.Write("Test passed. (GetData().Value)."); }
                console.Write("\n---------");

                console.Write("Testing string, string constructor.");
                test = new MySqlEntry("Test2", "TestValue");
                console.Write($"MySqlEntry Test:\n{test}\n---------");
                if (test.IsNull()) { throw new Exception("Test failed. (IsNull())"); } else { console.Write("Test passed. (IsNull())."); }
                if (test.GetField() != "TEST2") { throw new Exception("Test failed. (GetField())"); } else { console.Write("Test passed. (GetField())."); }
                if (!test.HasField("Test2")) { throw new Exception("Test failed. (HasField(\"Test2\"))"); } else { console.Write("Test passed. (HasField(\"Test2\"))."); }
                if (test.GetValue() != "TestValue") { throw new Exception("Test failed. (GetValue())"); } else { console.Write("Test passed. (GetValue())."); }
                if (!test.HasValue("TestValue")) { throw new Exception("Test failed. (HasValue(\"TestValue\"))"); } else { console.Write("Test passed. (HasValue(\"TestValue\"))."); }
                if (test.GetData().Key != "TEST2") { throw new Exception("Test failed. (GetData().Key)"); } else { console.Write("Test passed. (GetData().Key)."); }
                if (test.GetData().Value != "TestValue") { throw new Exception("Test failed. (GetData().Value)"); } else { console.Write("Test passed. (GetData().Value)."); }

                console.Write("Testing clone method.");
                entry = new MySqlEntry("Test3", "Clone Value");
                console.Write($"MySqlEntry Source:\n{entry}\n---------");
                test = entry.Clone() as MySqlEntry;
                console.Write($"MySqlEntry Test:\n{test}\n---------");
                if (test.IsNull()) { throw new Exception("Test failed. (IsNull())"); } else { console.Write("Test passed. (IsNull())."); }
                if (test.GetField() != "TEST3") { throw new Exception("Test failed. (GetField())"); } else { console.Write("Test passed. (GetField())."); }
                if (!test.HasField("Test3")) { throw new Exception("Test failed. (HasField(\"Test3\"))"); } else { console.Write("Test passed. (HasField(\"Test3\"))."); }
                if (test.GetValue() != "Clone Value") { throw new Exception("Test failed. (GetValue())"); } else { console.Write("Test passed. (GetValue())."); }
                if (!test.HasValue("Clone Value")) { throw new Exception("Test failed. (HasValue(\"TestValue\"))"); } else { console.Write("Test passed. (HasValue(\"TestValue\"))."); }
                if (test.GetData().Key != "TEST3") { throw new Exception("Test failed. (GetData().Key)"); } else { console.Write("Test passed. (GetData().Key)."); }
                if (test.GetData().Value != "Clone Value") { throw new Exception("Test failed. (GetData().Value)"); } else { console.Write("Test passed. (GetData().Value)."); }
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the MySqlEntry.\n" + e.Message);
                console.Pause();
                // throw new DataAccessLayerException("Failed to create the MySqlEntry object.", e);
            }
        }

        /// <summary>
        /// Test row methods.
        /// </summary>
        public static void TestMySqlRow() {
            try
            {

                console.Write("Testing MySqlRow");
                MySqlRow row = new MySqlRow();

                console.Write("Testing empty constructor.");
                console.Write($"{row}");
                if (!row.IsEmpty()) { throw new Exception("Should be empty."); } else { console.Write("Test passed. Empty row."); }

                row = new MySqlRow(new List<string>() {
                    "FirstName",
                    "LastName",
                    "Age"                    
                });
                row.AddEntry("Birthday", "1997-12-09 00:00:00");
                row.AddEntry("UserID", "1");
                console.Write("Testing field constructor.");
                console.Write($"{row}");
                if (row.IsEmpty()) { throw new Exception("Should not be empty."); } else { console.Write("Test passed. Row with fields."); }
                if (row[0].Length == 0) { throw new Exception("Empty string in place of field."); } else { console.Write($"Test passed. Field \"{row[0]}\" found."); }
                if (!row["LastName"].IsNull()) { throw new Exception("New entry should be null when only field is instantiated."); } else { console.Write($"Test passed. Entry \"{row["LastName"]}\" found."); }
                if (row["Birthday"].IsNull()) { throw new Exception("New entry should have value."); } else { console.Write($"Test passed. Entry \"{row["Birthday"]}\" found."); }
                console.Write("Overwriting entry value.");
                row.SetEntry("Birthday", "TEST-REWRITE");
                if (row["Birthday"].IsNull()) { throw new Exception("New entry should have value."); } else { console.Write($"Test passed. Entry \"{row["Birthday"]}\" overwritten."); }
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
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the MySqlRow.\n" + e.Message);
                console.Pause();
                // throw new DataAccessLayerException("Failed to create the MySqlRow object.", e);
            }
        }
        
    }
}
