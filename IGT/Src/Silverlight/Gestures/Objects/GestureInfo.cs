using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class GestureInfo
    {
        int _groupId = 0;
        public int GroupId
        {
            get
            {
                return _groupId;
            }
            set
            {
                _groupId = value;
            }
        }

        private List<FrameInfo> _frames = new List<FrameInfo>();
        public List<FrameInfo> Frames
        {
            get
            {
                return _frames;
            }
            set
            {
                _frames = value;
            }
        }
    }
}
