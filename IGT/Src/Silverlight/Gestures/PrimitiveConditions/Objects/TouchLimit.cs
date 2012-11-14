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
    public class TouchLimit : IPrimitiveConditionData
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

        private int _minValue = 0;
        public int Min
        {
            get
            {
                return _minValue;
            }
            set
            {
                _minValue = value;
            }
        }

        private int _maxValue = 0;
        public int Max
        {
            get
            {
                return _maxValue;
            }
            set
            {
                _maxValue = value;
            }
        }

        public bool Equals(IPrimitiveConditionData data)
        {
            bool result = false;
            if (data is TouchLimit)
            {
                var d1 = data as TouchLimit;
                if (d1.Type == this.Type &&
                    d1.Min == this.Min &&
                    d1.Max == this.Max)
                    result = true;
            }

            return result;
        }


        public void Union(IPrimitiveConditionData value)
        {
            TouchLimit touchLimit = value as TouchLimit;

            if (this.Min > touchLimit.Min)
                this.Min = touchLimit.Min;

            if ((this.Max != 0 || touchLimit.Max != 0))
            {
                if (!string.Equals(this.Type, "Range", StringComparison.OrdinalIgnoreCase))
                    this.Type = "Range";

                if (this.Max < touchLimit.Max)
                    this.Max = touchLimit.Max;
            }
        }


        public string ToGDL()
        {
            string gl = string.Empty;
            if (string.Equals(this.Type, "Range", StringComparison.OrdinalIgnoreCase))
            {
                gl = string.Format("Touch limit: {0}..{1}", this.Min, this.Max);
            }
            else
            {
                gl = string.Format("Touch limit: {0}", this.Min);
            }

            return gl;
        }


        public bool isComplex()
        {
            return true;
        }
    }
}
