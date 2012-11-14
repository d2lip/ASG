using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows;

namespace Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for InfoCalculatorTest and is intended
    ///to contain all InfoCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class InfoCalculatorTest
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
        [TestMethod()]
        public void InfoCalculator_Null_Input_Test()
        {
            InfoCalculator target = new InfoCalculator();
            ValidSetOfTouchPoints set = null;
            
            Info actual = target.Calculate(set) as Info;
            Assert.IsTrue(actual.Message == string.Empty);
        }

        [TestMethod()]
        public void InfoCalculator_Constructor_Test()
        {
            InfoCalculator target = new InfoCalculator();

            Assert.IsTrue(target.Message == string.Empty);
        }

        [TestMethod()]
        public void InfoCalculator_Valid_Input_Test()
        {
            //Setup preamble for 2 touchpoints to be used in calculate

            TouchInfo ti1 = new TouchInfo();
            ti1.TouchDeviceId = 2;
            ti1.ActionType = TouchAction2.Down;
            ti1.Position = new Point(1, 6);

            TouchInfo ti2 = new TouchInfo();
            ti2.TouchDeviceId = 2;
            ti2.ActionType = TouchAction2.Down;
            ti2.Position = new Point(2, 5);

            ValidSetOfTouchPoints test = new ValidSetOfTouchPoints();
            test.Add(new TouchPoint2(ti1, new UIElement()));
            test.Add(new TouchPoint2(ti2, new UIElement()));
            

            //Call appropriate methods to test the calculate
            InfoCalculator target = new InfoCalculator();
            Info result = target.Calculate(test) as Info;
            
            Assert.IsTrue(result.Message == string.Empty);
        }

    }
}
