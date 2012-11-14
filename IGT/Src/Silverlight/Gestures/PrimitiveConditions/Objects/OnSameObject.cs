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
    public class OnSameObject : IPrimitiveConditionData
    {
        private bool _condition = true;
        public bool Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
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

            OnSameObject onObject = value as OnSameObject;

            if (this.Condition == true || onObject.Condition == true)
                this.Condition = true;

        }


        public string ToGDL()
        {
            if (this.Condition == true)
                return string.Format("On same object");
            else
                return string.Empty;
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
