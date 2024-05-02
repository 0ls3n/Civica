using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Civica.Views
{
    /// <summary>
    /// Interaction logic for InformationPlaceholderView.xaml
    /// </summary>
    public partial class InformationPlaceholderView : UserControl
    {
        public double ImageWidth
        {
            get { return (double)GetValue(ImageWidthProperty); }
            set { SetValue(ImageWidthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageWidth.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageWidthProperty =
            DependencyProperty.Register("ImageWidth", typeof(double), typeof(InformationPlaceholderView), new PropertyMetadata(200.0, new PropertyChangedCallback(OnWidthChanged)));


        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(InformationPlaceholderView), new PropertyMetadata("Lorem Ipsum", new PropertyChangedCallback(OnTextChanged)));


        public InformationPlaceholderView()
        {
            InitializeComponent();
            placeholder_image.Width = ImageWidth;
            placeholder_image.Height = ImageWidth;
            placeholder_text.Text = PlaceholderText;
        }

        private static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InformationPlaceholderView InformationPlaceholderUserControl = d as InformationPlaceholderView;
            InformationPlaceholderUserControl.OnWidthChanged(e);
        }

        private void OnWidthChanged(DependencyPropertyChangedEventArgs e)
        {
            placeholder_image.Width = (double)e.NewValue;
            placeholder_image.Height = (double)e.NewValue;
        }

        private static void OnTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InformationPlaceholderView InformationPlaceholderUserControl = d as InformationPlaceholderView;
            InformationPlaceholderUserControl.OnTextChanged(e);
        }

        private void OnTextChanged(DependencyPropertyChangedEventArgs e)
        {
            placeholder_text.Text = e.NewValue.ToString();
        }
    }
}
