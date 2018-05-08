/*
    BusinessRole.cs
    ---
    Ian Effendi
 */

using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.BAL.Implementations
{
    public enum Phones
    {
        NONE,
        PRIMARY,
        HOME,
        MOBILE,
        WORK
    }

    public class BusinessPhone
    {
        // Static.
        private MySqlDatabase database;
        private BusinessPhones allPhones;
        public BusinessPhones AllPhones
        {
            get { return (allPhones ?? (allPhones = new BusinessPhones(database))); }
        }

        string code = "";
        string name = "";
        string desc = "";

        public string Code
        {
            get { return code; }
        }

        public string Name
        {
            get { return name; }
        }

        public string Description
        {
            get { return desc; }
        }

        Phones currentType = Phones.NONE;

        public Phones Phone
        {
            get { return currentType; }
            set { currentType = value; }
        }

        public BusinessPhone(MySqlDatabase database, string code)
        {
            this.database = database;
            this.SetRole(code);
        }

        public void SetRole(string code)
        {
            ICodeItem item = this.AllPhones.Glossary[code];
            if (item != null)
            {
                this.code = item.Code.SQLValue;
                this.name = item.Name;
                this.desc = item.Description;

                switch (item.Name)
                {
                    case "PRIMARY":
                        this.currentType = Phones.PRIMARY;
                        break;
                    case "MOBILE":
                        this.currentType = Phones.MOBILE;
                        break;
                    case "HOME":
                        this.currentType = Phones.HOME;
                        break;
                    case "WORK":
                        this.currentType = Phones.WORK;
                        break;
                    default:
                        this.currentType = Phones.NONE;
                        break;
                }
            }
        }

    }
}
