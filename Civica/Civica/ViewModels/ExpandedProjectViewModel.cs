using Civica.Interfaces;
using Civica.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class ExpandedProjectViewModel : ObservableObject, IViewModelChild
    {
        public MainViewModel mvm { get; set; }

        IRepository<Progress> progressRepo;
        ObservableCollection<ProgressViewModel> Progresses { get; set; } = new ObservableCollection<ProgressViewModel>();

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
        }

        public void GetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void UpdateList()
        {

        }
    }
}
