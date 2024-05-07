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
        private string loginButtonText = "Login";

        public string LoginButtonText
        {
            get { return loginButtonText; }
            set 
            { 
                loginButtonText = value; 
                OnPropertyChanged(nameof(LoginButtonText));
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

        public InProgressViewModel ipvm { get; set; } = new InProgressViewModel();
        public ExpandedProjectViewModel epvm { get; set; } = new ExpandedProjectViewModel();

        public ResourceProjectViewModel rpvm { get; set; } = new ResourceProjectViewModel();

        public SettingsViewModel svm { get; set; } = new SettingsViewModel();
        public LoginViewModel lvm { get; set; } = new LoginViewModel();

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
                    mvm.LoginView = WindowVisibility.Hidden;
                    mvm.ViewTitle = mvm.ipvm.WindowTitle;
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
                    mvm.ViewTitle = mvm.ipvm.SelectedProject.Name;
                    mvm.epvm.UpdateList();
                    mvm.epvm.SelectedProject = mvm.ipvm.SelectedProject;
                    mvm.epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                bool isEnabled = false;
                if (parameter is MainViewModel mvm)
                {
                    if (mvm.ipvm.SelectedProject != null)
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
                   mvm.ResourceView = WindowVisibility.Visible;
                   mvm.StatusDot = WindowVisibility.Visible;
                   mvm.ExpandedProjectView = WindowVisibility.Hidden;
                   mvm.InProgressView = WindowVisibility.Hidden;
                   mvm.SettingsView = WindowVisibility.Hidden;
                   mvm.LoginView = WindowVisibility.Hidden;
                   mvm.ViewTitle = mvm.ipvm.SelectedProject.Name;
                   mvm.rpvm.SelectedProject = mvm.ipvm.SelectedProject;
                   mvm.rpvm.SelectedResource = mvm.ipvm.SelectedResource;
                   mvm.rpvm.Audits.Clear();
                   mvm.rpvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                   mvm.rpvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                   mvm.rpvm.UpdateList();
               }
           },
           parameter =>
           {
               bool isEnabled = false;
               if (parameter is MainViewModel mvm)
               {
                   if (mvm.ipvm.SelectedProject != null)
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
                    mvm.ViewTitle = mvm.svm.WindowTitle;
                    mvm.svm.UpdateList();
                    mvm.svm.InformationVisibility = WindowVisibility.Visible;
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
                    if (mvm.LoginButtonText == "Login")
                    {
                        mvm.LoginView = WindowVisibility.Visible;
                        mvm.SettingsView = WindowVisibility.Hidden;
                        mvm.ExpandedProjectView = WindowVisibility.Hidden;
                        mvm.InProgressView = WindowVisibility.Hidden;
                        mvm.ResourceView = WindowVisibility.Hidden;
                        mvm.ViewTitle = mvm.lvm.WindowTitle;
                    }
                    else
                    {
                        mvm.CurrentUser = null;
                        mvm.LoginButtonText = "Login";
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

        public MainViewModel()
        {
            projectRepo = new Repository<Project>(id =>
            {
                return projectRepo.GetAll().FindAll(x => x.Id == id);
            });
            progressRepo = new Repository<Progress>(id =>
            {
                return progressRepo.GetAll().FindAll(x => x.RefId == id);
            });
            resourceRepo = new Repository<Resource>(id =>
            {
                return resourceRepo.GetAll().FindAll(x => x.RefId == id);
            });
            auditRepo = new Repository<Audit>(id =>
            {
                return auditRepo.GetAll().FindAll(x => x.RefId == id);
            });
            userRepo = new Repository<User>(id =>
            {
                return userRepo.GetAll().FindAll(x => x.Id == id);
            });
            ipvm.Init(this);
            epvm.Init(this);
            svm.Init(this);
            lvm.Init(this);
            rpvm.Init(this);
            InProgressView = WindowVisibility.Visible;
            StatusDot = WindowVisibility.Hidden;
            ExpandedProjectView = WindowVisibility.Hidden;
            SettingsView = WindowVisibility.Hidden;
            ResourceView = WindowVisibility.Hidden;
            LoginView = WindowVisibility.Hidden;
            
            ViewTitle = ipvm.WindowTitle;
        }

        public IRepository<Project> GetProjectRepo() => projectRepo;
        public IRepository<Progress> GetProgressRepo() => progressRepo;
        public IRepository<Resource> GetResourceRepo() => resourceRepo;
        public IRepository<Audit> GetAuditRepo() => auditRepo;
        public IRepository<User> GetUserRepo() => userRepo;
    }
}