using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Emgu.CV;
using Emgu.CV.Structure;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class TouchImage
    {
        public MCvConvexityDefect[] Defects { get; set; }       
        public MCvBox2D Box { get; set; }
        public Image<Bgr, Byte> Image { get; set; }

        public TouchImage()
        { 
            
        }

    }
}
