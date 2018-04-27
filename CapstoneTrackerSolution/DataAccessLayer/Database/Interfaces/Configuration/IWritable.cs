/***
 * IWritable.cs
 * ---
 * Ian Effendi
 * ***/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database.Interfaces
{

    /// <summary>
    /// Represents any operation that will *write* to the database.
    /// </summary>
    public interface IWritable
    {

        /// <summary>
        /// Return a result set with the number of rows affected (and no rows).
        /// </summary>
        /// <param name="query">SQL query to execute on the database.</param>
        /// <param name="parameters">Query parameters to prepare the statement with.</param>
        /// <returns>Returns a populated result set.</returns>
        IResultSet SetData(string query, List<string> parameters);

        /// <summary>
        /// Set data in the database, while returning a boolean flag based on operation success.
        /// </summary>
        /// <param name="query">SQL query to execute on the database.</param>
        /// <param name="parameters">Query parameters to prepare the statement with.</param>
        /// <param name="results">Result set containing number of rows affected.</param>
        /// <returns>Return true if operation was successful.</returns>
        bool TrySetData(string query, List<string> parameters, out IResultSet results);

    }
}
