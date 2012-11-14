using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class GestureManager
    {
        /// <summary>
        /// The dictionary of gestures available, keyed by Identifier
        /// </summary>
        [DataMember]
        private Dictionary<String, Gesture> GestureDictionary { get; set; }

        public int Count
        {
            get
            {
                return GestureDictionary.Count;
            }
        }

        private List<String> _keyList = null;
        private List<Gesture> _gestureList = null;

        /// <summary>
        /// Returns the list of all the keys in the GestureManager, i.e. the list of all possible Gesture Identifiers.
        /// </summary>
        /// <returns></returns>
        public List<String> KeyList
        {
            get
            {
                if(_keyList == null)
                    _keyList = GestureDictionary.Keys.ToList<String>();
                return _keyList;
            }
        }

        public List<Gesture> GestureList
        {
            get
            {


                return GestureDictionary.Values.ToList<Gesture>();
            }
        }

        public Gesture this[String key]
        {
            get
            {
                return GestureDictionary[key];
            }
            set
            {
                GestureDictionary[key] = value;
            }
        }


        public GestureManager()
        {
            GestureDictionary = new Dictionary<String, Gesture>();
        }

        /// <summary>
        /// Returns the gesture whose key is at the specified index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public Gesture ObjectAtIndex(int index)
        {
            return GestureDictionary[this.KeyList[index]];
        }
        public static GestureManager LoadFromFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(GestureManager));
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            GestureManager result = serializer.ReadObject(fileStream) as GestureManager;
            fileStream.Close();
            return result;
            #region Old Code
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Gesture>));
            //FileStream fileStream = new FileStream(fileName, FileMode.Open);
            //List<Gesture> result = (List<Gesture>)serializer.Deserialize(fileStream);
            //fileStream.Close();

            //foreach (var item in result)
            //{
            //    GestureDictionary.Add(item.Identifier, item);
            //}
            #endregion Old Code
        }
        private void SaveToFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(GestureManager));
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            serializer.WriteObject(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
            #region Old Code
            //List<Gesture> gestureList = GestureDictionary.Values.ToList<Gesture>();
            
            //XmlSerializer serializer = new XmlSerializer(typeof(List<Gesture>));
            //FileStream fileStream = new FileStream(fileName, FileMode.Create);
            //serializer.Serialize(fileStream, gestureList);
            //fileStream.Flush();
            //fileStream.Close();
            #endregion Old Code
        }

        public void Add(String identifier, Gesture gesture)
        {
            
                GestureDictionary.Add(identifier, gesture);
        }

        public void Clear()
        {
            GestureDictionary.Clear();
        }

        public void Remove(string identifier)
        {
            GestureDictionary.Remove(identifier);
        
        }


        public bool ContainsKey(string key)
        {
            return GestureDictionary.ContainsKey(key);
        }
    }
}
