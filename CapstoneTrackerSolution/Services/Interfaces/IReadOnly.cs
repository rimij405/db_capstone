/*
    IReadOnly.cs
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
    /// Implementations of this interface can have their editing toggled.
    /// </summary>
    public interface IReadOnly
    {

        /// <summary>
        /// Allow editing to be toggled.
        /// </summary>
        bool IsReadOnly { get; set; }

    }
}
