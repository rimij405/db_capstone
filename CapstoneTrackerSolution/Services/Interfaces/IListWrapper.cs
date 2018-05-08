/*
    IListWrapper.cs
    Contains the IListWrapper interface.
    ---
    Ian Effendi
 */

// using statements.
using System;
using System.Collections;
using System.Collections.Generic;

/*
    The Services.Interfaces namespace
    is a collection of services that can be
    used in implementations for any project.
     */
namespace Services.Interfaces
{ 
    /// <summary>
    /// Helper function that wraps an existing collection.
    /// </summary>
    public interface IListWrapper : IReplicate, IEmpty
    {
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to the collection in question.
        /// </summary>
        IList List { get; }

        /// <summary>
        /// Return the collection size.
        /// </summary>
        int Count { get; }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Return an element from the collection.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        /// <returns>Returns object at specific index.</returns>
        object this[int index] { get; set; }

        /// <summary>
        /// Returns index of input element, if it exists in the collection.
        /// </summary>
        /// <param name="element">Element to find index of.</param>
        /// <returns>Returns index of specific object.</returns>
        int this[object element] { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Empty the collection of all elements.
        /// </summary>
        void Clear();

        /// <summary>
        /// Check if the collection has at least this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is greater than or equal to input value.</returns>
        bool HasAtLeast(int count);

        /// <summary>
        /// Check if the collection has exactly this many items.
        /// </summary>
        /// <param name="count">Amount of rows to check if collection has.</param>
        /// <returns>Returns true if count is equal to input value.</returns>
        bool HasExactly(int count);

        /// <summary>
        /// Check if it has a particular index.
        /// </summary>
        /// <param name="index">Index to check for.</param>
        /// <returns>Returns true if the index is in bounds. False, if otherwise.</returns>
        bool HasIndex(int index);

        /// <summary>
        /// Checks if the element is contained within the collection.
        /// </summary>
        /// <param name="element">Element to check.</param>
        /// <returns>Returns true if element is found.</returns>
        bool HasElement(object element);
        
    }

    /// <summary>
    /// Helper functions that wrap a generic collection.
    /// </summary>
    /// <typeparam name="TElement"></typeparam>
    public interface IListWrapper<TElement>
    {

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Reference to the collection in question.
        /// </summary>
        IList<TElement> List { get; }

        //////////////////////
        // Indexer(s).

        /// <summary>
        /// Return an element from the collection.
        /// </summary>
        /// <param name="index">Index of the element to get.</param>
        /// <returns>Returns object at specific index.</returns>
        TElement this[int index] { get; set; }

        /// <summary>
        /// Returns index of input element, if it exists in the collection.
        /// </summary>
        /// <param name="element">Element to find index of.</param>
        /// <returns>Returns index of specific object.</returns>
        int this[TElement element] { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service method(s).

        /// <summary>
        /// Check if index is valid.
        /// </summary>
        /// <param name="index">Index to check.</param>
        /// <returns>Returns true if in bounds.</returns>
        bool IsValidIndex(int index);

        /// <summary>
        /// Checks if the element is contained within the collection.
        /// </summary>
        /// <param name="element">Element to check.</param>
        /// <returns>Returns true if element is found.</returns>
        bool HasElement(TElement element);

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        bool HasElements(IList<TElement> elements);

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        bool HasAnyElements(IList<TElement> elements);

        /// <summary>
        /// Check if all elements are present within the collection. Returns false if a single one is missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        bool HasElements(params TElement[] elements);

        /// <summary>
        /// Check if any of the elements are present within the collection. Returns false if all are missing.
        /// </summary>
        /// <param name="elements">Collection of elements to check.</param>
        /// <returns>Returns true if elements are found.</returns>
        bool HasAnyElements(params TElement[] elements);

        //////////////////////
        // Accessor method(s).

        /// <summary>
        /// Return index of input element. Returns -1 if element doesn't exist in collection.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        int GetIndex(TElement element);

        /// <summary>
        /// Return element at specified index. Returns null if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        TElement GetElement(int index);

        /// <summary>
        /// Return element at specified index via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns null if element doesn't exist.</returns>
        bool TryGetElement(int index, out TElement element);

        //////////////////////
        // Mutator method(s).

        /// <summary>
        /// Add element to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        TElement AddElement(TElement element);

        /// <summary>
        /// Add element to the collection if it doesn't already exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Resulting element.</param>
        /// <returns>Returns operation success.</returns>
        bool TryAddElement(TElement element, out TElement result);

        /// <summary>
        /// Add a collection of elements to the current collection.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        bool AddElements(IList<TElement> elements);
        
        /// <summary>
        /// Add a collection of elements to the current collection.
        /// </summary>
        /// <param name="elements">Elements to add.</param>
        /// <returns>Returns operation success.</returns>
        bool AddElements(params TElement[] elements);
        
        /// <summary>
        /// Set existing index to reference the input element.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns operation success.</returns>
        TElement SetElement(int index, TElement element);

        /// <summary>
        /// Set existing index to reference the input element.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Resulting element.</param>
        /// <returns>Returns operation success.</returns>
        bool TrySetElement(int index, TElement element, out TElement result);
        
        /// <summary>
        /// Remove element from the collection if it exists. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <returns>Returns removed element.</returns>
        TElement RemoveElement(TElement element);

        /// <summary>
        /// Remove element from the collection if it exists. Returns null if element was not found or could not be removed via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="element">Element to affect.</param>
        /// <param name="result">Element to return.</param>
        /// <returns>Returns operation success.</returns>
        bool TryRemoveElement(TElement element, out TElement result);

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <returns>Returns removed element.</returns>
        TElement RemoveAt(int index);

        /// <summary>
        /// Remove element from the collection if it exists, by index. Returns null if element was not found or could not be removed via out parameter. Returns false if element doesn't exist.
        /// </summary>
        /// <param name="index">Index of element reference.</param>
        /// <param name="result">Element to return.</param>
        /// <returns>Returns operation success.</returns>
        bool TryRemoveAt(int index, out TElement result);
        
    }

}
