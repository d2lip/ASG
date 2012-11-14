using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ActiveStoryTouch.DataModel
{
    [DataContract]
    public class CanvasSettings
    {
        private double _width = 1024;
        private double _height = 768;
        private int _alpha = 0;
        private int _red = 255;
        private int _green = 255;
        private int _blue = 255;

        [DataMember]
        public double Width
        {
            get
            {
                return _width;
            }
            set
            {
                if (value <= 0)
                {
                    _width = 1024;
                   return;
                }
                _width = value;
            }
        }
        [DataMember]
        public double Height
        {
            get
            {
                return _height;
            }
            set
            {
                if (value <= 0)
                {
                    _height = 768;
                    return;
                }
                _height = value;
            }
        }
        [DataMember]
        public int Alpha
        {
            get
            {
                return _alpha;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    _alpha = 0;
                    return;
                }
                _alpha = value;
            }
        }
        [DataMember]
        public int Red
        {
            get
            {
                return _red;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    _red = 255;
                    return;
                }
                _red = value;
            }
        }
        [DataMember]
        public int Green
        {
            get
            {
                return _green;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    _green = 255;
                    return;
                }
                _green = value;
            }
        }
        [DataMember]
        public int Blue
        {
            get
            {
                return _blue;
            }
            set
            {
                if (value < 0 || value > 255)
                {
                    _blue = 255;
                    return;
                }
                _blue = value;
            }
        }
    }
}
