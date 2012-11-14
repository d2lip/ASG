using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Exceptions;
using System.Windows;

namespace TouchToolkit.Framework.Tests
{


    /// <summary>
    ///This is a test class for SlopeChangedCalculatorTest and is intended
    ///to contain all SlopeChangedCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class SlopeChangedCalculatorTest
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
        [ExpectedException(typeof(InvalidDataSetException), "Slope can only be calculated for two touch points!")]
        public void SlopeChangedCalculator_Calculate_Null_Test()
        {
            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();

            SlopeChangedCalculator sc = new SlopeChangedCalculator();

            Assert.AreEqual(null, sc.Calculate(vp));

        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidDataSetException), "Slope can only be calculated for two touch points!")]
        public void SlopeChangedCalculator_Calculate_Low_Count_Test()
        {

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();
            vp.Add(new TouchPoint2(ti1, new UIElement()));

            SlopeChangedCalculator sc = new SlopeChangedCalculator();

            Assert.AreEqual(null, sc.Calculate(vp));
        }

        [TestMethod()]
        public void SlopeChangedCalculator_Calculate_No_History_Test()
        {
            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(3, 6);

            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();
            vp.Add(new TouchPoint2(ti1, new UIElement()));
            vp.Add(new TouchPoint2(ti2, new UIElement()));

            SlopeChangedCalculator sc = new SlopeChangedCalculator();

            SlopeChanged actualP = sc.Calculate(vp) as SlopeChanged;

            double expectedSlope = 26.58;
            double expectedDelta = 0;
            Console.Out.WriteLine(actualP.Delta);
            Assert.IsTrue(expectedDelta == actualP.Delta);
            Assert.IsTrue(expectedSlope == Math.Round(actualP.NewSlope, 2));
        }

        [TestMethod()]
        public void SlopeChangedCalculator_Calculate_With_History_Test()
        {
            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);
            TouchPoint2 tp1 = new TouchPoint2(ti1, new UIElement());
            tp1.Stroke.StylusPoints.Add(new System.Windows.Input.StylusPoint());
            tp1.Stroke.StylusPoints.Add(new System.Windows.Input.StylusPoint());

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(3, 6);
            TouchPoint2 tp2 = new TouchPoint2(ti1, new UIElement());
            tp2.Stroke.StylusPoints.Add(new System.Windows.Input.StylusPoint());
            tp2.Stroke.StylusPoints.Add(new System.Windows.Input.StylusPoint());

            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();
            vp.Add(tp1);
            vp.Add(tp2);

            SlopeChangedCalculator sc = new SlopeChangedCalculator();

            SlopeChanged actualP = sc.Calculate(vp) as SlopeChanged;

            double expectedSlope = 0;
            double expectedDelta = 0;

            Assert.IsTrue(expectedDelta == actualP.Delta);
            Assert.AreEqual(expectedSlope, Math.Round(actualP.NewSlope, 2));
        }
    }
}
