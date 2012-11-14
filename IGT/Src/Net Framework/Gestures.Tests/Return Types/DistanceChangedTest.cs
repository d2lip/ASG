using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for DistanceChangedTest and is intended
    ///to contain all DistanceChangedTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DistanceChangedTest
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


        /// <summary>
        ///A test for DistanceChanged Constructor
        ///</summary>
        [TestMethod()]
        public void DistanceChangedConstructorTest()
        {
            DistanceChanged target = new DistanceChanged();
            int test = 0;
            Assert.IsTrue(test == target.Delta);
        }


        #region Distance Tests

        [TestMethod()]
        public void DistanceChanged_Distance_Setter_Test()
        {
            DistanceChanged target = new DistanceChanged()
            {
                Distance = 3
            };

            bool expected = true;
            bool actual = target.Distance.Equals(3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DistanceChanged_Distance_Setter_Not_Set_Test()
        {
            DistanceChanged target = new DistanceChanged();

            bool expected = true;
            bool actual = target.Distance.Equals(0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DistanceChanged_Distance_Getter_Test()
        {
            DistanceChanged target = new DistanceChanged()
            {
                Distance = 4
            };

            int actualD = 4;
            Assert.IsTrue(actualD == target.Distance);
        }

        #endregion

        #region Delta Tests

        [TestMethod()]
        public void DistanceChanged_Delta_Setter_Test()
        {
            DistanceChanged target = new DistanceChanged()
            {
                Delta = 3
            };

            bool expected = true;
            bool actual = target.Delta.Equals(3);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DistanceChanged_Distance_Delta_Not_Set_Test()
        {
            DistanceChanged target = new DistanceChanged();

            bool expected = true;
            bool actual = target.Delta.Equals(0);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod()]
        public void DistanceChanged_Delta_Getter_Test()
        {
            DistanceChanged target = new DistanceChanged()
            {
                Delta = 4
            };

            int actualD = 4;
            Assert.IsTrue(actualD == target.Delta);
        }
        
        #endregion

    }
}
