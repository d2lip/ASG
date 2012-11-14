using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.Exceptions;
using System.Windows;

namespace Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for DistanceChangedCalculatorTest and is intended
    ///to contain all DistanceChangedCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DistanceChangedCalculatorTest
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
        ///A test for Calculate
        ///</summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidDataSetException), "Distance can only be calculated between two points.")]
        public void DistanceChangedCalculator_Calculate_Test_With_1_Input()
        {
            DistanceChangedCalculator target = new DistanceChangedCalculator();
            TouchPoint2 t2 = new TouchPoint2(new TouchInfo(), new System.Windows.UIElement());
            ValidSetOfTouchPoints set = new ValidSetOfTouchPoints();
            set.Add(t2);
            target.Calculate(set);
        }

        [TestMethod]
        [ExpectedException(typeof(NullReferenceException), "Set of points should not be null.")]
        public void DistanceChangedCalculator_Calculate_Test_With_Null_Input()
        {
            DistanceChangedCalculator target = new DistanceChangedCalculator();
            ValidSetOfTouchPoints set = null;
            target.Calculate(set);
        }

        [TestMethod()]
        public void DistanceChangedCalculator_Calculate_Test_With_Low_Counts()
        {
            //Setup preamble for 2 touchpoints to be used in calculate

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(2, 6);

            ValidSetOfTouchPoints test = new ValidSetOfTouchPoints();
            test.Add(new TouchPoint2(ti1, new UIElement()));
            test.Add(new TouchPoint2(ti2, new UIElement()));

            //Call appropriate methods
            DistanceChangedCalculator target = new DistanceChangedCalculator();

            DistanceChanged result = target.Calculate(test) as DistanceChanged;

            Assert.IsTrue(result.Delta == 0);
        }

        [TestMethod()]
        public void DistanceChangedCalculator_Calculate_Test_For_Distance()
        {
            //Setup preamble for 2 touchpoints to be used in calculate

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 5);

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(2, 6);

            ValidSetOfTouchPoints test = new ValidSetOfTouchPoints();
            test.Add(new TouchPoint2(ti1, new UIElement()));
            test.Add(new TouchPoint2(ti2, new UIElement()));

            //Call appropriate methods
            DistanceChangedCalculator target = new DistanceChangedCalculator();

            DistanceChanged result = target.Calculate(test) as DistanceChanged;
            result.Distance = Math.Round(result.Distance, 2);
            bool actual = result.Distance.Equals(1.41);

            Assert.AreEqual(true, actual);
        }
    }
}
