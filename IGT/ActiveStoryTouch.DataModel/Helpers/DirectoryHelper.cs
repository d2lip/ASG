using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace ActiveStoryTouch.DataModel.Helpers
{
    public static class DirectoryHelper
    {
        public static String GetExecutingDirectory()
        {
            return Assembly.GetExecutingAssembly().Location;
        }
    }
}
