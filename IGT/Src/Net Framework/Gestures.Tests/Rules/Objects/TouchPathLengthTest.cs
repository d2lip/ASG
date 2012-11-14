using TouchToolkit.GestureProcessor.Utility;
using Microsoft.VisualStudio.TestTools;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for TouchPathLengthTest and is intended
    ///to contain all TouchPathLengthTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchPathLengthTest
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

        //TODO Equals method implementation/test

        #region Union Tests

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void TouchPathLength_Union_With_A_Null_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength();

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void TouchPathLength_Union_Min_Test()
        {

            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 5,
                Min = 3
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchPathLength()
            {
                Max = 5,
                Min = 1
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the value of the min when union'd, to be the min of the 2nd rule, which is 1
            bool expected = true;
            bool actual = target.Min.Equals(1);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchPathLength_Union_Max_Test()
        {

            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 5,
                Min = 3
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchPathLength()
            {
                Max = 7,
                Min = 1
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the value of the max when union'd, to be the max of the 2nd rule, which is 7
            bool expected = true;
            bool actual = target.Max.Equals(7);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchPathLength_Union_Nothing_Set_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 7,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchPathLength()
            {
                Max = 5,
                Min = 3
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the value of the min to remain the same as the original rule, which is 1
            bool expected = true;
            bool actual = target.Min.Equals(1);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

            //We expect the value of the min to remain the same as the original rule, which is 7
            actual = target.Max.Equals(7);

            //Assert they are equal
            Assert.AreEqual(expected, actual);


        }

        [TestMethod()]
        public void TouchPathLength_Union_Both_Max_And_Min_Test()
        {

            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 3,
                Min = 2
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchPathLength()
            {
                Max = 5,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the value of the min to become the value of the 2nd rule, which is 0
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

            //We expect the value of the max to become the value of the 2nd rule, which is 5
            actual = target.Max.Equals(5);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region toGDL Tests

        [TestMethod()]
        public void TouchPathLength_toGDL_Test_With_Nothing_Set()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength();

            // Since the nothing was set in constructor, resulting value for max and min should be 0f
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch path length: 0..0");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }


        [TestMethod()]
        public void TouchPathLength_toGDL_Test_With_Max_Set()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 5
            };

            // Since the max was set in the constructor, this should be reflected in the output string
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch path length: 0..5");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchPathLength_toGDL_Test_With_Min_Set()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Min = 1
            };

            // Since the min was set in the constructor, this should be reflected in the output string
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch path length: 1..0");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchPathLength_toGDL_Test_With_Min_And_Max_Set()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Min = 1,
                Max = 5
            };

            // Since the nothing was set in constructor, resulting value for max and min should be 0f
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch path length: 1..5");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Max Tests

        [TestMethod()]
        public void TouchPathLength_Max_Setter_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
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
        public void TouchPathLength_Max_Setter_Not_Set_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength();

            //Since the max was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Max.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchPathLength_Max_Getter_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Max = 5
            };

            //The max was set to 5, so test the getter to check if it returns 5
            bool expected = true;
            int actualMax = 5;
            bool actual = actualMax == target.Max;
            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Min Tests

        [TestMethod()]
        public void TouchPathLength_Min_Setter_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
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
        public void TouchPathLength_Min_Setter_Not_Set_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength();

            //Since the min was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchPathLength_Min_Getter_Test()
        {
            // The type we are testing
            TouchPathLength target = new TouchPathLength()
            {
                Min = -1
            };

            //The min was set to -1, so test the getter to check if it returns -1
            bool expected = true;
            int actualMin = -1;
            bool actual = actualMin == target.Min;

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
