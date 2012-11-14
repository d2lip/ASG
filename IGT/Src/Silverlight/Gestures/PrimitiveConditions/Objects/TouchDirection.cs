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
    public class TouchDirection : IPrimitiveConditionData
    {
        public enum Directions
        {
            Left = 0,
            Right,
            Up,
            Down,
            DownLeft,
            DownRight,
            UpLeft,
            UpRight

            // NOTE: We are not providing any diagonal direction matching because
            //       that would make gestures dependent on Screen Orentation/position
        }

        private String _values;
        public String Values
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


        #region IRuleData Members

        public bool Equals(IPrimitiveConditionData rule)
        {
            if (!(rule is TouchDirection))
            {
                throw new Exception("Wrong Type Exception");
            }
            if (rule == null)
            {
                throw new Exception("Null Input Exception");
            }
            TouchDirection directionRule = rule as TouchDirection;

            return directionRule.Values.Equals( this.Values );
        }

        #endregion


        public void Union(IPrimitiveConditionData value)
        {
            throw new NotImplementedException();
        }


        public string ToGDL()
        {
            return string.Format("Touch direction: {0}", this.Values);
        }

        public bool isComplex()
        {
            return false;
        }
    }
}
