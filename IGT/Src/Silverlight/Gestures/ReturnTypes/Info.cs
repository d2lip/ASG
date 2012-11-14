using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace TouchToolkit.GestureProcessor.ReturnTypes
{
    public class Info : IReturnType
    {
        private string _value = string.Empty;
        public string Message
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}
