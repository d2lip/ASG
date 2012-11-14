using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchArea : IPrimitiveConditionData
    {
        private string _type = string.Empty;
        public string Type
        {
            get
            {
                return _type;
            }
            set
            {
                _type = value;
            }
        }

        private string _value = string.Empty;
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        private int _history = 0;
        public int HistoryLevel
        {
            get
            {
                return _history;
            }
            set
            {
                _history = value;
            }
        }

        private int _historyTimeline = 0;
        public int HistoryTimeLine
        {
            get
            {
                return _historyTimeline;
            }
            set
            {
                _historyTimeline = value;
            }
        }

        public int Height
        {
            get
            {
                // TODO: refactor
                string height = Value.Split("x".ToCharArray())[0];
                if (height == "")
                {
                    return 0;
                }
                else
                return int.Parse(height);
            }
        }

        public int Width
        {
            get
            {

                if (Value == "")
                {
                    return 0;

                }
                else
                {

                    if (Value.Split("x".ToCharArray()).Length < 2)
                    {
                        return int.Parse(Value.Split("x".ToCharArray())[0]);
                    }
                    return int.Parse(Value.Split("x".ToCharArray())[1]);
                }
            }
        }

        public bool Equals(IPrimitiveConditionData ruleData)
        {
            if (ruleData == null)
            {
                throw new Exception("Null value exception");
            }
            if (!(ruleData is TouchArea))
            {
                return false;
            }
            var data = ruleData as TouchArea;

            return (data.Type == this.Type && data.Value == this.Value);
        }


        public void Union(IPrimitiveConditionData value)
        {
            if (value == null)
            {
                throw new Exception("Null value exception");
            }
            if (!(value is TouchArea))
            {
                throw new Exception("TouchArea union not TouchArea exception");
            }
            TouchArea tArea = value as TouchArea;
            int height, width;
            string NewValue = string.Empty;

            if (this.Height < tArea.Height)
                height = tArea.Height;
            else
                height = this.Height;

            if (this.Width < tArea.Width)
                width = tArea.Width;
            else
                width = this.Width;


            if (this.Type == "Ellipse" && tArea.Type == "Ellipse")
            {
                NewValue = string.Format("{0}x{1}", height, width);
            }
            else if (this.Type == "Rect" && tArea.Type == "Rect")
            {
                NewValue = string.Format("{0}x{1}", height, width);
            }
            else if (this.Type == "Circle" && tArea.Type == "Circle")
            {
                NewValue = string.Format("{0}", height);
            }
            else //Different types always union to be a rect
            {
                NewValue = string.Format("{0}x{1}", height, width);
                this.Type = "Rect";
            }

            Value = NewValue;
        }


        public string ToGDL()
        {
            return string.Format("Touch area: {0} {1}", this.Type, this.Value);
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
