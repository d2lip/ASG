using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TouchToolkit.Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for SlopeChangedTest and is intended
    ///to contain all SlopeChangedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SlopeChangedTest
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
        public void SlopeChanged_Nothing_Set_Test()
        {
            SlopeChanged sChanged = new SlopeChanged();

            int expectedSlope = 0;
            int expectedDelta = 0;

            Assert.IsTrue(expectedSlope == sChanged.NewSlope);
            Assert.IsTrue(expectedDelta == sChanged.NewSlope);

        }

        [TestMethod()]
        public void SlopeChanged_To_String_Test()
        {
            SlopeChanged sChanged = new SlopeChanged();

            string expected = string.Format("Delta: 0, NewSlope: 0");

            Assert.AreEqual(expected, sChanged.ToString());
        }

        [TestMethod()]
        public void SlopeChanged_NewSlope_GetterSetter_Test()
        {

            SlopeChanged sChanged = new SlopeChanged()
            {
                NewSlope = 3
            };

            int expected = 3;

            Assert.AreEqual(expected, sChanged.NewSlope);

        }

        [TestMethod()]
        public void SlopeChanged_Delta_GetterSetter_Test()
        {
            SlopeChanged sChanged = new SlopeChanged()
            {
                Delta = 4
            };

            int expected = 4;

            Assert.AreEqual(expected, sChanged.Delta);
        }



    }
}
