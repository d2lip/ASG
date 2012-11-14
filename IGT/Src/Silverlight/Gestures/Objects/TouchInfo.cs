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
using System.Drawing.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;


namespace TouchToolkit.GestureProcessor.Objects
{
    public enum TouchAction2
    {
        Down = 1,
        Move = 2,
        Up = 3
    }

    public class TouchInfo
    {
        public int TouchDeviceId { get; set; }
        public TouchAction2 ActionType { get; set; }
        public Point Position { get; set; }
        public TouchImage Snapshot { get; set; }
        public string Tag { get; set; }
        public bool IsFinger { get; set; }
        public Rect Bounds { get; set; }
        
        private int _groupId = 0;
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

        public TouchInfo(){
           // Snapshot = new List<System.Drawing.Bitmap>();        
        }

        public void Update(TouchInfo info)
        {       
             
            ActionType = info.ActionType;
            Position = info.Position;
            if (info.Snapshot != null)
                Snapshot = info.Snapshot;
            //Snapshot
           // Snapshot.AddRange(info.Snapshot);      
        
        
        
        
        }

        public override string ToString()
        {
            return string.Format("Id:{0} Point:{1},{2} Action:{3} Group:{4}", TouchDeviceId, Position.X, Position.Y, ActionType.ToString(), GroupId);
        }}
    }

