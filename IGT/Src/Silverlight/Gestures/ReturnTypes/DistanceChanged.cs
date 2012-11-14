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
    public class DistanceChanged : IReturnType
    {
        public DistanceChanged()
        {
            // Set default values
            Delta = 0;
        }

        public double Distance { get; set; }

        public double Delta { get; set; }
    }
}
