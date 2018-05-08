/*
    IReplicate.cs
    Contains the generic replication interfaces, allowing objects to be cloned.
    ---
    Ian Effendi
 */

/*
    The Services.Interfaces namespace
    is a collection of services that can be
    used in implementations for any project.
     */
namespace Services.Interfaces
{
    /// <summary>
    /// Interface for allowing clonable collections/items.
    /// </summary>
    public interface IReplicate
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Return a cloned deep-copy of this object.
        /// </summary>
        /// <returns>Returns a clone of this object.</returns>
        object Clone(object objectToClone);
    }

    /// <summary>
    /// Interface for allowing clonable collections/items.
    /// </summary>
    /// <typeparam name="TClone">Type of object to clone.</typeparam>
    public interface IReplicate<TClone> : IReplicate
    {
        /// <summary>
        /// Return a cloned deep-copy of this object.
        /// </summary>
        /// <returns>Returns a clone of this object.</returns>
        TClone Clone();
    }
}
