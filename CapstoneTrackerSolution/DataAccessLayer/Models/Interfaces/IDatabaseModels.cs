/*
    IDatabaseModels.cs
    ---
    Ian Effendi
 */

 // using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// additional using statements.
using ISTE.DAL.Database.Interfaces;

namespace ISTE.DAL.Models.Interfaces
{    
    /// <summary>
    /// Things that can be associated with a user.
    /// </summary>
    public enum UserProperties
    {
        /// <summary>
        /// User email.
        /// </summary>
        Email,

        /// <summary>
        /// User phone number.
        /// </summary>
        PhoneNumber,

        /// <summary>
        /// User role.
        /// </summary>
        Role,

        /// <summary>
        /// Capstone property. (Only available to students).
        /// </summary>
        Capstone
    }

    /// <summary>
    /// Properties associated with a particular user ID.
    /// </summary>
    public interface IUserProperty
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Metadata defining this type of property.
        /// </summary>
        UserProperties PropertyType
        {
            get;
        }

        /// <summary>
        /// Owner associated with this property.
        /// </summary>
        IUIDFormat OwnerID
        {
            get;
        }
    }

    /// <summary>
    /// Properties associated with a student.
    /// </summary>
    public interface IStudentProperty
    { 
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Owner associated with this property.
        /// </summary>
        IUIDFormat StudentID
        {
            get;
        }
    }

    /// <summary>
    /// Properties associated with a faculty member.
    /// </summary>
    public interface IFacultyProperty
    { 
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Owner associated with this property.
        /// </summary>
        IUIDFormat FacultyID
        {
            get;
        }
    }

    /// <summary>
    /// Represents a capstone object.
    /// </summary>
    public interface ICapstoneModel : IUserProperty, IStudentProperty, IFacultyProperty, IDatabaseObjectModel, IDatabaseObjectReader, IDatabaseObjectInserter, IDatabaseObjectUpdater
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// ID of the capstone.
        /// </summary>
        IUIDFormat CapstoneID
        {
            get;
        }

        // StudentID covered by IStudentProperty.
        // FacultyID covered by IFacultyProperty.

        /// <summary>
        /// Represents the start term of the capstone.
        /// </summary>
        IUIDFormat CapstoneStartTerm
        {
            get;
            set;
        }

        /// <summary>
        /// Title of the capstone.
        /// </summary>
        string CapstoneTitle
        {
            get;
            set;
        }

        /// <summary>
        /// Capstone abstract.
        /// </summary>
        string CapstoneAbstract
        {
            get;
            set;
        }

        /// <summary>
        /// Plagiarism score.
        /// </summary>
        string PlagiarismScore
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the capstone defense date.
        /// </summary>
        IDateTimeFormat DefenseDate
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the approver of the defense date.
        /// </summary>
        IUIDFormat DefenseDateApproverID
        {
            get;
            set;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check property.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool HasDefenseDate();

        /// <summary>
        /// Check property.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool HasDefenseDateApproval();

        /// <summary>
        /// Check property.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool HasTitle();

        /// <summary>
        /// Check property.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool HasAbstract();

        /// <summary>
        /// Check property.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool HasChair();

        /// <summary>
        /// Check property status of the capstone.
        /// </summary>
        /// <returns>Return boolean designating state.</returns>
        bool IsActive();
        
    }

    /// <summary>
    /// Collection of followers for a capstone.
    /// </summary>
    public interface ICapstoneFollowers
    {
        /// <summary>
        /// ID of the capstone.
        /// </summary>
        IUIDFormat CapstoneID
        {
            get;
            set;
        }

        /// <summary>
        /// Collection of followers.
        /// </summary>
        List<ICapstoneFollowerModel> Followers
        {
            get;
        }
    }

    /// <summary>
    /// Represents a capstone follower object.
    /// </summary>
    public interface ICapstoneFollowerModel
    {
        /// <summary>
        /// ID of the capstone.
        /// </summary>
        IUIDFormat CapstoneID
        {
            get;
            set;
        }

        /// <summary>
        /// ID of the user.
        /// </summary>
        IUIDFormat UserID
        {
            get;
            set;
        }

        /// <summary>
        /// Is the user tracking this capstone?
        /// </summary>
        IFlagFormat TrackingCapstone
        {
            get;
            set;
        }

        /// <summary>
        /// Is the user on the committee?
        /// </summary>
        IFlagFormat OnCommittee
        {
            get;
            set;
        }

        /// <summary>
        /// Represents a timestamped status.
        /// </summary>
        IStatusEventModel StatusEvent
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a student.
    /// </summary>
    public interface IStudentModel : IUserModel
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Starting term of the student's masters degree program.
        /// </summary>
        ITermModel MastersStart
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the currently active capstone for a student.
        /// </summary>
        ICapstoneModel ActiveCapstone
        {
            get;
            set;
        }

        /// <summary>
        /// Reference to all capstones a student may have.
        /// </summary>
        List<ICapstoneModel> AllCapstones
        {
            get;
            set;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check if student has a capstone.
        /// </summary>
        /// <returns>Capstones must exist.</returns>
        bool HasCapstones();

        /// <summary>
        /// Check if the student has a capstone and if it hasn't been cancelled.
        /// </summary>
        /// <returns>Capstone must exist AND be active.</returns>
        bool HasActiveCapstone();
        
        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return the masters start term of the student.
        /// </summary>
        /// <returns>Returns term model.</returns>
        ITermModel GetMastersStart();

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Set the masters start term of the student.
        /// </summary>
        /// <param name="term">Term to set.</param>
        void SetMastersStart(ITermModel term);

        /// <summary>
        /// Add a new capstone to the student. (Cannot be added while there is an active capstone).
        /// </summary>
        /// <param name="capstone">Capstone to add.</param>
        void AddCapstone(ICapstoneModel capstone);

        /// <summary>
        /// Update capstone.
        /// </summary>
        /// <param name="capstone">Capstone to update.</param>
        void UpdateCapstone(ICapstoneModel capstone);
    }

    /// <summary>
    /// Represents the data Users will have.
    /// </summary>
    public interface IUserModel : IDatabaseObjectModel, IDatabaseObjectReader, IDatabaseObjectInserter, IDatabaseObjectUpdater, IDatabaseObjectDeleter
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Unique identifier associated with a user.
        /// </summary>
        IUIDFormat UserID
        {
            get;
        }

        /// <summary>
        /// Unique identifier associated with a user.
        /// </summary>
        string Username
        {
            get;
        }

        /// <summary>
        /// Password associated with the user.
        /// </summary>
        string Password
        {
            get;
        }

        /// <summary>
        /// First name of the user.
        /// </summary>
        string FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Last name of the user.
        /// </summary>
        string LastName
        {
            get;
            set;
        }
        
        /// <summary>
        /// Collection of user properties.
        /// </summary>
        List<IUserProperty> Properties
        {
            get;
        }

        /// <summary>
        /// Collection of phone numbers.
        /// </summary>
        List<IPhoneNumberModel> PhoneNumbers
        {
            get;
        }

        /// <summary>
        /// Collection of emails.
        /// </summary>
        List<IEmailModel> Emails
        {
            get;
        }

        /// <summary>
        /// Collection of user roles.
        /// </summary>
        List<IRoleModel> Roles
        {
            get;
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
        bool HasRole(string roleCode);

        /// <summary>
        /// Determine if user has input property value.
        /// </summary>
        /// <param name="email">Value of property to check.</param>
        /// <returns>Return true if property and its value is found.</returns>
        bool HasEmail(string email);

        /// <summary>
        /// Determine if user has input property value.
        /// </summary>
        /// <param name="phoneNumber">Value of property to check.</param>
        /// <returns>Return true if property and its value is found.</returns>
        bool HasPhoneNumber(string phoneNumber);

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Retrieve collection of user properties that match the type.
        /// </summary>
        /// <param name="propertyType">Type of properties to return.</param>
        /// <returns>Returns collection of properties.</returns>
        List<IUserProperty> GetProperties(UserProperties propertyType);

        /// <summary>
        /// Retrieve collection of user properties that match the type.
        /// </summary>
        /// <typeparam name="TUserProperty">Type of properties to return.</typeparam>
        /// <returns>Returns collection of properties.</returns>
        List<TUserProperty> GetProperties<TUserProperty>() where TUserProperty : IUserProperty;

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
        IUserModel Set(string password, string firstName, string lastName, params IUserProperty[] properties);

        /// <summary>
        /// Add a property to the collection of properties.
        /// </summary>
        /// <param name="property">Property to add.</param>
        void AddProperty<TUserProperty>(TUserProperty property) where TUserProperty : IUserProperty;

        /// <summary>
        /// Add a property to the collection of properties.
        /// </summary>
        /// <param name="propertyType">Type of property being added.</param>
        /// <param name="property">Property to add.</param>
        void AddProperty(UserProperties propertyType, IUserProperty property);

        /// <summary>
        /// Remove a property if it exists.
        /// </summary>
        /// <typeparam name="TUserProperty">A user-owned property that can be affected.</typeparam>
        /// <param name="property">Property to add or remove.</param>
        /// <returns>Returns the removed property.</returns>
        TUserProperty RemoveProperty<TUserProperty>(TUserProperty property) where TUserProperty : IUserProperty;
        
        /// <summary>
        /// Remove a property if it exists.
        /// </summary>
        /// <param name="propertyType">Type of property being removed.</param>
        /// <param name="property">Property to add or remove.</param>
        /// <returns>Returns the removed property.</returns>
        IUserProperty RemoveProperty(UserProperties propertyType, IUserProperty property);

        /// <summary>
        /// Add an email to the user.
        /// </summary>
        /// <param name="email">Email to add.</param>
        void AddEmail(IEmailModel email);

        /// <summary>
        /// Add a phone number to the user.
        /// </summary>
        /// <param name="phoneNumber">Phone number to add.</param>
        void AddPhoneNumber(IPhoneNumberModel phoneNumber);

        /// <summary>
        /// Add a role to the user.
        /// </summary>
        /// <param name="role">Role to add.</param>
        void AddRole(IRoleModel role);

        /// <summary>
        /// Remove an email from the user.
        /// </summary>
        /// <param name="email">Email to remove.</param>
        /// <returns>Returns the removed email.</returns>
        IEmailModel RemoveEmail(IEmailModel email);
        
        /// <summary>
        /// Remove a phone number from the user.
        /// </summary>
        /// <param name="phoneNumber">Phone number to remove.</param>
        /// <returns>Returns the removed phone number.</returns>
        IPhoneNumberModel RemovePhoneNumber(IPhoneNumberModel phoneNumber);

        /// <summary>
        /// Update the property.
        /// </summary>
        /// <param name="email">User property to update.</param>
        /// <returns>Returns success of operation.</returns>
        bool UpdateEmail(IEmailModel email);

        /// <summary>
        /// Update the property.
        /// </summary>
        /// <param name="phoneNumber">User property to update.</param>
        /// <returns>Returns success of operation.</returns>
        bool UpdatePhoneNumber(IPhoneNumberModel phoneNumber);
        
        /// <summary>
        /// Update a user role.
        /// </summary>
        /// <param name="role">Role to update.</param>
        /// <returns>Returns success of operation.</returns>
        bool UpdateRole(IRoleModel role);
        
    }

    /// <summary>
    /// Represents a user's phone number.
    /// </summary>
    public interface IPhoneNumberModel : IUserProperty, IDatabaseObjectModel, IDatabaseObjectReader, IDatabaseObjectInserter, IDatabaseObjectUpdater, IDatabaseObjectDeleter
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Data representing the phone number.
        /// </summary>
        string PhoneNumber {
            get;
            set;
        }

        /// <summary>
        /// Data representing phone type.
        /// </summary>
        IUIDFormat PhoneType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a user's email.
    /// </summary>
    public interface IEmailModel : IUserProperty, IDatabaseObjectModel, IDatabaseObjectReader, IDatabaseObjectInserter, IDatabaseObjectUpdater, IDatabaseObjectDeleter
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Data representing email.
        /// </summary>
        string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Data representing email type.
        /// </summary>
        IUIDFormat EmailType
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Represents a user's role.
    /// </summary>
    public interface IRoleModel : IUserProperty, IDatabaseObjectModel, IDatabaseObjectReader, IDatabaseObjectInserter, IDatabaseObjectUpdater
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Represents role associated with a role code.
        /// </summary>
        IUIDFormat RoleCode
        {
            get;
            set;
        }

        /// <summary>
        /// Status code of current status.
        /// </summary>
        IUIDFormat StatusCode
        {
            get;
            set;
        }

        /// <summary>
        /// Represents role associated with a role entry.
        /// </summary>
        IDateTimeFormat Timestamp
        {
            get;
            set;
        }

        /// <summary>
        /// Return true if the status event is authorizes for this role.
        /// </summary>
        /// <param name="database">Database to check validity from.</param>
        /// <returns>Returns true if role is in the database AND has a valid status.</returns>
        bool IsValid(IReadable database);
    }
    
}
