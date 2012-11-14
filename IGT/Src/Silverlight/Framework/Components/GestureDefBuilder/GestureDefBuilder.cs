using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using Gestures.Objects;
using Framework.Components.GestureRecording;

using Gestures.Rules;
using Framework.HardwareListeners;
using System.Text;
using Framework.Utility;
using Gestures.Rules.RuleValidators;
using Gestures.Rules.Objects;

namespace Framework.Components.GestureDefBuilder
{
    public class GestureDefBuilder : IDisposable
    {
        const string TabSapce = "    ";
        private VirtualTouchInputProvider _virtualTouchListener = new VirtualTouchInputProvider();
        List<IRuleValidator> PreConditionValidators = new List<IRuleValidator>();
        List<IRuleValidator> ConditionValidators = new List<IRuleValidator>();

        private List<GestureInfo> _gestureSamples = new List<GestureInfo>();
        public List<GestureInfo> GestureSamples
        {
            get
            {
                return _gestureSamples;
            }
        }

        public GestureDefBuilder()
        {
            Init();
        }

        /// <summary>
        /// Adds the gestureInfo to the end of GestureSamples collection
        /// </summary>
        /// <param name="gesureInfo"></param>
        public void AddGesture(GestureInfo gesureInfo)
        {
            _gestureSamples.Add(gesureInfo);
        }

        /// <summary>
        /// Clears all temporary variables including gesture samples 
        /// </summary>
        public void Reset()
        {
            _gestureSamples.Clear();

            _virtualTouchListener.Dispose();
            _virtualTouchListener = new VirtualTouchInputProvider();
        }

        /// <summary>
        /// Builds GestureInfo from the xml and adds into GestureSamples collection
        /// </summary>
        /// <param name="xml"></param>
        public void AddGesture(string xml)
        {
            GestureInfo info = SerializationHelper.Desirialize(xml);
            _gestureSamples.Add(info);
        }

        /// <summary>
        /// Returns a list of rules that are valid for the "PreCondition" block of gesture definition for the given gesture samples
        /// </summary>
        /// <returns></returns>
        public List<IRuleData> GetValidPreConditions()
        {
            List<IRuleData> rulesForPreConditionBlock = new List<IRuleData>();
            rulesForPreConditionBlock = BuildRuleDataForGestures(PreConditionValidators);
            return rulesForPreConditionBlock;
        }

        /// <summary>
        /// Returns a list of rules that are valid for the "Condition" block of gesture definition for the given gesture samples
        /// </summary>
        /// <returns></returns>
        public List<IRuleData> GetValidConditions()
        {
            List<IRuleData> rulesForConditionBlock = new List<IRuleData>();
            rulesForConditionBlock = BuildRuleDataForGestures(ConditionValidators);
            return rulesForConditionBlock;
        }

        /// <summary>
        /// Generates gesture code template based on the sample gestures
        /// </summary>
        /// <returns></returns>
        public string GenerateGestureDefinition()
        {
            string template = ContentHelper.GetEmbeddedTextContent(this, "GestureDefinitionTemplate.txt");
            StringBuilder preConditionCodes = new StringBuilder();
            StringBuilder conditionCodes = new StringBuilder();

            // Generate code for PreCondition block
            List<IRuleData> preConditionRuleData = GetValidPreConditions();


            foreach (IRuleData ruleData in preConditionRuleData)
            {
                preConditionCodes.Append(TabSapce);
                preConditionCodes.Append(ruleData.ToGDL());
                preConditionCodes.Append(Environment.NewLine);
            }

            // Generate entire gesture definitioin using the template
            template = template.Replace("<validationstate>", preConditionCodes.ToString());
            template = template.Replace("<condition>", preConditionCodes.ToString());

            return template;
        }

        private void Init()
        {
            //NOTE: We are considering only following two rules as pre-conditions: TouchLimit & TouchState. 
            PreConditionValidators.Clear();
            PreConditionValidators.Add(new TouchLimitValidator());
            PreConditionValidators.Add(new TouchStateValidator());

            // Get all validation rules except preconditions
            ConditionValidators.Clear();
            List<Type> allRules = GestureLanguageProcessor.GetAllRules();
            foreach (Type type in allRules)
            {
                if (!IsPreCondition(type))
                    ConditionValidators.Add(Activator.CreateInstance(type) as IRuleValidator);
            }
        }

        private IRuleData CombineAllRuleDataIntoOne(List<IRuleData> ruleDataList)
        {
            if (ruleDataList.Count > 1)
            {
                IRuleData ruleData = Activator.CreateInstance(ruleDataList[0].GetType()) as IRuleData;

                foreach (IRuleData data in ruleDataList)
                {
                    ruleData.Union(data);
                }

                return ruleData;
            }
            else if (ruleDataList.Count == 1)
            {
                return ruleDataList[0];
            }
            else
            {
                return null;
            }
        }

        private List<IRuleData> BuildRuleDataForGestures(List<IRuleValidator> ruleValidators)
        {
            List<IRuleData> preConditionRules = new List<IRuleData>();

            foreach (IRuleValidator validator in ruleValidators)
            {
                List<IRuleData> ruleDataList = new List<IRuleData>();
                foreach (GestureInfo gestureInfo in _gestureSamples)
                {
                    foreach (FrameInfo frameInfo in gestureInfo.Frames)
                    {
                        // A virtual touch provider will simulate the behaviour of any real touch provider (i.e. Silverlight framework)
                        List<TouchPoint2> touchPoints = _virtualTouchListener.UpdateActiveTouchPoints(frameInfo.Touches);

                        IRuleData ruleData = validator.GenerateRuleData(touchPoints);
                        ruleDataList.Add(ruleData);
                    }
                }

                IRuleData finalRuleData = CombineAllRuleDataIntoOne(ruleDataList);
                preConditionRules.Add(finalRuleData);
            }

            return preConditionRules;
        }

        private bool IsPreCondition(Type type)
        {
            bool result = false;
            foreach (IRuleValidator validator in PreConditionValidators)
            {
                if (validator.GetType().Equals(type))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// A call to Dispose(false) should only clean up native resources. A call to Dispose(true) should clean up both managed and native resources.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void Dispose(bool value)
        {
            if (value)
            {
                // Clean up both managed and native resources
                CleanUpManagedResources();
                CleanUpNativeResources();
            }
            else
            {
                // Clean up native resources
                CleanUpNativeResources();
            }

        }

        private void CleanUpNativeResources()
        {
            // Clean up native resources
        }

        private void CleanUpManagedResources()
        {
            // Clean up managed resources
            _virtualTouchListener.Dispose();
        }
    }
}
