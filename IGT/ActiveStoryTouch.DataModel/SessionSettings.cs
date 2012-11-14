using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class SessionSettings
    {
        [DataMember]
        public String TaskInstructions { get; set; }
        [DataMember]
        public long InitialPage { get; set; }

        public void SaveToFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SessionSettings));
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            serializer.WriteObject(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
        }

        public static SessionSettings LoadFromFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SessionSettings));
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            SessionSettings result = serializer.ReadObject(fileStream) as SessionSettings;
            fileStream.Close();
            return result;
        }
    }
}
