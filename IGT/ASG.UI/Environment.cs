using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ASG.UI
{
    public static class EnvironmentFolder
    {
       

       public static string getProjectsFolder()
       { 
        return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Projects\\";
       }


       public static string getImagesFolder()
       { 
        return System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName) + "\\Images\\";
       }


    }
}
