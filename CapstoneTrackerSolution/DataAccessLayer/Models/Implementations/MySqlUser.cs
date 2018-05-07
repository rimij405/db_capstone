/*
    Users.cs
    ---
    Ian Effendi
    Jake Toporoff
 */

using ISTE.DAL.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements.
using ISTE.DAL.Database.Implementations;
using ISTE.DAL.Models.Interfaces;
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Models.Implementations
{

    /// <summary>
    /// Represents a user model.
    /// </summary>
    public class MySqlUser : MySqlDatabaseObject, IUserModel
    {

        //////////////////////
        // Static Member(s).
        //////////////////////

        //////////////////////
        // Static field(s).

        //////////////////////
        // Static method(s).

        /// <summary>
        /// Create a row filled with the appropriate user fields.
        /// </summary>
        /// <returns>Returns a row.</returns>
        private static List<IEntry> CreateUserModel()
        {
            return new List<IEntry>() {
                new MySqlEntry("userID"),
                new MySqlEntry("username"),
                new MySqlEntry("password"),
                new MySqlEntry("firstName"),
                new MySqlEntry("lastName")
            };
        }

        /// <summary>
        /// Create a row filled with the appropriate user fields AND data.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <returns>Returns a populated row.</returns>
        private static List<IEntry> CreateUserModel(string userID, string username, string password, string firstName, string lastName)
        {
            return new List<IEntry>() {
                  new MySqlEntry("userID", userID),
                  new MySqlEntry("username", username),
                  new MySqlEntry("password", password),
                  new MySqlEntry("firstName", firstName),
                  new MySqlEntry("lastName", lastName)
            };
        }
                      
        //////////////////////
        // Conversion operators.

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Internal list of user properties.
        /// </summary>
        private List<IUserProperty> properties;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to the user ID.
        /// </summary>
        public IUIDFormat UserID
        {
            get
            {
                return new MySqlID(this.GetValue("userID"));
            }
            set
            {
                this.SetValue("userID", value.SQLValue);
            }
        }

        /// <summary>
        /// Username.
        /// </summary>
        public string Username
        {
            get
            {
                return this.GetValue("username");
            }
            set
            {
                this.SetValue("username", value);
            }
        }

        /// <summary>
        /// User password.
        /// </summary>
        public string Password
        {
            get
            {
                return this.GetValue("password");
            }
            set
            {
                this.SetValue("password", value);
            }
        }

        /// <summary>
        /// First name.
        /// </summary>
        public string FirstName
        {
            get
            {
                return this.GetValue("firstName");
            }
            set
            {
                this.SetValue("firstName", value);
            }
        }

        /// <summary>
        /// Last name.
        /// </summary>
        public string LastName
        {
            get
            {
                return this.GetValue("lastName");
            }
            set
            {
                this.SetValue("lastName", value);
            }
        }

        /// <summary>
        /// Collection of user properties.
        /// </summary>
        public List<IUserProperty> Properties
        {
            get
            {
                if (this.properties == null)
                {
                    this.properties = new List<IUserProperty>();
                }
                return this.properties;
            }
        }

        /// <summary>
        /// Collection of phone numbers.
        /// </summary>
        public List<IPhoneNumberModel> PhoneNumbers
        {
            get
            {
                return this.GetProperties<IPhoneNumberModel>();
            }
        }

        /// <summary>
        /// Collection of emails.
        /// </summary>
        public List<IEmailModel> Emails
        {
            get
            {
                return this.GetProperties<IEmailModel>();
            }
        }

        /// <summary>
        /// Collection of user's roles.
        /// </summary>
        public List<IRoleModel> Roles
        {
            get
            {
                return this.GetProperties<IRoleModel>();
            }
        }

        //////////////////////
        // Indexer(s).

        //////////////////////
        // Constructor(s).
        //////////////////////

        /// <summary>
        /// Creation of an empty user.
        /// </summary>
        public MySqlUser()
            : this(new MySqlID(null), "", "", "", "")
        { }

        /// <summary>
        /// Creation of a user with only the username and the password.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        public MySqlUser(MySqlID userID, string username, string password)
            : this(userID, username, password, "", "")
        { }

        /// <summary>
        /// Creation of a user with all possible information.
        /// </summary>
        /// <param name="userID">User ID.</param>
        /// <param name="username">Username.</param>
        /// <param name="password">Password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="properties">Various properties to set.</param>
        public MySqlUser(MySqlID userID, string username, string password, string firstName, string lastName, params IUserProperty[] properties)
        {
            this.Model = MySqlUser.CreateUserModel();
            this.UserID = userID;
            this.Username = username;
            this.Set(password, firstName, lastName, properties);
        }

        /// <summary>
        /// Clone user from other user.
        /// </summary>
        /// <param name="other">User to clone from.</param>
        public MySqlUser(MySqlUser other)
        {
            if (other != null && other != this)
            {
                this.UserID = other.UserID;
                this.Username = other.Username;
                this.Password = other.Password;
                this.FirstName = other.FirstName;
                this.LastName = other.LastName;

                // Deep copy properties.
                IUserProperty[] props = new IUserProperty[other.Properties.Count];
                other.properties.CopyTo(props);
                this.properties = props.ToList<IUserProperty>();
            }
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Return true if user has role and it's current.
        /// </summary>
        /// <param name="roleCode">Role to check.</param>
        /// <returns>Returns true if role is in the database AND has a valid status.</returns>
        public bool HasRole(string roleCode)
        {
            foreach (IRoleModel role in this.Roles)
            {
                if ((role.RoleCode is MySqlID code))
                {
                    if (code.SQLValue.ToUpper() == roleCode.ToUpper())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Determine if user has input property value.
        /// </summary>
        /// <param name="email">Value of property to check.</param>
        /// <returns>Return true if property and its value is found.</returns>
        public bool HasEmail(string email)
        {
            foreach (IEmailModel e in this.Emails)
            {
                if (e.Email.Trim().ToUpper() == email.Trim().ToUpper())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Determine if user has input property value.
        /// </summary>
        /// <param name="phoneNumber">Value of property to check.</param>
        /// <returns>Return true if property and its value is found.</returns>
        public bool HasPhoneNumber(string phoneNumber)
        {
            foreach (IPhoneNumberModel phone in this.PhoneNumbers)
            {
                string a = new string(phone.PhoneNumber.Where(Char.IsDigit).ToArray());
                string b = new String(phoneNumber.Where(Char.IsDigit).ToArray());
                if (a == b) { return true; }
            }
            return false;
        }
        
        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Retrieve collection of user properties that match the type.
        /// </summary>
        /// <param name="propertyType">Type of properties to return.</param>
        /// <returns>Returns collection of properties.</returns>
        public List<IUserProperty> GetProperties(UserProperties propertyType)
        {
            List<IUserProperty> selection = new List<IUserProperty>();
            foreach (IUserProperty property in properties)
            {
                if (property.PropertyType == propertyType)
                {
                    selection.Add(property);
                }
            }
            return selection;
        }

        /// <summary>
        /// Retrieve collection of user properties that match the type.
        /// </summary>
        /// <typeparam name="TUserProperty">Type of properties to return.</typeparam>
        /// <returns>Returns collection of properties.</returns>
        public List<TUserProperty> GetProperties<TUserProperty>() where TUserProperty : IUserProperty
        {
            List<TUserProperty> selection = new List<TUserProperty>();
            foreach (IUserProperty property in properties)
            {
                if (property is TUserProperty prop)
                {
                    selection.Add(prop);
                }
            }
            return selection;
        }

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set all non-primary key information.
        /// </summary>
        /// <param name="password">User password.</param>
        /// <param name="firstName">First name.</param>
        /// <param name="lastName">Last name.</param>
        /// <param name="properties">Various properties to set.</param>
        /// <returns>Returns reference to self.</returns>
        public IUserModel Set(string password, string firstName, string lastName, params IUserProperty[] properties)
        {
            this.Password = password;
            this.FirstName = firstName;
            this.LastName = lastName;
            foreach (IUserProperty property in properties)
            {
                this.AddProperty(property.PropertyType, property);
            }
            return this;
        }

        public IResultSet Fetch(IReadable database, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public bool TryFetch(IReadable database, out IResultSet results, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public IResultSet Insert(IWritable database, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public bool TryInsert(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public IResultSet Delete(IWritable database, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public bool TryDelete(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public IResultSet Update(IWritable database, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }

        public bool TryUpdate(IWritable database, out IResultSet results, out DatabaseError errorCode)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// Adds a property to the collection.
        /// </summary>
        /// <typeparam name="TUserProperty">Property type.</typeparam>
        /// <param name="property">Property being added.</param>
        public void AddProperty<TUserProperty>(TUserProperty property) where TUserProperty : IUserProperty
        {
            if (property != null) {
                this.AddProperty(property.PropertyType, property);
            }
        }

        /// <summary>
        /// Adds a property to the collection.
        /// </summary>
        /// <param name="propertyType">Property type.</param>
        /// <param name="property">Property being added.</param>
        public void AddProperty(UserProperties propertyType, IUserProperty property)
        {
            if (property != null)
            {
                switch (propertyType)
                {
                    case UserProperties.Capstone:
                        if (this is IStudentModel student)
                        {
                            student.AddCapstone((ICapstoneModel)property);
                        }
                        break;
                    case UserProperties.Email:
                        this.AddEmail((IEmailModel)property);
                        break;
                    case UserProperties.PhoneNumber:
                        this.AddPhoneNumber((IPhoneNumberModel)property);
                        break;
                    case UserProperties.Role:
                        this.AddRole((IRoleModel)property);
                        break;
                }
            }
        }

        /// <summary>
        /// Remove a property if it exists.
        /// </summary>
        /// <typeparam name="TUserProperty">A user-owned property that can be affected.</typeparam>
        /// <param name="property">Property to add or remove.</param>
        /// <returns>Returns the removed property.</returns>
        public TUserProperty RemoveProperty<TUserProperty>(TUserProperty property) where TUserProperty : IUserProperty
        {
            if (property != null)
            {
                return (TUserProperty)this.RemoveProperty(property.PropertyType, property);
            }
            return default(TUserProperty);
        }

        /// <summary>
        /// Remove a property if it exists.
        /// </summary>
        /// <param name="propertyType">Type of property being removed.</param>
        /// <param name="property">Property to add or remove.</param>
        /// <returns>Returns the removed property.</returns>
        public IUserProperty RemoveProperty(UserProperties propertyType, IUserProperty property)
        {
            if (property != null)
            {
                foreach (IUserProperty prop in this.GetProperties(propertyType))
                {
                    switch (propertyType)
                    {
                        case UserProperties.Capstone:
                            if (this is IStudentModel student)
                            {
                                throw new NotImplementedException();
                                // student.UpdateCapstone((ICapstoneModel)property);
                            }
                            return null;
                        case UserProperties.Email:
                            return this.RemoveEmail((IEmailModel)property);
                        case UserProperties.PhoneNumber:
                            return this.RemovePhoneNumber((IPhoneNumberModel)property);
                        case UserProperties.Role:
                            throw new NotImplementedException();
                            // this.RemoveRole((IRoleModel)property);
                    }
                }
            }
            return null;
        }
        
        public void AddEmail(IEmailModel email)
        {
            throw new NotImplementedException();
        }

        public void AddPhoneNumber(IPhoneNumberModel phoneNumber)
        {
            throw new NotImplementedException();
        }

        public void AddRole(IRoleModel role)
        {
            throw new NotImplementedException();
        }

        public IEmailModel RemoveEmail(IEmailModel email)
        {
            throw new NotImplementedException();
        }

        public IPhoneNumberModel RemovePhoneNumber(IPhoneNumberModel phoneNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateEmail(IEmailModel email)
        {
            throw new NotImplementedException();
        }

        public bool UpdatePhoneNumber(IPhoneNumberModel phoneNumber)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRole(IRoleModel role)
        {
            throw new NotImplementedException();
        }
        
        /*
        string userId;
        string username;
        string password;
        string firstName;
        string lastName;
        MySqlDatabase sql = new MySqlDatabase(new MySqlConfiguration());

        public User(string uid, string un, string pw, string fn, string ln)
        {
            userId = uid;
            username = un;
            password = pw;
            firstName = fn;
            lastName = ln;
        }

        public MySqlResultSet Fetch(MySqlDatabase sqlDb)
        {
            string sqlStr = "SELECT * FROM Users WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.GetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            userId = rs[0, "userID"].Value;
            username = rs[0, "username"].Value;
            password = rs[0, "password"].Value;
            firstName = rs[0, "firstName"].Value;
            lastName = rs[0, "lastName"].Value;

            return rs;
        }

        // Update existing record with new information at given ID
        public MySqlResultSet Put(MySqlDatabase sqlDb)
        {
            string sqlStr = "UPDATE Users SET username = @username, password = @password, firstName = @firstName, " +
                "lastName = @lastName WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@username", username },
                {"@password", password },
                {"@firstName", firstName },
                {"@lastName", lastName },
                {"@userId", userId }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Add new value to database
        public MySqlResultSet Post(MySqlDatabase sqlDb)
        {
            string sqlStr = "INSERT INTO Users(userID, username, " +
                "password, firstName, lastName) VALUES(@userId, @username, @password, " +
                "@firstName, @lastName);";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId},
                {"@username", username },
                {"@password", password },
                {"@firstName", firstName },
                {"@lastName", lastName }
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }

        // Delete record from database at given ID
        public MySqlResultSet Delete(MySqlDatabase sqlDb)
        {
            string sqlStr = "DELETE FROM Users WHERE userID = @userId;";
            MySqlParameters parameters = new Dictionary<string, string>
            {
                {"@userId", userId}
            };
            MySqlResultSet rs = sqlDb.SetData(sqlStr, parameters.Dictionary) as MySqlResultSet;
            return rs;
        }
        */
    }
}
