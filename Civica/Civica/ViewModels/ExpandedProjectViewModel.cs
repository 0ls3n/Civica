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
    public class ExpandedProjectViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;
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
            p.Phase = Helper.Phases.FirstOrDefault(x => x.Value == SelectedProgress.Phase).Key;
            p.Status = Helper.Statuses.FirstOrDefault(x => x.Value == SelectedProgress.Status).Key;
            p.Description = SelectedProgress.Description;
            progressRepo.Update(p);

            UpdateList();
    
            SelectedProgress = Progresses.FirstOrDefault(x => x.GetId() == p.Id);
            SelectedProject.SetColor(p.Status);
        }

        public void RemoveProgress()
        {
            progressRepo.Remove(progressRepo.GetById(x => x.Id == SelectedProgress.GetId()));
            UpdateList();
        }

        public void RemoveProject()
        {
            int pID = SelectedProject.GetId();
            int rID = resourceRepo.GetById(x => x.RefId == pID).Id;
            auditRepo.RemoveByRefId(rID);
            worktimeRepo.RemoveByRefId(rID);
            progressRepo.RemoveByRefId(pID);
            resourceRepo.RemoveByRefId(pID);
            projectRepo.Remove(projectRepo.GetById(x => x.Id == pID));
            UpdateList();
            SelectedProject = null;
        }

        public RelayCommand CancelCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedProjectViewModel epvm)
             {
                 epvm.UpdateList();

                 epvm.ProgressVisibility = WindowVisibility.Hidden;
                 epvm.EditProgressVisibility = WindowVisibility.Hidden;
                 epvm.ProgressVisibility = WindowVisibility.Hidden;
                 epvm.InformationVisibility = WindowVisibility.Hidden;
                 epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
             }
         },
         parameter =>
         {
             if (parameter is ExpandedProjectViewModel epvm)
             {
                 if (epvm.SelectedProject != null && epvm.GetCurrentUser() != null)
                 {
                     return true;
                 }
             }
             return false;
         });

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
                    epvm.InformationVisibility = WindowVisibility.Hidden;
                    Progress p = epvm.progressRepo.GetById(x => x.Id == epvm.SelectedProgress.GetId());
                    epvm.cpvm.Phase = p.Phase;
                    epvm.cpvm.Status = p.Status;
                    epvm.cpvm.Description = p.Description;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    if (epvm.SelectedProgress != null && epvm.mvm.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
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
                    epvm.InformationVisibility = WindowVisibility.Visible;
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
                        epvm.InformationVisibility = WindowVisibility.Hidden;
                        epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
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
        public RelayCommand CreateProgressViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {
                    epvm.cpvm.Description = "";
                    epvm.SelectedProgress = null;

                    epvm.cpvm.Phase = Phase.IDENTIFIED;
                    epvm.cpvm.Status = Status.NONE;

                    epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    epvm.CreateProgressVisibility = WindowVisibility.Visible;
                    epvm.ProgressVisibility = WindowVisibility.Hidden;
                    epvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    epvm.EditProjectVisibility = WindowVisibility.Hidden;
                    epvm.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedProjectViewModel epvm)
                {

                    if (epvm.mvm.CurrentUser != null)
                    {
                        return true;
                    }

                }
                return false;
            }
        );
    }
}
