using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class PerpendicularTo : IPrimitiveConditionData
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
            return string.Format("{0} PerpendicularTo {1}", this._gesture1, this._gesture2);
        }


        public bool isComplex()
        {
            return true;
        }

        #endregion
    }
}
