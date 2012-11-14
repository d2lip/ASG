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
    public class SlopeChanged : IReturnType
    {
        /// <summary>
        /// New slope value in degree
        /// </summary>
        public double NewSlope { get; set; }
        
        /// <summary>
        /// The amount of slope changed (in degree)
        /// </summary>
        public double Delta { get; set; }

        public override string ToString()
        {
            return string.Format("Delta: {0}, NewSlope: {1}", Delta, NewSlope);
        }
    }
}
