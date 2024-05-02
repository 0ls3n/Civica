using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using System.Collections.ObjectModel;
using Civica.Commands;

namespace Civica.ViewModels
{
    public class SettingsViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;
        private IRepository<User> userRepo;
        public ObservableCollection<UserViewModel> Users { get; set; } = new ObservableCollection<UserViewModel>();
        private UserViewModel selectedUser;

        public UserViewModel SelectedUSer
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged(nameof(SelectedUSer));
            }
        }

        public void SetRepo(IRepository<User> userRepo)
        {
            this.userRepo = userRepo;
        }
        public void Init(ObservableObject o)
        {
            this.mvm = (o as MainViewModel);


        }
        public void UpdateUser(UserViewModel userVM) 
        {
            User u = userRepo.GetById(userVM.GetId());
            u.FirstName = userVM.FirstName;
            u.LastName = userVM.LastName;
            u.Password = userVM.Password;
            
            userRepo.Update(u);
        }
        #region ViewCommands

        public RelayCommand CreateUserViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is InProgressViewModel ipvm)
                {
                    //ipvm.CreateVisibility = WindowVisibility.Visible;
                    //ipvm.InformationVisibility = WindowVisibility.Hidden;
                    //ipvm.EditVisibility = WindowVisibility.Hidden;
                    //ipvm.ProgressVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                return true;
            }
        );

        public RelayCommand UpdateUserViewCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is MainViewModel ipvm)
                {
                    //ipvm.EditVisibility = WindowVisibility.Visible;
                    //ipvm.InformationVisibility = WindowVisibility.Hidden;
                    //ipvm.CreateVisibility = WindowVisibility.Hidden;
                    //ipvm.ProgressVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is MainViewModel ipvm)
                {
                    if (ipvm.SelectedProject != null)
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
                    if (ipvm.SelectedProject != null)
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
                    if (ipvm.Projects.FirstOrDefault(x => x.Name.ToLower() == ipvm.CreateProjectVM.ProjectName.ToLower()) is null || ipvm.CreateProjectVM.ProjectName.ToLower() == ipvm.OldName.ToLower())
                    {
                        ipvm.UpdateProject(ipvm.SelectedProject);

                        ipvm.EditVisibility = WindowVisibility.Hidden;
                        ipvm.InformationVisibility = WindowVisibility.Visible;
                    }
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

        #endregion

    }
}
