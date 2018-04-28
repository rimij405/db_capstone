/****
 * IReadable.cs
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
    /// Represents database class that can perform reading operations from the database.
    /// </summary>
    public interface IReadable
    {

        /// <summary>
        /// Return a result set filled with the results from a query.
        /// </summary>
        /// <param name="query">SQL query to execute on the database.</param>
        /// <param name="parameters">Query parameters to prepare the statement with.</param>
        /// <returns>Returns a populated result set.</returns>
        IResultSet GetData(string query, IDictionary<string, string> parameters);
        
    }
}
