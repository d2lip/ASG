using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class ReturnTypeInfo
    {
        public Type ReturnType { get; set; }
        public Type CalculatorType { get; set; }

        //TODO: temporary workaround for "Info" return type
        public string AdditionalInfo { get; set; }
    }
}
