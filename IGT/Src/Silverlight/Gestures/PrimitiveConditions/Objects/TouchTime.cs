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
    public class TouchTime : IPrimitiveConditionData
    {



        private float _value = 0f;
        public float Value
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

        private string _unit = "sec";
        public string Unit
        {
            get
            {
                return _unit;
            }
            set
            {
                _unit = value;
            }
        }

        public bool Equals(IPrimitiveConditionData rule)
        {
            throw new NotImplementedException();
        }

        public void Union(IPrimitiveConditionData value)
        {
            TouchTime touchTime = value as TouchTime;

            if (this.Unit == touchTime.Unit)
            {
                if (this.Value < touchTime.Value)
                    this.Value = touchTime.Value;
            }
            else if (this.Unit == string.Empty && touchTime.Unit != string.Empty)
            {
                this.Unit = touchTime.Unit;
                this.Value = touchTime.Value;
            }
            else
            {
                if ((this.Unit == "sec" || this.Unit == "secs") && (touchTime.Unit == "msec" || touchTime.Unit == "msecs"))
                {
                    if ((this.Value * 1000) < touchTime.Value)
                    {
                        this.Value = touchTime.Value / 1000;
                    }

                }
                else if ((this.Unit == "msec" || this.Unit == "msecs") && (touchTime.Unit == "sec" || touchTime.Unit == "secs"))
                {
                    if ((this.Value) < (touchTime.Value * 1000))
                    {
                        this.Value = touchTime.Value * 1000;
                    }
                }
                else if ((this.Unit == "msec" || this.Unit == "msecs") && (touchTime.Unit == "msec" || touchTime.Unit == "msecs"))
                {
                    if ((this.Value) < (touchTime.Value))
                    {
                        this.Value = touchTime.Value;
                    }
                }
                else
                {
                    if ((this.Value) < (touchTime.Value))
                    {
                        this.Value = touchTime.Value;
                    }
                }
            }
        }

        public string ToGDL()
        {
            return string.Format("Touch time: {0} {1}", this.Value, this.Unit);
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
