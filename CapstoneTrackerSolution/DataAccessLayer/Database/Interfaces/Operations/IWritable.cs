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
        IResultSet SetData(string query, IDictionary<string, string> parameters);

    }
}
