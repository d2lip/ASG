using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for ClosedLoopTest and is intended
    ///to contain all ClosedLoopTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ClosedLoopTest
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

        #region Equals tests
        /*
        [TestMethod()]
        public void EqualsTest()
        {
            //Equals not implemented yet
        }*/
        #endregion

        #region ToGDL tests
        [TestMethod()]
        public void ClosedLoop_ToGDL_true_test()
        {
            ClosedLoop target = new ClosedLoop();
            target.State = "true";
            string expected = "Closed loop";
            string actual;
            actual = target.ToGDL();
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ClosedLoop_ToGDL_other_test()
        {
            ClosedLoop target = new ClosedLoop();
            target.State = "false";
            string expected = string.Empty;
            string actual;
            actual = target.ToGDL();
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Union tests
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Null Value Exception")]
        public void ClosedLoop_Union_null_throws_exception()
        {
            ClosedLoop target = new ClosedLoop();
            IPrimitiveConditionData value = null;
            target.Union(value);
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Invalid Type Exception")]
        public void ClosedLoop_union_different_type_throws_exception()
        {
            ClosedLoop target = new ClosedLoop();
            EnclosedArea input = new EnclosedArea();
            target.Union(input);
        }
        [TestMethod()]
        public void ClosedLoop_union_two_false_gives_false()
        {
            ClosedLoop target = new ClosedLoop()
            {
                State = "false"
            };
            ClosedLoop input = new ClosedLoop()
            {
                State = "false"
            };

            target.Union(input);
            string expected = "false";
            string actual = target.State;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ClosedLoop_union_false_and_true_gives_false()
        {
            ClosedLoop target = new ClosedLoop()
            {
                State = "false"
            };
            ClosedLoop input = new ClosedLoop()
            {
                State = "true"
            };

            target.Union(input);
            string expected = "false";
            string actual = target.State;
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void ClosedLoop_union_true_and_true_gives_true()
        {
            ClosedLoop target = new ClosedLoop()
            {
                State = "true"
            };
            ClosedLoop input = new ClosedLoop()
            {
                State = "true"
            };

            target.Union(input);
            string expected = "true";
            string actual = target.State;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region state tests
        [TestMethod()]
        public void ClosedLoop_state_get_and_set_work()
        {
            ClosedLoop target = new ClosedLoop();
            target.State = "false";
            string expected = "false";
            string actual = target.State;
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
