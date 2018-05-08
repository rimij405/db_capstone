/*
    RoleType.cs
    ---
    Ian Effendi
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ISTE.DAL.Models.Implementations;
using ISTE.BAL.Interfaces;
using ISTE.DAL.Models.Interfaces;
using ISTE.DAL.Database.Interfaces;
using ISTE.DAL.Models;
using ISTE.DAL.Database.Implementations;

namespace ISTE.BAL.Implementations
{
    public class BusinessPhones : BusinessCodes<MySqlPhoneTypes, IPhoneTypeModel>
    {
        public BusinessPhones(MySqlDatabase database)
        {
            this.glossary = MySqlPhoneTypes.GetInstance(database);
        }
    }

    public class PhoneType : BusinessCodeItem<MySqlPhoneType>, IBusinessPhoneType
    {
        /// <summary>
        /// Construct a type.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static MySqlPhoneType Create(string code, string name = "", string description = "")
        {
            return MySqlPhoneType.Create(code, name, description);
        }

        public PhoneType(MySqlDatabase database, string code)
            : base(Create(code))
        {
            // Get the code associated with this.
            this.codeItem.TryFetch(database, out IResultSet results, out DatabaseError errorCode);
        }
    }

    public class BusinessEmails : BusinessCodes<MySqlEmailTypes, IEmailTypeModel>
    {
        public BusinessEmails(MySqlDatabase database)
        {
            this.glossary = MySqlEmailTypes.GetInstance(database);
        }
    }

    public class EmailType : BusinessCodeItem<MySqlEmailType>, IBusinessEmailType
    {
        /// <summary>
        /// Construct a type.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static MySqlEmailType Create(string code, string name = "", string description = "")
        {
            return MySqlEmailType.Create(code, name, description);
        }

        public EmailType(MySqlDatabase database, string code)
            : base(Create(code))
        {
            // Get the code associated with this.
            this.codeItem.TryFetch(database, out IResultSet results, out DatabaseError errorCode);
        }
    }

    public class BusinessRoles : BusinessCodes<MySqlRoleTypes, IRoleTypeModel>
    {
        public BusinessRoles(MySqlDatabase database)
        {
            this.glossary = MySqlRoleTypes.GetInstance(database);
        }
    }
    
    public class RoleType : BusinessCodeItem<MySqlRoleType>, IBusinessRoleType
    {
        /// <summary>
        /// Construct a role type.
        /// </summary>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public static MySqlRoleType Create(string code, string name = "", string description = "")
        {
            return MySqlRoleType.Create(code, name, description);
        }
        
        public RoleType(MySqlDatabase database, string code)
            : base(Create(code))
        {
            // Get the code associated with this role.
            this.codeItem.TryFetch(database, out IResultSet results, out DatabaseError errorCode);
        }
    }
}
