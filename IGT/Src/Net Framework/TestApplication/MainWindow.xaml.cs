using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TouchToolkit.GestureProcessor.Feedbacks.TouchFeedbacks;
using TouchToolkit.GestureProcessor.ReturnTypes;
using TouchToolkit.Framework;
using TouchToolkit.Framework.TouchInputProviders;

namespace TestApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Initialize Gesture Framework
            var provider = new Windows7TouchInputProvider();
            TestControl testControl = new TestControl(provider);
            LayoutRoot.Children.Add(testControl);

            testControl.LayoutRoot.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            testControl.LayoutRoot.Height = System.Windows.SystemParameters.PrimaryScreenHeight+ 200;


            testControl.LayoutGrid.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            testControl.LayoutGrid.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
        }
    }
}
