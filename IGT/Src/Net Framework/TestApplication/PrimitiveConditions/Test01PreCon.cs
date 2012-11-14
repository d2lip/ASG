using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Objects;

namespace TestApplication.PrimitiveConditions
{
    public class Test01PreCon : IPrimitiveConditionData
    {
        public string State
        {
            get { return string.Empty; }
            set { }
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
            throw new NotImplementedException();
        }

        #endregion
    }
}
