using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TouchToolkit.Framework.GestureEvents;
using System.Collections.Generic;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using TouchToolkit.GestureProcessor.PrimitiveConditions.Validators;

using TouchToolkit.GestureProcessor.Feedbacks.GestureFeedbacks;
using TouchToolkit.GestureProcessor.Utility.TouchHelpers;
using TouchToolkit.Framework.Utility;
using TouchToolkit.Framework.Exceptions;
using System.Diagnostics.Contracts;
using TouchToolkit.Framework.TouchInputProviders;
using TouchToolkit.Framework.UI;
using System.Reflection;

namespace TouchToolkit.Framework
{
    public class GestureFramework
    {
        protected GestureFramework()
        { }

        public enum DebugPanels
        {
            GestureRecorder,
            CodeTemplateGenerator
        }




        static GestureRecordingPanel _debugPanel = null;
        //static GestureDefinitionBuilder _defBuilder = null;

        static List<ITouchFeedback> _commonBehaviors = new List<ITouchFeedback>();
        static Dictionary<string, List<Type>> _gestureFeedbacks = new Dictionary<string, List<Type>>();

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(LayoutRoot != null, "LayoutRoot cannot be set to null as different modules can interact with the UI from different thread.");
        }






        public static Canvas LayoutRoot
        {
            get;
            private set;
        }

        internal static bool IsInitialized
        {
            get;
            private set;
        }

        internal static List<ITouchFeedback> CommonBehaviors
        {
            get
            {
                return _commonBehaviors;
            }
        }

        internal static Dictionary<string, List<Type>> GestureFeedbacks
        {
            get
            {
                return _gestureFeedbacks;
            }
        }

        private static TouchToolkit.Framework.GestureEvents.EventManager _eventManager = new TouchToolkit.Framework.GestureEvents.EventManager();
        public static TouchToolkit.Framework.GestureEvents.EventManager EventManager
        {
            get
            {
                return _eventManager;
            }
        }

        private static Assembly _gestureProcessor = null;
        internal static Assembly GestureProcessorAssembly
        {
            get
            {
                if (_gestureProcessor == null)
                    _gestureProcessor = (new BubblesPath()).GetType().Assembly;

                return _gestureProcessor;
            }
        }




        private static Assembly _host;
        public static Assembly HostAssembly
        {
            get { return _host; }
            set { _host = value; }
        }


        /// <summary>
        /// Initializes necessary components in the framework (i.e. Rule validators, Touch input listeners, Gesture language processors, etc.)
        /// </summary>
        /// <param name="listenerType"></param>
        /// <param name="layoutRoot"></param>
        public static void Initialize(TouchInputProvider inputProvider, Panel layoutRoot, Assembly host)
        {
            _host = host;            
            Contract.Requires(inputProvider != null, "You must specify the touch input provider that framework will use to get the raw touch data.");
            Contract.Requires(layoutRoot != null, "You must provide the panel where you want the framework to render UI (i.e. Touch/Gesture effects)");
            Contract.Requires(layoutRoot is Canvas, "Currently, the framework only supports \"Canvas\" as layout root");

            
            LayoutRoot = layoutRoot as Canvas;
            
            // Initialize input provider
            inputProvider.Init();
            UpdateInputProvider(inputProvider);

            // Load gesture definitions
            GestureLanguageProcessor.LoadGestureDefinitions();

            IsInitialized = true;
        }

        internal static void UpdateInputProvider(TouchInputProvider hl)
        {
            TouchInputManager.SetTouchInputHardware(hl);
        }

        /// <summary>
        /// Adds the feedback for the specified gesture. The framework will automatically run the feedbacks when a gesture is detected
        /// </summary>
        /// <param name="gestureName"></param>
        /// <param name="feedback"></param>
        public static void AddGesturFeedback(string gestureName, Type feedback)
        {
            if (feedback.IsTypeOf(typeof(IGestureFeedback)))
            {
                if (!_gestureFeedbacks.ContainsKey(gestureName.ToLower()))
                    _gestureFeedbacks[gestureName] = new List<Type>();

                _gestureFeedbacks[gestureName].Add(feedback);
            }
        }

        /// <summary>
        /// Adds the specified behavior into existing list. The framework will automatically run them as necessary
        /// </summary>
        /// <param name="behaviorType"></param>
        public static void AddTouchFeedback(Type behaviorType)
        {
            if (behaviorType.IsTypeOf(typeof(ITouchFeedback)))
            {
                ITouchFeedback behavior = Activator.CreateInstance(behaviorType) as ITouchFeedback;

                behavior.Init(LayoutRoot, LayoutRoot.Dispatcher);
                _commonBehaviors.Add(behavior);
            }
        }

        /// <summary>
        /// Removes the specified behavior from active behavior list
        /// </summary>
        /// <param name="behavior"></param>
        public static void RemoveCommonBehavior(ITouchFeedback behavior)
        {
            _commonBehaviors.Remove(behavior);
            behavior.Dispose();
        }

        /// <summary>
        /// Renders the specified debug panel at the top left corner of the screen
        /// </summary>
        /// <param name="panel"></param>
        public static void ShowDebugPanel(DebugPanels panel)
        {
            if (panel == DebugPanels.GestureRecorder)
            {
                ShowGestureRecorder();
            }
            else if (panel == DebugPanels.GestureRecorder)
            {
                ShowGestureDefBuilder();
            }
            else
            {
                throw new FrameworkException("Unrecognized debug panel");
            }
        }

        public static void ShowGestureRecorder()
        {
            if (_debugPanel == null)
                _debugPanel = new GestureRecordingPanel();

            //_debugPanel.VerticalAlignment = VerticalAlignment.Top;
            //_debugPanel.HorizontalAlignment = HorizontalAlignment.Right;

            var left = LayoutRoot.ActualWidth - _debugPanel.Width - 40;
            if (left < 0)
            {
                left = 0;
            }
            _debugPanel.SetValue(Canvas.TopProperty, 20.0);
            _debugPanel.SetValue(Canvas.LeftProperty, left);

            LayoutRoot.Children.Add(_debugPanel);
        }

        private static void ShowGestureDefBuilder()
        {
            //if (_defBuilder == null)
            //    _defBuilder = new GestureDefinitionBuilder();

            //_defBuilder.VerticalAlignment = VerticalAlignment.Top;
            //LayoutRoot.Children.Add(_defBuilder);
        }

        /// <summary>
        /// Hides any existing debug panel
        /// </summary>
        public static void HideDebugPanel()
        {
            LayoutRoot.Children.Remove(_debugPanel);
        }
    }
}
