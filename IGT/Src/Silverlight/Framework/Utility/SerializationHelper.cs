using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Diagnostics.Contracts;

using TouchToolkit.GestureProcessor.Objects.LanguageTokens;
using TouchToolkit.GestureProcessor.Objects;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;
using System.Reflection;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;

namespace TouchToolkit.Framework.Utility
{
    public static class SerializationHelper
    {
        private static DataContractJsonSerializer _serializer = new DataContractJsonSerializer(typeof(List<GestureToken>), GetAllPrimitiveConditionDataTypes());

        private static Type[] GetAllTypes()
        {
            List<Type> types = new List<Type>();

            // Get all type definitions from TouchToolkit.GestureProcessor assembly
            types.AddRange(GestureFramework.GestureProcessorAssembly.GetTypes());

            if (GestureFramework.HostAssembly != null)
            {
                // Get all type definitions from users project assembly
                types.AddRange(GestureFramework.HostAssembly.GetTypes());
            }

            return types.ToArray();
        }

        /// <summary>
        /// Returns all primitive condition types defined in framework and also in user assembly
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Type> GetAllPrimitiveConditionDataTypes()
        {
            List<Type> requiredTypes = new List<Type>();

            Type[] types = GetAllTypes();
            foreach (var type in types)
            {
                Type[] interfaces = type.GetInterfaces();
                foreach (var i in interfaces)
                {
                    if (i.IsAssignableFrom(typeof(IPrimitiveConditionData)))
                        requiredTypes.Add(type);
                }
            }

            return requiredTypes;
        }

        /// <summary>
        /// Converts string into collection of bytes
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        internal static List<ArraySegment<byte>> ToBytes(this string content)
        {
            List<ArraySegment<byte>> bytes = new List<ArraySegment<byte>>();
            bytes.Add(new ArraySegment<byte>(UnicodeEncoding.Unicode.GetBytes(content)));

            return bytes;
        }

        /// <summary>
        /// Builds collection of gesture tokens from serialized string content in JSON format
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        internal static List<GestureToken> Deserialize(this string json)
        {
            byte[] bytes = UnicodeEncoding.Unicode.GetBytes(json);
            MemoryStream stream = new MemoryStream(bytes);
            try
            {
                var gestures = _serializer.ReadObject(stream) as List<GestureToken>;
                return gestures;
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Builds collection of gestureToken objects from stream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        internal static List<GestureToken> Deserialize(this Stream stream)
        {
            try
            {
                var gestures = _serializer.ReadObject(stream) as List<GestureToken>;
                return gestures;
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Serializes the gesture tokens into json string
        /// </summary>
        /// <param name="gestureTokens"></param>
        /// <returns></returns>
        public static string Serialize(List<GestureToken> gestureTokens)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                _serializer.WriteObject(ms, gestureTokens);

                ms.Position = 0;
                StreamReader sr = new StreamReader(ms);
                string json = sr.ReadToEnd();
                sr.Close();

                return json;
            }
        }


        //------------------------

        /// <summary>
        /// Determines whether specified objects are logically equal
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemToCheck"></param>
        /// <returns></returns>
        public static bool IsEqual(this FrameInfo self, FrameInfo itemToCheck)
        {
            if (self.TimeStamp == itemToCheck.TimeStamp &&
                self.Touches.IsEqual(itemToCheck.Touches) &&
                self.WaitTime == itemToCheck.WaitTime)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determines whether specified objects are logically equal
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemToCheck"></param>
        /// <returns></returns>
        public static bool IsEqual(this List<TouchInfo> self, List<TouchInfo> itemToCheck)
        {
            bool result = true;
            if (self.Count == itemToCheck.Count)
            {
                for (int i = 0; i < self.Count; i++)
                {
                    if (!(self[i].ActionType == itemToCheck[i].ActionType &&
                        self[i].Position.X == itemToCheck[i].Position.X &&
                        self[i].Position.Y == itemToCheck[i].Position.Y &&
                        self[i].TouchDeviceId == itemToCheck[i].TouchDeviceId))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        static List<Type> _knownTypes = new List<Type>();
        private static List<Type> GetRequiredKnownTypesForTouchFrameEventArgSerialization()
        {
            if (_knownTypes.Count == 0)
            {
                _knownTypes.Add(typeof(TouchInfo));
                _knownTypes.Add(typeof(FrameInfo));
                _knownTypes.Add(typeof(List<TouchInfo>));
                _knownTypes.Add(typeof(List<FrameInfo>));

                // TODO: Fix cross platform serialization 
                // NOTE: The 'Point' class is implemented differently in .NET Framework and Silverlight.
                //       As as result, data recorded in Silverlight application can not be used in .NET App and vise versa
                // To fix this: we need a generic Point class. Currently the X & Y property is serialized differently in 
                // the two frameworks.
                _knownTypes.Add(typeof(Point));
            }


            return _knownTypes;
        }

        /// <summary>
        /// Builds GestureInfo objects from serialized xml content
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static GestureInfo Desirialize(string xml)
        {
            Contract.Requires(!string.IsNullOrEmpty(xml), "Xml content can not be null or empty");

            GestureInfo gInfo = new GestureInfo();
            // Create xml reader from string
            MemoryStream stream = new System.IO.MemoryStream(Encoding.UTF8.GetBytes(xml));
            //XmlReader reader = XmlReader.Create(stream);

            try
            {
                // Build the deserializer
                DataContractSerializer serializer = new DataContractSerializer(typeof(FrameInfo), GetRequiredKnownTypesForTouchFrameEventArgSerialization());
                System.Diagnostics.Debug.WriteLine(xml);
                gInfo.Frames = serializer.ReadObject(stream) as List<FrameInfo>;

                return gInfo;
            }
            catch (Exception e)
            {
                //TODO: Testing
                string msg = e.Message;
                System.Diagnostics.Debug.WriteLine(msg);
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Builds collection of FrameInfo objects from serialized xml content
        /// </summary>
        /// <param name="frames"></param>
        /// <returns></returns>
        public static string Serialize(List<FrameInfo> frames)
        {
            // Local variables
            string xml = string.Empty;
            MemoryStream stream = new MemoryStream();

            try
            {
                // Tools to serialize full object graph into xml
                DataContractSerializer serializer = new DataContractSerializer(typeof(FrameInfo), GetRequiredKnownTypesForTouchFrameEventArgSerialization());
                serializer.WriteObject(stream, frames);

                // Convert memory stream into string
                stream.Position = 0;
                StreamReader sr = new StreamReader(stream);
                xml = sr.ReadToEnd();

                return xml;
            }
            catch
            {
                throw;
            }
            finally
            {
                stream.Dispose();
            }
        }

        /// <summary>
        /// Generates 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static string ToXml(this List<FrameInfo> self)
        {
            return Serialize(self);
        }
    }
}
