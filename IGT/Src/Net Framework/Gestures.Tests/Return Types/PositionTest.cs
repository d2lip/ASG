using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Windows;

namespace Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for PositionTest and is intended
    ///to contain all PositionTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionTest
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

        /*
        [TestMethod()]
        public void Position_ToString_Null_Test()
        {
            Position target = new Position();

            string expected = "X:0,Y:0";

            Assert.AreEqual(expected, target.ToString());
        }

        [TestMethod()]
        public void Position_ToString_Set_Test()
        {
            Position target = new Position()
            {
                X = 2,
                Y = 1
            };

            string expected = "X:2,Y:1";

            Assert.AreEqual(expected, target.ToString());
        }

        [TestMethod()]
        public void Position_X_Setter_Getter_Test()
        {
            Position target = new Position()
            {
               X = 2
            };

            int actual = 2;

            Assert.IsTrue(actual == target.X);
        }

        [TestMethod()]
        public void Position_Y_Setter_Getter_Test()
        {
            Position target = new Position()
            {
                Y = 2
            };

            int actual = 2;

            Assert.IsTrue(actual == target.Y);
        }
  
         */ }
}
