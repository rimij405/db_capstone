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
    class PhoneTypes
    {
        string phoneCode;
        string name;
        string description;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public PhoneTypes(string cd, string nm, string desc)
        {
            phoneCode = cd;
            name = nm;
            description = desc;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM PhoneTypes WHERE phoneCode = @phoneCode;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@phoneCode", phoneCode}
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            phoneCode = rs[0, "phoneCode"].Value;
            name = rs[0, "name"].Value;
            description = rs[0, "description"].Value;

            return rs;
        }
    }
}
