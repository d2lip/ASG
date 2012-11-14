using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace TouchToolkit.Framework.Storage
{
    public sealed class StorageManager : IDataStorage
    {
        private IDataStorage _storage = null;

        public StorageManager()
        {

#if SILVERLIGHT
            _storage = new SilverlightDataStorage();
#elif XNA
            _storage = new XNAClientStorage();
#else
            _storage = new NETClientStorage();
#endif
        }

        #region IDataStorage Members


        public void SaveGesture(string projectName, string gestureName, string value, SaveGestureCallback callback)
        {
            _storage.SaveGesture(projectName, gestureName, value, callback);
        }

        public void GetGesture(string projectName, string gestureName, GetGestureCallback callback)
        {
            _storage.GetGesture(projectName, gestureName, callback);
        }

        public void GetAllProjects(GetAllProjectsCallback callback)
        {
            _storage.GetAllProjects(callback);
        }

        public void ValidateCache()
        {

        }
        #endregion

        public string AccountName
        {
            get { return _storage.AccountName; }
        }

        public bool IsLoggedIn()
        {
            return _storage.IsLoggedIn();
        }

        public void Login(string accountName)
        {
            _storage.Login(accountName);
        }

        public void Logout()
        {
            _storage.Logout();
        }
    }
}
