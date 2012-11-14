using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Exceptions;
using System.Windows;

namespace TouchToolkit.Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for PositionChangedCalculatorTest and is intended
    ///to contain all PositionChangedCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionChangedCalculatorTest
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


        [TestMethod]
        [ExpectedException(typeof(InvalidDataSetException), "At least one touch point required to calculate position change. The parameter contains 0 touch point(s)")]
        public void PositionChanged_Exception_Test()
        {
            PositionChangedCalculator target = new PositionChangedCalculator();

            ValidSetOfTouchPoints tps = new ValidSetOfTouchPoints();

            target.Calculate(tps);
        }

        [TestMethod()]
        public void PositionChanged_Test()
        {
            PositionChangedCalculator target = new PositionChangedCalculator();
            int expectedY = 0;
            int expectedX = 0;

            //Setup preamble for 2 touchpoints to be used in calculate

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(3, 6);

            ValidSetOfTouchPoints test = new ValidSetOfTouchPoints();
            test.Add(new TouchPoint2(ti1, new UIElement()));
            test.Add(new TouchPoint2(ti2, new UIElement()));

            PositionChanged actualP = target.Calculate(test) as PositionChanged;

            //Assert that both X and Y of position are correct after calculation
            Assert.AreEqual(expectedX, actualP.X);
            Assert.AreEqual(expectedY, actualP.Y);
        }



    }
}
