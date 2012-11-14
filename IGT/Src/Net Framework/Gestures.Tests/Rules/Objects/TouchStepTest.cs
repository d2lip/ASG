using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for TouchStepTest and is intended
    ///to contain all TouchStepTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchStepTest
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

        #region TouchCount Tests

        [TestMethod()]
        public void TouchStep_TouchCount_Setter_Not_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep();

            //Since the TouchCount was not set, the TouchCount should be 0
            bool expected = true;
            bool actual = target.TouchCount == 0;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_TouchCount_Setter_Test()
        {

            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 3
            };

            //Since the TouchCount was set to 3, we want to check that this was set
            bool expected = true;
            bool actual = 3 == target.TouchCount;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void TouchStep_TouchCount_Getter_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 2
            };

            //Since the TouchCount was set to 2, we want to check that we get this
            bool expected = true;
            bool actual = target.TouchCount == 2;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region TimeLimit Tests

        [TestMethod()]
        public void TouchStep_TimeLimit_Setter_Not_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep();

            //Since the TimeLimit was not set, the TimeLimit should be 0
            bool expected = true;
            bool actual = target.TimeLimit == 0;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_TimeLimit_Setter_Test()
        {

            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TimeLimit = 4
            };

            //Since the TimeLimit was set to 4, we want to check that this was set
            bool expected = true;
            bool actual = 4 == target.TimeLimit;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void TouchStep_TimeLimit_Getter_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TimeLimit = 2
            };

            //Since the TimeLimit was set to 2, we want to check that we get this
            bool expected = true;
            bool actual = target.TimeLimit == 2;

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Unit Tests

        [TestMethod()]
        public void TouchStep_Unit_Setter_Not_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep();

            //Since the Unit was not set, the string should be empty
            bool expected = true;
            bool actual = target.Unit.Equals(string.Empty);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_Unit_Setter_Test()
        {

            // The type we are testing
            TouchStep target = new TouchStep()
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
        public void TouchStep_Unit_Getter_Test()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                Unit = "msec"
            };

            //Since the Behavior was set to decreasing, we want to check that this was set
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
        public void TouchStep_Union_With_A_Null_Test()
        {
            // The type we are testing

            TouchStep target = new TouchStep();

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void TouchStep_Union_With_Test_TouchCount()
        {

            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount  = 2
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep()
            {
                TouchCount = 5
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TouchCount of the union to be the TouchCount of 2nd rule, since it is bigger, which is 5
            bool expected = true;
            bool actual = target.TouchCount == 5;
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_Union_With_Test_TimeLimit()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TimeLimit = 2,
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep()
            {
                TimeLimit = 5,
                Unit = "sec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TimeLimit of the union to be the TimeLimit of 2nd rule, since it is bigger AND units are the same, which is 5
            bool expected = true;
            bool actual = target.TimeLimit == 5;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchStep_Union_With_Nothing_Set_In_2nd_Union_Rule()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 2,
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep();

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TimeLimit of the union to remain the same, as nothing was set in 2nd rule
            bool expected = true;
            bool actual = target.TimeLimit == 2;
            Assert.AreEqual(expected, actual);

            //We expect the TouchCount of the union to remain the same, as nothing was set in 2nd rule
            expected = true;
            actual = target.TouchCount == 1;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to remain the same, as nothing was set in 2nd rule
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchStep_Union_With_Nothing_Set_In_1st_Union_Rule()
        {

            // The type we are testing
            TouchStep target = new TouchStep();

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 2,
                Unit = "sec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TimeLimit of the union to be the TimeLimit of the 2nd rule, as nothing was set in 1st rule
            bool expected = true;
            bool actual = target.TimeLimit == 2;
            Assert.AreEqual(expected, actual);

            //We expect the TouchCount of the union to be the TouchCount of the 2nd rule, as nothing was set in 1st rule
            expected = true;
            actual = target.TouchCount == 1;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the 2nd rule, as nothing was set in 1st rule
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_Union_With_Nothing_Different_Units_But_Smaller()
        {

            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 1,
                Unit = "sec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 2000,
                Unit = "msec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TimeLimit of the union to be the TimeLimit of the 2nd rule, as 2nd TimeLimit is larger
            bool expected = true;
            bool actual = target.TimeLimit == 2;
            Assert.AreEqual(expected, actual);

            //We expect the TouchCount of the union remain the same, as no condition was needed to be met
            expected = true;
            actual = target.TouchCount == 1;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the 1st rule, despite the 2nd rule being in a different unit
            expected = true;
            actual = target.Unit.Equals("sec");
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void TouchStep_Union_With_Nothing_Different_Units_But_Bigger()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 2000,
                Unit = "msec"
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new TouchStep()
            {
                TouchCount = 1,
                TimeLimit = 5,
                Unit = "sec"
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the TimeLimit of the union to be the TimeLimit of the 2nd rule, as 2nd TimeLimit is larger
            bool expected = true;
            bool actual = target.TimeLimit == 5000;
            Assert.AreEqual(expected, actual);

            //We expect the TouchCount of the union remain the same, as no condition was needed to be met
            expected = true;
            actual = target.TouchCount == 1;
            Assert.AreEqual(expected, actual);

            //We expect the Unit of the union to be the Unit of the 1st rule, despite the 2nd rule being in a different unit
            expected = true;
            actual = target.Unit.Equals("msec");
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region toGDL Tests

        [TestMethod()]
        public void TouchStep_toGDL_Test_With_Nothing_Set()
        {
            // The type we are testing
            TouchStep target = new TouchStep();

            // Since the nothing was set in constructor, resulting values and string output of toGDL should be 0 touches within 0
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch step: 0 touches within 0 ");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void TouchStep_toGDL_Test_With_Variables_Set()
        {
            // The type we are testing
            TouchStep target = new TouchStep()
            {
                TouchCount = 3,
                TimeLimit = 2,
                Unit = "sec"

            };

            // Since the values were set in the constructor, we expect this in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Touch step: 3 touches within 2 sec");

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

    }
}
