using Civica.Commands;
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
    public class InProgressViewModel : ObservableObject
    {

        public MainViewModel mvm;

        private string _Windowtitle;
        public string WindowTitle
        {
            get => _Windowtitle;
            set
            {
                _Windowtitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        #region Create
        private string _projectName = "";
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string _projectOwner = "";
        public string ProjectOwner
        {
            get => _projectOwner;
            set
            {
                _projectOwner = value;
                OnPropertyChanged(nameof(ProjectOwner));
            }
        }

        private string _projectManager = "";

        public string ProjectManager
        {
            get => _projectManager;
            set
            {
                _projectManager = value;
                OnPropertyChanged(nameof(ProjectManager));
            }
        }

        private string _projectDescription = "";

        public string ProjectDescription
        {
            get => _projectDescription;
            set
            {
                _projectDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }

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

        public void CreateProject()
        {
            Project p = new Project(ProjectName, ProjectOwner, ProjectManager, ProjectDescription);
            projectRepo.Add(p);

            UpdateList();

            ProjectName = "";
            ProjectManager = "";
            ProjectOwner = "";
            ProjectDescription = "";
        }

        public void CreateProgress(Phase phase, Status status, string description)
        {
            Progress prog = new Progress(SelectedProject.GetId(), phase, status, DateTime.Now, description);

            progressRepo.Add(prog);

            SelectedProgresses.Add(new ProgressViewModel(prog));
        }

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

        public RelayCommand CreateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.CreateProject();
                    ipvm.CreateVisibility = "Hidden";
                    ipvm.InformationVisibility = "Visible";
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is InProgressViewModel ipvm)
                {
                    if (!string.IsNullOrEmpty(ipvm.ProjectName))
                    {
                        succes = true;
                    }
                }
                return succes;
            }
        );
        #endregion

        #region Update
        public string OldName;

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

        public void UpdateProject(ProjectViewModel projectVM)
        {
            projectRepo.Update(projectRepo.Get(projectVM.GetId()), projectVM.Name, projectVM.Owner, projectVM.Manager, projectVM.Description);
        }

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

        public RelayCommand UpdateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.Projects.FirstOrDefault(x => x.Name.ToLower() == ipvm.ProjectName.ToLower()) is null || ipvm.ProjectName.ToLower() == ipvm.OldName.ToLower())
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
        #endregion

        #region Progress
        public ObservableCollection<string> Phases { get; set; } = new ObservableCollection<string> { "Identificeret", "Planlægning", "Gennemførsel", "Drift", "Opfølgning", "Afsluttet" };
        public ObservableCollection<string> Statuses { get; set; } = new ObservableCollection<string> { "Ingen vurdering", "Kritisk", "Forsinket", "Planmæssigt" };
        public ObservableCollection<ProgressViewModel> SelectedProgresses { get; set; } = new ObservableCollection<ProgressViewModel>();
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

        private string _progressDescription = "";
        public string ProgressDescription
        {
            get
            {
                return _progressDescription;
            }
            set
            {
                _progressDescription = value;
                OnPropertyChanged(nameof(ProgressDescription));
            }
        }
        private string _selectedPhase;

        public string SelectedPhase
        {
            get { return _selectedPhase; }
            set
            {
                _selectedPhase = value;
                OnPropertyChanged(nameof(SelectedPhase));
                switch (value)
                {
                    case "Identificeret":
                        EnumPhase = Phase.IDENTIFIED;
                        break;
                    case "Planlægning":
                        EnumPhase = Phase.PLANNING;
                        break;
                    case "Gennemførsel":
                        EnumPhase = Phase.IMPLEMENTATION;
                        break;
                    case "Drift":
                        EnumPhase = Phase.OPERATION;
                        break;
                    case "Opfølgning":
                        EnumPhase = Phase.FOLLOW_UP;
                        break;
                    case "Afsluttet":
                        EnumPhase = Phase.DONE;
                        break;
                }
            }
        }

        private Phase _enumPhase;
        public Phase EnumPhase
        {
            get
            { return _enumPhase; }
            set
            {
                _enumPhase = value;
                OnPropertyChanged(nameof(EnumPhase));
            }
        }
        private string _selectedStatus;

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set 
            { 
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                switch (value)
                
                {
                    case "Ingen Vurdering":
                        EnumStatus = Status.NONE;
                        break;
                    case "Kritisk":
                        EnumStatus = Status.CRITICAL;
                        break;
                    case "Forsinket":
                        EnumStatus = Status.DELAYED;
                        break;
                    case "Planmæssigt":
                        EnumStatus = Status.ON_TRACK;
                        break;
                }
            }
        }


        private Status _enumStatus;
        public Status EnumStatus
        {
            get
            {
                return _enumStatus;
            }
            set
            {
                _enumStatus = value;
                OnPropertyChanged(nameof(EnumStatus));
            }
        }

        public RelayCommand ProgressProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.CreateProgress(ipvm.EnumPhase, ipvm.EnumStatus, ipvm.ProgressDescription);

                    ipvm.UpdateList();

                    ipvm.ProgressVisibility = "Hidden";
                    ipvm.InformationVisibility = "Visible";
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject is not null)
                    {
                        if (!string.IsNullOrEmpty(ipvm.ProgressDescription))
                        {
                            succes = true;
                        }
                    }
                }
                return succes;
            }
        );
        public RelayCommand ProgressProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.ProgressDescription = "";
                    ipvm.SelectedPhase = "Identificeret";
                    ipvm.SelectedStatus = "Ingen Vurdering";

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
        #endregion

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

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
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

        private ProjectRepository projectRepo = new ProjectRepository();
        private ProgressRepository progressRepo = new ProgressRepository();

        public InProgressViewModel()
        {
            UpdateList();

            WindowTitle = "Igangværende";

            CreateVisibility = "Hidden";
            EditVisibility = "Hidden";
            ProgressVisibility = "Hidden";
            InformationVisibility = "Hidden";

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

        public void Init(MainViewModel mvm)
        {
            this.mvm = mvm;
        }

        public void RemoveProject()
        {
            projectRepo.Remove(projectRepo.Get(SelectedProject.GetId()));
            UpdateList();
        }
    }
}
