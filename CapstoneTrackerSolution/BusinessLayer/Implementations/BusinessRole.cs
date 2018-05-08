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
    public enum Roles
    {
        NONE,
        STUDENT,
        FACULTY,
        STAFF,
        CHAIR,
        DIRECTOR
    }

    public class BusinessRole
    {
        // Static.
        private MySqlDatabase database;
        private BusinessRoles allRoles;
        public BusinessRoles AllRoles
        {
            get { return (allRoles ?? (allRoles = new BusinessRoles(database))); }
        }

        string userRoleCode = "";
        string userRoleName = "";
        string userRoleDescription = "";

        public string Code
        {
            get { return userRoleCode; }
        }

        public string Name
        {
            get { return userRoleName; }
        }

        public string Description
        {
            get { return userRoleDescription; }
        }

        Roles currentRole = Roles.NONE;

        public Roles Role
        {
            get { return currentRole; }
            set { currentRole = value; }
        }

        public BusinessRole(MySqlDatabase database, string roleCode)
        {
            this.database = database;
            this.SetRole(roleCode);
        }

        public void SetRole(string code)
        {
            ICodeItem item = this.AllRoles.Glossary[code];
            if (item != null)
            {
                this.userRoleCode = item.Code.SQLValue;
                this.userRoleName = item.Name;
                this.userRoleDescription = item.Description;

                switch (item.Name)
                {
                    case "STUDENT":
                        this.currentRole = Roles.STUDENT;
                        break;
                    case "STAFF":
                        this.currentRole = Roles.STAFF;
                        break;
                    case "CHAIR":
                        this.currentRole = Roles.CHAIR;
                        break;
                    case "DIRECTOR":
                        this.currentRole = Roles.DIRECTOR;
                        break;
                    case "FACULTY":
                        this.currentRole = Roles.FACULTY;
                        break;
                    default:
                        this.currentRole = Roles.NONE;
                        break;
                }
            }
        }

    }
}
