using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System.Collections.ObjectModel;
using System.Windows;

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

        private WindowVisibility _createVisibility;
        public WindowVisibility CreateVisibility
        {
            get => _createVisibility;
            set
            {
                _createVisibility = value;
                OnPropertyChanged(nameof(CreateVisibility));
            }
        }

        private WindowVisibility _editVisibility;
        public WindowVisibility EditVisibility
        {
            get => _editVisibility;
            set
            {
                _editVisibility = value;
                OnPropertyChanged(nameof(EditVisibility));
            }
        }

        private WindowVisibility _progressVisibility;
        public WindowVisibility ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(nameof(ProgressVisibility));
            }
        }

        private WindowVisibility _informationVisibility;
        public WindowVisibility InformationVisibility
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

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
            {
                if (value is not null)
                {
                    _selectedProject = value;
                    InformationVisibility = WindowVisibility.Hidden;
                    CreateVisibility = WindowVisibility.Hidden;
                    ProgressVisibility = WindowVisibility.Hidden;
                    EditVisibility = WindowVisibility.Hidden;

                    Progress prog = progressRepo.GetByRefId(SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                    SelectedProgress = null;
                    if (prog is not null)
                    {
                        SelectedProgress = new ProgressViewModel(prog);
                    }

                    Resource r = resourceRepo.GetByRefId(SelectedProject.GetId()).FirstOrDefault();

                    SelectedAudit = null;
                    SelectedResource = null;
                    if (r is not null)
                    {
                        SelectedResource = new ResourceViewModel(r);

                        Audit aud = auditRepo.GetByRefId(r.Id).OrderByDescending(x => x.Year).FirstOrDefault();
                        if (aud is not null)
                        {
                            SelectedAudit = new AuditViewModel(aud);
                        }
                    }
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public string Title { get; set; } = "Audits";


        private AuditViewModel _selectedAudit = null;
        public AuditViewModel SelectedAudit
        {
            get => _selectedAudit;
            set
            {
                _selectedAudit = value;
                OnPropertyChanged(nameof(SelectedAudit));
            }
        }

        private ResourceViewModel _selectedResource;
        public ResourceViewModel SelectedResource
        {
            get => _selectedResource;
            set
            {
                _selectedResource = value;
                OnPropertyChanged(nameof(SelectedResource));
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

        private IRepository<Project> projectRepo;
        private IRepository<Progress> progressRepo;
        private IRepository<Resource> resourceRepo;
        private IRepository<Audit> auditRepo;

        public CreateProjectViewModel CreateProjectVM { get; set; }
        public CreateProgressViewModel CreateProgressVM { get; set; }

        public InProgressViewModel()
        {
            CreateProjectVM = new CreateProjectViewModel();
            CreateProjectVM.Init(this);

            CreateProgressVM = new CreateProgressViewModel();
            CreateProgressVM.Init(this);

            WindowTitle = "Igangværende";

            CreateVisibility = WindowVisibility.Hidden;
            EditVisibility = WindowVisibility.Hidden;
            ProgressVisibility = WindowVisibility.Hidden;
            InformationVisibility = WindowVisibility.Visible;
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
                Progress prog = progressRepo.GetByRefId(p.GetId()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

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
            projectRepo = this.mvm.GetProjectRepo();
            progressRepo = this.mvm.GetProgressRepo();
            resourceRepo = this.mvm.GetResourceRepo();
            auditRepo = this.mvm.GetAuditRepo();

            CreateProgressVM.SetRepo(progressRepo);
            CreateProjectVM.SetRepo(projectRepo);
            CreateProjectVM.SetRepo(resourceRepo);
            CreateProjectVM.SetRepo(auditRepo);

            UpdateList();
        }

        public void RemoveProject()
        {
            projectRepo.Remove(projectRepo.GetById(SelectedProject.GetId()));
            UpdateList();
            SelectedProject = null;
        }

        public void UpdateProject(ProjectViewModel projectVM, AuditViewModel auditVM)
        {
            Project p = projectRepo.GetById(projectVM.GetId());
            p.Name = projectVM.Name;
            p.Owner = projectVM.Owner;
            p.Manager = projectVM.Manager;
            p.Description = projectVM.Description;

            projectRepo.Update(p);

            Audit a = new Audit(mvm.CurrentUser.GetId(), auditVM.GetRefId(), int.Parse(auditVM.Amount), auditVM.Year, DateTime.Now);

            auditRepo.Add(a);
        }

        public UserViewModel GetCurrentUser()
        {
            return mvm.CurrentUser;
        }
        #region ViewCommands

        public RelayCommand CreateProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.CreateVisibility = WindowVisibility.Visible;
                    ipvm.InformationVisibility = WindowVisibility.Hidden;
                    ipvm.EditVisibility = WindowVisibility.Hidden;
                    ipvm.ProgressVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.mvm.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        public RelayCommand UpdateProjectViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    ipvm.EditVisibility = WindowVisibility.Visible;
                    ipvm.InformationVisibility = WindowVisibility.Hidden;
                    ipvm.CreateVisibility = WindowVisibility.Hidden;
                    ipvm.ProgressVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null && ipvm.mvm.CurrentUser != null)
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

                    ipvm.ProgressVisibility = WindowVisibility.Visible;
                    ipvm.EditVisibility = WindowVisibility.Hidden;
                    ipvm.InformationVisibility = WindowVisibility.Hidden;
                    ipvm.CreateVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null && ipvm.mvm.CurrentUser != null)
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
                    ipvm.UpdateProject(ipvm.SelectedProject, ipvm.SelectedAudit);

                    ipvm.EditVisibility = WindowVisibility.Hidden;
                    ipvm.InformationVisibility = WindowVisibility.Visible;
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
                        ipvm.InformationVisibility = WindowVisibility.Visible;
                    }
                }
            },
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null && ipvm.mvm.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        #endregion


    }
}
