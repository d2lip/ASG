using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using TouchToolkit.GestureProcessor.Objects;

namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class InfoCalculator : IReturnTypeCalculator
    {
        public string Message = string.Empty;

        #region IReturnTypeCalculator Members

        public IReturnType Calculate(ValidSetOfTouchPoints set)
        {
            return new Info() { Message = Message };
        }

        #endregion
    }
}
