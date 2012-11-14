using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchShape : IPrimitiveConditionData
    {
        private string _values = "";
        public string Values
        {
            get
            {
                return _values;
            }
            set
            {
                _values = value;
            }

        }

        public bool Equals(IPrimitiveConditionData value)
        {
            TouchShape v = value as TouchShape;
            if (v == null)
            {
                throw new Exception("Null value or wrong type exception");
            }
            return v.Values.Equals(Values);
        }

        public void Union(IPrimitiveConditionData value)
        {
            //Nothing
        }

        public string ToGDL()
        {
            string GDL = "Touch shape: " + Values;
            return GDL;
        }


        public bool isComplex()
        {
            return false;
        }
    }
}