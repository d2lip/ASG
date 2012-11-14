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

namespace TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks
{
    public interface ITouchFeedback: IDisposable
    {
        void FrameChanged(FrameInfo frameInfo);
        
        void Init(Panel rootPanel, System.Windows.Threading.Dispatcher dispatcher);
    }
}
