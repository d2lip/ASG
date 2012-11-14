using TouchToolkit.GestureProcessor.ReturnTypes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TouchToolkit.GestureProcessor.Objects;
using System.Windows;

namespace TouchToolkit.Framework.Tests
{
    
    
    /// <summary>
    ///This is a test class for PositionCalculatorTest and is intended
    ///to contain all PositionCalculatorTest Unit Tests
    ///</summary>
    [TestClass()]
    public class PositionCalculatorTest
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
        /*
        [TestMethod()]
        public void PositionCalculator_Zero_Test()
        {
            ValidSetOfTouchPoints vp = new ValidSetOfTouchPoints();

            PositionCalculator pc = new PositionCalculator();

            Assert.AreEqual(null, pc.Calculate(vp));
        }

        [TestMethod()]
        public void PositionCalculator_Calculate_Test()
        {
            PositionCalculator pCalc = new PositionCalculator();

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

            Position p_ret = pCalc.Calculate(test) as Position;

            int expectedY = 5;
            int expectedX = 1;

            //Assert that both X and Y of position are correct after calculation
            Assert.AreEqual(expectedX, p_ret.X);
            Assert.AreEqual(expectedY, p_ret.Y);

        }
        */

    }
}
