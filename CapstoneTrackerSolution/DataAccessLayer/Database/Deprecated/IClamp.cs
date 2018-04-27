/*
    IClamp.cs
    Clamp a particular value using an input type.
    ***
    ISTE 330.02 - Group 16
    Ian Effendi
    Jacob Toporoff
 */

 // Using statements.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISTE.DAL.Database
{
    /// <summary>
    /// Clamper that will clamp things between a comparable value.
    /// </summary>
    /// <typeparam name="TComparable">Comparable type to check.</typeparam>
    public interface IClamp<TComparable> where TComparable : IComparable
    {
        /// <summary>
        /// Returns a value that is clamped between a maximum and a minimum.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        /// <returns>Returns clamped value.</returns>
        TComparable Clamp(TComparable value);

        /// <summary>
        /// Sets the maximum of the clamper.
        /// </summary>
        /// <param name="max">Maximum to make the clamper set to.</param>
        /// <returns>Returns reference to self.</returns>
        IClamp<TComparable> SetMaximum(TComparable max);

        /// <summary>
        /// Set the minimum of the clamper.
        /// </summary>
        /// <param name="min">Minimum to make the clamper set to.</param>
        /// <returns>Returns reference to self.</returns>
        IClamp<TComparable> SetMinimum(TComparable min);

        /// <summary>
        /// Check if input value is between minimum and maximum.
        /// </summary>
        /// <param name="value">Value to check.</param>
        /// <returns>Returns boolean.</returns>
        bool IsBetween(TComparable value);        
    }
}
