using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.ReturnTypes;


namespace TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks
{
    public interface IGestureFeedback
    {
        /// <summary>
        /// Renders selected area on the specified canvas using the dispatcher thread
        /// </summary>
        /// <param name="dispatcher"></param>
        /// <param name="feedbackCanvas"></param>
        /// <param name="values"></param>
        void RenderUI(Dispatcher dispatcher, Canvas feedbackCanvas, List<IReturnType> values);
    }
}
