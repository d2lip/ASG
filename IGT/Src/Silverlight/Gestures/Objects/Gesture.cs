using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace TouchToolkit.GestureProcessor.Objects
{
    public class Gesture
    {

        private static Dictionary<string, double> _languageBuffer;


        public static Dictionary<string, double> BufferInstance()
        {
            if (_languageBuffer == null)
                _languageBuffer = new Dictionary<string, double>();

            return _languageBuffer;
        }
        
        public string Name { get; set; }
        public List<ValidationBlock> ValidationBlocks = new List<ValidationBlock>();
        //public PrimitiveConditionCollection PreConditions = new PrimitiveConditionCollection();
        //public PrimitiveConditionCollection Rules = new PrimitiveConditionCollection();
        public ReturnTypeInfoCollection ReturnTypes = new ReturnTypeInfoCollection();



    }
}
