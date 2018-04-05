﻿using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public List<List<string>> Fetch()
        {
            List<List<string>> ary = null;
            string sqlStr = "SELECT * FROM EmailTypes WHERE emailCode = '" + emailCode + "';";
            ary = sql.GetData(sqlStr);
            if (ary != null && ary.Count != 0)
            {
                emailCode = ary[0][1];
                name = ary[0][2];
                description = ary[0][3];
            }
            return ary;
        }
    }
}