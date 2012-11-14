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
    public class EnclosedArea : IPrimitiveConditionData
    {
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

        #region IRuleData Members

        public bool Equals(IPrimitiveConditionData rule)
        {
            throw new NotImplementedException();
        }

        #endregion


        public void Union(IPrimitiveConditionData value)
        {
            EnclosedArea encArea = value as EnclosedArea;

            if (this.Min > encArea.Min)
                this.Min = encArea.Min;

            if (this.Max < encArea.Max)
                this.Max = encArea.Max;
        }


        public string ToGDL()
        {
            return string.Format("Enclosed Area: {0}..{1}", this.Min, this.Max);
        }


        public bool isComplex()
        {
            return false;
        }
    }
}
