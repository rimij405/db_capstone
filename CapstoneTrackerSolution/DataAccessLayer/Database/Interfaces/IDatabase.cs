/*
    IDatabase.cs
    Interface describing connections to a database. 
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

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Interface declaring connection functionality to a database.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Attempt to connect to a database, using the implementation's connection object.
        /// </summary>
        /// <returns>Returns true on success. False, if otherwise.</returns>
        bool Connect();

        /// <summary>
        /// Attempt to close a database.
        /// </summary>
        /// <returns>Returns true on success. False, if otherwise.</returns>
        bool Close();

        /// <summary>
        /// Returns value determining if the program is currently connected to the database.
        /// </summary>
        /// <returns>Returns boolean.</returns>
        bool IsConnected();
    }
}
