using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.IsolatedStorage;
using TouchToolkit.Framework.Exceptions;
using System.IO;
using System.Runtime.InteropServices;

namespace TouchToolkit.Framework.Storage
{
    public class SilverlightDataStorage : IDataStorage
    {
        GestureDictionary _localCache = new GestureDictionary();
        WebStorage _webStorage;

        IsolatedStorageFile _fileStorage = IsolatedStorageFile.GetUserStoreForApplication();
        static IsolatedStorageSettings _userSettings = IsolatedStorageSettings.ApplicationSettings;

        private bool _firstTime = true;

        public string AccountName
        {
            get
            {
                if (_userSettings.Contains("accountName"))
                {
                    return (string)_userSettings["accountName"];
                }
                else
                {
                    return null;
                }
            }
            protected set
            {
                _userSettings["accountName"] = value;
                _userSettings.Save();
            }
        }

        private string _filename
        {
            get
            {
                return AccountName + ".dat";
            }
        }

        public bool IsLoggedIn()
        {
            if (!string.IsNullOrEmpty(AccountName))
            {
                
                _webStorage.Login(AccountName);
                return true;
            }
            else
                return false;
        }

        public SilverlightDataStorage()
        {
            _webStorage = new WebStorage();
        }


        public void Login(string accountName)
        {
            AccountName = accountName;
            _webStorage.Login(accountName);
            _firstTime = true;
        }

        public void Logout()
        {
            
            AccountName = string.Empty;
            _webStorage.Logout();
            SaveToFile(_filename);
            _localCache = new GestureDictionary();
        }

        #region IDataStorage Members
        public void SaveGesture(string projectName, string gestureName, string value, SaveGestureCallback callback)
        {
            //Save to permanent storage
            _webStorage.SaveGesture(projectName, gestureName, value, (gesName, error) =>
                {
                    if (error == null)
                    {
                        _localCache.Add(projectName, gesName, value);
                    }
                    callback(gesName, error);
                });
            
            SaveToFile(_filename);
        }

        public void GetGesture(string projectName, string gestureName, GetGestureCallback callback)
        {
            Exception e = null;
            string data = string.Empty;
            if (_localCache.Contains(projectName, gestureName))
            {
                data = _localCache.Get(projectName, gestureName);
            }
            else
            {
                e = new Exception("No Gesture or project by that name");
            }

            if (callback != null)
            {
                callback(projectName, gestureName, data, e);
            }
        }

        public void GetAllProjects(GetAllProjectsCallback callback)
        {
            if (_firstTime)
            {
                //Load from the web
                _webStorage.GetAllProjects((projectDetails, error) =>
                {
                    if (error == null)
                    {
                        LoadDictionary(projectDetails);
                        callback(projectDetails);
                    }
                });
                _firstTime = false;
            }
            else
            {
                if (callback != null)
                {
                    callback(_localCache.ProjectDetails);
                }
            }
        }
        #endregion

        #region ILocalStorage Members

        public void ValidateCache()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region FileStorage Members
        private void LoadFromFile(string filename)
        {
            _localCache = new GestureDictionary();
            try
            {
                Stream mystream = new IsolatedStorageFileStream(filename, FileMode.Open, _fileStorage);
                TextReader reader = new StreamReader(mystream);
                string next = reader.ReadLine();
                while (next != null)
                {
                    string projectName = next;
                    string gestureName = reader.ReadLine();
                    string data = reader.ReadLine();
                    _localCache.Add(projectName, gestureName, data);
                    next = reader.ReadLine();
                }

                reader.Close();
            }
            catch(Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
            }
        }

        private void SaveToFile(string filename)
        {
            Stream mystream = new IsolatedStorageFileStream(filename, FileMode.Create, _fileStorage);
            TextWriter writer = new StreamWriter(mystream);
            List<ProjectDetail> details = _localCache.ProjectDetails;
            foreach(var detail in details)
            {
                string project = detail.ProjectName;
                foreach (var gesture in detail.GestureNames)
                {
                    writer.WriteLine(project);
                    writer.WriteLine(gesture);
                    writer.WriteLine(_localCache.Get(project, gesture));
                }
            }
            writer.Flush();
            writer.Close();
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

        #endregion

        private void LoadDictionary(List<ProjectDetail> details)
        {
            foreach (var detail in details)
            {
                string projectName = detail.ProjectName;
                foreach (var gestureName in detail.GestureNames)
                {
                    _webStorage.GetGesture(projectName, gestureName, (projName, gesName, data, error) =>
                    {
                        _localCache.Add(projName, gesName, data);
                    });
                }
            }
        }
    }
}
