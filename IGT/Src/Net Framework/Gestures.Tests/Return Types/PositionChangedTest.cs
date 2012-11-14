using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TouchToolkit.Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for PositionChangedTest and is intended
    ///to contain all PositionChangedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionChangedTest
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

        [TestMethod()]
        public void PositionChanged_Setters_Not_Test()
        {
            PositionChanged pChange = new PositionChanged();

            int expectedX = 0;
            int expectedY = 0;

            Assert.AreEqual(expectedX, pChange.X);
            Assert.AreEqual(expectedY, pChange.Y);
            
        }

        [TestMethod()]
        public void PositionChanged_X_Setter_Test()
        {
            PositionChanged pChange = new PositionChanged()
            {
                X = -2
            };

            int expectedX = -2;

            Assert.AreEqual(expectedX, pChange.X);
        }

        [TestMethod()]
        public void PositionChanged_X_Getter_Test()
        {
            PositionChanged pChange = new PositionChanged()
            {
                X = -2
            };

            int expectedX = -2;
            Assert.IsTrue(expectedX == pChange.X);
        }

        [TestMethod()]
        public void PositionChanged_Y_Setter_Test()
        {
            PositionChanged pChange = new PositionChanged()
            {
                Y = 5
            };

            int expectedY = 5;

            Assert.AreEqual(expectedY, pChange.Y);
        }

        [TestMethod()]
        public void PositionChanged_Y_Getter_Test()
        {
            PositionChanged pChange = new PositionChanged()
            {
               Y = 3
            };

            int expectedY = 3;
            Assert.IsTrue(expectedY == pChange.Y);
        }

        [TestMethod()]
        public void PositionChanged_ToString_Null_Test()
        {
            PositionChanged pChange = new PositionChanged();

            string expected = "0,0";

            Assert.AreEqual(expected, pChange.ToString());
        }

        [TestMethod()]
        public void PositionChanged_ToString_Test()
        {
            PositionChanged pChange = new PositionChanged()
            {
                X = 2,
                Y = 3

            };

            string expected = "2,3";

            Assert.AreEqual(expected, pChange.ToString());
        }




    }
}
