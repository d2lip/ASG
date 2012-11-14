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
    public class Tag : IPrimitiveConditionData
    {
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



        public bool Equals(IPrimitiveConditionData data)
        {
            return false;
        }


        public void Union(IPrimitiveConditionData value)
        {

        }


        public string ToGDL()
        {
            string gdl = string.Format("Touch tag: {0}", Value);
            return gdl;
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
