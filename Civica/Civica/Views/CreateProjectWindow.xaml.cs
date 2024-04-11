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
using Civica.ViewModels;

namespace Civica
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class CreateProjectWindow : Window
    {
        private CreateProjectViewModel mvm; 
        public CreateProjectWindow()
        {
            InitializeComponent();
            mvm = new CreateProjectViewModel();
            this.DataContext = mvm;        
        }
    }
}