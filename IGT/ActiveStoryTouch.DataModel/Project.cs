using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class Project
    {
        [DataMember]
        public long LastAssignedUniqueID;
        [DataMember]
        public String ProjectName { get; set; }
        [DataMember]
        public CanvasSettings DefaultCanvasSettings { get; set; }
        [DataMember]
        public ObservableDictionary <long, ASGPage> PageDictionary { get; private set; }
        [DataMember]
        private long _lastPageNumber;

     

        public Project()
        {
            LastAssignedUniqueID = 0;
            _lastPageNumber = 0;
            PageDictionary = new ObservableDictionary<long, ASGPage>();
            ProjectName = "Untitled Project";
            DefaultCanvasSettings = new CanvasSettings();
        }

        public long GetNextUniqueId()
        {
         LastAssignedUniqueID++;
            return LastAssignedUniqueID;
        }


        public void RefreshPageNumber()
        {
            _lastPageNumber = PageDictionary.Count();
        
        }

        public long GetNextPageNumber()
        {
            _lastPageNumber++;
            return _lastPageNumber;
        }

        

        public void SaveToFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Project));
            FileStream fileStream = new FileStream(fileName, FileMode.Create);
            serializer.WriteObject(fileStream, this);
            fileStream.Flush();
            fileStream.Close();
        }

        public static Project LoadFromFile(String fileName)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(Project));
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            Project result = serializer.ReadObject(fileStream) as Project;
            fileStream.Close();
            return result;
        }
    }
}
