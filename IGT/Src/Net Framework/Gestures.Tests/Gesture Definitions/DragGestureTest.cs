using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Windows.Controls;
using TouchToolkit.GestureProcessor.Utility;
using TouchToolkit.GestureProcessor.ReturnTypes;
using System.Threading;
using TouchToolkit.Framework;
using TouchToolkit.GestureProcessor.Gesture_Definitions;

namespace TouchToolkit.GestureProcessor.Tests.Gesture_Definitions
{
    [TestClass]
    public class DragGestureTest
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

        [ClassInitialize()]
        public static void InitializeTest(TestContext testContext)
        {
            GestureTestFramework.Init("DefaultTT", "WPF 4.0 Testing");
        }

        [TestMethod]
        public void Drag()
        {
            bool gestureDetected = false;
            Position storedP = new Position();
            PositionChanged storedPositionChanged = new PositionChanged();
            var threadHolder = new AutoResetEvent(false);

            GestureTestFramework.Validate("Drag", "Drag",

                // On successful gesture detection
                (sender, e) =>
                {
                    gestureDetected = true;

                    if (e.Error == null)
                    {
                        var position = e.Values.Get<Position>();
                        var positionChanged = e.Values.Get<PositionChanged>();
                        Assert.IsNotNull(positionChanged, "Failed to retrieve return value: position-changed");
                        Assert.IsNotNull(position, "Failed to retrieve return value: position");

                        Assert.IsTrue(!position.Equals(storedP), "The position of the drag did not change, incorrect movement");
                        Assert.IsTrue(!positionChanged.Equals(storedPositionChanged), "The position changed did not change, incorrect movement");

                        storedP = position;
                        storedPositionChanged = positionChanged;
                    }
                    else
                    {
                        Assert.Fail(e.Error.Message);
                    }
                },

                // On gesture playback completion
                () =>
                {
                    threadHolder.Set();
                });

            threadHolder.WaitOne();
            Assert.IsTrue(gestureDetected, "Failed to detect the gesture!");
        }
    }
}
