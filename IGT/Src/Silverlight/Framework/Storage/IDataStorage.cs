using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchToolkit.Framework.Storage
{
    internal interface IDataStorage
    {
        string AccountName { get; }

        bool IsLoggedIn();

        void Login(string accountName);

        void Logout();

        void SaveGesture(string projectName, string gestureName, string value, SaveGestureCallback callback);

        void GetGesture(string projectName, string gestureName, GetGestureCallback callback);

        void GetAllProjects(GetAllProjectsCallback callback);
    }
}
