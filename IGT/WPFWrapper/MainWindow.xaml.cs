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
using ASG.UI;


namespace WPFWrapper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     


        public MainWindow()
        {
            InitializeComponent();

            mainCanvasRoot.Height =  System.Windows.SystemParameters.PrimaryScreenHeight;
            mainCanvasRoot.Width =  System.Windows.SystemParameters.PrimaryScreenWidth;
            Main asg = new Main(this, mainCanvasRoot);

            asg.Height = mainCanvasRoot.Height;
            asg.Width = mainCanvasRoot.Width;

            mainCanvasRoot.Children.Add(asg);

        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();

        }
    }
}
