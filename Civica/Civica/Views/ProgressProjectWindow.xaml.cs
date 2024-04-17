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
using System.Windows.Shapes;
using Civica.ViewModels;

namespace Civica.Views
{
    /// <summary>
    /// Interaction logic for ProgressProjectWindow.xaml
    /// </summary>
    public partial class ProgressProjectWindow : Window
    {
        private MainViewModel mvm;
        private ProgressProjectViewModel ppvm;

        public ProgressProjectWindow(MainViewModel mvm)
        {
            InitializeComponent();
            this.mvm = mvm;
            ppvm = new ProgressProjectViewModel(mvm);
            DataContext = ppvm;
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
