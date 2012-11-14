using TouchToolkit.Framework.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading;
using System.Collections.Generic;

namespace Framework.Tests.Storage
{


    /// <summary>
    ///This is a test class for StorageManagerTest and is intended
    ///to contain all StorageManagerTest Unit Tests
    ///</summary>
    [TestClass()]
    public class StorageManagerTest
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

        private string GetUniqueGestureName()
        {
            return "TestGesture-" + Guid.NewGuid().ToString();
        }

        private string GetUniqueProjectName()
        {
            return "TestProject-" + Guid.NewGuid().ToString();
        }

        private string GetUniqueUserName()
        {
            return "TestUser-" + Guid.NewGuid().ToString();
        }

        
        /// <summary>
        ///A test for GetAllProjects
        ///</summary>
        [TestMethod()]
        public void GetAllProjectsTest()
        {
            var threadBlocker = new AutoResetEvent(false);
            bool asyncCallbackReceived = false;

            // Data set
            string userName = GetUniqueUserName(),
                projectName = GetUniqueProjectName(),
                gestureName = GetUniqueGestureName(),
                gestureData = "GestureData-" + Guid.NewGuid().ToString();

            StorageManager target = new StorageManager();
            target.Login(userName);

            // 1. Create some gestures and projects
            target.SaveGesture(projectName, gestureName, gestureData, null);

            // 2. Creating inline function as the callback method
            GetAllProjectsCallback callback = (projects, error) =>
            {
                if (error == null)
                {
                    if (projects != null)
                    {
                        if (projects.Count == 1)
                        {
                            if (projects[0].ProjectName == projectName)
                            {
                                if (projects[0].GestureNames.Count == 1)
                                {
                                    if (projects[0].GestureNames[0] == gestureData)
                                    {
                                        Assert.AreSame(projects[0].GestureNames[0], gestureData, "Method failed to return expected values.");

                                    }
                                }
                            }
                        }
                    }
                }

                asyncCallbackReceived = true;
                threadBlocker.Set();
            };

            // 3. Test the method; the callback will validate the return values
            target.GetAllProjects(callback);

            threadBlocker.WaitOne();
            Assert.IsTrue(asyncCallbackReceived, "Failed to receive the async callback");
        }

        /// <summary>
        ///A test for GetGesture
        ///</summary>
        [TestMethod()]
        public void GetGestureTest()
        {
            bool callbackReceived = false;
            var threadBlocker = new AutoResetEvent(false);

            // Data set
            string userName = GetUniqueGestureName(),
                gestureName = GetUniqueGestureName(),
                projectName = GetUniqueProjectName(),
                gestureData = "GestureData-" + Guid.NewGuid();

            StorageManager target = new StorageManager();
            target.Login(userName);

            // 1. Save the test data in storage
            target.SaveGesture(projectName, gestureName, gestureData, (gestureReturnName, errorOnSave) =>
            {
                if (errorOnSave == null)
                {
                    // 2. Get the data from storage and validate
                    target.GetGesture(projectName, gestureReturnName, (projName, gesName, gesData, errorOnGet) =>
                    {
                        if (errorOnGet == null)
                        {
                            // 4. Check the validation result
                            Assert.AreSame(projectName, projName, "Project name doesn't match with the expected value");
                            Assert.AreSame(gestureName, gesName, "Gesture name doesn't match with the expected value");
                            Assert.AreSame(gestureData, gesData, "Gesture data doesn't match with the expected value");
                        }
                        else
                        {
                            Assert.Fail(errorOnGet.Message);
                        }
                    });
                }
                else
                {
                    Assert.Fail(errorOnSave.Message);
                }

                callbackReceived = true;
                threadBlocker.Set();
            });

            threadBlocker.WaitOne();
            Assert.IsTrue(callbackReceived, "Failed to receive the callback from async method");
        }


        /// <summary>
        ///A test for SaveGesture
        ///</summary>
        [TestMethod()]
        public void SaveGestureTest()
        {
            var threadBlocker = new AutoResetEvent(false);
            bool asyncCallbackReceived = false;

            // Data set
            string userName = GetUniqueGestureName(),
                gestureName = GetUniqueGestureName(),
                projectName = GetUniqueProjectName(),
                gestureData = "GestureData-" + Guid.NewGuid();

            StorageManager target = new StorageManager();
            target.Login(userName);

            // 1. Save a new gesture
            target.SaveGesture(projectName, gestureName, gestureData, (gestureReturnName, errorOnSave) =>
            {
                if (errorOnSave == null)
                {
                    // 2. Retrive data from storage to match
                    target.GetGesture(projectName, gestureReturnName, (projName, gesName, gesData, errorOnGet) =>
                    {
                        if (errorOnGet == null)
                        {
                            Assert.AreSame(gestureData, gesData, "Failed to save gesture data");
                        }
                        else
                        {
                            Assert.Fail(errorOnGet.Message);
                        }
                    });
                }
                else
                {
                    Assert.Fail(errorOnSave.Message);
                }

                asyncCallbackReceived = true;
                threadBlocker.Set();
            });

            threadBlocker.WaitOne();
            Assert.IsTrue(asyncCallbackReceived, "Failed to receive callback from the async method");
        }
    }
}
