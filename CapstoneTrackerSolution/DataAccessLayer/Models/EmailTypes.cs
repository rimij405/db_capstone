﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Models
{
    class EmailTypes
    {
        string emailCode;
        string name;
        string description;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public EmailTypes(string cd, string nm, string desc)
        {
            emailCode = cd;
            name = nm;
            description = desc;
        }

        // Query database for info at given ID
        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM EmailTypes WHERE emailCode = @emailCode;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@emailCode", emailCode}
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            emailCode = rs[0, "emailCode"].Value;
            name = rs[0, "name"].Value;
            description = rs[0, "description"].Value;

            return rs;
        }
    }
}
