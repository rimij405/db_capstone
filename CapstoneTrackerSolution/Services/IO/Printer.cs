/*
    Printer.cs
    Contains library files for simplifying console application statements. 
    ***
    04/03/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

// Using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Library namespace.
namespace Services
{
    /// <summary>
    /// Printer object that simply wraps the Console.
    /// </summary>
    public class Printer
    {

        // Fields.

        /// <summary>
        /// The background color of the console.
        /// </summary>
        private ConsoleColor backgroundColor;

        /// <summary>
        /// The foreground color of the console.
        /// </summary>
        private ConsoleColor foregroundColor;

        /// <summary>
        /// Debug mode.
        /// </summary>
        private bool debugMode;

        /// <summary>
        /// Title of the console window.
        /// </summary>
        private string title;

        // Properties.

        /// <summary>
        /// The background <see cref="ConsoleColor"/> of the <see cref="Console"/>.
        /// </summary>
        public ConsoleColor Background
        {
            get { return this.backgroundColor; }
            set { SetBackground(value); }
        }

        /// <summary>
        /// The foreground <see cref="ConsoleColor"/> of the <see cref="Console"/>.
        /// </summary>
        public ConsoleColor Foreground
        {
            get { return this.foregroundColor; }
            set { SetForeground(value); }
        }

        /// <summary>
        /// Flag referencing the debug mode.
        /// </summary>
        public bool DebugMode
        {
            get { return this.debugMode; }
            set { SetDebugMode(value); }
        }

        /// <summary>
        /// Title of the console window.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { SetTitle(value); }
        }

        // Constructor(s).

        /// <summary>
        /// Creates a printer object that stores properties for printing to the console.
        /// </summary>
        public Printer(string _title, bool _debugMode = false)
        {
            this.Title = _title;
            this.DebugMode = _debugMode;
            Console.ResetColor();
        }

        // Service Methods.

        /// <summary>
        /// Writes message to the <see cref="Console"/>.
        /// </summary>
        /// <param name="message">Message to write to the console.</param>
        /// <param name="trim">Flag determines if the message should be trimmed before printing.</param>
        /// <param name="newline">Flag determines if a newline should be printed.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer Write(string message, bool trim = false, bool newline = true)
        {
            string msg = message;
            if(trim) { msg = msg.Trim(); }
            if(newline) { msg += '\n'; }
            Console.Write(msg);
            return this;
        }

        /// <summary>
        /// Writes a series of messages to the <see cref="Console"/>. By default, it delimits strings with a newline.
        /// </summary>
        /// <param name="messages">Collection of messages to write.</param>
        /// <param name="delimiter">String to append after ever message.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer Write(List<string> messages, string delimiter = "\n")
        {
            string msg = "";
            foreach (string message in messages)
            {
                msg += message + delimiter;
            }
            return this.Write(msg);
        }

        /// <summary>
        /// Prints a debug message, if the debug flag is set to true.
        /// </summary>
        /// <param name="message">Message to write to the console.</param>
        /// <param name="trim">Flag determines if the message should be trimmed before printing.</param>
        /// <param name="newline">Flag determines if a newline should be printed.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer Debug(string message, bool trim = false, bool newline = true)
        {
            if (this.DebugMode)
            {
                this.Write("[Debug]: " + message, trim, newline);
            }
            return this;
        }

        /// <summary>
        /// Writes a series of debug messages, if the debug flag is set to true.
        /// </summary>
        /// <param name="messages">Collection of messages to write.</param>
        /// <param name="delimiter">String to append after ever message.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer Debug(List<string> messages, string delimiter = "\n")
        {
            if (this.DebugMode)
            {
                if (messages.Count > 0) { messages[0] = "[Debug]: " + messages[0]; }
                this.Write(messages, delimiter);
            }
            return this;
        }

        /// <summary>
        /// Pauses the console until the user presses a key.
        /// </summary>
        /// <param name="message">Message to print while waiting for input.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer Pause(string message = "Press any key to continue...")
        {
            this.Write(message);
            Console.ReadKey(true);
            return this;
        }

        /// <summary>
        /// Checks to see if the user input matches a single expected output.
        /// </summary>
        /// <param name="query">Query to ask the user.</param>
        /// <param name="answer">Expected answer.</param>
        /// <param name="sameLength">Check if it has to be exactly the same length.</param>
        /// <param name="caseSensitive">Check if the answer is case sensitive.</param>
        /// <returns>Returns true if there was a match.</returns>
        public bool Input(string query, string answer, bool sameLength = false, bool caseSensitive = false)
        {            
            this.Write(query);
            string output = Console.ReadLine().Trim();
            string expected = answer.Trim();
            if(sameLength && (output.Length != expected.Length)) { return false; }
            output = output.Substring(0, expected.Length).Trim(); // Trims output for whitespace and truncates it if it was longer than the expected answer.
            if (!caseSensitive) { output = output.ToLower(); expected = expected.ToLower(); }
            return output == expected;
        }

        // Mutator Methods.

        /// <summary>
        /// Set the <see cref="Console"/>'s background color.
        /// </summary>
        /// <param name="_color"><see cref="ConsoleColor"/> to set background to.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer SetBackground(ConsoleColor _color)
        {
            Console.BackgroundColor = _color;
            this.backgroundColor = _color;
            return this;
        }

        /// <summary>
        /// Set the <see cref="Console"/>'s foreground color.
        /// </summary>
        /// <param name="_color"><see cref="ConsoleColor"/> to set foreground to.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer SetForeground(ConsoleColor _color)
        {
            Console.ForegroundColor = _color;
            this.foregroundColor = _color;
            return this;
        }

        /// <summary>
        /// Set the debug mode flag.
        /// </summary>
        /// <param name="_mode">Value to set <see cref="debugMode"/> to.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer SetDebugMode(bool _mode)
        {
            this.debugMode = _mode;
            return this;
        }

        /// <summary>
        /// Set the title value of the <see cref="Console"/> window.
        /// </summary>
        /// <param name="_title">Value to set <see cref="title"/> to.</param>
        /// <returns>Returns reference to this <see cref="Printer"/> object.</returns>
        public Printer SetTitle(string _title)
        {
            Console.Title = _title;
            this.title = _title;
            return this;
        }
    }


}
