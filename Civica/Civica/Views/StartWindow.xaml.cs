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
    /// Interaction logic for StartWindow.xaml
    /// </summary>
    public partial class StartWindow : Window

    {
        private MainViewModel mainViewModel;
        public StartWindow()
        {
            InitializeComponent();
            mainViewModel = new MainViewModel(); 
            this.DataContext = mainViewModel;

        }
    }
}
