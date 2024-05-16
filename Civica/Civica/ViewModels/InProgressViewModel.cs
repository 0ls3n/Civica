using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using GVMR;
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
                    EditVisibility = WindowVisibility.Hidden;

                    Progress prog = progressRepo.GetById(x => x.RefId == SelectedProject.GetId());


                    SelectedProgress = null;
                    if (prog is not null)
                    {
                        SelectedProgress = new ProgressViewModel(prog);
                    }
                    //Resource r = resourceRepo.GetById(x => x.RefId == SelectedProject.GetId());


                    //SelectedAudit = null;
                    //SelectedResource = null;
                    //if (r is not null)
                    //{
                    //    SelectedResource = new ResourceViewModel(r);

                    //    Audit aud = auditRepo.GetById(x => x.RefId == r.Id);
                    //    if (aud is not null)
                    //    {
                    //        SelectedAudit = new AuditViewModel(aud);
                    //    }
                    //}
                    OnPropertyChanged(nameof(SelectedProject));
                }
                else
                {
                    _selectedProject = value;
                    OnPropertyChanged(nameof(SelectedProject));
                }
            }
        }

        public string Title { get; set; } = "Audits";


        //private AuditViewModel _selectedAudit = null;
        //public AuditViewModel SelectedAudit
        //{
        //    get => _selectedAudit;
        //    set
        //    {
        //        _selectedAudit = value;
        //        OnPropertyChanged(nameof(SelectedAudit));
        //    }
        //}

        //private ResourceViewModel _selectedResource;
        //public ResourceViewModel SelectedResource
        //{
        //    get => _selectedResource;
        //    set
        //    {
        //        _selectedResource = value;
        //        OnPropertyChanged(nameof(SelectedResource));
        //    }
        //}

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

        public InProgressViewModel()
        {
            CreateProjectVM = new CreateProjectViewModel();
            CreateProjectVM.Init(this);


            WindowTitle = "Igangværende";

            CreateVisibility = WindowVisibility.Hidden;
            EditVisibility = WindowVisibility.Hidden;
            InformationVisibility = WindowVisibility.Visible;
        }

        public void UpdateList()
        {
            Projects.Clear();
            foreach (Project p in projectRepo.GetAll())
            {
                ProjectViewModel pvm = new ProjectViewModel(p);

                Projects.Add(pvm);

                Progress latestProg = progressRepo.GetListById(x => x.RefId == p.Id).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (latestProg != null)
                {
                    if (latestProg.Phase == Models.Enums.Phase.DONE)
                    {
                        Projects.Remove(pvm);
                    }
                }
            }

            foreach (ProjectViewModel p in Projects)
            {
                Progress prog = progressRepo.GetById(x => x.RefId == p.GetId());

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

            CreateProjectVM.SetRepo(projectRepo);
            CreateProjectVM.SetRepo(resourceRepo);

            UpdateList();
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

        #endregion
    }
}
