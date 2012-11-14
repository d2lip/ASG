using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class ASGPage
    {
        [DataMember]
        public long UniqueId { get; set; }
        [DataMember]
        public long PageNumber { get; set; }
        [DataMember]
        public String Name { get; set; }
        [DataMember]
        public ObservableDictionary<long, PrototypeElement> PrototypeElementDictionary { get; set; }
        [DataMember]
        public SerializableStrokeCollection Strokes { get; set; }
        [DataMember]
        public String BackgroundImageSource { get; set; }

       

        public System.Windows.Ink.StrokeCollection StrokeToBind
        {
            get
            {
                return StrokeHelper.ConvertToStrokeCollection(Strokes);
            }
        }


       
        
        public CanvasSettings PageCanvasSettings { get; set; }
        public ASGPage()
        {
            PrototypeElementDictionary = new ObservableDictionary<long,PrototypeElement>();
            Strokes = new SerializableStrokeCollection();
            PageCanvasSettings = new CanvasSettings();
        }


        public void SaveToFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ASGPage));
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            serializer.WriteObject(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
        }

        public static ASGPage LoadFromFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(ASGPage));
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            ASGPage result = serializer.ReadObject(fileStream) as ASGPage;
            fileStream.Close();
            return result;
        }
    }
}
