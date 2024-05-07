using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
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
        private MainViewModel mvm { get; set; }

        IRepository<Progress> progressRepo;
        public ObservableCollection<ProgressViewModel> Progresses { get; set; } = new ObservableCollection<ProgressViewModel>();

        private ProgressViewModel _selectedProgress;
        public ProgressViewModel SelectedProgress 
        {
            get => _selectedProgress;
            set
            {
                InformationPlaceholderVisibility = WindowVisibility.Hidden;
                _selectedProgress = value;
                OnPropertyChanged(nameof(SelectedProgress));
            }
        }

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private WindowVisibility _informationPlaceholderVisibility;
        public WindowVisibility InformationPlaceholderVisibility
        {
            get => _informationPlaceholderVisibility;
            set
            {
                _informationPlaceholderVisibility = value;
                OnPropertyChanged(nameof(InformationPlaceholderVisibility));
            }
        }

        public string Title { get; set; } = "Audits";

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            progressRepo = this.mvm.GetProgressRepo();
        }

        public void GetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void UpdateList()
        {
            Progresses.Clear();
            foreach (Progress p in progressRepo.GetByRefId(SelectedProject.GetId()).OrderByDescending(x=> x.CreatedDate))
            {
                Progresses.Add(new ProgressViewModel(p));
            }
        }
    }
}
