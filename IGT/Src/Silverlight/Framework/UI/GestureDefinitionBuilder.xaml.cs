using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Framework.Components.GestureDefBuilder;
using Framework.Utility;
using Gestures.Objects;

namespace Framework.UI
{
    public sealed partial  class GestureDefinitionBuilder : UserControl, IDisposable
    {
        GestureDefBuilder defBuilder = new GestureDefBuilder();
        GestureInfo gestureInfo = null;

        public GestureDefinitionBuilder()
        {
            InitializeComponent();

            this.Loaded += new RoutedEventHandler(GestureDefinitionBuilder_Loaded);
        }

        void GestureDefinitionBuilder_Loaded(object sender, RoutedEventArgs e)
        {
            // Load gesture data
            DataService.GestureServiceSoapClient client = new DataService.GestureServiceSoapClient();
            client.GetGestureDataAsync("V2", "demo", "move5");
            client.GetGestureDataCompleted += client_GetGestureDataCompleted;
        }

        void client_GetGestureDataCompleted(object sender, DataService.GetGestureDataCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                gestureInfo = SerializationHelper.Desirialize(e.Result);
                defBuilder.AddGesture(gestureInfo);
            }
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var x = defBuilder.GenerateGestureDefinition();

        }


        public void Dispose()
        {
            defBuilder.Dispose();            
        }

    }
}
