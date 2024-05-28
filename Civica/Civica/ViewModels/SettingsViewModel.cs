using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using System.Collections.ObjectModel;
using Civica.Commands;
using Civica.Models.Enums;
using System.Windows;
using GVMR;

namespace Civica.ViewModels
{
    public class SettingsViewModel : ObservableObject
    {
        private IRepository<User> userRepo;
        public string WindowTitle { get; } = "Indstillinger";
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

        private WindowVisibility _updateVisibility;
        public WindowVisibility UpdateVisibility
        {
            get => _updateVisibility;
            set
            {
                _updateVisibility = value;
                OnPropertyChanged(nameof(UpdateVisibility));
            }
        }
        private WindowVisibility _informationVisibility;

        public WindowVisibility InformationVisibility
        {
            get { return _informationVisibility; }
            set
            {
                _informationVisibility = value;
                OnPropertyChanged(nameof(InformationVisibility));
            }
        }
        private ObservableCollection<UserViewModel> users;

        public ObservableCollection<UserViewModel> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        private UserViewModel _selectedUser;

        public UserViewModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                if (value is not null)
                {
                    _selectedUser = value;
                    InformationVisibility = WindowVisibility.Visible;
                    CreateVisibility = WindowVisibility.Hidden;
                    UpdateVisibility = WindowVisibility.Hidden;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }
        private string oldName;
        private int oldPassword;

        public void UpdateList()
        {
            Users = new ObservableCollection<UserViewModel>
            (
                userRepo.GetAll().Select(user => new UserViewModel(user as User))
            );
        }
        #region ViewCommands

        public RelayCommand CreateUserViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    svm.CreateVisibility = WindowVisibility.Visible;
                    svm.UpdateVisibility = WindowVisibility.Hidden;
                    svm.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    if (MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );

        public RelayCommand UpdateUserViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    svm.UpdateVisibility = WindowVisibility.Visible;
                    svm.InformationVisibility = WindowVisibility.Hidden;
                    svm.CreateVisibility = WindowVisibility.Hidden;
                    svm.oldName = svm.SelectedUser.FullName;
                    svm.oldPassword = int.Parse(svm.SelectedUser.Password);
                }
            },
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    if (svm.SelectedUser != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            }
        );
        #endregion

        #region FunctionalityCommands

        public RelayCommand UpdateUserCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    if (svm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.Password == int.Parse(svm.SelectedUser.Password)) is null ||
                        int.Parse(svm.SelectedUser.Password) == svm.oldPassword)
                    {
                        if (svm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.FullName.ToLower() == svm.SelectedUser.FullName.ToLower()) is null ||
                        svm.SelectedUser.FullName.ToLower() == svm.oldName.ToLower())
                        {
                            CRUDUserViewModel.Instance.UpdateUser(svm.SelectedUser);

                            svm.UpdateVisibility = WindowVisibility.Hidden;
                            svm.InformationVisibility = WindowVisibility.Visible;
                            svm.UpdateList();
                        }
                        else
                        {
                            MessageBox.Show("Navnet " + svm.SelectedUser.FullName + " findes allerede.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Adgangskoden " + svm.SelectedUser.Password + " er allerede i brug");
                    }
                }
            },
            parameter =>
            {
                bool result = false;

                if (parameter is SettingsViewModel svm)
                {
                    if (svm.SelectedUser is not null)
                    {
                        if (svm.SelectedUser.FirstName is not "" && svm.SelectedUser.LastName is not "" && svm.SelectedUser.Password.Length == 4)
                        {
                            result = true;

                        }
                    }
                }
                return result;
            }
        );

        public RelayCommand DeleteUserCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    MessageBoxButton button = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{svm.SelectedUser.FullName}'?", "Bekræft sletning", button);

                    if (result == MessageBoxResult.OK)
                    {
                        CRUDUserViewModel.Instance.DeleteUser(svm.SelectedUser);
                        svm.InformationVisibility = WindowVisibility.Visible;
                    }
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is SettingsViewModel svm)
                {
                    if (svm.SelectedUser != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        succes = true;
                    }
                }
                return succes;
            }
        );

        #endregion

        //Singleton
        private SettingsViewModel()
        {
            userRepo = MainViewModel.Instance.GetUserRepo();

            UpdateList();

            CreateVisibility = WindowVisibility.Hidden;
            UpdateVisibility = WindowVisibility.Hidden;
            InformationVisibility = WindowVisibility.Visible;
        }

        private static readonly object _lock = new object();
        private static SettingsViewModel _instance;

        public static SettingsViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new SettingsViewModel();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}