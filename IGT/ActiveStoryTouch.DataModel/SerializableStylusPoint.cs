using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class SerializableStylusPoint
    {
        [DataMember]
        public float PressureFactor { get; set; }
        [DataMember]
        public double X { get; set; }
        [DataMember]
        public double Y { get; set; }

        // NOTE: Not including the StylusPointDescription property in WPF, consisting of StylusPointProperties... if needed add later.
    }
}
