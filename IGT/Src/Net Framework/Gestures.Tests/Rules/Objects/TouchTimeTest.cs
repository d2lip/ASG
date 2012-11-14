
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for TouchTimeTest and is intended
    ///to contain all TouchTimeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchTimeTest
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

        #region togDL Tests

        [TestMethod()]
        public void TouchTime_toGDL_Test_With_Nothing_Set()
        {
            // The type we are testing
            TouchTime target = new TouchTime();

            // Since cnothing was set in constructor, resulting values and string output of toGDL should be 0 sec
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch time: 0 sec");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchTime_toGDL_Test_With_Variables_Set()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 5,
                Unit = "secs"

            };

            // Since the values were set in the constructor, we expect this in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch time: 5 secs");

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Value Tests

        [TestMethod()]
        public void TouchTime_Value_Setter_Not_Test()
        {
            // The type we are testing
            TouchTime target = new TouchTime();

            //Since the Value was not set, the Value should be 0
            bool expected = true;
            bool actual = target.Value == 0;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchTime_Value_Setter_Test()
        {

            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 3
            };

            //Since the Value was set to 3, we want to check that this was set
            bool expected = true;
            bool actual = 3 == target.Value;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void TouchTime_Value_Getter_Test()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 1
            };

            //Since the Value was set to 1, we want to check that we get this
            bool expected = true;
            bool actual = target.Value == 1;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Unit Tests

        [TestMethod()]
        public void TouchTime_Unit_Setter_Not_Test()
        {
            // The type we are testing
            TouchTime target = new TouchTime();

            //Since the Unit was not set, the string should be sec
            bool expected = true;
            bool actual = target.Unit.Equals("sec");

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchTime_Unit_Setter_Test()
        {

            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Unit = "msecs"
            };

            //Since the Unit was set to msecs, we want to check that this was set
            bool expected = true;
            bool actual = target.Unit.Equals("msecs");

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void TouchTime_Unit_Getter_Test()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Unit = "msec"
            };

            //Since the Unit was set to msec, we want to check that this was set
            bool expected = true;
            string unitS = "msec";
            bool actual = unitS.Equals(target.Unit);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Union Tests

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void TouchTime_Union_With_A_Null_Test()
        {
            // The type we are testing

            TouchTime target = new TouchTime();

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void TouchTime_Union_Test_Value()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 3
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 5
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of 2nd rule, since it is bigger, which is 5
            bool expected = true;
            bool actual = target.Value == 5;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchTime_Union_Test_Unit()
        {

            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Unit = "msec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Unit of the union to be the Unit of original rule
            bool expected = true;
            bool actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchTime_Union_With_Nothing_In_1st_Union_Rule()
        {

            // The type we are testing
            TouchTime target = new TouchTime();

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 3,
                Unit = "sec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 2nd rule, as nothing was set in 1st rule
            bool expected = true;
            bool actual = target.Value == 3;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the 2nd rule, as nothing was set in 1st rule
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchTime_Union_With_Nothing_In_2nd_Union_Rule()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 3,
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime();

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 1st rule, as nothing was set in 2nd rule
            bool expected = true;
            bool actual = target.Value == 3;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the 1st rule, as nothing was set in 2nd rule
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchTime_Union_With_Differing_Units_Sec_Msec()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 1,
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 5000,
                Unit = "msec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 2nd rule, as 2nd was larger
            bool expected = true;
            bool actual = target.Value == 5;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the original rule
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchTime_Union_With_Differing_Units_Msec_Sec()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 1,
                Unit = "msecs"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 3,
                Unit = "secs"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 2nd rule, as 2nd was larger
            bool expected = true;
            bool actual = target.Value == 3000;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the original rule
            expected = true;
            actual = target.Unit.Equals("msecs");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchTime_Union_With_Differing_Units_Msecs_Msec()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 1,
                Unit = "msecs"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 3,
                Unit = "msec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 2nd rule, as 2nd was larger
            bool expected = true;
            bool actual = target.Value == 3;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the original rule
            expected = true;
            actual = target.Unit.Equals("msecs");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchTime_Union_With_Differing_Units_Secs_Sec()
        {
            // The type we are testing
            TouchTime target = new TouchTime()
            {
                Value = 1,
                Unit = "secs"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchTime()
            {
                Value = 3,
                Unit = "sec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the Value of the union to be the Value of the 2nd rule, as 2nd was larger
            bool expected = true;
            bool actual = target.Value == 3;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the original rule
            expected = true;
            actual = target.Unit.Equals("secs");
            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
