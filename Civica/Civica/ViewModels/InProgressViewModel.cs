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
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                if (value is not null)
                {
                    InformationVisibility = WindowVisibility.Hidden;
                    CreateVisibility = WindowVisibility.Hidden;
                    EditVisibility = WindowVisibility.Hidden;

                    Progress prog = progressRepo.GetListById(x => x.RefId == SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();


                    SelectedProgress = null;
                    if (prog is not null)
                    {
                        SelectedProgress = new ProgressViewModel(prog);
                    }
                }
            }
        }

        public string Title { get; set; } = "Audits";
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
                Progress prog = progressRepo.GetListById(x => x.RefId == p.GetId()).OrderByDescending(x => x.CreatedDate).FirstOrDefault();

                if (prog != null)
                {
                    p.SetColor(prog.Status);
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
