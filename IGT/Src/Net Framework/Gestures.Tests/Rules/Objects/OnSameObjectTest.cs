using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Tests.Rules.Objects;

namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for OnSameObjectTest and is intended
    ///to contain all OnSameObjectTest Unit Tests
    ///</summary>
    [TestClass()]
    public class OnSameObjectTest
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

        #region Condition Tests

        [TestMethod()]
        public void OnSameObject_Condition_Not_Set_Test()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject();

            //Since the condition was not set, we are verifying that it was set to true as specified in original code
            bool expected = true;
            bool actual = target.Condition.Equals(true);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void OnSameObject_Condition_Set_Test()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject() 
            { 
                Condition = false
            };

            //Since the condition was set, we are verifying that it was set to false as specified in original code
            bool expected = true;
            bool actual = target.Condition.Equals(false);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void OnSameObject_Condition_Getter_Test()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = true
            };

            //Since the condition was set, we are verifying that it was set to false as specified in original code
            bool expected = true;
            bool actual = Boolean.Equals(true, target.Condition);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region toGDL Tests

        [TestMethod()]
        public void OnSameObject_toGDL_Test_With_Nothing_Set()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject();

            // Since the nothing was set in constructor, resulting value and string should indicate true
            bool expected = true;
            bool actual = target.ToGDL().Equals("On same object");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void OnSameObject_toGDL_Test_With_Condition_Set()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = true
            };

            // Since the condition was set in constructor, resulting value and string should indicate true
            bool expected = true;
            bool actual = target.ToGDL().Equals("On same object");

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void OnSameObject_toGDL_Test_With_Condition_Set_To_False()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = false
            };

            // Since the condition was set in constructor, resulting value and string should indicate true
            bool expected = true;
            bool actual = target.ToGDL().Equals(string.Empty);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Union Tests

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void OnSameObject_Union_With_A_Null_Test()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                //Note that for this test to pass, the condition must be set to false, as the code specifies either 1st rule or 2nd rule to be true
                Condition = false
            };

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void OnSameObject_Union_With_One_False_Condition()
        {

            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = false
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new OnSameObject()
            {
                Condition = true
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the condition of the union to be the condition of 2nd rule, since it was true
            bool expected = true;
            bool actual = target.Condition.Equals(true);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void OnSameObject_Union_With_Both_False_Conditions()
        {
            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = false
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new OnSameObject()
            {
                Condition = false
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the condition of the union to be the false, since none of the conditions are met
            bool expected = true;
            bool actual = target.Condition.Equals(false);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void OnSameObject_Union_With_Both_True_Conditions()
        {

            // The type we are testing
            OnSameObject target = new OnSameObject()
            {
                Condition = true
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new OnSameObject()
            {
                Condition = true
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the condition of the union to be true, despite both being set to true as per the conditions
            bool expected = true;
            bool actual = target.Condition.Equals(true);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

    }
}
