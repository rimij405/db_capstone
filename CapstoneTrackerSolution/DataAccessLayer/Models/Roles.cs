using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Models
{
    class Roles
    {
        string code;
        string name;
        string description;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public Roles(string cd, string nm, string desc)
        {
            code = cd;
            name = nm;
            description = desc;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Roles WHERE code = @code;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@code", code }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            code = rs[0, "code"].Value;
            name = rs[0, "name"].Value;
            description = rs[0, "description"].Value;

            return rs;
        }
    }
}
