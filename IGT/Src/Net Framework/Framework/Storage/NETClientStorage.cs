using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.Framework.Storage;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows;

namespace TouchToolkit.Framework.Storage
{
    public class NETClientStorage : IDataStorage
    {
        GestureDictionary _localCache = new GestureDictionary();
        WebStorage _webStorage;

        private string _accountName;

        private bool _firstTime = true;

        //Checks to see if the server is available
        private bool HasConnection(string hostName)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 120;
            bool connection = false;
            try
            {
                PingReply reply = pingSender.Send(hostName, timeout, buffer, options);
                connection = reply.Status == IPStatus.Success;
            }
            catch
            {
                return false;
            }
            return connection;
        }

        public NETClientStorage()
        {
            _webStorage = new WebStorage();
        }
        public string AccountName
        {
            get
            {
                if (_accountName != string.Empty)
                {
                    return _accountName;
                }
                else
                {
                    return string.Empty;
                }
            }
            protected set
            {
                _accountName = value;
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

        }

        #region IDataStorage Members
        public void SaveGesture(string projectName, string gestureName, string value, SaveGestureCallback callback)
        {
            //Save to local cache
            _localCache.Add(projectName, gestureName, value);
            SaveToFile(_filename);
            //Save to permanent storage
            _webStorage.SaveGesture(projectName, gestureName, value, callback);
        }

        public void GetGesture(string projectName, string gestureName, GetGestureCallback callback)
        {
            Exception e = null;
            string data = string.Empty;
            if (_firstTime)
            {
                GetAllProjects(null);
                Thread.Sleep(10000); //Wait for all projects to download from web
            }
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
                _webStorage.GetAllProjects((projectDetails, error) =>
                {
                    if (error == null)
                    {
                        LoadDictionary(projectDetails);
                        if (callback != null)
                        {
                            callback(projectDetails);
                        }
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
        public void LoadFromFile(string filename)
        {
            
            _localCache = new GestureDictionary();
            try
            {
                FileStream file = new FileStream(filename, FileMode.Open);
                TextReader reader = new StreamReader(file);
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
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
            }
            finally
            {
            }
        }

        public void SaveToFile(string filename)
        {
            FileStream file = new FileStream(filename, FileMode.Create);
            TextWriter writer = new StreamWriter(file);
            List<ProjectDetail> details = _localCache.ProjectDetails;
            foreach (var detail in details)
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
