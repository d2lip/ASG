using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using DataService.Objects;

namespace DataService
{
    public class DataAccess
    {
        private DataClassesDataContext _database = new DataClassesDataContext();

        public List<User> GetUsers()
        {
            return _database.Users.ToList<User>();
        }

        public List<UserProject> GetProjects(int userId)
        {
            return _database.UserProjects.Where(p => p.UserId == userId).ToList<UserProject>();
        }

        public List<Gesture> GetGestures(int projectId)
        {
            return _database.Gestures.Where(g => g.ProjectId == projectId).ToList<Gesture>();
        }

        public string[] GetGestures(string userName, string projectName)
        {
            User user = GetUser(userName);
            UserProject proj = GetUserProject(projectName, user.Id);

            var gestures = from g in _database.Gestures
                           where g.ProjectId == proj.Id && g.UserId == user.Id
                           select g.GestureName;

            return gestures.ToArray<string>();
        }

        public string GetGestureData(string userName, string projectName, string gestureName)
        {
            User user = GetUser(userName);
            UserProject project = GetUserProject(projectName, user.Id);

            if (project != null)
            {
                var gestureData = from g in _database.Gestures
                                  where g.UserId == user.Id && g.ProjectId == project.Id && g.GestureName == gestureName
                                  select g;

                if (gestureData.FirstOrDefault() != null)
                    return gestureData.FirstOrDefault().Data;
                else
                    return string.Empty;
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Adds gesture data into database
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="projectName"></param>
        /// <param name="gestureName"></param>
        /// <param name="gestureData"></param>
        public string AddGestureData(string userName, string projectName, string gestureName, string gestureData)
        {
            User user = GetUser(userName);
            UserProject project = GetUserProject(projectName, user.Id);

            var existing = _database.Gestures.Where(g => g.ProjectId == project.Id && g.GestureName == gestureName);

            if (existing.Count() > 0)
                gestureName = gestureName + DateTime.Now.Ticks;

            Gesture nGesture = new Gesture() { ProjectId = project.Id, UserId = user.Id, GestureName = gestureName, Data = gestureData };

            _database.Gestures.InsertOnSubmit(nGesture);
            _database.SubmitChanges();

            return gestureName;
        }

        /// <summary>
        /// Returns the userProject object fram database. If record does not exists, it will create a new record and return that.
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UserProject GetUserProject(string projectName, int userId)
        {
            UserProject proj = _database.UserProjects.Where<UserProject>(p => p.Name == projectName && p.UserId == userId).FirstOrDefault();

            if (proj == null)
            {
                UserProject np = new UserProject() { Name = projectName, UserId = userId };
                _database.UserProjects.InsertOnSubmit(np);
                _database.SubmitChanges();

                return np;
            }
            else
            {
                return proj;
            }
        }

        /// <summary>
        /// Returns the user object fram database. If record does not exists, it will create a new record and return that.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public User GetUser(string userName)
        {

            User user = _database.Users.Where<User>(u => u.UserName == userName).FirstOrDefault();

            if (user == null)
            {
                User nuser = new User() { UserName = userName };
                _database.Users.InsertOnSubmit(nuser);
                _database.SubmitChanges();

                return nuser;
            }
            else
            {
                return user;
            }
        }

        public void AddGestureData(int userId, int projectId, string gestureName, string gestureData)
        {
            // Create new object
            Gesture g = new Gesture()
            {
                ProjectId = projectId,
                UserId = userId,
                GestureName = gestureName,
                Data = gestureData,
            };

            // Save in database
            _database.Gestures.InsertOnSubmit(g);
            _database.SubmitChanges();

            // Update Activity Log
            UpdateUserActivity(userId, string.Format("Adding Gesture Data| project Id:{0}, gesture name: {1}", projectId, gestureName));
        }

        public bool IsUserNameUnique(string userName)
        {
            var user = _database.Users.Where<User>(u => u.UserName == userName).FirstOrDefault();

            return (user == null);
        }

        public void UpdateUserActivity(int userId, string action)
        {
            UserActivity ua = new UserActivity() { Time = DateTime.Now, UserId = userId, Action = action };

            _database.UserActivities.InsertOnSubmit(ua);
            _database.SubmitChanges();

        }

        public DateTime LastUpdatedAt(string userName)
        {
            User user = GetUser(userName);

            var activity = _database.GetLastActivityByUser(user.Id).FirstOrDefault<UserActivity>();

            if (activity != null)
                return activity.Time;
            else
                return DateTime.MinValue;
        }

        public List<ProjectInfo> GetProjects(string userName)
        {
            User user = GetUser(userName);
            var projectNames = from p in _database.UserProjects
                               where p.UserId == user.Id
                               select p.Name;

            List<ProjectInfo> list = new List<ProjectInfo>();
            foreach (var projName in projectNames)
            {
                ProjectInfo projectInfo = new ProjectInfo()
                {
                    ProjectName = projName,
                    GestureNames = GetGestures(userName, projName)
                };

                list.Add(projectInfo);
            }

            return list;
        }
    }
}
