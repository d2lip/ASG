using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DataService.Objects;

namespace DataService
{
    /// <summary>
    /// Summary description for GestureService
    /// </summary>
    [WebService(Namespace = "http://labs.shahed.net/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class GestureService : System.Web.Services.WebService
    {
        DataAccess _dataAccess = new DataAccess();

        [WebMethod]
        public bool ConnectivityCheck()
        {
            if (IsValidRequest())
                return true;
            else
                return false;
        }

        [WebMethod]
        public string AddGestureData(string userName, string projectName, string gestureName, string gestureData)
        {
            if (IsValidRequest())
            {
                return _dataAccess.AddGestureData(userName, projectName, gestureName, gestureData);
            }

            return string.Empty;
        }

        [WebMethod]
        public List<ProjectInfo> GetProjectsByUser(string userName)
        {
            if (IsValidRequest())
            {
                return _dataAccess.GetProjects(userName);
            }
            else
            {
                return null;
            }
        }

        //[WebMethod]
        //public string[] GetGesturesByProject(string userName, string projectName)
        //{
        //    if (IsValidRequest())
        //    {
        //        return _dataAccess.GetGestures(userName, projectName);
        //    }
        //    else
        //    {
        //        return new string[0];
        //    }
        //}

        [WebMethod]
        public string GetGestureData(string userName, string projectName, string gestureName)
        {
            if (IsValidRequest())
            {
                return _dataAccess.GetGestureData(userName, projectName, gestureName);
            }
            else
            {
                return string.Empty;
            }
        }

        [WebMethod]
        public void Delete(int id)
        {

        }

        [WebMethod]
        public bool IsUserNameUnique(string userName)
        {
            if (IsValidRequest())
            {
                return _dataAccess.IsUserNameUnique(userName);
            }
            else
            {
                return false;
            }
        }

        [WebMethod]
        public DateTime LastUpdatedAt(string userName)
        {
            return _dataAccess.LastUpdatedAt(userName);
        }

        private bool IsValidRequest()
        {
            // TODO: Add cookie/application base authentication
            return true;
        }
    }
}
