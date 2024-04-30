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
using System.Windows.Input;

namespace Civica.ViewModels
{
    public class InProgressViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;

        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        #region VisibilityProperties

        private string _createVisibility;
        public string CreateVisibility
        {
            get => _createVisibility;
            set
            {
                _createVisibility = value;
                OnPropertyChanged(nameof(CreateVisibility));
            }
        }

        private string _editVisibility;
        public string EditVisibility
        {
            get => _editVisibility;
            set
            {
                _editVisibility = value;
                OnPropertyChanged(nameof(EditVisibility));
            }
        }

        private string _progressVisibility;
        public string ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(nameof(ProgressVisibility));
            }
        }

        private string _informationVisibility;
        public string InformationVisibility
        {
            get => _informationVisibility;
            set
            {
                _informationVisibility = value;
                OnPropertyChanged(nameof(InformationVisibility));
            }
        }

        #endregion

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        //public ObservableCollection<ProgressViewModel> SelectedProgresses { get; set; } = new ObservableCollection<ProgressViewModel>();

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (value is not null)
                {
                    _selectedProject = value;
                    InformationVisibility = "Hidden";
                    CreateVisibility = "Hidden";
                    ProgressVisibility = "Hidden";
                    EditVisibility = "Hidden";

                    List<Progress> sortedList = progressRepo.Get(SelectedProject.GetId()).OrderByDescending(x => x.Date).ToList();
                    Progress prog = sortedList.FirstOrDefault();

                    SelectedProgress = null;
                    if (prog is not null)
                    {
                        SelectedProgress = new ProgressViewModel(sortedList.FirstOrDefault());
                    }
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        private ProgressViewModel _selectedProgress = null;
        public ProgressViewModel SelectedProgress
        {
            get
            {
                return _selectedProgress;
            }
            set
            {
                _selectedProgress = value;
                OnPropertyChanged(nameof(SelectedProgress));
            }
        }

        public string OldName;

        private Repository<Project> projectRepo;
        private Repository<Progress> progressRepo;

        public CreateProjectViewModel CreateProjectVM { get; set; }
        public CreateProgressViewModel CreateProgressVM { get; set; }

        public InProgressViewModel()
        {
            projectRepo = new Repository<Project>(id =>
            {
                return projectRepo.GetAll().FindAll(x => x.Id == id);
            });
            progressRepo = new Repository<Progress>(id =>
            {
                return progressRepo.GetAll().FindAll(x => x.RefId == id);
            });

            CreateProjectVM = new CreateProjectViewModel();
            CreateProjectVM.Init(this);
            CreateProjectVM.GetRepo(projectRepo);

            CreateProgressVM = new CreateProgressViewModel();
            CreateProgressVM.Init(this);
            CreateProgressVM.GetRepo(progressRepo);

            UpdateList();

            WindowTitle = "Igangværende";

            CreateVisibility = "Hidden";
            EditVisibility = "Hidden";
            ProgressVisibility = "Hidden";
            InformationVisibility = "Visible";
        }

        public void UpdateList()
        {
            Projects.Clear();
            foreach (Project p in projectRepo.GetAll())
            {
                Projects.Add(new ProjectViewModel(p));
            }

            foreach (ProjectViewModel p in Projects)
            {
                List<Progress> sortedList = progressRepo.Get(p.GetId()).OrderByDescending(x => x.Date).ToList();

                Progress prog = sortedList.FirstOrDefault();

                if (prog != null)
                {
                    switch (prog.Status)
                    {
                        case Models.Enums.Status.ON_TRACK:
                            p.StatusColor = "#008000";
                            break;
                        case Models.Enums.Status.DELAYED:
                            p.StatusColor = "#FDC300";
                            break;
                        case Models.Enums.Status.CRITICAL:
                            p.StatusColor = "#E20F1A";
                            break;
                        default:
                            p.StatusColor = "#E8E8E8";
                            break;
                    }
                }
                else
                {
                    p.StatusColor = "#E8E8E8";
                }
            }
        }

        public void Init(ObservableObject o)
        {
            this.mvm = (o as MainViewModel);
        }

        public void RemoveProject()
        {
            projectRepo.Remove(projectRepo.GetById(SelectedProject.GetId()));
            UpdateList();
        }

        public void UpdateProject(ProjectViewModel projectVM)
        {
            Project p = projectRepo.GetById(projectVM.GetId());
            p.Name = projectVM.Name;
            p.Owner = projectVM.Owner;
            p.Manager = projectVM.Manager;
            p.Description = projectVM.Description;

            projectRepo.Update(p);
        }

        #region ViewCommands

        public RelayCommand CreateProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.CreateVisibility = "Visible";
                    ipvm.InformationVisibility = "Hidden";
                    ipvm.EditVisibility = "Hidden";
                    ipvm.ProgressVisibility = "Hidden";
                }
            },
            parameter =>
            {
                return true;
            }
        );

        public RelayCommand UpdateProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.EditVisibility = "Visible";
                    ipvm.InformationVisibility = "Hidden";
                    ipvm.CreateVisibility = "Hidden";
                    ipvm.ProgressVisibility = "Hidden";
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        public RelayCommand ProgressProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.CreateProgressVM.ProgressDescription = "";

                    ipvm.CreateProgressVM.SelectedPhase = Phase.IDENTIFIED;
                    ipvm.CreateProgressVM.SelectedStatus = Status.NONE;

                    ipvm.ProgressVisibility = "Visible";
                    ipvm.EditVisibility = "Hidden";
                    ipvm.InformationVisibility = "Hidden";
                    ipvm.CreateVisibility = "Hidden";
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        #endregion

        #region FunctionalityCommands

        public RelayCommand UpdateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.Projects.FirstOrDefault(x => x.Name.ToLower() == ipvm.CreateProjectVM.ProjectName.ToLower()) is null || ipvm.CreateProjectVM.ProjectName.ToLower() == ipvm.OldName.ToLower())
                    {
                        ipvm.UpdateProject(ipvm.SelectedProject);

                        ipvm.EditVisibility = "Hidden";
                        ipvm.InformationVisibility = "Visible";
                    }
                }
            },
            parameter =>
            {
                return true;
            }
        );

        public RelayCommand RemoveProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    MessageBoxButton button = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{ipvm.SelectedProject.Name}'?", "Bekræft sletning", button);

                    if (result == MessageBoxResult.OK)
                    {
                        ipvm.RemoveProject();
                        ipvm.InformationVisibility = "Visible";
                    }
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null)
                    {
                        succes = true;
                    }
                }
                return succes;
            }
        );

        #endregion


    }
}
