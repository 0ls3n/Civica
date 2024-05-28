using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using GVMR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class ExpandedProjectViewModel : ObservableObject
    {
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
                UpdateProgressVisibility = WindowVisibility.Hidden;
                CreateProgressVisibility = WindowVisibility.Hidden;
                InformationVisibility = WindowVisibility.Visible;
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

        private WindowVisibility _progressVisibility;
        public WindowVisibility ProgressVisibility
        {
            get
            {
                return _progressVisibility;
            }
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(nameof(ProgressVisibility));
            }
        }

        private WindowVisibility _updateProgressVisibility;
        public WindowVisibility UpdateProgressVisibility
        {
            get
            {
                return _updateProgressVisibility;
            }
            set
            {
                _updateProgressVisibility = value;
                OnPropertyChanged(nameof(UpdateProgressVisibility));
            }
        }

        private WindowVisibility _createProgressVisibility;
        public WindowVisibility CreateProgressVisibility
        {
            get
            {
                return _createProgressVisibility;
            }
            set
            {
                _createProgressVisibility = value;
                OnPropertyChanged(nameof(CreateProgressVisibility));
            }
        }

        private WindowVisibility _updateProjectVisibility;
        public WindowVisibility UpdateProjectVisibility
        {
            get
            {
                return _updateProjectVisibility;
            }
            set
            {
                _updateProjectVisibility = value;
                OnPropertyChanged(nameof(UpdateProjectVisibility));
            }
        }

        private WindowVisibility _informationVisibility;
        public WindowVisibility InformationVisibility
        {
            get
            {
                return _informationVisibility;
            }
            set
            {
                _informationVisibility = value;
                OnPropertyChanged(nameof(InformationVisibility));
            }
        }

        public void UpdateList()
        {
            Progresses.Clear();
            Progresses = new ObservableCollection<ProgressViewModel>(progressRepo.GetListById(x => x.RefId == SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate).Select(x => new ProgressViewModel(x)));
            SelectedProgress = null;
        }

        public RelayCommand CancelCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedProjectViewModel epvm)
             {
                 epvm.UpdateList();

                 epvm.ProgressVisibility = WindowVisibility.Hidden;
                 epvm.UpdateProgressVisibility = WindowVisibility.Hidden;
                 epvm.ProgressVisibility = WindowVisibility.Hidden;
                 epvm.InformationVisibility = WindowVisibility.Hidden;
                 epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
             }
         },
         parameter =>
         {
             if (parameter is ExpandedProjectViewModel epvm)
             {
                 if (epvm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                 {
                     return true;
                 }
             }
             return false;
         });

        public RelayCommand UpdateProjectViewCmd { get; set; } = new RelayCommand(
             parameter =>
             {
                 if (parameter is ExpandedProjectViewModel epvm)
                 {
                     epvm.UpdateProjectVisibility = WindowVisibility.Visible;
                 }
             },
             parameter =>
             {
                 if (parameter is ExpandedProjectViewModel epvm)
                 {
                     if (epvm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                     {
                         return true;
                     }
                 }
                 return false;
             }
          );

        public RelayCommand UpdateProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {

                    epvm.UpdateProgressVisibility = WindowVisibility.Visible;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    if (epvm.SelectedProgress != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        public RelayCommand UpdateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    CRUDProjectViewModel.Instance.UpdateProject(epvm.SelectedProject);

                    epvm.UpdateProjectVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                return true;
            }
        );

        public RelayCommand DeleteProjectCmd { get; set; } = new RelayCommand
       (
           parameter =>
           {
               if (parameter is ExpandedProjectViewModel epvm)
               {
                   MessageBoxButton button = MessageBoxButton.OKCancel;
                   MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{epvm.SelectedProject.Name}'?", "Bekræft sletning", button);

                   if (result == MessageBoxResult.OK)
                   {
                       CRUDProjectViewModel.Instance.DeleteProject(epvm.SelectedProject);

                       InProgressViewModel.Instance.SelectedProject = null;
                       InProgressViewModel.Instance.SelectedProgress = null;

                       epvm.SelectedProject = null;
                       epvm.SelectedProgress = null;

                       MainViewModel.Instance.InProgressViewCmd.Execute(MainViewModel.Instance);

                   }
               }
           },
           parameter =>
           {
               if (parameter is ExpandedProjectViewModel ipvm)
               {
                   if (ipvm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                   {
                       return true;
                   }
               }
               return false;
           }
       );

        public RelayCommand UpdateProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    CRUDProgressViewModel.Instance.UpdateProgress(epvm.SelectedProgress);

                    epvm.UpdateProgressVisibility = WindowVisibility.Hidden;
                    epvm.ProgressVisibility = WindowVisibility.Visible;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                return true;
            }
        );
        public RelayCommand DeleteProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    MessageBoxButton button = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette denne?", "Bekræft sletning", button);

                    if (result == MessageBoxResult.OK)
                    {
                        CRUDProgressViewModel.Instance.DeleteProgress(epvm.SelectedProgress);
                        epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                        epvm.UpdateProgressVisibility = WindowVisibility.Hidden;
                        epvm.ProgressVisibility = WindowVisibility.Hidden;
                        epvm.UpdateProjectVisibility = WindowVisibility.Hidden;
                        epvm.InformationVisibility = WindowVisibility.Hidden;
                        epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                    }
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );
        public RelayCommand CreateProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    CRUDProgressViewModel.Instance.Description = "";
                    epvm.SelectedProgress = null;

                    CRUDProgressViewModel.Instance.Phase = Phase.IDENTIFIED;
                    CRUDProgressViewModel.Instance.Status = Status.NONE;

                    epvm.UpdateProgressVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Visible;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.UpdateProjectVisibility = WindowVisibility.Hidden;
                    epvm.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {

                    if (MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }

                }
                return false;
            }
        );

        //Singleton
        private ExpandedProjectViewModel()
        {
            progressRepo = MainViewModel.Instance.GetProgressRepo();
        }

        private static readonly object _lock = new object();
        private static ExpandedProjectViewModel _instance;

        public static ExpandedProjectViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new ExpandedProjectViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        //Singleton - Lazy
        //private static readonly Lazy<ExpandedProjectViewModel> lazy = new Lazy<ExpandedProjectViewModel>(() => new ExpandedProjectViewModel());

        //public static ExpandedProjectViewModel Instance => lazy.Value;
    }
}