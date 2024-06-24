using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using Civica.Views;
using GVMR;

namespace Civica.ViewModels
{
    public class MainViewModel : ObservableObject
    {

        private string _viewTitle;
        public string ViewTitle
        {
            get => _viewTitle;
            set
            {
                _viewTitle = value;
                OnPropertyChanged(nameof(ViewTitle));
            }
        }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }
        private UserViewModel currentUser;

        public UserViewModel CurrentUser
        {
            get { return currentUser; }
            set
            {
                currentUser = value;
                OnPropertyChanged(nameof(CurrentUser));
            }
        }
        private WindowVisibility _inProgressView;
        public WindowVisibility InProgressView
        {
            get => _inProgressView;

            set
            {
                _inProgressView = value;
                OnPropertyChanged(nameof(InProgressView));
            }
        }

        private WindowVisibility _expandedProjectView;
        public WindowVisibility ExpandedProjectView
        {
            get => _expandedProjectView;
            set
            {
                _expandedProjectView = value;
                OnPropertyChanged(nameof(ExpandedProjectView));
            }
        }
        private WindowVisibility _resourceView;
        public WindowVisibility ResourceView
        {
            get => _resourceView;
            set
            {
                _resourceView = value;
                OnPropertyChanged(nameof(ResourceView));
            }
        }
        private WindowVisibility _settingsView;

        public WindowVisibility SettingsView
        {
            get { return _settingsView; }
            set
            {
                _settingsView = value;
                OnPropertyChanged(nameof(SettingsView));
            }
        }
        private WindowVisibility _loginView;

        public WindowVisibility LoginView
        {
            get { return _loginView; }
            set
            {
                _loginView = value;
                OnPropertyChanged(nameof(LoginView));
            }
        }

        private WindowVisibility _statusDot;
        public WindowVisibility StatusDot
        {
            get => _statusDot;
            set
            {
                _statusDot = value;
                OnPropertyChanged(nameof(StatusDot));
            }
        }

        private WindowVisibility _archiveView;
        public WindowVisibility ArchiveView
        {
            get => _archiveView;
            set
            {
                _archiveView = value;
                OnPropertyChanged(nameof(ArchiveView));
            }
        }

        private string _userIconPath = "/Resources/Images/login.png";
        public string UserIconPath
        {
            get => _userIconPath;
            set
            {
                _userIconPath = value;
                OnPropertyChanged(nameof(UserIconPath));
            }
        }

        public RelayCommand InProgressViewCmd { get; set; } = new RelayCommand
        (
            execute: (object? parameter) =>
            {
                if (parameter is MainViewModel mvm)
                {
                    mvm.InProgressView = WindowVisibility.Visible;
                    mvm.ExpandedProjectView = WindowVisibility.Hidden;
                    mvm.SettingsView = WindowVisibility.Hidden;
                    mvm.ResourceView = WindowVisibility.Hidden;
                    mvm.StatusDot = WindowVisibility.Hidden;
                    mvm.ArchiveView = WindowVisibility.Hidden;
                    mvm.LoginView = WindowVisibility.Hidden;
                    InProgressViewModel.Instance.CreateVisibility = WindowVisibility.Hidden;
                    InProgressViewModel.Instance.InformationVisibility = WindowVisibility.Visible;
                    mvm.ViewTitle = InProgressViewModel.Instance.WindowTitle;
                    InProgressViewModel.Instance.UpdateList();
                }
            },
            canExecute: (object? parameter) =>
            {
                return true;
            });

        public RelayCommand ExpandedProjectViewCmd { get; set; } = new RelayCommand
            (
            parameter =>
            {
                if (parameter is MainViewModel mvm)
                {
                    mvm.ExpandedProjectView = WindowVisibility.Visible;
                    mvm.StatusDot = WindowVisibility.Visible;
                    mvm.InProgressView = WindowVisibility.Hidden;
                    mvm.SettingsView = WindowVisibility.Hidden;
                    mvm.LoginView = WindowVisibility.Hidden;
                    mvm.ResourceView = WindowVisibility.Hidden;
                    mvm.ArchiveView = WindowVisibility.Hidden;
                    mvm.ViewTitle = InProgressViewModel.Instance.SelectedProject.Name;
                    ExpandedProjectViewModel.Instance.SelectedProject = InProgressViewModel.Instance.SelectedProject;
                    ExpandedProjectViewModel.Instance.UpdateList();
                    ExpandedProjectViewModel.Instance.InformationPlaceholderVisibility = WindowVisibility.Visible;
                    ExpandedProjectViewModel.Instance.UpdateProjectVisibility = WindowVisibility.Hidden;
                    ExpandedProjectViewModel.Instance.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                bool isEnabled = false;
                if (parameter is MainViewModel mvm)
                {
                    if (InProgressViewModel.Instance.SelectedProject != null)
                    {
                        isEnabled = true;
                    }
                }
                return isEnabled;
            });

        public RelayCommand ResourceViewCmd { get; set; } = new RelayCommand
           (
           parameter =>
           {
               if (parameter is MainViewModel mvm)
               {
                   #region MVM Visibility
                   mvm.ResourceView = WindowVisibility.Visible;
                   mvm.StatusDot = WindowVisibility.Visible;
                   mvm.ExpandedProjectView = WindowVisibility.Hidden;
                   mvm.InProgressView = WindowVisibility.Hidden;
                   mvm.SettingsView = WindowVisibility.Hidden;
                   mvm.LoginView = WindowVisibility.Hidden;
                   mvm.ArchiveView = WindowVisibility.Hidden;
                   #endregion

                   mvm.ViewTitle = InProgressViewModel.Instance.SelectedProject.Name;
                   ExpandedResourceViewModel.Instance.Title = "Omkostninger";

                   ExpandedResourceViewModel.Instance.Audits.Clear();
                   ExpandedResourceViewModel.Instance.Worktimes.Clear();

                   ExpandedResourceViewModel.Instance.SelectedProject = InProgressViewModel.Instance.SelectedProject;
                   ExpandedResourceViewModel.Instance.SelectedResource = new ResourceViewModel(mvm.resourceRepo.GetById(x => x.RefId == InProgressViewModel.Instance.SelectedProject.GetId()));

                   ExpandedResourceViewModel.Instance.InformationPlaceholderVisibility = WindowVisibility.Visible;

                   #region Resource Visibility
                   ExpandedResourceViewModel.Instance.AuditDetailsVisibility = WindowVisibility.Hidden;
                   #endregion

                   #region Audit Visibility
                   ExpandedResourceViewModel.Instance.UpdateAuditVisibility = WindowVisibility.Hidden;
                   ExpandedResourceViewModel.Instance.CreateAuditVisibility = WindowVisibility.Hidden;
                   ExpandedResourceViewModel.Instance.UpdateResourceVisibility = WindowVisibility.Hidden;
                   ExpandedResourceViewModel.Instance.ResourceVisiblity = WindowVisibility.Visible;
                   ExpandedResourceViewModel.Instance.AuditListVisibility = WindowVisibility.Visible;
                   #endregion

                   #region Worktime Visibility
                   ExpandedResourceViewModel.Instance.CreateWorktimeVisibility = WindowVisibility.Hidden;
                   ExpandedResourceViewModel.Instance.UpdateWorktimeVisibility = WindowVisibility.Hidden;
                   ExpandedResourceViewModel.Instance.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                   #endregion

                   ExpandedResourceViewModel.Instance.UpdateList();
               }
           },
           parameter =>
           {
               bool isEnabled = false;
               if (parameter is MainViewModel mvm)
               {
                   if (InProgressViewModel.Instance.SelectedProject != null)
                   {
                       isEnabled = true;
                   }
               }
               return isEnabled;
           });

        public RelayCommand SettingsViewCmd { get; set; } = new RelayCommand
            (
            parameter =>
            {
                if (parameter is MainViewModel mvm)
                {
                    mvm.SettingsView = WindowVisibility.Visible;
                    mvm.StatusDot = WindowVisibility.Hidden;
                    mvm.ExpandedProjectView = WindowVisibility.Hidden;
                    mvm.InProgressView = WindowVisibility.Hidden;
                    mvm.LoginView = WindowVisibility.Hidden;
                    mvm.ResourceView = WindowVisibility.Hidden;
                    mvm.ViewTitle = SettingsViewModel.Instance.WindowTitle;
                    mvm.ArchiveView = WindowVisibility.Hidden;
                    SettingsViewModel.Instance.UpdateList();
                    SettingsViewModel.Instance.InformationVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                return true;

            });

        public RelayCommand ArchiveViewCmd { get; set; } = new RelayCommand
            (
            parameter =>
            {
                if (parameter is MainViewModel mvm)
                {
                    mvm.ViewTitle = ArchiveViewModel.Instance.WindowTitle;
                    mvm.SettingsView = WindowVisibility.Hidden;
                    mvm.StatusDot = WindowVisibility.Hidden;
                    mvm.ExpandedProjectView = WindowVisibility.Hidden;
                    mvm.InProgressView = WindowVisibility.Hidden;
                    mvm.LoginView = WindowVisibility.Hidden;
                    mvm.ResourceView = WindowVisibility.Hidden;
                    SettingsViewModel.Instance.InformationVisibility = WindowVisibility.Hidden;
                    mvm.ArchiveView = WindowVisibility.Visible;
                    ArchiveViewModel.Instance.UpdateList();
                }
            },
            parameter =>
            {
                return true;

            });

        public RelayCommand LoginViewCmd { get; set; } = new RelayCommand
            (
            parameter =>
            {
                if (parameter is MainViewModel mvm)
                {
                    if (mvm.CurrentUser == null)
                    {
                        mvm.LoginView = WindowVisibility.Visible;
                        mvm.SettingsView = WindowVisibility.Hidden;
                        mvm.ExpandedProjectView = WindowVisibility.Hidden;
                        mvm.InProgressView = WindowVisibility.Hidden;
                        mvm.ResourceView = WindowVisibility.Hidden;
                        mvm.ArchiveView = WindowVisibility.Hidden;
                        mvm.ViewTitle = LoginViewModel.Instance.WindowTitle;
                    }
                    else
                    {
                        mvm.CurrentUser = null;
                        mvm.UserIconPath = "/Resources/Images/login.png";
                        mvm.ViewTitle = InProgressViewModel.Instance.WindowTitle;
                    }
                }
            },
            parameter =>
            {
                return true;
            });

        private IRepository<Project> projectRepo;
        private IRepository<Progress> progressRepo;
        private IRepository<Resource> resourceRepo;
        private IRepository<Audit> auditRepo;
        private IRepository<User> userRepo;
        private IRepository<Worktime> worktimeRepo;

        public IRepository<Project> GetProjectRepo() => projectRepo;
        public IRepository<Progress> GetProgressRepo() => progressRepo;
        public IRepository<Resource> GetResourceRepo() => resourceRepo;
        public IRepository<Audit> GetAuditRepo() => auditRepo;
        public IRepository<User> GetUserRepo() => userRepo;
        public IRepository<Worktime> GetWorktimeRepo() => worktimeRepo;

        public void ShowStatusDot(bool show)
        {
            if (show)
                StatusDot = WindowVisibility.Visible;
            else
                StatusDot = WindowVisibility.Hidden;
        }

        //Singleton
        private MainViewModel()
        {
            projectRepo = new Repository<Project>();
            progressRepo = new Repository<Progress>();
            resourceRepo = new Repository<Resource>();
            auditRepo = new Repository<Audit>();
            userRepo = new Repository<User>();
            worktimeRepo = new Repository<Worktime>();

            InProgressView = WindowVisibility.Hidden;
            StatusDot = WindowVisibility.Hidden;
            ExpandedProjectView = WindowVisibility.Hidden;
            SettingsView = WindowVisibility.Hidden;
            ResourceView = WindowVisibility.Hidden;
            LoginView = WindowVisibility.Visible;
            ArchiveView = WindowVisibility.Hidden;

            ViewTitle = "Login";
        }

        private static readonly object _lock = new object();
        private static MainViewModel _instance;

        public static MainViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new MainViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        //Singleton - Lazy
        //private static readonly Lazy<MainViewModel> lazy = new Lazy<MainViewModel>(() => new MainViewModel());

        //public static MainViewModel Instance => lazy.Value;
    }
}