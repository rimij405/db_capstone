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

namespace TestDataAccessLayer
{

    /// <summary>
    /// Test status.
    /// </summary>
    public enum TestStatus
    {
        /// <summary>
        /// An error was thrown while testing.
        /// </summary>
        ERROR = -1,

        /// <summary>
        /// The test resolved as fail state.
        /// </summary>
        FAILURE = 0,

        /// <summary>
        /// The test resolved as success state.
        /// </summary>
        SUCCESS = 1,
    }

    /// <summary>
    /// Result of running a test method.
    /// </summary>
    public struct TestResults
    {
        //////////////////////
        // Static Method(s).
        //////////////////////

        /// <summary>
        /// Returns a status based on the appropriate code.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static TestStatus CreateTestStatus(int code)
        {
            throw new NotImplementedException();
        }

        //////////////////////
        // Field(s).
        //////////////////////

        /// <summary>
        /// Result of the test.
        /// </summary>
        private TestStatus status;



        //////////////////////
        // Properties.
        //////////////////////

        public string Message
        {
            get { return ""; }
        }

        //////////////////////
        // Constructor(s).
        //////////////////////

        //////////////////////
        // Method(s).
        //////////////////////

        //////////////////////
        // Service(s).

        //////////////////////
        // Accessor(s).

        //////////////////////
        // Mutator(s).




    }

    /// <summary>
    /// 
    /// </summary>
    public class TestFailedException : CustomException
    {
        public TestFailedException(TestResults testResults)
            : base(testResults.Message)
        {
        }

        public override string GetExceptionName()
        {
            throw new NotImplementedException();
        }
    }

}
