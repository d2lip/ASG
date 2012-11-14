using System;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class AngleBetween : IPrimitiveConditionData
    {
        private string _gesture1 = string.Empty;
        public string Gesture1
        {
            get
            {
                return _gesture1;
            }
            set
            {
                _gesture1 = value;
            }
        }

        private string _gesture2 = string.Empty;
        public string Gesture2
        {
            get
            {
                return _gesture2;
            }
            set
            {
                _gesture2 = value;
            }
        }

        private double _min = 0f;
        public double Min
        {
            get
            {
                return _min;
            }
            set
            {
                _min = value;
            }
        }

        private double _max = 0f;
        public double Max
        {
            get
            {
                return _max;
            }
            set
            {
                _max = value;
            }
        }

        #region IPrimitiveConditionData Members

        public bool Equals(IPrimitiveConditionData value)
        {
            throw new NotImplementedException();
        }

        public void Union(IPrimitiveConditionData value)
        {
            throw new NotImplementedException();
        }

        public string ToGDL()
        {
            if (Gesture1 == string.Empty && Gesture2 == string.Empty)
                return string.Format("Angle between");


            if (this._max == 0)
            {
                return string.Format("Angle between step{0},step{1}:{2} ", this.Gesture1, this.Gesture2, this.Min);
            }
            else 
            {
                return string.Format("Angle between step{0},step{1}:{2}..{3} ", this.Gesture1, this.Gesture2, this.Min, this.Max);            
            }
        }

        public bool isComplex()
        {
            return true;
        }

        #endregion
    }
}