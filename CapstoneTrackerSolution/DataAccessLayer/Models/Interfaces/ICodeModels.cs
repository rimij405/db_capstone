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
using ISTE.DAL.Database.Implementations;
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
            set;
        }
        
    }
    
    /// <summary>
    /// Represents a collection of code models.
    /// </summary>
    public interface ICodeGlossary<TCodeItem> : IDatabaseObjectReader where TCodeItem : ICodeItem
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
        TCodeItem Find(string code);

        /// <summary>
        /// Returns a code model associated with the input name. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="name">Name of model to find.</param>
        /// <returns>Returns the code model.</returns>
        TCodeItem FindByName(string name);
        
    }

    /// <summary>
    /// Collection of term models.
    /// </summary>
    public interface ITerms
    {
        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="code">Code to find model for.</param>
        /// <returns>Returns the code model.</returns>
        ITermModel Find(string code);

    }

    /// <summary>
    /// Glossary of all status events.
    /// </summary>
    public interface IStatusEvents
    {
        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Returns a code model associated with the input code. Returns null if it doesn't exist.
        /// </summary>
        /// <param name="capstoneID">Primary key composite.</param>
        /// <param name="statusCode">Primary key composite.</param>
        /// <param name="timestamp">Primary key composite.</param>
        /// <returns>Returns the code model.</returns>
        IStatusEventModel Find(string capstoneID, string statusCode, string timestamp);
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
        IDateTimeFormat TermStart
        {
            get;
        }

        /// <summary>
        /// End of the term.
        /// </summary>
        IDateTimeFormat TermEnd
        {
            get;
        }

        /// <summary>
        /// Grade deadline.
        /// </summary>
        IDateTimeFormat GradeDeadline
        {
            get;
        }

        /// <summary>
        /// Add-drop deadline.
        /// </summary>
        IDateTimeFormat AddDropDeadline
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
    public interface IStatusModel : ICodeItem{}
    
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
    public interface IRoleTypeModel : ICodeItem{}

    /// <summary>
    /// Glosary of all email types.
    /// </summary>
    public interface IEmailTypes : ICodeGlossary<IEmailTypeModel>{}

    /// <summary>
    /// Represents an email type.
    /// </summary>
    public interface IEmailTypeModel : ICodeItem{}

    /// <summary>
    /// Glossary of all phone types.
    /// </summary>
    public interface IPhoneTypes : ICodeGlossary<IPhoneTypeModel>{}

    /// <summary>
    /// Represents a phone type.
    /// </summary>
    public interface IPhoneTypeModel : ICodeItem{}
    
}
