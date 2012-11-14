
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BehaviourTypes = TouchToolkit.GestureProcessor.PrimitiveConditions.Objects.DistanceBetweenPoints_Accessor.BehaviourTypes;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;


namespace TouchToolkit.GestureProcessor.Tests.Rules.Objects
{


    /// <summary>
    ///This is a test class for DistanceBetweenPointsTest and is intended
    ///to contain all DistanceBetweenPointsTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DistanceBetweenPointsTest
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
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
        }

        //Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void MyClassCleanup()
        {
        }

        //Use TestInitialize to run code before running each test
        [TestInitialize()]
        public void MyTestInitialize()
        {
        }

        //Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void MyTestCleanup()
        {
        }

        #endregion

        #region Equals Tests

        [TestMethod()]
        public void EqualsTest_Null_Check()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            bool expected = false;
            bool actual = target.Equals(null);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EqualsTest_Different_Types_of_RuleData()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Any random type - just not the type that we are testing
            IPrimitiveConditionData anotherRuleData = new ClosedLoop();

            // Since the ruleData are of different type, it should not be equal
            bool expected = false;
            bool actual = target.Equals(anotherRuleData);

            Assert.AreEqual(expected, actual, "Different types of rules should not match!");
        }

        [TestMethod()]
        public void EqualsTest_Same_Type_of_RuleData()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = -1,
                Min = -1
            };

            // Since the values are different, it should not be equal
            bool expected = false;
            bool actual = target.Equals(anotherRuleData);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void EqualsTest_Same_Type_of_RuleData_with_same_value()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Since the values are same, it should be equal
            bool expected = true;
            bool actual = target.Equals(anotherRuleData);

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Union Tests

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid Union Type")]
        public void Invalid_Union_Test_with_Increasing_and_Decreasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = -1,
                Min = -1
            };

            // Since the behavior is different (increasing and decreasing), appropriate exception should be shown
            target.Union(anotherRuleData);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Invalid Union Type")]
        public void Invalid_Union_Test_with_Decreasing_and_Increasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = -1,
                Min = -1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = -1,
                Min = -1
            };

            // Since the behavior is different (decreasing and increasing), appropriate exception should be shown
            target.Union(anotherRuleData);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Object reference not set to an instance of an object")]
        public void Union_With_A_Null_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 2,
                Min = 1
            };

            // Since the ruleData is null, the union should fail
            IPrimitiveConditionData anotherRuleData = null;

            //Union should fail
            target.Union(anotherRuleData);
        }

        [TestMethod()]
        public void Union_Unchanged_and_Increasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 2,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = 3,
                Min = 2
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the max to be the max of 2nd rule, which is 3
            bool expected = true;
            bool actual = target.Max.Equals(3);
            Assert.AreEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "increasing", given the union types
            expected = true;
            actual = target.Behaviour.Equals("increasing");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Union_Unchanged_and_Decreasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the min to be the min of 2nd rule, which is 0
            bool expected = true;
            bool actual = target.Min.Equals(0);
            Assert.AreEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "decreasing", given the union types
            expected = true;
            actual = target.Behaviour.Equals("decreasing");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Union_Unchanged_and_Unchanged()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the min to be the original min and different from the 2nd rule
            bool expected = true;
            bool actual = target.Min.Equals(0);
            Assert.AreNotEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "unchanged", as nothing was union'd
            expected = true;
            actual = target.Behaviour.Equals("unchanged");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Union_Increasing_and_Unchanged()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the max to be the original max (which is 3) and different from the 2nd rule
            bool expected = true;
            bool actual = target.Max.Equals(3);
            Assert.AreEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "increasing", given the union types
            expected = true;
            actual = target.Behaviour.Equals("increasing");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Union_Decreasing_and_Unchanged()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the min to be the original min and different from the 2nd rule
            bool expected = true;
            bool actual = target.Min.Equals(0);
            Assert.AreNotEqual(expected, actual);

            //We expect the behavior of the union'd rule to still be "decreasing"
            expected = true;
            actual = target.Behaviour.Equals("decreasing");
            Assert.AreEqual(expected, actual);
        }
        
        [TestMethod()]
        public void Union_Increasing_and_Increasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the max to be the max of 2nd rule, which is 4
            bool expected = true;
            bool actual = target.Max.Equals(4);
            Assert.AreEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "increasing", given the union types
            expected = true;
            actual = target.Behaviour.Equals("increasing");
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void Union_Decreasing_and_Decreasing()
        {

            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = 3,
                Min = 1
            };

            // Another instance of same type of ruleData
            IPrimitiveConditionData anotherRuleData = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = 4,
                Min = 0
            };

            //Union the 2 rules
            target.Union(anotherRuleData);

            //We expect the min to be the min of 2nd rule, which is 0
            bool expected = true;
            bool actual = target.Min.Equals(0);
            Assert.AreEqual(expected, actual);

            //We expect the behavior of the union'd rule to be "decreasing", given the union types
            expected = true;
            actual = target.Behaviour.Equals("decreasing");
            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region toGDL Tests

        [TestMethod()]
        public void toGDL_Unchanged_test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 2,
                Min = 0
            };


            // Since the behavior was set to unchanged, that should be reflected in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Distance between points: unchanged 0%");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void toGDL_Range_test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Range,
                Max = 2,
                Min = 0
            };


            // Since the behavior was set to range, that should be reflected in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Distance between points: 0..2");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void toGDL_Increasing_test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing,
                Max = 2,
                Min = 0
            };


            // Since the behavior was set to increasing, that should be reflected in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Distance between points: increasing");

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void toGDL_Decreasing_test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Decreasing,
                Max = 2,
                Min = 0
            };


            // Since the behavior was set to decreasing, that should be reflected in the output of toGDL
            bool expected = true;
            bool actual = target.ToGDL().Equals("Distance between points: decreasing");

            Assert.AreEqual(expected, actual);
        }

        #endregion

        #region Behavior Tests

        [TestMethod()]
        public void Behavior_Setter_Not_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {};

            //Since the Behavior was not set, the string should be empty
            bool expected = true;
            bool actual = target.Behaviour.Equals(string.Empty);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void Behavior_Setter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints() 
            { 
               Behaviour = BehaviourTypes.Decreasing
            };

            //Since the Behavior was set to decreasing, we want to check that this was set
            bool expected = true;
            bool actual = target.Behaviour.Equals(BehaviourTypes.Decreasing);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }


        [TestMethod()]
        public void Behavior_Getter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.Increasing
            };

            //Since the Behavior was set to decreasing, we want to check that this was set
            bool expected = true;
            bool actual = BehaviourTypes.Increasing.Equals(target.Behaviour);

            //Assert they are the equal
            Assert.AreEqual(expected, actual);

        }

        #endregion

        #region Min Tests

        [TestMethod()]
        public void DistanceBtwPoints_Min_Setter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Min = 0
            };

            //The min was set to 0, so we confirm that the min was indeed set to 0
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void DistanceBtwPoints_Min_Setter_Not_Set_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
            };

            //Since the min was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Min.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DistanceBtwPoints_Min_Getter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
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
        public void DistanceBtwPoints_Max_Setter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
                Max = 3
            };

            //The max was set to 3, so we confirm that the min was indeed set to 3
            bool expected = true;
            bool actual = target.Max.Equals(3);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void DistanceBtwPoints_Max_Setter_Not_Set_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
            };

            //Since the max was not set, we are verifying that it was set to 0 as specified in original code
            bool expected = true;
            bool actual = target.Max.Equals(0);

            //Assert they are equal
            Assert.AreEqual(expected, actual);

        }

        [TestMethod()]
        public void DistanceBtwPoints_Max_Getter_Test()
        {
            // The type we are testing
            DistanceBetweenPoints target = new DistanceBetweenPoints()
            {
                Behaviour = BehaviourTypes.UnChanged,
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