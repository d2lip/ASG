using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TouchToolkit.GestureProcessor.ReturnTypes;

namespace TestApplication.ReturnType
{
    public class Test01ReturnType : IReturnType
    {
    }

    public class Test01ReturnTypeCalculator : IReturnTypeCalculator
    {
        #region IReturnTypeCalculator Members

        public IReturnType Calculate(TouchToolkit.GestureProcessor.Objects.ValidSetOfTouchPoints set)
        {
            return new Test01ReturnType();
        }

        #endregion
    }

}
