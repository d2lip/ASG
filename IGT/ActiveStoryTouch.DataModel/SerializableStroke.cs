using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class SerializableStroke
    {
        [DataMember]
        public SerializableDrawingAttributes DrawingAttributes { get; set; }
        [DataMember]
        public List<SerializableStylusPoint> StylusPoints { get; set; }

        public SerializableStroke()
        {
            StylusPoints = new List<SerializableStylusPoint>();
        }
    }
}
