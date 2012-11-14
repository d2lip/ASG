using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;


namespace TouchToolkit.GestureProcessor.Tests.RuleValidator
{


    /// <summary>
    ///This is a test class for DistanceBetweenPointsValidatorTest and is intended
    ///to contain all DistanceBetweenPointsValidatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DistanceBetweenPointsValidatorTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion

        /// <summary>
        ///A test for Validate
        ///</summary>
        [TestMethod()]
        public void ValidateTest_Check_Null_State()
        {
            DistanceBetweenPointsValidator target = new DistanceBetweenPointsValidator();
            ValidSetOfPointsCollection sets = null;
            ValidSetOfPointsCollection actual;
            actual = target.Validate(sets);

            Assert.IsTrue(actual.Count == 0);
        }

    }
}
