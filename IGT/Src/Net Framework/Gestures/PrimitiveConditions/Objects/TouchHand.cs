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
    public class TouchHand : IPrimitiveConditionData
    {

        public const string FLAT = "Flat";
        public const string OPEN = "Open";
        public const string VERTICAL = "Vertical";
        public const string HAND = "Hand";

        public const string LEFT = "Left";
        public const string RIGHT = "Right";


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

        private string _side = string.Empty;
        public string Side
        {
            get
            {
                return _side;
            }
            set
            {
                _side = value;
            }
        }

        private string _kind = string.Empty;
        public string Kind
        {
            get
            {
                return _kind;
            }
            set
            {
                _kind = value;
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

            if (Type != String.Empty)
                Type = "," + Type;

            if (Side != String.Empty)
                Side = "," + Side;


            string gdl = string.Format("Touch blob: {0}{1}{2}",Kind, Type, Side);
            return gdl;
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
