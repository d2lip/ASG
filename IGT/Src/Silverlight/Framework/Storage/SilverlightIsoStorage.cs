using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using Framework.Exceptions;

namespace Framework.Storage
{
    internal class SilverlightIsoStorage : IDataStorage
    {
        IsolatedStorageSettings _userSettings = IsolatedStorageSettings.ApplicationSettings;

        private string _accountName = string.Empty;

        public void Login(string accountName)
        {
            _accountName = accountName;
        }

        public void Logout()
        {
            _accountName = string.Empty;
        }

        public string AccountName
        {
            get
            {
                return _accountName;
            }
        }

        private List<string> ProjectNames
        {
            get
            {
                if (_userSettings["projects"] == null)
                    _userSettings["projects"] = new List<string>();

                return (List<string>)_userSettings["projects"];
            }
        }

        public void SaveGesture(string projectName, string gestureName, string value, SaveGestureCallback callback)
        {
            if( !ProjectNames.Contains(projectName))
                ProjectNames.Add(projectName);

            _userSettings.Add(gestureDataKey, data);
            if (CheckIsolatedStorageAvaliableSpace(data))
            {
                _userSettings.Save();
                callback();
            }
            else
            {
                callback(new FrameworkException("Failed to save in local cache. Not enough space available!");
            }
        }

        public void GetGesture(string projectName, string gestureName, GetGestureCallback callback)
        {
            throw new NotImplementedException();
        }

        public void GetGestureNames(string projectName, GetGestureNamesCallback callback)
        {
            throw new NotImplementedException();
        }

        public void GetAllProjects(GetAllProjectsCallback callback)
        {
            throw new NotImplementedException();
        }

        private bool CheckIsolatedStorageAvaliableSpace(string data)
        {
            Int64 requiredSpace = data.Count() * 2;
            Int64 fiveMB = 1024 * 1024 * 5;
            bool isEnoughSpaceAvailable = false;

            IsolatedStorageFile storage = null;
            try
            {
                storage = IsolatedStorageFile.GetUserStoreForApplication();
                if (storage.AvailableFreeSpace < requiredSpace)
                {
                    long newQuota = storage.Quota + fiveMB;
                    isEnoughSpaceAvailable = storage.IncreaseQuotaTo(newQuota);

                    if (!isEnoughSpaceAvailable)
                    {
                        throw new FrameworkException("Failed to save content in local cache due to space limitation!");
                    }
                }
                else
                {
                    isEnoughSpaceAvailable = true;
                }
            }
            catch
            {
                //TODO: handle possible error
            }

            return isEnoughSpaceAvailable;
        }
    }
}
