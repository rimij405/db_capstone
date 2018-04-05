/*
    IDataWriter.cs
    Write data to a database.
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
    /// Write data to the database.
    /// </summary>
    public interface IDataWriter
    {

        bool SetData(List<List<string>> data);

    }

}
