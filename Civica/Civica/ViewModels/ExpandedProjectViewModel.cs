using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class ExpandedProjectViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            MessageBox.Show(mvm.ToString());
        }
    }
}
