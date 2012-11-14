using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace ActiveStoryTouch.DataModel
{
    public class SerializableStrokeCollection : List<SerializableStroke>
    {
        public void SaveToFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SerializableStrokeCollection));
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            serializer.WriteObject(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
        }

        public static SerializableStrokeCollection LoadFromFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(SerializableStrokeCollection));
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            SerializableStrokeCollection result = serializer.ReadObject(fileStream) as SerializableStrokeCollection;
            fileStream.Close();
            return result;
        }
    }
}
