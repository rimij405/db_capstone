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
        // Field(s).
        //////////////////////

        private TestStatus 

        //////////////////////
        // Properties.
        //////////////////////

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



    }

}
