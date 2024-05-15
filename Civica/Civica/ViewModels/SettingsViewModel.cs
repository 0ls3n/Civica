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

namespace Civica.ViewModels
{
    public class SettingsViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;
        private IRepository<User> userRepo;
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
                    EditVisibility = WindowVisibility.Hidden;
                    OnPropertyChanged(nameof(SelectedUser));
                }
            }
        }
        public string OldName;
        public int OldPassword;
        public CreateUserViewModel CreateUserVM { get; set; }

        public SettingsViewModel()
        {
            CreateUserVM = new CreateUserViewModel();
            CreateUserVM.Init(this);
            CreateUserVM.SetRepo(userRepo);

            WindowTitle = "Indstillinger";

            CreateVisibility = WindowVisibility.Hidden;
            EditVisibility = WindowVisibility.Hidden;
            InformationVisibility = WindowVisibility.Visible;
        }

        public void Init(ObservableObject o)
        {
            this.mvm = (o as MainViewModel);
            userRepo = mvm.GetUserRepo();

            CreateUserVM.SetRepo(userRepo);
            UpdateList();

        }
        public void UpdateUser(UserViewModel userVM)
        {
            User u = userRepo.GetById(x => x.Id == userVM.GetId());
            u.FirstName = userVM.FirstName;
            u.LastName = userVM.LastName;
            u.Password = int.Parse(userVM.Password);

            userRepo.Update(u);
        }
        public void RemoveUser()
        {
            userRepo.Remove(userRepo.GetById(x => x.Id == SelectedUser.GetId()));
            UpdateList();
        }
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
                    svm.EditVisibility = WindowVisibility.Hidden;
                    svm.InformationVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    if (svm.mvm.CurrentUser != null)
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
                    svm.EditVisibility = WindowVisibility.Visible;
                    svm.InformationVisibility = WindowVisibility.Hidden;
                    svm.CreateVisibility = WindowVisibility.Hidden;
                    svm.OldName = svm.SelectedUser.FullName;
                    svm.OldPassword = int.Parse(svm.SelectedUser.Password);
                }
            },
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    if (svm.SelectedUser != null && svm.mvm.CurrentUser != null)
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
                        int.Parse(svm.SelectedUser.Password) == svm.OldPassword)
                    {
                        if (svm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.FullName.ToLower() == svm.SelectedUser.FullName.ToLower()) is null ||
                        svm.SelectedUser.FullName.ToLower() == svm.OldName.ToLower())
                        {
                            svm.UpdateUser(svm.SelectedUser);

                            svm.EditVisibility = WindowVisibility.Hidden;
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

        public RelayCommand RemoveUserCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is SettingsViewModel svm)
                {
                    MessageBoxButton button = MessageBoxButton.OKCancel;
                    MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{svm.SelectedUser.FullName}'?", "Bekræft sletning", button);

                    if (result == MessageBoxResult.OK)
                    {
                        svm.RemoveUser();
                        svm.InformationVisibility = WindowVisibility.Visible;
                    }
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is SettingsViewModel svm)
                {
                    if (svm.SelectedUser != null && svm.mvm.CurrentUser != null)
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
