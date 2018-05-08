/*
    IBusinessObject.cs
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
using ISTE.DAL.Models.Interfaces;

namespace ISTE.BAL.Interfaces
{
    /// <summary>
    /// Business layer object.
    /// </summary>
    public interface IBusinessObject 
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Returns the model this business object is wrapping.
        /// </summary>
        IDatabaseObjectModel Model { get; }
        
    }

    /// <summary>
    /// Update methods for a business object.
    /// </summary>
    public interface IBusinessUpdater 
    {

        //////////////////////
        // Method(s).
        //////////////////////
                
        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Attempt to update database persistance for a model.
        /// </summary>
        /// <param name="model">Model to post to the database.</param>
        /// <returns>Returns operation success.</returns>
        bool Post(IDatabaseObjectUpdater model);
    }

    /// <summary>
    /// Delete methods for a business object.
    /// </summary>
    public interface IBusinessDeleter 
    {

        //////////////////////
        // Method(s).
        //////////////////////
        
        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Attempt to delete from database persistance for a model.
        /// </summary>
        /// <param name="model">Model to delete from the database.</param>
        /// <returns>Returns operation success.</returns>
        bool Delete(IDatabaseObjectDeleter model);
    }

    /// <summary>
    /// Create methods for a business object.
    /// </summary>
    public interface IBusinessInserter 
    {

        //////////////////////
        // Method(s).
        //////////////////////
        
        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Attempt to insert into database persistance for a model.
        /// </summary>
        /// <param name="model">Model to insert into the database.</param>
        /// <returns>Returns operation success.</returns>
        bool Create(IDatabaseObjectInserter model);
    }

    /// <summary>
    /// Read methods for a business object.
    /// </summary>
    public interface IBusinessReader 
    {

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Attempt to insert into database persistance for a model.
        /// </summary>
        /// <param name="model">Model to insert into the database.</param>
        /// <returns>Returns operation success.</returns>
        bool Read(IDatabaseObjectReader model);
    } 
    
}
