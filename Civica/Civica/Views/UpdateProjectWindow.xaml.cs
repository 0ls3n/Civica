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
    /// Interaction logic for UpdateProjectWindow.xaml
    /// </summary>
    public partial class UpdateProjectWindow : Window
    {

        private UpdateProjectViewModel upvm;
        public UpdateProjectWindow(MainViewModel mvm)
        {
            InitializeComponent();
            upvm = new UpdateProjectViewModel(mvm);
            this.DataContext = upvm;
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Button_Back(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
