/*
    INullable.cs
    Contains the INullable interface.
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
    /// Implementations of this interface can have a 'null' value.
    /// </summary>
    public interface INullable
    { 

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Check if the instance is a custom version of null.
        /// </summary>
        bool IsNull { get; }

    }

    /// <summary>
    /// Implementations of this interface can have a 'null' value.
    /// </summary>
    /// <typeparam name="T">Type that is being made nullable.</typeparam>
    public interface INullable<T> : INullable
    {
        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// When called, turns the instance into its 'null' form.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        T MakeNull();
    }
}
