using System;
using System.Collections.Generic;
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
using Microsoft.Surface;
using Microsoft.Surface.Presentation;
using Microsoft.Surface.Presentation.Controls;
using Microsoft.Surface.Presentation.Input;

namespace SurfaceApplication
{
    /// <summary>
    /// Interaction logic for IGTOptions.xaml
    /// </summary>
    public partial class IGTOptions : UserControl
    {
        public IGTOptions()
        {
            InitializeComponent();
           
        }

        


        private void NoiseSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            noise.Text = NoiseSlider.Value.ToString();
        }

        private void MatchSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            accuracy.Text = MatchSlider.Value.ToString();
        }

      
    }
}
