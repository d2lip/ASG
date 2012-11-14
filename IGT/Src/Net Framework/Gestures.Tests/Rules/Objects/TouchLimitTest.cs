using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for TouchLimitTest and is intended
    ///to contain all TouchLimitTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchLimitTest
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

        #region Min Tests

        [TestMethod()]
        public void TouchLimit_Min_Setter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Min = 0
            };

            //The min was set to 0, so we confirm that the min was indeed set to 0
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchLimit_Min_Setter_Not_Set_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit();

            //Since the min was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchLimit_Min_Getter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Min = -1
            };

            //The max was set to 5, so test the getter to check if it returns 5
            bool expected = true;
            int actualMin = -1;
            bool actual = actualMin == (target.Min);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Max Tests

        [TestMethod()]
        public void TouchLimit_Max_Setter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Max = 3
            };

            //The max was set to 3, so we confirm that the min was indeed set to 3
            bool expected = true;
            bool actual = target.Max.Equals(3);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchLimit_Max_Setter_Not_Set_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit();

            //Since the max was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Max.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchLimit_Max_Getter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Max = 5
            };

            //The max was set to 5, so test the getter to check if it returns 5
            bool expected = true;
            int actualMax = 5;
            bool actual = actualMax == (target.Max);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Type Tests

        [TestMethod()]
        public void TouchLimit_Type_Setter_Not_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit() { };

            //Since the Type was not set, the string should be empty
            bool expected = true;
            bool actual = target.Type.Equals(string.Empty);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchLimit_Type_Setter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Type = "Range"
            };

            //Since the Type was set to Range, we want to check this was set
            bool expected = true;
            bool actual = target.Type.Equals("Range");

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void TouchLimit_Type_Getter_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Type = "FixedValue"
            };

            //Since the Behavior was set to decreasing, we want to check that this was set
            bool expected = true;

            bool actual = string.Equals("FixedValue", target.Type);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Equals Tests

        [TestMethod()]
        public void TouchLimit_EqualsTest_Null_Check()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit();

            //We expect the equals to fail, as there is nothing to equal with
            bool expected = false;
            bool actual = target.Equals(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchLimit_EqualsTest_False_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Min = 3,
                Max = 5
            };

            // A second rule
            IPrimitiveConditionData anotherRuleData = new TouchLimit()
            {

                Min = 3,
                Max = 6
            };

            //We expect the equals to fail, as there is nothing to equal with
            bool expected = false;
            bool actual = target.Equals(anotherRuleData);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchLimit_EqualsTest_True_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Min = 3,
                Max = 5
            };

            // A second rule
            IPrimitiveConditionData anotherRuleData = new TouchLimit()
            {

                Min = 3,
                Max = 5
            };

            //We expect the equals to be true, as the min/max are the same
            bool expected = true;
            bool actual = target.Equals(anotherRuleData);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Union Tests

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void TouchLimit_Union_With_A_Null_Test()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit();

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void TouchLimit_Union_With_Min_Set()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Min = 3
            };

            // The 2nd rule
            IPrimitiveConditionData anotherRuleData = new TouchLimit()
            {
                Min = 1
            };

            //Union of the 2 rules, should result in rule with min of 1
            target.Union(anotherRuleData);

            //We expect the union to have a min of 1
            bool expected = true;
            bool actual = target.Min.Equals(1);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchLimit_Union_With_Max_Set()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Max = 1
            };

            // The 2nd rule
            IPrimitiveConditionData anotherRuleData = new TouchLimit()
            {
                Max = 7
            };

            //Union of the 2 rules, should result in rule with max of 7
            target.Union(anotherRuleData);

            //We expect the union to have a max of 7
            bool expected = true;
            bool actual = target.Max.Equals(7);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchLimit_Union_With_Type_Change()
        {
            // The type we are testing
            TouchLimit target = new TouchLimit()
            {
                Max = 1,
                Type = "FixedValue"
            };

            // The 2nd rule
            IPrimitiveConditionData anotherRuleData = new TouchLimit()
            {
                Max = 7
            };

            //Union of the 2 rules, should result in rule with max of 7
            target.Union(anotherRuleData);

            //We expect the union to have a max of 7, but the type should now be range, as it was not before
            bool expected = true;
            bool actual = target.Type.Equals("Range");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

    }
}
