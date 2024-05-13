using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class ExpandedProjectViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm { get; set; }
        private CreateProgressViewModel cpvm { get; set; }

        private IRepository<Progress> progressRepo;
        private ObservableCollection<ProgressViewModel> _progresses = new ObservableCollection<ProgressViewModel>();

        public ObservableCollection<ProgressViewModel> Progresses
        {
            get { return _progresses; }
            set { _progresses = value; OnPropertyChanged(nameof(Progresses)); }
        }


        private ProgressViewModel _selectedProgress;
        public ProgressViewModel SelectedProgress
        {
            get => _selectedProgress;
            set
            {
                InformationPlaceholderVisibility = WindowVisibility.Hidden;
                ProgressVisibility = WindowVisibility.Visible;
                EditProgressVisibility = WindowVisibility.Hidden;
                CreateProgressVisibility = WindowVisibility.Hidden;
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
        private string _selectedDescription;

        public string SelectedDescription
        {
            get { return _selectedDescription; }
            set { _selectedDescription = value; OnPropertyChanged(nameof(SelectedDescription)); }
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
        private WindowVisibility _createProgressVisibility;

        public WindowVisibility CreateProgressVisibility
        {
            get { return _createProgressVisibility; }
            set { _createProgressVisibility = value; OnPropertyChanged(nameof(CreateProgressVisibility)); }
        }



        public string Title { get; set; } = "Audits";

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            cpvm = mvm.cpvm;
            progressRepo = mvm.GetProgressRepo();
        }

        public void GetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void UpdateList()
        {
            Progresses.Clear();
            Progresses = new ObservableCollection<ProgressViewModel>(progressRepo.GetByRefId(SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate).Select(x => new ProgressViewModel(x)));
            SelectedProgress = null;
        }
        public void UpdateProgress()
        {
            Progress p = progressRepo.GetById(SelectedProgress.GetId());
            p.Phase = SelectedPhase;
            p.Status = SelectedStatus;
            p.Description = SelectedDescription;
            progressRepo.Update(p);

            UpdateList();
            SelectedProgress = Progresses.FirstOrDefault(x => x.GetId() == p.Id);
        }
        public void RemoveProgress()
        {
            progressRepo.Remove(progressRepo.GetById(SelectedProgress.GetId()));
            UpdateList();
        }
       
        public RelayCommand EditProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    
                    epvm.EditProgressVisibility = WindowVisibility.Visible;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                    Progress p = epvm.progressRepo.GetById(epvm.SelectedProgress.GetId());
                    epvm.SelectedPhase = p.Phase;
                    epvm.SelectedStatus = p.Status;
                    epvm.SelectedDescription = p.Description;
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
                    epvm.UpdateProgress();

                    epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    epvm.ProgressVisibility = WindowVisibility.Visible;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                return true;
            }
        );
        public RelayCommand RemoveProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    MessageBoxButton button = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette denne?", "Bekræft sletning", button);

                    if (result == MessageBoxResult.OK)
                    {
                        epvm.RemoveProgress();
                        epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                        epvm.EditProgressVisibility = WindowVisibility.Hidden;
                        epvm.ProgressVisibility = WindowVisibility.Hidden;
                        epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                    }
                }
            },
            parameter =>
            {
                return true;
            }
        );
        public RelayCommand CreateProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    epvm.cpvm.ProgressDescription = "";

                    epvm.cpvm.SelectedPhase = Phase.IDENTIFIED;
                    epvm.cpvm.SelectedStatus = Status.NONE;

                    epvm.CreateProgressVisibility = WindowVisibility.Visible;
                    epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    
                     return true;
                    
                }
                return false;
            }
        );
    }
}
