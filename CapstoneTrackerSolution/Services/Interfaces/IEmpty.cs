/*
    IEmpty.cs
    Contains the IEmpty interface.
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
    /// Implementations of this interface can be empty.
    /// </summary>
    public interface IEmpty
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Check if this can be empty.
        /// </summary>
        bool IsEmpty { get; }

    }

    /// <summary>
    /// Implementations of this interface can be empty.
    /// </summary>
    /// <typeparam name="T">Type to make empty.</typeparam>
    public interface IEmpty<T> : IEmpty
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// When called, turns the instance into its 'empty' form.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        T MakeEmpty();
    }
}
