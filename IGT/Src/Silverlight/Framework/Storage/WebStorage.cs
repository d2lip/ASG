using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.Framework.Exceptions;

namespace TouchToolkit.Framework.Storage
{
    public class WebStorage : IDataStorage
    {
        private string _accountName = string.Empty;
        TouchToolkit.Framework.DataService.GestureServiceSoapClient _dataService = new TouchToolkit.Framework.DataService.GestureServiceSoapClient();

        public string AccountName
        {
            get
            {
                return _accountName;
            }
        }

        public bool IsLoggedIn()
        {
            return !string.IsNullOrEmpty(AccountName);
        }

        public WebStorage()
        {
            Init();
        }

        private void Init()
        {
            // Subscribe to webservice async callback events
            _dataService.AddGestureDataCompleted += _dataService_AddGestureDataCompleted;
            _dataService.GetGestureDataCompleted += _dataService_GetGestureDataCompleted;
            _dataService.GetProjectsByUserCompleted += _dataService_GetProjectsByUserCompleted;
            _dataService.ConnectivityCheckCompleted += _dataService_ConnectivityCheckCompleted;

            // Check service connectivity
            _dataService.ConnectivityCheckAsync();
        }

        
        void _dataService_ConnectivityCheckCompleted(object sender, TouchToolkit.Framework.DataService.ConnectivityCheckCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new FrameworkException("Failed to connect to remote data storage");
            }
        }


        #region Save Gesture
        public void SaveGesture(string projectName, string gestureName, string data, SaveGestureCallback callback)
        {
            Tuple<string, string, SaveGestureCallback> state = new Tuple<string, string, SaveGestureCallback>(projectName, gestureName, callback);

            _dataService.AddGestureDataAsync(_accountName, projectName, gestureName, data, state);
        }

        private void _dataService_AddGestureDataCompleted(object sender, TouchToolkit.Framework.DataService.AddGestureDataCompletedEventArgs e)
        {
            // Get the asyncState object that was passed while calling the async method
            var asyncState = e.UserState as Tuple<string, string, SaveGestureCallback>;

            // Notify client through the callback method
            asyncState.Item3(e.Result, e.Error);
        }

        
        #endregion

        #region Get Gesture
        public void GetGesture(string projectName, string gestureName, GetGestureCallback callback)
        {
            Tuple<string, string, GetGestureCallback> state = new Tuple<string, string, GetGestureCallback>(projectName, gestureName, callback);

            _dataService.GetGestureDataAsync(_accountName, projectName, gestureName, state);
        }

        private void _dataService_GetGestureDataCompleted(object sender, TouchToolkit.Framework.DataService.GetGestureDataCompletedEventArgs e)
        {
            // Get the asyncState object that was passed while calling the async method
            var asyncState = e.UserState as Tuple<string, string, GetGestureCallback>;

            asyncState.Item3(asyncState.Item1, asyncState.Item2, e.Result, e.Error);
        }
        #endregion

        #region Get All Projects
        public void GetAllProjects(GetAllProjectsCallback callback)
        {
            _dataService.GetProjectsByUserAsync(_accountName, callback);
        }

        void _dataService_GetProjectsByUserCompleted(object sender, TouchToolkit.Framework.DataService.GetProjectsByUserCompletedEventArgs e)
        {
            GetAllProjectsCallback callback = e.UserState as GetAllProjectsCallback;

            List<ProjectDetail> projects = new List<ProjectDetail>();

            if (e.Error == null)
            {
                foreach (var projectInfo in e.Result)
                {
                    ProjectDetail p = new ProjectDetail()
                    {
                        ProjectName = projectInfo.ProjectName,
                        GestureNames = projectInfo.GestureNames.ToList<string>()
                    };

                    projects.Add(p);
                }

                callback(projects);
            }
            else
            {
                callback(null, e.Error);
            }
        }

        #endregion

        public void Login(string accountName)
        {
            _accountName = accountName;
        }

        public void Logout()
        {
            _accountName = string.Empty;
        }
    }
}
