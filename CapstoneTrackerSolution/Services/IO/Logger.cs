/*
    Logger.cs
    Handles printing information to files.    
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
using System.IO;

namespace Services
{

    /// <summary>
    /// Static class that handles printing custom messages to a file.
    /// </summary>
    public class Logger
    {

        // General logger instance.

        /// <summary>
        /// Static, singleton instance of a Logger.
        /// </summary>
        private static Logger instance = null;

        /// <summary>
        /// Creates a new logger and assigns it as the singleton instance.
        /// </summary>
        /// <returns></returns>
        private static Logger CreateInstance()
        {
            if(instance == null)
            {
                instance = new Logger();
            }
            return instance;
        }

        /// <summary>
        /// Grants access to singleton instance of a general Logger for most printing needs.
        /// </summary>
        public static Logger Instance 
        {
            get { return (instance == null) ? CreateInstance() : instance; }
        }

        // Field(s).

        /// <summary>
        /// Filename to print text information to.
        /// </summary>
        private string filename = "log";

        /// <summary>
        /// Filepath to print text file in.
        /// </summary>
        private string filepath = "";

        /// <summary>
        /// Extension to append.
        /// </summary>
        private string extension = "txt";
        
        // Properties.

        /// <summary>
        /// (Read-only) Returns the formatted filepath of the logger.
        /// </summary>
        public string Filepath
        {
            get { return filepath + filename + "." + extension; }
        }

        // Constructor(s).

        /// <summary>
        /// Logger object writes log information to a set file.
        /// </summary>
        /// <param name="_filepath">Filepath to store file.</param>
        /// <param name="_filename">Filename to assign to file.</param>
        /// <param name="_ext">Extension to save file under.</param>
        public Logger(string _filepath = "", string _filename = "log", string _ext = "txt")
        {
            this.filename = _filename;
            this.filepath = _filepath;
            this.extension = _ext;
        }

        /// <summary>
        /// Clear the text file associated with this logger of all text.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public Logger Clear()
        {
            if (File.Exists(this.Filepath))
            {
                File.WriteAllText(this.Filepath, String.Empty);
            }
            return this;
        }

        /// <summary>
        /// Write a line in the text file.
        /// </summary>
        /// <param name="message">Message to print.</param>
        /// <returns>Returns reference to this logger.</returns>
        public Logger Write(string message)
        {
            string content = message;
            string[] lines = content.Split('\n');

            using (StreamWriter write = File.AppendText(this.Filepath))
            {
                foreach (string text in lines)
                {
                    if (text.Length > 0)
                    {
                        write.WriteLine(text);
                    }
                }
            }

            return this;
        }

        /// <summary>
        /// Write messages to this log's text file. If newline is true, each line is placed on its own line.
        /// </summary>
        /// <param name="messages">Messages to print.</param>
        /// <param name="newline">Newline separation between lines.</param>
        /// <returns>Returns reference to this logger.</returns>
        public Logger Write(List<string> messages, bool newline = true)
        {
            if (messages.Count > 0)
            {
                string text = "";
                foreach (string message in messages)
                {
                    if (newline)
                    {
                        this.Write(message);
                        continue;
                    }
                    else
                    {
                        text += " " + message;
                    }
                }
                
                if(!newline) { return this.Write(text); }
            }
            return this;
        }
    }
}
