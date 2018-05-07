/********************************************
 * TestResults.cs
 * Ian Effendi
 * ***
 * Contains results for assertions of tests. 
 * ******************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Services;
using Services.Interfaces;

namespace TestDataAccessLayer
{
    
    /// <summary>
    /// Result of running a test method.
    /// </summary>
    public class TestResults
    {
        //////////////////////
        // Static Method(s).
        //////////////////////

        /// <summary>
        /// Return the null test status.
        /// </summary>
        public static OperationStatus NullState
        {
            get { return TestResults.GetTestStatus(-2); }
        }

        /// <summary>
        /// Return the Error test status.
        /// </summary>
        public static OperationStatus ErrorState
        {
            get { return TestResults.GetTestStatus(-1); }
        }

        /// <summary>
        /// Return failure test status.
        /// </summary>
        public static OperationStatus FailureState
        {
            get { return TestResults.GetTestStatus(0); }
        }

        /// <summary>
        /// Return success test status.
        /// </summary>
        public static OperationStatus SuccessState
        {
            get { return TestResults.GetTestStatus(1); }
        }

        /// <summary>
        /// Returns a status based on the appropriate code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns>Returns enum value.</returns>
        private static OperationStatus GetTestStatus(int code)
        {
            switch (code)
            {
                case 0:
                    return OperationStatus.FAILURE;
                case 1:
                    return OperationStatus.SUCCESS;
                case -1:
                    return OperationStatus.ERROR;
                default:
                    return OperationStatus.NULL;
            }
        }

        /// <summary>
        /// Returns true if the input status values match.
        /// </summary>
        /// <param name="a">First status.</param>
        /// <param name="b">Second status.</param>
        /// <returns>Returns true if match is made.</returns>
        private static bool IsMatch(OperationStatus a, OperationStatus b)
        {
            return (a == b);
        }

        /// <summary>
        /// Create a test result object.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to set results object to.</param>
        /// <param name="nullMessage">Null message to set.</param>
        /// <param name="successMessage">Success message to set.</param>
        /// <param name="failureMessage">Failure message to set.</param>
        /// <param name="errorMessage">Error message to set.</param>
        /// <returns>Returns a constructed results object.</returns>
        public static TestResults Create(string title, OperationStatus currentState = OperationStatus.NULL, string nullMessage = null, string successMessage = null, string failureMessage = null, string errorMessage = null)
        {
            return new TestResults(title, currentState, nullMessage, successMessage, failureMessage, errorMessage);
        }

        /// <summary>
        /// Create a test result object in the success state.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="successMessage">Success message to set.</param>
        /// <returns>Returns a constructed results object.</returns>
        public static TestResults CreateSuccess(string title, string successMessage = null)
        {
            return Create(title, currentState: OperationStatus.SUCCESS, successMessage: successMessage);
        }

        /// <summary>
        /// Create a test result object in the failed state.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="failureMessage">Failure message to set.</param>
        /// <returns>Returns a constructed results object.</returns>
        public static TestResults CreateFailure(string title, string failureMessage = null)
        {
            return Create(title, currentState: OperationStatus.FAILURE, failureMessage: failureMessage);
        }

        /// <summary>
        /// Create a test result object in the error thrown state.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="errorMessage">Error message to set.</param>
        /// <returns>Returns a constructed results object.</returns>
        public static TestResults CreateError(string title, string errorMessage = null)
        {
            return Create(title, currentState: OperationStatus.ERROR, errorMessage: errorMessage);
        }


        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Name of the test.
        /// </summary>
        private string title;

        /// <summary>
        /// Result of the test.
        /// </summary>
        private OperationStatus state;

        /// <summary>
        /// Contains map of message for each state.
        /// </summary>
        private IDictionary<OperationStatus, string> messages;

        /// <summary>
        /// Stack of messages listing operations that happened in the results set.
        /// </summary>
        private List<string> stack;

        //////////////////////
        // Properties.
        //////////////////////

        /// <summary>
        /// Access property for the title.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Result of the test.
        /// </summary>
        public OperationStatus CurrentState
        {
            get { return this.state; }
            private set { this.state = value; }
        }

        /// <summary>
        /// Returns the message associated with this state.
        /// </summary>
        public string Message
        {
            get { return this.GetCurrentMessage(); }
        }

        /// <summary>
        /// Reference to the stack.
        /// </summary>
        public List<string> Stack
        {
            get
            {
                if(this.stack == null)
                {
                    this.stack = new List<string>();
                }
                return this.stack;
            }
        }

        /// <summary>
        /// Collection of messages mapped by test state.
        /// </summary>
        private IDictionary<OperationStatus, string> Messages
        {
            get
            {
                if (this.messages == null || this.messages.Count == 0)
                {
                    this.messages = new Dictionary<OperationStatus, string>()
                    {
                        { TestResults.NullState, "No test results generated." },
                        { TestResults.SuccessState, "Test passed assertion." },
                        { TestResults.FailureState, "Test failed assertion." },
                        { TestResults.ErrorState, "Test failed with an exception." }
                    };
                }
                return this.messages;
            }            
        }

        /// <summary>
        /// Check if test result is in the success state.
        /// </summary>
        public bool IsSuccessful
        {
            get { return this.IsState(TestResults.SuccessState); }
        }

        /// <summary>
        /// Check if test result is in the failure state.
        /// </summary>
        public bool IsFailure
        {
            get { return this.IsState(TestResults.FailureState); }
        }

        /// <summary>
        /// Check if test result is in the error state.
        /// </summary>
        public bool IsError
        {
            get { return this.IsState(TestResults.ErrorState); }
        }

        /// <summary>
        /// Check if test result is in the null state.
        /// </summary>
        public bool IsNull
        {
            get { return this.IsState(TestResults.NullState); }
        }

        /// <summary>
        /// Returns stack trace if it exists, else, an empty string.
        /// </summary>
        public string StackTrace
        {
            get
            {
                string stackTrace = "";

                for (int index = 0; index < this.Count; index++)
                {
                    stackTrace += $"{this[index]}";
                    if((index + 1) < this.Count) { stackTrace += "\n"; }
                }

                return stackTrace;
            }
        }

        /// <summary>
        /// Number of messages in the stack.
        /// </summary>
        public int Count
        {
            get { return this.Stack.Count; }
        }

        /// <summary>
        /// Indexer access by enum key to the messages mapped to each state.
        /// </summary>
        /// <param name="state">State to set message for.</param>
        /// <returns>Returns message the index location associated with the state.</returns>
        public string this[OperationStatus state]
        {
            get { return this.Messages[state]; }
            set { this.Messages[state] = value.Trim(); }
        }

        /// <summary>
        /// Returns message at index if the index is within bounds for the stack.
        /// </summary>
        /// <param name="stackIndex"></param>
        /// <returns></returns>
        public string this[int stackIndex]
        {
            get
            {
                string stackMessage = "";
                if (stackIndex >= 0 || stackIndex < this.Count)
                {
                    stackMessage = this.Stack[stackIndex];
                }
                return stackMessage;
            }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////
        
        /// <summary>
        /// Creates a TestResults object using all the input values.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to set results object to.</param>
        /// <param name="nullMessage">Null message to set.</param>
        /// <param name="successMessage">Success message to set.</param>
        /// <param name="failureMessage">Failure message to set.</param>
        /// <param name="errorMessage">Error message to set.</param>
        public TestResults(string title,
                            OperationStatus currentState, 
                            string nullMessage = null,
                            string successMessage = null,
                            string failureMessage = null,
                            string errorMessage = null)
        {
            this.Title = title;
            this.CurrentState = currentState;
            if (nullMessage != null) { this[TestResults.NullState] = nullMessage; }
            if (successMessage != null) { this[TestResults.SuccessState] = successMessage; }
            if (failureMessage != null) { this[TestResults.FailureState] = failureMessage; }
            if (errorMessage != null) { this[TestResults.ErrorState] = errorMessage; }
        }

        /// <summary>
        /// Creates a TestResults object set to the null state, using all the input values.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="nullMessage">Null message to set.</param>
        /// <param name="successMessage">Success message to set.</param>
        /// <param name="failureMessage">Failure message to set.</param>
        /// <param name="errorMessage">Error message to set.</param>
        public TestResults(string title,
                            string nullMessage = null,
                            string successMessage = null,
                            string failureMessage = null,
                            string errorMessage = null)
            : this(title, TestResults.NullState, nullMessage, successMessage, failureMessage, errorMessage)
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Create a TestResults object using a current state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="messages">Map of messages for each state.</param>
        public TestResults(string title, OperationStatus currentState, List<KeyValuePair<OperationStatus, string>> messages)
            : this(title, currentState)
        {
            // Assign the input value pairs.
            foreach (KeyValuePair<OperationStatus, string> map in messages)
            {
                this[map.Key] = map.Value;
            }
        }

        /// <summary>
        /// Create a TestResults object using a null state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="messages">Map of messages for each state.</param>
        public TestResults(string title, List<KeyValuePair<OperationStatus, string>> messages)
            : this(title, TestResults.NullState, messages)
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Create a TestResults object using a current state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="messages">Map of messages for each state.</param>
        public TestResults(string title, OperationStatus currentState, params KeyValuePair<OperationStatus, string>[] messages)
            : this(title, currentState, messages.ToList<KeyValuePair<OperationStatus, string>>())
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Create a TestResults object using a null state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="messages">Map of messages for each state.</param>
        public TestResults(string title, params KeyValuePair<OperationStatus, string>[] messages)
            : this(title, TestResults.NullState, messages)
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Create a TestResults object using a current state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="map">Map of messages for each state.</param>
        public TestResults(string title, OperationStatus currentState, IDictionary<OperationStatus, string> map)
            : this(title, currentState, map.ToList<KeyValuePair<OperationStatus, string>>())
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Create a TestResults object using a null state and a map of messages.
        /// </summary>
        /// <param name="title">Title of the object.</param>
        /// <param name="currentState">State to assign.</param>
        /// <param name="map">Map of messages for each state.</param>
        public TestResults(string title, IDictionary<OperationStatus, string> map)
            : this(title, TestResults.NullState, map)
        {
            // Uses other constructor.
        }

        /// <summary>
        /// Returns a cloned TestResults object.
        /// </summary>
        /// <param name="other">TestResults object to clone.</param>
        private TestResults(TestResults other)
            : this(other.Title, other.CurrentState, other.GetMessageMap())
        {
            // Uses other constructor.

            // Copy the stack.
            this.Log(other.Stack);
        }

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        /// <summary>
        /// Check if result is in the input state.
        /// </summary>
        /// <param name="other">State to compare.</param>
        /// <returns>Returns true if current state matches input.</returns>
        private bool IsState(OperationStatus other)
        {
            return TestResults.IsMatch(this.CurrentState, other);
        }

        /// <summary>
        /// Return map as a collection of key/value pairs.
        /// </summary>
        /// <returns>Returns collection of <see cref="KeyValuePair{TKey, TValue}"/>.</returns>
        private List<KeyValuePair<OperationStatus, string>> GetMessageMap()
        {
            return this.Messages.ToList<KeyValuePair<OperationStatus, string>>();
        }

        /// <summary>
        /// Set to the associated TestStatus state.
        /// </summary>
        /// <param name="stateMessage">Optional message to assign.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Fail(string stateMessage = null)
        {
            this.CurrentState = TestResults.FailureState;
            this[TestResults.FailureState] = stateMessage ?? this[TestResults.FailureState];
            return this;
        }

        /// <summary>
        /// Set to the associated TestStatus state.
        /// </summary>
        /// <param name="stateMessage">Optional message to assign.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Pass(string stateMessage = null)
        {
            this.CurrentState = TestResults.SuccessState;
            this[TestResults.SuccessState] = stateMessage ?? this[TestResults.SuccessState];
            return this;
        }

        /// <summary>
        /// Set to the associated TestStatus state.
        /// </summary>
        /// <param name="stateMessage">Optional message to assign.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Null(string stateMessage = null)
        {
            this.CurrentState = TestResults.NullState;
            this[TestResults.NullState] = stateMessage ?? this[TestResults.NullState];
            return this;
        }

        /// <summary>
        /// Set to the associated TestStatus state, assigning a message in the process.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public TestResults Error(string errorMessage = null)
        {
            this.CurrentState = TestResults.ErrorState;
            this[TestResults.ErrorState] = $"{(errorMessage ?? "Unspecified error.")}";
            return this;
        }

        /// <summary>
        /// Set to the associated TestStatus state, assigning a message in the process.
        /// </summary>
        /// <returns>Returns reference to self.</returns>
        public TestResults Error(Exception e = null)
        {
            if(e == null) { return this; }
            return this.Error("Exception thrown. " + e.Message);
        }

        /// <summary>
        /// Create a custom test exception using this as the input in order to throw.
        /// </summary>
        /// <returns>Creates exception to throw.</returns>
        public TestException Throw(string errorMessage = null, Exception inner = null)
        {
            this.Error(errorMessage);
            if(inner != null) { return new TestException(this.Error(inner), inner); }
            return new TestException(this);
        }

        /// <summary>
        /// Add message to the stack.
        /// </summary>
        /// <param name="stackMessage">Message to add.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Log(string stackMessage)
        {
            this.Stack.Add(stackMessage);
            return this;
        }

        /// <summary>
        /// Add collection of messages to the stack.
        /// </summary>
        /// <param name="stackMessages">Messages to add.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Log(List<string> stackMessages)
        {
            foreach (string message in stackMessages)
            {
                this.Log(message);
            }
            return this;
        }
        
        /// <summary>
        /// Add collection of messages to the stack.
        /// </summary>
        /// <param name="stackMessages">Messages to add.</param>
        /// <returns>Returns reference to self.</returns>
        public TestResults Log(params string[] stackMessages)
        {
            return this.Log(stackMessages.ToList<string>());
        }
        
        /// <summary>
        /// Returns a cloned TestResults object.
        /// </summary>
        /// <returns>Returns clone.</returns>
        public TestResults Clone()
        {
            return new TestResults(this);
        }

        //////////////////////
        // Accessor(s).

        /// <summary>
        /// Returns the current state's message.
        /// </summary>
        /// <returns>Returns message mapped to current state.</returns>
        public string GetCurrentMessage()
        {
            return this[this.CurrentState];
        }

        //////////////////////
        // Mutator(s).

        /// <summary>
        /// Set the current state of the test results.
        /// </summary>
        /// <param name="other">Value to set state to.</param>
        /// <returns>Returns reference to self.</returns>
        private TestResults SetCurrentState(OperationStatus other)
        {
            this.CurrentState = other;
            return this;
        }
        
    }

    /// <summary>
    /// Custom exception thrown if a test fails during execution.
    /// </summary>
    public class TestException : CustomException
    {

        /// <summary>
        /// Stores reference to the test results object.
        /// </summary>
        private TestResults results;

        /// <summary>
        /// Access property for the results object.
        /// </summary>
        public TestResults Results
        {
            get { return this.results; }
            private set { this.results = value; }
        }

        /// <summary>
        /// Construction of a test exception requires a test result object.
        /// </summary>
        /// <param name="testResults">Test results object passed to exception.</param>
        public TestException(TestResults testResults)
            : base(testResults.Message)
        {
            // Clone the input test result.
            this.Results = testResults.Clone();
        }

        /// <summary>
        /// Construction of a test exception requires a test result object.
        /// </summary>
        /// <param name="testResults">Test results object passed to exception.</param>
        public TestException(TestResults testResults, Exception innerException)
            : base(testResults.Message, innerException)
        {
            // Clone the input test result.
            this.Results = testResults.Clone();
        }

        /// <summary>
        /// Return the name of the exception, when requested.
        /// </summary>
        /// <returns>Returns whitespace trimmed string containing the exception name.</returns>
        public override string GetExceptionName()
        {
            return $"[Failed Test Exception - {results.Title}]";
        }
    }

}
