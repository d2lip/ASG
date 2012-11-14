using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows;

namespace TouchToolkit.Framework.Tests
{


    /// <summary>
    ///This is a test class for TouchPointsCalculatorTest and is intended
    ///to contain all TouchPointsCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchPointsCalculatorTest
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
        public void TouchPointsCalculator_Null_Calculate_Test()
        {
            TouchPointsCalculator tpCalc = new TouchPointsCalculator();
            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();

            TouchPoints returned = tpCalc.Calculate(vp) as TouchPoints;

            Assert.IsTrue(returned.Count == 0);

        }

        [TestMethod()]
        public void TouchPointsCalculator_Calculate_With_A_Count()
        {
            TouchPointsCalculator tpCalc = new TouchPointsCalculator();

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();
            vp.Add(new TouchPoint2(ti1, new UIElement()));

            TouchPoints returned = tpCalc.Calculate(vp) as TouchPoints;

            Assert.IsTrue(returned.Count == 1);
            Assert.AreEqual(returned[0].ToString(), "1,5");
        }

    }
}
