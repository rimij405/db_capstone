/*
    IDataRetriever.cs
    Retrieve data from a database.
    ***
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
    /// Retrieve data from a given database.
    /// </summary>
    public interface IDataRetriever
    {

        /// <summary>
        /// Returns a 2-D collection of data from 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        List<List<string>> GetData(string query);

    }
}
