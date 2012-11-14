using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for EnclosedAreaTest and is intended
    ///to contain all EnclosedAreaTest Unit Tests
    ///</summary>
    [TestClass()]
    public class EnclosedAreaTest
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

        //TODO complete equals method for EnclosedArea

        #region Union Tests

        [TestMethod()]
        public void EnclosedArea_Union_Test_Min()
        {

            // The type we are testing
            EnclosedArea target = new EnclosedArea()
            {
                Max = 5,
                Min = 2
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new EnclosedArea()
            {
                Max = 5,
                Min = 1
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the min of the union to be the min of 2nd rule, since it is smaller, which is 1
            bool expected = true;
            bool actual = target.Min.Equals(1);
            Assert.AreEqual(expected, actual);

            //We expect the max of the union to remain the same, since nothing changed, which is 5
            expected = true;
            actual = target.Max.Equals(5);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void EnclosedArea_Union_Test_Max()
        {

            // The type we are testing
            EnclosedArea target = new EnclosedArea()
            {
                Max = 2,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new EnclosedArea()
            {
                Max = 5,
                Min = 1
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the max of the union to be the max of 2nd rule, since it is larger, which is 5
            bool expected = true;
            bool actual = target.Max.Equals(5);
            Assert.AreEqual(expected, actual);

            //We expect the min of the union to remain the same, since nothing changed, which is 1
            expected = true;
            actual = target.Min.Equals(1);
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void EnclosedArea_Union_With_Nothing_Set_In_Resulting_Union()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
            {
                Max = 2,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new EnclosedArea()
            {
                Max = 2,
                Min = 1
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the max of the union to be the max of target, since nothing changed, which is 2
            bool expected = true;
            bool actual = target.Max.Equals(2);
            Assert.AreEqual(expected, actual);

            //We expect the min of the union to be the min of target, since nothing changed, which is 1
            expected = true;
            actual = target.Min.Equals(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void EnclosedArea_Union_With_A_Null_Test()
        {
            // The type we are testing

            EnclosedArea target = new EnclosedArea()
            {
                Max = 2,
                Min = 1
            };

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }


        #endregion

        #region toGDL Tests

        [TestMethod()]
        public void EnclosedArea_toGDL_Test_With_Nothing_Set()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea();

            // Since the nothing was set in constructor, resulting values and string output of toGDL should be 0..0
            bool expected = true;
            bool actual = target.ToGDL().Equals("Enclosed Area: 0..0");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EnclosedArea_toGDL_Test_With_Variables_Set()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
            {
                Max = 3,
                Min = 1
            };

            // Since max/min were set in constructor, resulting values and string output of toGDL should be 1..3
            bool expected = true;
            bool actual = target.ToGDL().Equals("Enclosed Area: 1..3");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Min Tests

        [TestMethod()]
        public void EnclosedArea_Min_Setter_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
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
        public void EnclosedArea_Min_Setter_Not_Set_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea();

            //Since the min was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EnclosedArea_Min_Getter_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
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
        public void EnclosedArea_Max_Setter_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
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
        public void EnclosedArea_Max_Setter_Not_Set_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea();

            //Since the max was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Max.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void EnclosedArea_Max_Getter_Test()
        {
            // The type we are testing
            EnclosedArea target = new EnclosedArea()
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

    }
}
