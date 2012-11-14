using System;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;

namespace TouchToolkit.GestureProcessor.PrimitiveConditions.Objects
{
    public class TouchRelativePosition : IPrimitiveConditionData
    {
        public const string LEFT = "Left";
        public const string RIGHT = "Right";
        
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


        private string _position = string.Empty;
        public string Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
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
                return string.Format("Relative position between");

             return string.Format("Relative position between step{0},step{1}: {2} ", this.Gesture1, this.Gesture2, this.Position);
      
           
        }

        public bool isComplex()
        {
            return true;
        }

        #endregion
    }
}