using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.ComponentModel;
using System.Windows;
using System.Drawing;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    [DisplayName("Prototype Element")]
    public class PrototypeElement
    {
        [DataMember]
        [Browsable(false)]          //Makes it invisible in property grids.
        public ElementTypes ElementType { get; set; }

        [DataMember]
        [DisplayName("Top")]
        [Description("The distance of the element from the top side of the canvas.")]
        public double Top { get; set; }

        [DataMember]
        [DisplayName("Left")]
        [Description("The distance of the element from the left side of the canvas.")]
        public double Left { get; set; }

        [DataMember]
        [DisplayName("Width")]
        [Description("The width of the element.")]
        public double Width { get; set; }

        [DataMember]
        [DisplayName("Height")]
        [Description("The height of the element.")]
        public double Height { get; set; }

        [DataMember]
        [DisplayName("Content")]
        [Description("The content to be displayed within the prototype element.")]
        public String Content { get; set; }

        [DataMember]
        [DisplayName("Content Visible")]
        [Description("Set to false to hide the content.")]
        public bool ContentVisibile { get; set; }

        [DataMember]
        [DisplayName("Orientation")]
        [Description("The orientation of the component.")]
        public double Orientation { get; set; }

        [DataMember]
        [DisplayName("Center")]
        [Description("The position of the component.")]
        public System.Windows.Point Center { get; set; }

        [DataMember]
        [DisplayName("Image")]      
        public String BackgroundImage { get; set; }

        [DataMember]
        [Browsable(false)]          //Makes it invisible in property grids.
        public long UniqueId { get; set; }

        /// <summary>
        /// The keys for this dictionary are the identifiers from Gesture objects.
        /// The values are the unique identifier of pages.
        /// 
        /// Do NOT set this property. set is public in order to accomodate DataContract deserialization.
        /// </summary>
        [DataMember]
        [Browsable(false)]          //Makes it invisible in property grids.
        public Dictionary<String, long> GestureTargetPageMap { get; set; }

        #region Constructor
        public PrototypeElement()
        {
            GestureTargetPageMap = new Dictionary<String, long>();
        }
        #endregion

    }


}
