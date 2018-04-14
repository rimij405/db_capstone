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
            TestConfiguration();

            // TESTING MySqlDatabase: Connect
            TestConnect();

            // TESTING MySqlEntry object.
            TestMySqlEntry();

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
                console.Pause("Press any key to exit the program...");
                throw new DataAccessLayerException("Failed to create the configuration.", e);
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
                console.Pause("Press any key to exit the program...");
                throw new DataAccessLayerException("Failed to create the database.", e);
            }
        }

        /// <summary>
        /// Tests methods of Entry.
        /// </summary>
        public static void TestMySqlEntry()
        {
            try
            {
                console.Debug("Testing: MySqlEntry object.");
                MySqlEntry test;

                console.Debug("Testing empty constructor.");
                test = new MySqlEntry("Test");
                console.Debug($"\nMySqlEntry Test:\n{test}\n---------");
                if (!test.IsNull()) { throw new Exception("Test failed. (IsNull())"); }
                if (test.GetField() != "Test") { throw new Exception("Test failed. (GetField())"); }
                if (test.GetValue() != MySqlEntry.NULL_VALUE) { throw new Exception("Test failed. (GetValue())"); }
                if (test.GetData().Key != "Test") { throw new Exception("Test failed. (GetData().Key)"); }
                if (test.GetData().Value != MySqlEntry.NULL_VALUE) { throw new Exception("Test failed. (GetData().Value)"); }

                console.Debug("Testing string, string constructor.");
                test = new MySqlEntry("Test2", "TestValue");
                console.Debug($"\nMySqlEntry Test:\n{test}\n---------");
                if (!test.IsNull()) { throw new Exception("Test failed. (IsNull())"); }
                if (test.GetField() != "Test2") { throw new Exception("Test failed. (GetField())"); }
                if (test.GetValue() != "TestValue") { throw new Exception("Test failed. (GetValue())"); }
                if (test.GetData().Key != "Test2") { throw new Exception("Test failed. (GetData().Key)"); }
                if (test.GetData().Value != "TestValue") { throw new Exception("Test failed. (GetData().Value)"); }

                console.Debug("Testing clone method.");
                entry = new MySqlEntry("Test3", "Clone Value");
                console.Debug($"\nMySqlEntry Source:\n{entry}\n---------");
                test = entry.Clone() as MySqlEntry;
                console.Debug($"\nMySqlEntry Test:\n{test}\n---------");
                if (!test.IsNull()) { throw new Exception("Test failed. (IsNull())"); }
                if (test.GetField() != "Test2") { throw new Exception("Test failed. (GetField())"); }
                if (test.GetValue() != "TestValue") { throw new Exception("Test failed. (GetValue())"); }
                if (test.GetData().Key != "Test2") { throw new Exception("Test failed. (GetData().Key)"); }
                if (test.GetData().Value != "TestValue") { throw new Exception("Test failed. (GetData().Value)"); }                
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the MySqlEntry.\n" + e.Message);
                console.Pause("Press any key to exit the program...");
                throw new DataAccessLayerException("Failed to create the MySqlEntry object.", e);
            }
        }
        
    }
}
