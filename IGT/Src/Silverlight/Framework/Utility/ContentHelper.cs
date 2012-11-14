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
using System.Windows.Media.Imaging;
using System.IO;
using System.Reflection;
using TouchToolkit.Framework.Utility;
using System.Collections.Generic;

using TouchToolkit.GestureProcessor.Objects.LanguageTokens;

namespace TouchToolkit.Framework.Utility
{
    public static class ContentHelper
    {
        /// <summary>
        /// Returns the specified gesture tokens stored as embedded resources in the assembly
        /// </summary>
        /// <param name="callingObject"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static List<GestureToken> GetEmbeddedGestureDefinition(object callingObject, string resourceName)
        {
            Stream stream = GetEmbeddedResource(callingObject, resourceName);

            if (stream != null)
                return stream.Deserialize();
            else
                return null;
        }

    

        /// <summary>
        /// Returns the specified gesture tokens stored as embedded resources in the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        /// 
        public static List<GestureToken> GetEmbeddedGestureDefinition(Assembly assembly, string resourceName)
        {
            Stream stream =  GetEmbeddedResource(resourceName, assembly);

            if (stream != null)
                return stream.Deserialize();
           
                    return null;
            
        }


        public static List<GestureToken> GetOnTheFlyGestureDefinition(Assembly assembly, string resourceName)
        {
            Stream stream = new FileStream(resourceName, FileMode.Open, FileAccess.Read);
                if (stream != null)
                    return stream.Deserialize();
                else

                    return null;
            
        }


        /// <summary>
        /// Returns the specified text content stored as embedded resources in the assembly
        /// </summary>
        /// <param name="callingObject"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetEmbeddedTextContent(object callingObject, string fileName)
        {
            Stream stream = GetEmbeddedResource(callingObject, fileName);
            StreamReader reader = new StreamReader(stream);
            string content = reader.ReadToEnd();
            reader.Close();

            return content;
        }

        /// <summary>
        /// Returns the specified image stored as embedded resources in the assembly
        /// </summary>
        /// <param name="callingObject"></param>
        /// <param name="imageName"></param>
        public static BitmapImage GetEmbeddedImage(object callingObject, string imageName)
        {
            Stream s = GetEmbeddedResource(callingObject, imageName);
            BitmapImage img = new BitmapImage();

#if SILVERLIGHT
            img.SetSource(s);
#else
            img.StreamSource = s;
#endif
            return img;
        }

        /// <summary>
        /// Returns the specified content as "Stream" that is stored as embedded resources in the assembly
        /// </summary>
        /// <param name="callingObject"></param>
        /// <param name="resourceName"></param>
        /// <returns></returns>
        public static Stream GetEmbeddedResource(object callingObject, string resourceName)
        {
            Type t = callingObject.GetType();
            Assembly assembly = t.Assembly;
            return GetEmbeddedResource(resourceName, assembly);
        }

        /// <summary>
        /// Returns the specified content as "Stream" in the assembly
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static Stream GetEmbeddedResource(string resourceName, Assembly assembly)
        {
            // The control template must be in an embeded resource - find it
            Stream resourceStream = null;

            // If the assembly is built with VS there will be a prefix before
            // the resource name we expect. The resource name will be at the 
            // end after a dot
            string dotResource = '.' + resourceName;

            string[] names = assembly.GetManifestResourceNames();

            foreach (string name in names)
            {
                if (name.Equals(resourceName) || name.EndsWith(dotResource))
                {
                    resourceStream = assembly.GetManifestResourceStream(name);
                }
            }

            return resourceStream;
        }
    }
}
