/*
    ILoggable.cs
    Contains interface that enables classes the chance to log items to a file. 
    ***
    04/04/2018
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;

namespace ISTE.DAL.Database
{

    /// <summary>
    /// Enables message logging on a class implementation.
    /// </summary>
    public interface ILoggable
    {

        /// <summary>
        /// Executes a write operation, using the input <see cref="Logger"/> object.
        /// </summary>
        /// <param name="log"><see cref="Logger"/> object that will be used to write.</param>
        /// <returns>Returns reference to self.</returns>
        ILoggable Write(Logger log);

    }
}
