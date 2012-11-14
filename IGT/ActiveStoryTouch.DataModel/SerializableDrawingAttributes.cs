using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Runtime.Serialization;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class SerializableDrawingAttributes
    {
        [DataMember]
        public Color Color { get; set; }
        [DataMember]
        public double Width { get; set; }
        [DataMember]
        public double Height { get; set; }

        #region WPF-specific features
        public enum StylusTips { Rectangle, Ellipse }

        [DataMember]
        public bool FitToCurve { get; set; }
        [DataMember]
        public bool IgnorePressure { get; set; }
        [DataMember]
        public bool IsHighlighter { get; set; }
        [DataMember]
        public StylusTips StylusTip { get; set; }
        [DataMember]
        public Matrix StylusTipTransform { get; set; }
        #endregion
    }
}
