/*
    MySqlUserProperties.cs
    ---
    Ian Effendi
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements.
using ISTE.DAL.Database;
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Database.Interfaces;
using ISTE.DAL.Models.Interfaces;

namespace ISTE.DAL.Models.Implementations
{

    /// <summary>
    /// User property class.
    /// </summary>
    public abstract class MySqlUserProperty : MySqlDatabaseObjectModel, IUserProperty
    {
        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Type of property.
        /// </summary>
        private UserProperties propertyType;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Type of user property.
        /// </summary>
        public UserProperties PropertyType
        {
            get { return this.propertyType; }
            protected set { this.propertyType = value; }
        }

        /// <summary>
        /// Reference to 'owner' of this property.
        /// </summary>
        public virtual IUIDFormat OwnerID
        {
            get { return new MySqlID(this["userID"].Value); }
            protected set { this["userID"].SetValue(value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of a user property.
        /// </summary>
        /// <param name="type">Type of property.</param>
        /// <param name="userID">User ID.</param>
        public MySqlUserProperty(UserProperties type, IUIDFormat userID)
        {
            this.Model.AddEntry(new MySqlPrimaryKeyEntry(true, "userID"));
            this.PropertyType = type;
            this.OwnerID = userID;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Query string.
        /// </summary>
        /// <param name="operation">Operation to get query for.</param>
        /// <returns>Returns string.</returns>
        protected abstract string GetQuery(DatabaseOperation operation);

        /// <summary>
        /// Parameters for execution.
        /// </summary>
        /// <param name="operation">Operation to get parameters for.</param>
        /// <returns>Returns parameters object.</returns>
        protected abstract MySqlParameters GetParameters(DatabaseOperation operation);

        /// <summary>
        /// Used to set data after fetching.
        /// </summary>
        /// <param name="results">Results of the fetch.</param>
        protected abstract void Set(IResultSet results);

        /// <summary>
        /// Get data for property.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a populated result set.</returns>
        public override IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            MySqlResultSet results = new MySqlResultSet();
            results.Error();
            errorCode = DatabaseError.UNKNOWN;

            if (database is MySqlDatabase mysqldb)
            {
                string query = GetQuery(DatabaseOperation.Read);
                MySqlParameters parameters = GetParameters(DatabaseOperation.Read);
                results = mysqldb.GetData(query, parameters) as MySqlResultSet;
                this.Set(results);
                results.Pass();
            }

            return results;
        }

        /// <summary>
        /// Get data for property.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="results">Results.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Operation success.</returns>
        public override bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode)
        {
            results = new MySqlResultSet();
            results.Fail();

            try
            {
                this.Fetch(database, out errorCode);
                results.Pass();
                return true;
            }
            catch (DatabaseOperationException doe)
            {
                results.Error();
                errorCode = doe.ErrorCode;
                return false;
            }
        }
        
        /// <summary>
        /// Insert new entry into the database.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public virtual IResultSet Insert(IWritable database, out DatabaseError errorCode)
        {
            MySqlResultSet results = new MySqlResultSet();
            results.Error();
            errorCode = DatabaseError.UNKNOWN;

            if (database is MySqlDatabase mysqldb)
            {
                string query = GetQuery(DatabaseOperation.Create);
                MySqlParameters parameters = GetParameters(DatabaseOperation.Create);
                results = mysqldb.SetData(query, parameters) as MySqlResultSet;
                if (results.RowsAffected == 1)
                {
                    results.Pass();
                }
                else
                {
                    results.Fail();
                }
            }

            return results;
        }

        /// <summary>
        /// Insert new data for property.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="results">Results.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Operation success.</returns>
        public virtual bool TryInsert(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            results = new MySqlResultSet();
            results.Fail();

            try
            {
                this.Insert(database, out errorCode);
                results.Pass();
                return true;
            }
            catch (DatabaseOperationException doe)
            {
                results.Error();
                errorCode = doe.ErrorCode;
                return false;
            }
        }

        /// <summary>
        /// Update entry in the database.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public virtual IResultSet Update(IWritable database, out DatabaseError errorCode)
        {
            MySqlResultSet results = new MySqlResultSet();
            results.Error();
            errorCode = DatabaseError.UNKNOWN;

            if (database is MySqlDatabase mysqldb)
            {
                string query = GetQuery(DatabaseOperation.Update);
                MySqlParameters parameters = GetParameters(DatabaseOperation.Update);
                results = mysqldb.SetData(query, parameters) as MySqlResultSet;
                if (results.RowsAffected == 1)
                {
                    results.Pass();
                }
                else
                {
                    results.Fail();
                }
            }

            return results;
        }

        /// <summary>
        /// Update existing data for property.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="results">Results.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Operation success.</returns>
        public virtual bool TryUpdate(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            results = new MySqlResultSet();
            results.Fail();

            try
            {
                this.Update(database, out errorCode);
                results.Pass();
                return true;
            }
            catch (DatabaseOperationException doe)
            {
                results.Error();
                errorCode = doe.ErrorCode;
                return false;
            }
        }
        
        /// <summary>
        /// Delete entry from the database.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Returns a result set.</returns>
        public virtual IResultSet Delete(IWritable database, out DatabaseError errorCode)
        {
            MySqlResultSet results = new MySqlResultSet();
            results.Error();
            errorCode = DatabaseError.UNKNOWN;

            if (database is MySqlDatabase mysqldb)
            {
                string query = GetQuery(DatabaseOperation.Delete);
                MySqlParameters parameters = GetParameters(DatabaseOperation.Delete);
                results = mysqldb.SetData(query, parameters) as MySqlResultSet;
                if (results.RowsAffected == 1)
                {
                    results.Pass();
                }
                else
                {
                    results.Fail();
                }
            }

            return results;
        }

        /// <summary>
        /// Delete existing data for property.
        /// </summary>
        /// <param name="database">Database.</param>
        /// <param name="results">Results.</param>
        /// <param name="errorCode">Error code.</param>
        /// <returns>Operation success.</returns>
        public virtual bool TryDelete(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            results = new MySqlResultSet();
            results.Fail();

            try
            {
                this.Delete(database, out errorCode);
                results.Pass();
                return true;
            }
            catch (DatabaseOperationException doe)
            {
                results.Error();
                errorCode = doe.ErrorCode;
                return false;
            }
        }
    }

    /// <summary>
    /// Student property class.
    /// </summary>
    public abstract class MySqlStudentProperty : MySqlUserProperty, IStudentProperty
    {
        /// <summary>
        /// Student identifier.
        /// </summary>
        public virtual IUIDFormat StudentID
        {
            get { return this.OwnerID; }
            set { this.OwnerID = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of a user property.
        /// </summary>
        /// <param name="type">Type of property.</param>
        /// <param name="userID">User ID.</param>
        public MySqlStudentProperty(UserProperties type, IUIDFormat userID)
            : base(type, userID)
        {
            this.StudentID = userID;
        }
    }

    /// <summary>
    /// Student property class.
    /// </summary>
    public abstract class MySqlFacultyProperty : MySqlUserProperty, IFacultyProperty
    {
        /// <summary>
        /// Student identifier.
        /// </summary>
        public virtual IUIDFormat FacultyID
        {
            get { return this.OwnerID; }
            set { this.OwnerID = value; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of a user property.
        /// </summary>
        /// <param name="type">Type of property.</param>
        /// <param name="userID">User ID.</param>
        public MySqlFacultyProperty(UserProperties type, IUIDFormat userID)
            : base(type, userID)
        {
            this.FacultyID = userID;
        }
    }

    /*

    /// <summary>
    /// Represents a student Capstone.
    /// </summary>
    public class MySqlCapstone : MySqlStudentProperty, ICapstoneModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        private const string SELECT = "SELECT capstoneID, studentID, chairID, capstoneStartTerm, defenseDateApprovedBy, defenseDate, title, abstract, plagiarismScore FROM capstonedb.capstone WHERE capstoneID=@capstoneID;";

        private const string INSERT = "INSERT INTO capstonedb.capstone(studentID, chairID, capstoneStartTerm, defenseDateApprovedBy, defenseDate, title, abstract, plagiarismScore) VALUES (@userID, @chairID, @startTerm, @approverID, @defenseDate, @title, @abstract, @plagiarism);";

        private const string UPDATE = "UPDATE capstonedb.capstone SET chairID=@chairID, capstoneStartTerm=@startTerm, defenseDate=@defenseDate, title=@title, abstract=@abstract, plagiarismScore=@plagiarism WHERE capstoneID=@capstoneID;";
        
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Student identifier. (Same as owner id).
        /// </summary>
        public override IUIDFormat StudentID
        {
            get { return new MySqlID(this["studentID"].Value); }
            set { this["studentID"].SetValue(value.SQLValue); }
        }

        /// <summary>
        /// Capstone identifier.
        /// </summary>
        public IUIDFormat CapstoneID
        {
            get { return new MySqlID(this.GetValue("capstoneID")); }
            set { this.SetValue("capstoneID", value.SQLValue); }
        }

        /// <summary>
        /// Faculty identifier.
        /// </summary>
        public IUIDFormat FacultyID
        {
            get { return new MySqlID(this.GetValue("chairID")); }
            set { this.SetValue("chairID", value.SQLValue); }
        }

        /// <summary>
        /// Capstone start term.
        /// </summary>
        public IUIDFormat CapstoneStartTerm
        {
            get { return new MySqlID(this.GetValue("capstoneStartTerm")); }
            set { this.SetValue("capstoneStartTerm", value.SQLValue); }
        }

        /// <summary>
        /// Title of the capstone.
        /// </summary>
        public string CapstoneTitle {
            get { return this.GetValue("title"); }
            set { this.SetValue("title", value); }
        }

        /// <summary>
        /// Abstract of the capstone.
        /// </summary>
        public string CapstoneAbstract
        {
            get { return this.GetValue("abstract"); }
            set { this.SetValue("abstract", value); }
        }
        
        /// <summary>
        /// Plagiarism score.
        /// </summary>
        public string PlagiarismScore
        {
            get { return this.GetValue("plagiarismScore"); }
            set { this.SetValue("plagiarismScore", value); }
        }

        /// <summary>
        /// Defense date.
        /// </summary>
        public IDateTimeFormat DefenseDate
        {
            get { return new MySqlDateTime(this.GetValue("defenseDate")); }
            set { this.SetValue("defenseDate", value.SQLValue); }
        }

        /// <summary>
        /// UserID of staff/faculty who approved defense date.
        /// </summary>
        public IUIDFormat DefenseDateApproverID
        {
            get { return new MySqlID(this.GetValue("defenseDateApprovedBy")); }
            set { this.SetValue("defenseDateApprovedBy", value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Capstone object.
        /// </summary>
        /// <param name="capstoneID">Capstone identifier.</param>
        /// <param name="studentID">Student identifier.</param>
        /// <param name="chairID">Committee chair.</param>
        /// <param name="startTerm">Start term of capstone.</param>
        /// <param name="approverID">Approver of defense date.</param>
        /// <param name="defenseDate">Defense date requested.</param>
        /// <param name="title">Title.</param>
        /// <param name="abstr">Abstract.</param>
        /// <param name="plagiarism">Plagiarism.</param>
        public MySqlCapstone(IUIDFormat capstoneID, IUIDFormat studentID, IUIDFormat chairID, IDateTimeFormat startTerm, IUIDFormat approverID, IDateTimeFormat defenseDate, string title, string abstr, string plagiarism)
            : base(UserProperties.Capstone, studentID)
        {
            this.Model.Add(new MySqlEntry("capstoneID", capstoneID.SQLValue));
            this.Model.Add(new MySqlEntry("studentID", studentID.SQLValue));
            this.Model.Add(new MySqlEntry("chairID", chairID.SQLValue));
            this.Model.Add(new MySqlEntry("capstoneStartTerm", startTerm.SQLValue));
            this.Model.Add(new MySqlEntry("defenseDateApprovedBy", approverID.SQLValue));
            this.Model.Add(new MySqlEntry("defenseDate", defenseDate.SQLValue));
            this.Model.Add(new MySqlEntry("title", title));
            this.Model.Add(new MySqlEntry("abstract", abstr));
            this.Model.Add(new MySqlEntry("plagiarismScore", plagiarism));
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        public bool HasAbstract()
        {
            throw new NotImplementedException();
        }

        public bool HasChair()
        {
            throw new NotImplementedException();
        }

        public bool HasDefenseDate()
        {
            throw new NotImplementedException();
        }

        public bool HasDefenseDateApproval()
        {
            throw new NotImplementedException();
        }

        public bool HasTitle()
        {
            throw new NotImplementedException();
        }

        public bool IsActive()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Query for execution.
        /// </summary>
        /// <param name="operation">Operation to get query for.</param>
        /// <returns>Returns string.</returns>
        protected override string GetQuery(DatabaseOperation operation)
        {
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return SELECT;
                case DatabaseOperation.Create:
                    return INSERT;
                case DatabaseOperation.Update:
                    return UPDATE;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Parameters for execution.
        /// </summary>
        /// <param name="operation">Operation to get parameters for.</param>
        /// <returns>Returns parameters.</returns>
        protected override MySqlParameters GetParameters(DatabaseOperation operation)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue }
                    };
                case DatabaseOperation.Create:
                    return new Dictionary<string, string> {
                        { "@userID", this.StudentID.SQLValue },
                        { "@chairID", this.FacultyID.SQLValue },
                        { "@startTerm", this.CapstoneStartTerm.SQLValue },
                        { "@approverID", this.DefenseDateApproverID.SQLValue },
                        { "@title", this.CapstoneTitle },
                        { "@abstract", this.CapstoneAbstract },
                        { "@plagiarism", this.PlagiarismScore }
                    };
                case DatabaseOperation.Update:
                    return new Dictionary<string, string> {
                        { "@capstoneID", this.CapstoneID.SQLValue },
                        { "@userID", this.StudentID.SQLValue },
                        { "@chairID", this.FacultyID.SQLValue },
                        { "@startTerm", this.CapstoneStartTerm.SQLValue },
                        { "@approverID", this.DefenseDateApproverID.SQLValue },
                        { "@title", this.CapstoneTitle },
                        { "@abstract", this.CapstoneAbstract },
                        { "@plagiarism", this.PlagiarismScore }
                    };
            }
            return parameters;
        }

        /// <summary>
        /// Set data from fetch.
        /// </summary>
        /// <param name="results">Results to set.</param>
        protected override void Set(IResultSet results)
        {
            // Assign values based on the results object.
            if (results != null)
            {
                this.CapstoneID = new MySqlID(results[0, "capstoneID"].Value);
                this.StudentID = new MySqlID(results[0, "studentID"].Value);
                this.FacultyID = new MySqlID(results[0, "chairID"].Value);
                this.CapstoneStartTerm = new MySqlID(results[0, "capstoneStartTerm"].Value);
                this.DefenseDateApproverID = new MySqlID(results[0, "defenseDateApprovedBy"].Value);
                this.DefenseDate = new MySqlDateTime(results[0, "defenseDate"].Value);
                this.CapstoneTitle = results[0, "title"].Value;
                this.CapstoneAbstract = results[0, "abstract"].Value;
                this.PlagiarismScore = results[0, "plagiarismScore"].Value;
            }
        }
    }

    /// <summary>
    /// Represents individual entries from the role code table in the database.
    /// </summary>
    public class MySqlRole : MySqlUserProperty, IRoleModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        private const string SELECT = "SELECT userID, roleCode, currentStatus, statusTimestamp FROM capstonedb.userroles WHERE userID=@userID;";

        private const string INSERT = "INSERT INTO capstonedb.userroles(userID, roleCode, currentStatus, statusTimestamp) VALUES (@userID, @roleCode, @statusCode, @timeStamp);";

        private const string UPDATE = "UPDATE capstonedb.userroles SET roleCode=@roleCode, currentStatus=@statusCode, statusTimestamp=@timeStamp WHERE userID=@userID;";

        /// <summary>
        /// Cached result of the inactive status code.
        /// </summary>
        private static int InactiveStatusCode = -1;

        /// <summary>
        /// Retrieves the status code for the 'INACTIVE' role status.
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        private static int GetInactiveStatusCode(IReadable database)
        {
            if (InactiveStatusCode != -1) { return InactiveStatusCode; }
            MySqlStatus status = MySqlStatuses.GetInstance(database).FindByName("INACTIVE") as MySqlStatus;
            if (status.TryFetch(database, out IResultSet results, out DatabaseError errorCode))
            {
                return status.Code.Value;
            }
            // If status code can't be found:
            return (InactiveStatusCode = -1);
        }
        
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Role code.
        /// </summary>
        public IUIDFormat RoleCode
        {
            get { return new MySqlID(this.GetValue("roleCode")); }
            set { this.SetValue("roleCode", value.SQLValue); }
        }

        /// <summary>
        /// Most recent status's status code.
        /// </summary>
        public IUIDFormat StatusCode
        {
            get { return new MySqlID(this.GetValue("currentStatus")); }
            set { this.SetValue("currentStatus", value.SQLValue); }
        }

        /// <summary>
        /// Timestamp of the most recent status.
        /// </summary>
        public IDateTimeFormat Timestamp
        {
            get { return new MySqlDateTime(this.GetValue("statusTimestamp")); }
            set { this.SetValue("statusTimestamp", value.SQLValue); }
        }

        /// <summary>
        /// Determine if role is valid.
        /// </summary>
        public bool IsValid(IReadable database)
        {
            return (this.StatusCode.Value != MySqlRole.GetInactiveStatusCode(database));
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of a role user property.
        /// </summary>
        /// <param name="userID">Property owner.</param>
        /// <param name="roleCode">Role code.</param>
        /// <param name="statusCode">Status code.</param>
        /// <param name="timestamp">Timestamp of status.</param>
        public MySqlRole(IUIDFormat userID, IUIDFormat roleCode, IUIDFormat statusCode, IDateTimeFormat timestamp)
            : base(UserProperties.Role, userID)
        {
            this.Model.Add(new MySqlEntry("roleCode", roleCode.SQLValue));
            this.Model.Add(new MySqlEntry("currentStatus", statusCode.SQLValue));
            this.Model.Add(new MySqlEntry("statusTimestamp", timestamp.SQLValue));
        }

        //////////////////////
        // Method(s).
        //////////////////////
        
        //////////////////////
        // Accessor method(s).
        
        /// <summary>
        /// Query for execution.
        /// </summary>
        /// <param name="operation">Operation to get query for.</param>
        /// <returns>Returns string.</returns>
        protected override string GetQuery(DatabaseOperation operation)
        {
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return SELECT;
                case DatabaseOperation.Create:
                    return INSERT;
                case DatabaseOperation.Update:
                    return UPDATE;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Parameters for execution.
        /// </summary>
        /// <param name="operation">Operation to get parameters for.</param>
        /// <returns>Returns parameters.</returns>
        protected override MySqlParameters GetParameters(DatabaseOperation operation)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue }
                    };
                case DatabaseOperation.Create:
                case DatabaseOperation.Update:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@roleCode", this.RoleCode.SQLValue },
                        { "@statusCode", this.StatusCode.SQLValue },
                        { "@timeStamp", this.Timestamp.SQLValue }
                    };
            }
            return parameters;
        }

        //////////////////////
        // Mutator method(s).
        
        /// <summary>
        /// Used to set data after fetching.
        /// </summary>
        /// <param name="results">Results of the fetch.</param>
        protected override void Set(IResultSet results)
        {
            // Assign values based on the results object.
            if (results != null)
            {
                this.RoleCode = new MySqlID(results[0, "roleCode"].Value);
                this.StatusCode = new MySqlID(results[0, "statusCode"].Value);
                this.Timestamp = new MySqlDateTime(results[0, "roleCode"].Value);
            }
        }   
    }

    /// <summary>
    /// Email property.
    /// </summary>
    public class MySqlEmail : MySqlUserProperty, IEmailModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        private const string SELECT = "SELECT userID, email, emailType FROM capstonedb.useremails WHERE userID=@userID;";

        private const string INSERT = "INSERT INTO capstonedb.useremails(userID, email, emailType) VALUES (@userID, @email, @emailType);";

        private const string UPDATE = "UPDATE capstonedb.useremails SET email=@email, emailType=@emailType WHERE userID=@userID;";

        private const string DELETE = "DELETE FROM capstonedb.useremails WHERE userID=@userID AND email=@email;";

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Email address.
        /// </summary>
        public string Email
        {
            get { return this.GetValue("email"); }
            set { this.SetValue("email", value); }
        }

        /// <summary>
        /// Email type code.
        /// </summary>
        public IUIDFormat EmailType
        {
            get { return new MySqlID(this.GetValue("emailType")); }
            set { this.SetValue("emailType", value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of an email user property.
        /// </summary>
        /// <param name="userID">Property owner.</param>
        /// <param name="emailAddress">Email address.</param>
        /// <param name="emailType">Email type code.</param>
        public MySqlEmail(IUIDFormat userID, string emailAddress, IUIDFormat emailType)
            : base(UserProperties.Email, userID)
        {
            this.Model.Add(new MySqlEntry("email", emailAddress));
            this.Model.Add(new MySqlEntry("emailType", emailType.SQLValue));
        }

        /// <summary>
        /// Query for execution.
        /// </summary>
        /// <param name="operation">Operation to get query for.</param>
        /// <returns>Returns string.</returns>
        protected override string GetQuery(DatabaseOperation operation)
        {
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return SELECT;
                case DatabaseOperation.Create:
                    return INSERT;
                case DatabaseOperation.Update:
                    return UPDATE;
                case DatabaseOperation.Delete:
                    return DELETE;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Parameters for execution.
        /// </summary>
        /// <param name="operation">Operation to get parameters for.</param>
        /// <returns>Returns parameters.</returns>
        protected override MySqlParameters GetParameters(DatabaseOperation operation)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue }
                    };
                case DatabaseOperation.Create:
                    return new Dictionary<string, string>
                    {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@email", this.Email },
                        { "@emailType", this.EmailType.SQLValue }
                    };
                case DatabaseOperation.Update:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@email", this.Email },
                        { "@emailType", this.EmailType.SQLValue }
                    };
                case DatabaseOperation.Delete:
                    return new Dictionary<string, string>
                    {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@email", this.Email },
                    };
            }
            return parameters;
        }

        /// <summary>
        /// Used to set data after fetching.
        /// </summary>
        /// <param name="results">Results of the fetch.</param>
        protected override void Set(IResultSet results)
        {
            // Assign values based on the results object.
            if (results != null)
            {
                this.Email = (string) new MySqlID(results[0, "email"].Value);
                this.EmailType = new MySqlID(results[0, "emailType"].Value);
            }
        }
    }

    /// <summary>
    /// Phone number property.
    /// </summary>
    public class MySqlPhoneNumber : MySqlUserProperty, IPhoneNumberModel
    {
        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        private const string SELECT = "SELECT userID, number, phoneType FROM capstonedb.userphones WHERE userID=@userID;";

        private const string INSERT = "INSERT INTO capstonedb.userphones(userID, number, phoneType) VALUES (@userID, @phoneNumber, @phoneType);";

        private const string UPDATE = "UPDATE capstonedb.useremails SET number=@phoneNumber, phoneType=@phoneType WHERE userID=@userID;";

        private const string DELETE = "DELETE FROM capstonedb.userphones WHERE userID=@userID AND number=@phoneNumber;";

        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Reference to the phone number.
        /// </summary>
        public string PhoneNumber
        {
            get { return this.GetValue("number"); }
            set { this.SetValue("number", new String(value.Where(Char.IsDigit).ToArray())); }
        }

        /// <summary>
        /// Phone type code.
        /// </summary>
        public IUIDFormat PhoneType
        {
            get { return new MySqlID(this.GetValue("phoneType")); }
            set { this.SetValue("phoneType", value.SQLValue); }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of a phone number user property.
        /// </summary>
        /// <param name="userID">Property owner.</param>
        /// <param name="phoneNumber">Phone number.</param>
        /// <param name="phoneType">Phone type code.</param>
        public MySqlPhoneNumber(IUIDFormat userID, string phoneNumber, IUIDFormat phoneType)
            : base(UserProperties.PhoneNumber, userID)
        {
            this.Model.Add(new MySqlEntry("number", phoneNumber));
            this.Model.Add(new MySqlEntry("phoneType", phoneType.SQLValue));
        }

        //////////////////////
        // Method(s).
        //////////////////////

        /// <summary>
        /// Get parameters for query execution.
        /// </summary>
        /// <param name="operation">Operation to be run.</param>
        /// <returns>Returns parameters.</returns>
        protected override MySqlParameters GetParameters(DatabaseOperation operation)
        {
            MySqlParameters parameters = new Dictionary<string, string>();
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue },
                    };
                case DatabaseOperation.Create:
                case DatabaseOperation.Update:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@phoneNumber", this.PhoneNumber },
                        { "@phoneType", this.PhoneType.SQLValue }
                    };
                case DatabaseOperation.Delete:
                    return new Dictionary<string, string> {
                        { "@userID", this.OwnerID.SQLValue },
                        { "@phoneNumber", this.PhoneNumber }                     
                    };
            }
            return parameters;
        }

        /// <summary>
        /// Get query for execution.
        /// </summary>
        /// <param name="operation">Operation to be run.</param>
        /// <returns>Returns a string.</returns>
        protected override string GetQuery(DatabaseOperation operation)
        {
            switch (operation)
            {
                case DatabaseOperation.Read:
                    return SELECT;
                case DatabaseOperation.Create:
                    return INSERT;
                case DatabaseOperation.Update:
                    return UPDATE;
                case DatabaseOperation.Delete:
                    return DELETE;
                default:
                    return "";
            }
        }

        /// <summary>
        /// Sets data from fetch request.
        /// </summary>
        /// <param name="results">Results from fetch.</param>
        protected override void Set(IResultSet results)
        {
            // Assign values based on the results object.
            if (results != null)
            {
                this.PhoneNumber = (string) new MySqlID(results[0, "number"].Value);
                this.PhoneType = new MySqlID(results[0, "phoneType"].Value);
            }
        }
    }

    */


}

