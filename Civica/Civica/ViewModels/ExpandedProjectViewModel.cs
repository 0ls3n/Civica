using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
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
    public class ExpandedProjectViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm { get; set; }
        public CreateProgressViewModel cpvm { get; set; } = new CreateProgressViewModel();

        private IRepository<Progress> progressRepo;
        private IRepository<Project> projectRepo;
        private IRepository<Resource> resourceRepo;
        private IRepository<Audit> auditRepo;
        private IRepository<Worktime> worktimeRepo;
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
            get
            {
                return _selectedDescription;
            }
            set
            {
                _selectedDescription = value;
                OnPropertyChanged(nameof(SelectedDescription));
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

        private WindowVisibility _editProgressVisibility;
        public WindowVisibility EditProgressVisibility
        {
            get
            {
                return _editProgressVisibility;
            }
            set
            {
                _editProgressVisibility = value;
                OnPropertyChanged(nameof(EditProgressVisibility));
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

        private WindowVisibility _editProjectVisibility;
        public WindowVisibility EditProjectVisibility
        {
            get
            {
                return _editProjectVisibility;
            }
            set
            {
                _editProjectVisibility = value;
                OnPropertyChanged(nameof(EditProjectVisibility));
            }
        }



        public string Title { get; set; } = "Audits";
        public ExpandedProjectViewModel()
        {
            cpvm = new CreateProgressViewModel();
            cpvm.Init(this);
        }
        public UserViewModel GetCurrentUser()
        {
            return mvm.CurrentUser;
        }
        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            progressRepo = mvm.GetProgressRepo();
            auditRepo = mvm.GetAuditRepo();
            worktimeRepo = mvm.GetWorktimeRepo();
            resourceRepo = mvm.GetResourceRepo();
            projectRepo = mvm.GetProjectRepo();

            cpvm.SetRepo(progressRepo);
        }

        public void GetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void UpdateList()
        {
            Progresses.Clear();
            Progresses = new ObservableCollection<ProgressViewModel>(progressRepo.GetListById(x => x.RefId == SelectedProject.GetId()).OrderByDescending(x => x.CreatedDate).Select(x => new ProgressViewModel(x)));
            SelectedProgress = null;
        }
        public void UpdateProgress()
        {
            Progress p = progressRepo.GetById(x => x.Id == SelectedProgress.GetId());
            p.Phase = SelectedPhase;
            p.Status = SelectedStatus;
            p.Description = SelectedDescription;
            progressRepo.Update(p);

            UpdateList();
            SelectedProgress = Progresses.FirstOrDefault(x => x.GetId() == p.Id);
        }

        public void RemoveProgress()
        {
            progressRepo.Remove(progressRepo.GetById(x => x.Id == SelectedProgress.GetId()));
            UpdateList();
        }

        public void RemoveProject()
        {
            int pID = SelectedProject.GetId();
            int rID = resourceRepo.GetByRefId(pID).FirstOrDefault().Id;
            auditRepo.RemoveByRefId(rID);
            worktimeRepo.RemoveByRefId(rID);
            progressRepo.RemoveByRefId(pID);
            resourceRepo.RemoveByRefId(pID);
            projectRepo.Remove(projectRepo.GetById(pID));
            projectRepo.Remove(projectRepo.GetById(x => x.Id == SelectedProject.GetId()));
            UpdateList();
            SelectedProject = null;
        }

        public void UpdateProject(ProjectViewModel projectVM)
        {
            Project p = projectRepo.GetById(x => x.Id == projectVM.GetId());
            p.Name = projectVM.Name;
            p.Owner = projectVM.Owner;
            p.Manager = projectVM.Manager;
            p.Description = projectVM.Description;

            projectRepo.Update(p);

            Resource r = resourceRepo.GetById(x => x.RefId == p.Id);
        }

        public RelayCommand EditProjectViewCmd { get; set; } = new RelayCommand(
             parameter =>
             {
                 if (parameter is ExpandedProjectViewModel epvm)
                 {
                     epvm.EditProjectVisibility = WindowVisibility.Visible;
                 }
             },
             parameter =>
             {
                 if (parameter is ExpandedProjectViewModel epvm)
                 {
                     if (epvm.SelectedProject != null && epvm.mvm.CurrentUser != null)
                     {
                         return true;
                     }
                 }
                 return false;
             }
          );

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
                    Progress p = epvm.progressRepo.GetById(x => x.Id == epvm.SelectedProgress.GetId());
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

        public RelayCommand EditProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    epvm.UpdateProject(epvm.SelectedProject);

                    epvm.EditProjectVisibility = WindowVisibility.Hidden;
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
               if (parameter is ExpandedProjectViewModel epvm)
               {
                   MessageBoxButton button = MessageBoxButton.OKCancel;
                   MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{epvm.SelectedProject.Name}'?", "Bekræft sletning", button);

                   if (result == MessageBoxResult.OK)
                   {
                       epvm.RemoveProject();

                       epvm.mvm.ipvm.SelectedProject = null;
                       epvm.mvm.ipvm.SelectedAudit = null;
                       epvm.mvm.ipvm.SelectedResource = null;
                       epvm.mvm.ipvm.SelectedProgress = null;

                       epvm.SelectedProject = null;
                       epvm.SelectedProgress = null;

                       epvm.mvm.InProgressViewCmd.Execute(epvm.mvm);

                   }
               }
           },
           parameter =>
           {
               if (parameter is ExpandedProjectViewModel ipvm)
               {
                   if (ipvm.SelectedProject != null && ipvm.mvm.CurrentUser != null)
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
                    epvm.UpdateProgress();

                    epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    epvm.ProgressVisibility = WindowVisibility.Visible;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Hidden;
                    epvm.EditProjectVisibility = WindowVisibility.Hidden;
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
                        epvm.EditProjectVisibility = WindowVisibility.Hidden;
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

                    epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Visible;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.EditProjectVisibility = WindowVisibility.Hidden;
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
