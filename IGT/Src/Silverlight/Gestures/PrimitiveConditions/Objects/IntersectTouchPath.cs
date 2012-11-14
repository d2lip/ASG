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
    public class IntersectTouchPath : IPrimitiveConditionData
    {
        bool _result;
        public bool Result
        {
            get
            {
                return _result;
            }
            set
            {
                _result = value;
            }
        }

        #region IRuleData Members

        public bool Equals(IPrimitiveConditionData rule)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Union(IPrimitiveConditionData value)
        {
            throw new NotImplementedException();
        }


        public string ToGDL()
        {
            return string.Format("Intersect touch path: {0}", _result);
        }


        public bool isComplex()
        {
            return true;
        }
    }
}
