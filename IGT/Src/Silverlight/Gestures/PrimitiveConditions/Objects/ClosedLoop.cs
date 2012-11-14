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
    public class ClosedLoop : IPrimitiveConditionData
    {
        string _state = string.Empty;
        public string State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
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
            if (value == null)
            {
                throw new Exception("Null Value Exception");
            }
            if (!(value is ClosedLoop))
            {
                throw new Exception("Invalid Type Exception");
            }
            ClosedLoop cLoop = value as ClosedLoop;

            if (this.State == "false" || cLoop.State == "false")
                this.State = "false";
            else
                this.State = "true";

        }

        public string ToGDL()
        {
            if (this.State == string.Empty)
                return string.Format("Closed loop");


            if (this.State == "true")
                return string.Format("Closed loop: true");
            else
                return string.Empty;
        }

        public bool isComplex() {
            return false;
        }
    }
}