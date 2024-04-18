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
using Civica.Models;
using Civica.ViewModels;

namespace Civica.Views
{
    /// <summary>
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window

    {
        private MainViewModel mvm;
        public StartWindow()
        {
            InitializeComponent();
            mvm = new MainViewModel();
            this.DataContext = mvm;
        }

        private void Button_Create(object sender, RoutedEventArgs e)
        {
            CreateProjectWindow cpw = new CreateProjectWindow(mvm);
            cpw.ShowDialog();

            if (cpw.DialogResult == true)
            {
                cpw.Close();
            }
        }

        private void Button_Update(object sender, RoutedEventArgs e)
        {
            UpdateProjectWindow upw = new UpdateProjectWindow(mvm);
            upw.ShowDialog();

            if (upw.DialogResult == true)
            {
                upw.Close();
            }
        }

        private void Button_Delete(object sender, RoutedEventArgs e)
        {
            if (mvm.SelectedProject != null)
            {
                mvm.RemoveProject();
            }
        }

        private void Button_Progress(object sender, RoutedEventArgs e)
        {
            ProgressProjectWindow ppw = new ProgressProjectWindow(mvm);
            ppw.ShowDialog();

            if (ppw.DialogResult == true)
            {
                ppw.Close();
            }
        }
    }
}
