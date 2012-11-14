using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TouchToolkit.GestureProcessor.Tests
{
    
    [TestClass()]
    public class TouchAreaTest
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

        #region Constructor tests
        [TestMethod()]
        public void TouchArea_circle_width_same_as_height()
        {
            TouchArea target = new TouchArea();
            target.Type = "Circle";
            target.Value = "34";
            int expected = target.Height;
            int actual = target.Width;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Equals tests

        [TestMethod()]
        public void TouchArea_different_types_equals_false()
        {
            TouchArea target = new TouchArea();
            TouchTime ruleData = new TouchTime();
            bool expected = false;
            bool actual = target.Equals(ruleData);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TouchArea_two_empty_values_equals_true()
        {
            TouchArea target = new TouchArea();
            TouchArea ruleData = new TouchArea();
            bool expected = true;
            bool actual = target.Equals(ruleData);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TouchArea_empty_and_nonempty_equals_false()
        {
            TouchArea target = new TouchArea();
            TouchArea ruleData = new TouchArea()
            {
                Type = "ellipse",
                Value = "4x5"
            };
            bool expected = false;
            bool actual = target.Equals(ruleData);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TouchArea_different_values_equals_false()
        {
            TouchArea target = new TouchArea()
            {
                Type = "Rect",
                Value = "3x3"
            };
            TouchArea ruleData = new TouchArea()
            {
                Type = "Circle",
                Value = "3"
            };
            bool expected = false;
            bool actual = target.Equals(ruleData);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        public void TouchArea_same_values_equals_true()
        {
            TouchArea target = new TouchArea()
                {
                    Type = "Circle",
                    Value = "3"
                };
            TouchArea ruleData = new TouchArea()
                {
                    Type = "Circle",
                    Value = "3"
                };
            bool expected = true;
            bool actual = target.Equals(ruleData);
            Assert.AreEqual(expected, actual);
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Null Value Exception")]
        public void TouchArea_equals_null_throws_exception()
        {
            TouchArea target = new TouchArea();
            TouchArea data = null;
            target.Equals(data);
        }
        #endregion

        #region GDL tests
        [TestMethod()]
        public void TouchArea_toGDL_Standard_Input()
        {
            TouchArea target = new TouchArea();
            target.Value = "10x5";
            target.Type = "Rect";
            string expected = "TouchArea: Rect 10x5";
            string actual;
            actual = target.ToGDL();
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Union tests
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Null Value Exception")]
        public void TouchArea_union_null_throws_exception()
        {
            TouchArea Target = new TouchArea();
            TouchArea input = null;
            Target.Union(input);
        }
        [TestMethod()]
        [ExpectedException(typeof(Exception), "TouchArea union not TouchArea exception")]
        public void TouchArea_union_different_type_throws_exception()
        {
            TouchArea target = new TouchArea();
            ClosedLoop input = new ClosedLoop();
            target.Union(input);
        }
        [TestMethod()]
        public void TouchArea_same_types_union_gives_same()
        {
            TouchArea target = new TouchArea()
                {
                    Type = "Circle",
                    Value = "4"
                };
            TouchArea value = new TouchArea()
                {
                    Type = "Circle",
                    Value = "6"
                };
            target.Union(value);
            Assert.IsTrue( target.Equals(value));
        }
        [TestMethod()]
        public void TouchArea_union_smaller_object_gives_larger()
        {
            TouchArea larger = new TouchArea()
            {
                Type = "Rect",
                Value = "6x4"
            };
            TouchArea smaller = new TouchArea()
            {
                Type = "Rect",
                Value = "2x2"
            };
            TouchArea expected = new TouchArea()
            {
                Type = "Rect",
                Value = "6x4"
            };
            larger.Union(smaller);
            Assert.IsTrue(larger.Equals(expected));
        }
        [TestMethod()]
        public void TouchArea_union_larger_object_gives_larger()
        {
            TouchArea larger = new TouchArea()
            {
                Type = "Rect",
                Value = "6x4"
            };
            TouchArea smaller = new TouchArea()
            {
                Type = "Rect",
                Value = "2x2"
            };
            TouchArea expected = new TouchArea()
            {
                Type = "Rect",
                Value = "6x4"
            };
            smaller.Union(larger);
            Assert.IsTrue(smaller.Equals(expected));
        }
        [TestMethod()]
        public void TouchArea_union_gives_largest_values_for_all_dimensions()
        {
            TouchArea target = new TouchArea()
            {
                Type = "Ellipse",
                Value = "2x4"
            };
            TouchArea input = new TouchArea()
            {
                Type = "Ellipse",
                Value = "6x2"
            };
            TouchArea expected = new TouchArea()
            {
                Type = "Ellipse",
                Value = "6x4"
            };
            target.Union(input);
            Assert.IsTrue(target.Equals(expected));
        }
        [TestMethod()]
        public void TouchArea_union_different_type_gives_rect()
        {
            TouchArea target = new TouchArea()
            {
                Type = "Circle",
                Value = "4"
            };
            TouchArea input = new TouchArea()
            {
                Type = "Ellipse",
                Value = "2x3"
            };
            TouchArea expected = new TouchArea()
            {
                Type = "Rect",
                Value = "4x4"
            };
            target.Union(input);
            Assert.IsTrue(target.Equals(expected));
        }
        #endregion
    }
}
