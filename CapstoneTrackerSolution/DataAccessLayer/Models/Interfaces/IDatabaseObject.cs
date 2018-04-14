/*
    IDatabaseObject.cs
    DBO interface. 
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
using ISTE.DAL.Database;

namespace ISTE.DAL.Models
{
    public interface IDatabaseObjectReader
    {

        /// <summary>
        /// Fetch information for this object from the database.
        /// </summary>
        /// <returns></returns>
        // bool Fetch(IDatabase);
    
    }
}
