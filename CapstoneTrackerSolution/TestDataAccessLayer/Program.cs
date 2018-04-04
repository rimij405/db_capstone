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
        /// Entry point for the <see cref="TestDataAccessLayer"/> <see cref="Program"/>.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            // Create the printer service.
            Printer console = new Printer("Test Data Access Layer Library", true);
            console.Debug("Printer initialized.");

            MySqlConfiguration configuration;
            MySqlDatabase database;

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
                return;
            }

            try
            {
                console.Debug("Testing: Creating the MySqlDatabase.");
                database = new MySqlDatabase(configuration);
                database.Connect();
                console.Debug("Created the database successfully:\n" + database.ToString());
            }
            catch (Exception e)
            {
                console.Debug("Failed to create the database.\n" + e.Message);
                console.Pause("Press any key to exit the program...");
                return;
            }

            // Wait for user input.
            console.Pause("Press any key to exit the program...");
        }
    }
}
