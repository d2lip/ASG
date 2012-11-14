using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace TouchToolkit.GestureProcessor.Tests
{
    
    
    /// <summary>
    ///This is a test class for TouchPathBoundingBoxTest and is intended
    ///to contain all TouchPathBoundingBoxTest Unit Tests
    ///</summary>
    [TestClass()]
    public class TouchPathBoundingBoxTest
    {

        private TestContext testContextInstance;

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

        #region Equals tests
        /*
        [TestMethod()]
        public void EqualsTest()
        {
            //This method was not implemented
        }*/
        #endregion

        #region ToGDL tests
        [TestMethod()]
        public void TouchPathBoundingBox_toGDL_normal_input_test()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
                {
                    MaxHeight = 6,
                    MinHeight = 4,
                    MaxWidth = 4,
                    MinWidth = 2
                };
            string expected = "Touch path bounding box: 4x2..6x4";
            string actual = target.ToGDL();
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region Union tests
        [TestMethod()]
        [ExpectedException(typeof(Exception), "Null Value Exception")]
        public void TouchPathBoundingBox_union_null_throws_exception()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            TouchPathBoundingBox value = null;
            target.Union(value);
        }

        [TestMethod()]
        [ExpectedException(typeof(Exception), "Wrong Input Type Exception")]
        public void TouchPathBoundingBox_union_different_type_throws_exception()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            ClosedLoop value = new ClosedLoop();
            target.Union(value);
        }

        [TestMethod()]
        public void TouchBoundingBox_union_subset_gives_same()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
            {
                MaxHeight = 9,
                MaxWidth = 9,
                MinHeight = 1,
                MinWidth = 1
            };
            TouchPathBoundingBox input = new TouchPathBoundingBox()
            {
                MaxHeight = 8,
                MaxWidth = 8,
                MinHeight = 2,
                MinWidth = 2
            };
            target.Union(input);
            double expected_max_height = 9;
            double expected_max_width = 9;
            double expected_min_height = 1;
            double expected_min_width = 1;
            

            double actual_min_height = target.MinHeight;
            double actual_max_height = target.MaxHeight;
            double actual_min_width = target.MinWidth;
            double actual_max_width = target.MaxWidth;

            Assert.AreEqual<double>(expected_max_height, actual_max_height);
            Assert.AreEqual<double>(expected_min_height, actual_min_height);
            Assert.AreEqual<double>(expected_max_width, actual_max_width);
            Assert.AreEqual<double>(expected_min_width, actual_min_width);
        }
        [TestMethod()]
        public void TouchBoundingBox_union_superset_gives_superset()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
            {
                MaxHeight = 4,
                MaxWidth = 3,
                MinHeight = 2,
                MinWidth = 1
            };
            TouchPathBoundingBox input = new TouchPathBoundingBox()
            {
                MaxHeight = 5,
                MaxWidth = 6,
                MinHeight = 0,
                MinWidth = -1
            };
            target.Union(input);
            double expected_max_height = 5;
            double expected_max_width = 6;
            double expected_min_height = 0;
            double expected_min_width = -1;


            double actual_min_height = target.MinHeight;
            double actual_max_height = target.MaxHeight;
            double actual_min_width = target.MinWidth;
            double actual_max_width = target.MaxWidth;

            Assert.AreEqual<double>(expected_max_height, actual_max_height);
            Assert.AreEqual<double>(expected_min_height, actual_min_height);
            Assert.AreEqual<double>(expected_max_width, actual_max_width);
            Assert.AreEqual<double>(expected_min_width, actual_min_width);
        }
        [TestMethod()]
        public void TouchBoundingBox_union_offset_gives_boundry()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
            {
                MaxHeight = 4,
                MaxWidth = 3,
                MinHeight = 2,
                MinWidth = 1
            };
            TouchPathBoundingBox input = new TouchPathBoundingBox()
            {
                MaxHeight = 9,
                MaxWidth = 8,
                MinHeight = 7,
                MinWidth = 6
            };
            target.Union(input);
            double expected_max_height = 9;
            double expected_max_width = 8;
            double expected_min_height = 2;
            double expected_min_width = 1;


            double actual_min_height = target.MinHeight;
            double actual_max_height = target.MaxHeight;
            double actual_min_width = target.MinWidth;
            double actual_max_width = target.MaxWidth;

            Assert.AreEqual<double>(expected_max_height, actual_max_height);
            Assert.AreEqual<double>(expected_min_height, actual_min_height);
            Assert.AreEqual<double>(expected_max_width, actual_max_width);
            Assert.AreEqual<double>(expected_min_width, actual_min_width);
        }
        [TestMethod()]
        public void TouchBoundingBox_union_negative_values()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
            {
                MaxHeight = 4,
                MaxWidth = 3,
                MinHeight = 2,
                MinWidth = 1
            };
            TouchPathBoundingBox input = new TouchPathBoundingBox()
            {
                MaxHeight = -9,
                MaxWidth = -8,
                MinHeight = -7,
                MinWidth = -6
            };
            target.Union(input);
            double expected_max_height = 4;
            double expected_max_width = 3;
            double expected_min_height = -7;
            double expected_min_width = -6;


            double actual_min_height = target.MinHeight;
            double actual_max_height = target.MaxHeight;
            double actual_min_width = target.MinWidth;
            double actual_max_width = target.MaxWidth;

            Assert.AreEqual<double>(expected_max_height, actual_max_height);
            Assert.AreEqual<double>(expected_min_height, actual_min_height);
            Assert.AreEqual<double>(expected_max_width, actual_max_width);
            Assert.AreEqual<double>(expected_min_width, actual_min_width);
        }
        [TestMethod()]
        public void TouchBoundingBox_union_very_large_values()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox()
            {
                MaxHeight = 4,
                MaxWidth = 3,
                MinHeight = 2,
                MinWidth = 1
            };
            TouchPathBoundingBox input = new TouchPathBoundingBox()
            {
                MaxHeight = 1235146141234165,
                MaxWidth = 98709878783251,
                MinHeight = 3450987872198,
                MinWidth = 12348785123412
            };
            target.Union(input);
            double expected_max_height = 1235146141234165;
            double expected_max_width = 98709878783251;
            double expected_min_height = 2;
            double expected_min_width = 1;


            double actual_min_height = target.MinHeight;
            double actual_max_height = target.MaxHeight;
            double actual_min_width = target.MinWidth;
            double actual_max_width = target.MaxWidth;

            Assert.AreEqual<double>(expected_max_height, actual_max_height);
            Assert.AreEqual<double>(expected_min_height, actual_min_height);
            Assert.AreEqual<double>(expected_max_width, actual_max_width);
            Assert.AreEqual<double>(expected_min_width, actual_min_width);
        }
        #endregion

        #region MaxHeight tests
        [TestMethod()]
        public void TouchPathBoundingBox_maxheight_get_same_as_set()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            double expected = 10;
            double actual;
            target.MaxHeight = expected;
            actual = target.MaxHeight;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region MaxWidth tests
        [TestMethod()]
        public void TouchPathBoundingBox_maxwidth_get_same_as_set()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            double expected = 10;
            double actual;
            target.MaxWidth = expected;
            actual = target.MaxWidth;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region MinHeight tests
        [TestMethod()]
        public void TouchPathBoundingBox_minheight_get_same_as_set()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            double expected = 10;
            double actual;
            target.MinHeight = expected;
            actual = target.MinHeight;
            Assert.AreEqual(expected, actual);
        }
        #endregion

        #region MinWidth tests
        [TestMethod()]
        public void TouchPathBoundingBox_minwidth_get_same_as_set()
        {
            TouchPathBoundingBox target = new TouchPathBoundingBox();
            double expected = 10;
            double actual;
            target.MinWidth = expected;
            actual = target.MinWidth;
            Assert.AreEqual(expected, actual);
        }
        #endregion
    }
}
