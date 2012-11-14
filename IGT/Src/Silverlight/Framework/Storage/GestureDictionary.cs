using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TouchToolkit.Framework.Storage
{
    public class GestureDictionary
    {
        //A dictionary of dictionaries: Keyed by Project Names then by Gesture Names and finally stores the gesture data 
        private Dictionary<string, Dictionary<string, string>> _projectDictionary = new Dictionary<string, Dictionary<string, string>>();
        public List<ProjectDetail> ProjectDetails = new List<ProjectDetail>();

        public bool Contains(string projectName, string gestureName)
        {
            return _projectDictionary.ContainsKey(projectName) && _projectDictionary[projectName].ContainsKey(gestureName);
        }

        public string Get(string projectName, string gestureName)
        {
            if (this.Contains(projectName,gestureName))
            {
                return _projectDictionary[projectName][gestureName];
            }
            else
            {
                return string.Empty;
            }
        }
        public List<string> GetRawData()
        {
            List<string> rawData = new List<string>();
            foreach (var project in _projectDictionary.Keys)
            {
                foreach (var data in _projectDictionary[project].Values)
                {
                    rawData.Add(data);
                }
            }
            return rawData;
        }
        public void Add(string projectName, string gestureName, string value)
        {
            
            //Save data locally
            if (_projectDictionary.ContainsKey(projectName))
            {
                if (_projectDictionary[projectName].ContainsKey(gestureName))
                {
                    //Can't overwrite gestures?
                    //_projectDictionary[projectName][gestureName] = value;
                }
                else
                {
                    _projectDictionary[projectName].Add(gestureName, value);
                }
            }
            else
            {
                _projectDictionary.Add(projectName, new Dictionary<string, string>());
                _projectDictionary[projectName].Add(gestureName, value);
            }

            AddToDetailList(projectName, gestureName);
        }

        private void AddToDetailList(string projectName, string gestureName)
        {
            foreach (var detail in ProjectDetails)
            {
                if (detail.ProjectName == projectName)
                {
                    if (!detail.GestureNames.Contains(gestureName))
                    {
                        detail.GestureNames.Add(gestureName);
                    }
                    return;
                }
            }
            ProjectDetail project = new ProjectDetail();
            project.GestureNames = new List<string>();
            project.ProjectName = projectName;
            project.GestureNames.Add(gestureName);
            ProjectDetails.Add(project);
        }
    }
}
