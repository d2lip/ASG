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
    public class ProjectDetail
    {
        public string ProjectName { get; set; }
        public List<string> GestureNames { get; set; }

        public override string ToString()
        {
            return ProjectName;
        }
    }

    public delegate void GetGestureCallback(string projectName, string gestureName, string data, Exception error = null);
    public delegate void GetGestureNamesCallback(string projectName, List<string> gestureNames, Exception error = null);
    public delegate void GetAllProjectsCallback(List<ProjectDetail> projects, Exception error = null);
    public delegate void SaveGestureCallback(string gestureName, Exception error = null);
}
