using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// Represents execution state of an operation.
    /// </summary>
    public enum OperationStatus
    {
        /// <summary>
        /// Null operation status.
        /// </summary>
        NULL = -2,

        /// <summary>
        /// An error was thrown during the operation.
        /// </summary>
        ERROR = -1,

        /// <summary>
        /// The operation resolved to its fail state.
        /// </summary>
        FAILURE = 0,

        /// <summary>
        /// The operation resolved to its success state.
        /// </summary>
        SUCCESS = 1,
    }

    /// <summary>
    /// Service that can handle tracking of an operation's state.
    /// </summary>
    public interface IOperationStateMachine
    {
        
        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Current state of the operation.
        /// </summary>
        OperationStatus State { get; }
        
        /// <summary>
        /// Check if in error state.
        /// </summary>
        bool IsError { get; }

        /// <summary>
        /// Check if in fail state.
        /// </summary>
        bool IsFailure { get; }

        /// <summary>
        /// Check if in success state.
        /// </summary>
        bool IsSuccess { get; }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Services.	

        /// <summary>
        /// Check if matches input state.
        /// </summary>
        /// <param name="other">State to compare.</param>
        /// <returns>Returns true if match made.</returns>
        bool IsState(OperationStatus other);

        /// <summary>
        /// Check if matches input state, by integer code.
        /// </summary>
        /// <param name="code">Code to find comparison state.</param>
        /// <returns>Returns true if match made.</returns>
        bool IsState(int code);

        /// <summary>
        /// Set to the error state.
        /// </summary>
        void Error();

        /// <summary>
        /// Set to the fail state.
        /// </summary>
        void Fail();

        /// <summary>
        /// Set to the pass state.
        /// </summary>
        void Pass();

        //////////////////////
        // Helpers.						

        //////////////////////
        // Accessors.

        /// <summary>
        /// Return the current operation state.
        /// </summary>
        /// <returns>Returns state.</returns>
        OperationStatus GetState();

        //////////////////////
        // Mutators.				

        /// <summary>
        /// Set the operation state.
        /// </summary>
        /// <param name="state">State to set.</param>
        void SetState(OperationStatus state);

        /// <summary>
        /// Set the operation state, by integer code.
        /// </summary>
        /// <param name="code">Code to get operation state to set.</param>
        /// <returns>Returns false if state cannot be set.</returns>
        bool SetState(int code);
        
    }

}
