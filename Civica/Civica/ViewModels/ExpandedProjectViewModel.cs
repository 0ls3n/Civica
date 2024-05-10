using Civica.Commands;
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

        private IRepository<Progress> progressRepo;
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
        private Phase _selectedPhase;
        public Phase SelectedPhase
        {
            get { return _selectedPhase; }
            set
            {
                _selectedPhase = value;
                OnPropertyChanged(nameof(SelectedPhase));
            }
        }

        private Status _selectedStatus;

        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
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
        private WindowVisibility _progressVisibility;

        public WindowVisibility ProgressVisibility
        {
            get { return _progressVisibility; }
            set { _progressVisibility = value; OnPropertyChanged(nameof(ProgressVisibility)); }
        }
        private WindowVisibility _editProgressVisibility;

        public WindowVisibility EditProgressVisibility
        {
            get { return _editProgressVisibility; }
            set { _editProgressVisibility = value; OnPropertyChanged(nameof(EditProgressVisibility)); }
        }


        public string Title { get; set; } = "Audits";

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            progressRepo = mvm.GetProgressRepo();
        }

        public void GetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void UpdateList()
        {
            Progresses.Clear();
            foreach (Progress p in progressRepo.GetByRefId(SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate))
            {
                Progresses.Add(new ProgressViewModel(p));
            }
        }
        public void UpdateProgress(ProgressViewModel pvm)
        {
            Progress p = progressRepo.GetById(pvm.GetId());
            p.Phase = SelectedPhase;
            p.Status = SelectedStatus;
            p.Description = pvm.Description;
            progressRepo.Update(p);
        }
        public RelayCommand EditProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
            if (parameter is ExpandedProjectViewModel epvm)
            {
                epvm.EditProgressVisibility = WindowVisibility.Visible;
                epvm.ProgressVisibility = WindowVisibility.Hidden;
                    ProgressViewModel pvm = epvm.SelectedProgress;
                    int i = pvm.GetId();
        Progress p = epvm.progressRepo.GetById(i);
        epvm.SelectedPhase = p.Phase;
                    epvm.SelectedStatus = p.Status;
                }
},
            parameter =>
            {
                return true;
            }
        );
public RelayCommand UpdateProgressCmd { get; set; } = new RelayCommand
(
    parameter =>
    {
        if (parameter is ExpandedProjectViewModel epvm)
        {
            epvm.UpdateProgress(epvm.SelectedProgress);
            epvm.EditProgressVisibility = WindowVisibility.Hidden;
            epvm.ProgressVisibility = WindowVisibility.Visible;
        }
    },
    parameter =>
    {
        return true;
    }
);
    }
}
