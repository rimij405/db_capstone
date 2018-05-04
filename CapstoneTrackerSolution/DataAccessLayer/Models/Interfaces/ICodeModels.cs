/*
    ICodeModels.cs
    ---
    Ian Effendi
 */

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
    /// Represents a timestamp.
    /// </summary>
    public interface ITimestamp
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Timestamp associated with a particular model.
        /// </summary>
        IDateTimeFormat Timestamp
        {
            get;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Timestamp associated with a particular model.
        /// </summary>
        /// <returns>Return timestamp.</returns>
        IDateTimeFormat GetTimestamp();

        /// <summary>
        /// Set the timestamp.
        /// </summary>
        /// <param name="timestamp">Time to set.</param>
        void SetTimestamp(IDateTimeFormat timestamp);
    }

    /// <summary>
    /// Represents a collection of code models.
    /// </summary>
    public interface ICodeGlossary<TCodeModel> : IDatabaseObjectReader where TCodeModel : ICodeModel
    {
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Return collection of code models.
        /// </summary>
        IList<ICodeItem> Glossary
        {
            get;
        }

        /// <summary>
        /// Get a code model associated with a particular code. Returns null if nothing is associated with the input code.
        /// </summary>
        /// <param name="code">Code to find model for.</param>
        /// <returns>Returns the code model.</returns>
        ICodeItem this[string code]
        {
            get;
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="code">Code to find model for.</param>
        /// <returns>Returns the code model.</returns>
        TCodeModel Find(string code);

        /// <summary>
        /// Returns a code model associated with the input name. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="name">Name of model to find.</param>
        /// <returns>Returns the code model.</returns>
        TCodeModel FindByName(string name);

        /// <summary>
        /// Return a code model created from the input code.
        /// </summary>
        /// <param name="code">Code associated with type.</param>
        /// <returns>Returns a phone type object.</returns>
        TCodeModel GetModel(string code);

    }

    /// <summary>
    /// Represents a key/value code in the database.
    /// </summary>
    public interface ICodeModel : IDatabaseObject, IDatabaseObjectReader
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Code numeral.
        /// </summary>
        IUIDFormat Code
        {
            get;
        }
    }
    
    /// <summary>
    /// Represents a term.
    /// </summary>
    public interface ITermModel : ICodeModel
    {
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Range of dates between start and end.
        /// </summary>
        IDateTimeRange TermRange
        {
            get;
        }

        /// <summary>
        /// Start of the term.
        /// </summary>
        IDateTimeRange TermStart
        {
            get;
        }

        /// <summary>
        /// End of the term.
        /// </summary>
        IDateTimeRange TermEnd
        {
            get;
        }

        /// <summary>
        /// Grade deadline.
        /// </summary>
        IDateTimeRange GradeDeadline
        {
            get;
        }

        /// <summary>
        /// Add-drop deadline.
        /// </summary>
        IDateTimeRange AddDropDeadline
        {
            get;
        }        
    }

    /// <summary>
    /// Represents a key/value code in the database.
    /// </summary>
    public interface ICodeItem : ICodeModel
    {
        //////////////////////
        // Properties.
        //////////////////////
        
        /// <summary>
        /// Name associated with a code.
        /// </summary>
        string Name
        {
            get;
        }

        /// <summary>
        /// Description associated with a given code.
        /// </summary>
        string Description
        {
            get;
        }
    }

    /// <summary>
    /// Glossary of all statuses.
    /// </summary>
    public interface IStatuses : ICodeGlossary<IStatusModel>{}

    /// <summary>
    /// Represents a status that can be used.
    /// </summary>
    public interface IStatusModel : ICodeItem
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return the code of the status.
        /// </summary>
        /// <returns>Return role code.</returns>
        string GetStatusCode();

        /// <summary>
        /// Return the name of the status.
        /// </summary>
        /// <returns>Return name.</returns>
        string GetStatusName();

        /// <summary>
        /// Return the description of the status.
        /// </summary>
        /// <returns>Return description.</returns>
        string GetStatusDescription();
    }

    /// <summary>
    /// Represents a status and a timestamp.
    /// </summary>
    public interface IStatusEventModel : IStatusModel, ITimestamp {}

    /// <summary>
    /// Glossary of all role types.
    /// </summary>
    public interface IRoleTypes : ICodeGlossary<IRoleTypeModel>{}

    /// <summary>
    /// Represents a role that a user can have.
    /// </summary>
    public interface IRoleTypeModel : ICodeItem
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return the code of the role.
        /// </summary>
        /// <returns>Return role code.</returns>
        string GetRoleCode();

        /// <summary>
        /// Return the name of the role.
        /// </summary>
        /// <returns>Return name.</returns>
        string GetRoleName();

        /// <summary>
        /// Return the description of the role.
        /// </summary>
        /// <returns>Return description.</returns>
        string GetRoleDescription();
    }

    /// <summary>
    /// Glosary of all email types.
    /// </summary>
    public interface IEmailTypes : ICodeGlossary<IEmailTypeModel>{}

    /// <summary>
    /// Represents an email type.
    /// </summary>
    public interface IEmailTypeModel : ICodeItem
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the code associated with the given email type.
        /// </summary>
        /// <returns>Returns code.</returns>
        string GetEmailTypeCode();

        /// <summary>
        /// Returns the name of the email type.
        /// </summary>
        /// <returns>Returns type.</returns>
        string GetEmailType();

        /// <summary>
        /// Returns the description associated with the email.
        /// </summary>
        /// <returns>Returns description.</returns>
        string GetEmailTypeDescription();
    }

    /// <summary>
    /// Glossary of all phone types.
    /// </summary>
    public interface IPhoneTypes : ICodeGlossary<IPhoneTypeModel>{}

    /// <summary>
    /// Represents a phone type.
    /// </summary>
    public interface IPhoneTypeModel : ICodeItem
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Get the code associated with the given phone type.
        /// </summary>
        /// <returns>Returns code.</returns>
        string GetPhoneTypeCode();

        /// <summary>
        /// Returns the name of the phone type.
        /// </summary>
        /// <returns>Returns type.</returns>
        string GetPhoneType();

        /// <summary>
        /// Returns the description associated with the phone.
        /// </summary>
        /// <returns>Returns description.</returns>
        string GetPhoneTypeDescription();
    }
    
}
