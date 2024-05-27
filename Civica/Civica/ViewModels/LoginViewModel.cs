using Civica.Commands;
using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using Civica.Models.Enums;
using System.Windows;
using GVMR;

namespace Civica.ViewModels
{
    public class LoginViewModel : ObservableObject
    {
        private IRepository<User> userRepo;
        public string WindowTitle { get; } = "Login";

        private string password = string.Empty;
        public string Password
        {
            get { return password; }
            set
            {
                if (value.Length < 5)
                {
                    password = value;
                }
                else
                {
                    MessageBox.Show("Password må kun være på 4 cifre");
                    password = value.Substring(0, 4);
                }
                OnPropertyChanged(nameof(Password));
            }
        }

        public RelayCommand LoginCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is LoginViewModel lvm)
                {
                    User u = lvm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.Password == int.Parse(lvm.Password));
                    if (u is not null)
                    {
                        UserViewModel uvm = new UserViewModel(u);
                        MainViewModel.Instance.CurrentUser = uvm;
                        MainViewModel.Instance.LoginView = WindowVisibility.Hidden;
                        MainViewModel.Instance.InProgressView = WindowVisibility.Visible;
                        InProgressViewModel.Instance.InformationVisibility = WindowVisibility.Visible;
                        InProgressViewModel.Instance.UpdateList();
                        lvm.Password = string.Empty;
                        MainViewModel.Instance.UserIconPath = "/Resources/Images/logout.png";
                        MainViewModel.Instance.ViewTitle = InProgressViewModel.Instance.WindowTitle;
                    }
                    else
                    {
                        MessageBox.Show("Adgangskoden " + lvm.Password + " findes ikke");
                    }
                }
            },
            parameter =>
            {
                bool result = false;
                if (parameter is LoginViewModel lvm)
                {
                    if (lvm.Password.Length == 4)
                    {
                        return true;
                    }
                }
                return result;
            }
        );

        public RelayCommand GuestCmd { get; set; } = new RelayCommand
            (
            parameter =>
            {
                if (parameter is LoginViewModel lvm)
                {
                    MainViewModel.Instance.LoginView = WindowVisibility.Hidden;
                    MainViewModel.Instance.InProgressView = WindowVisibility.Visible;
                    InProgressViewModel.Instance.InformationVisibility = WindowVisibility.Visible;
                    InProgressViewModel.Instance.UpdateList();
                    lvm.Password = string.Empty;
                }
            },
            parameter =>
            {
                return true;
            }
        );

        //Singleton
        private LoginViewModel()
        {
            userRepo = MainViewModel.Instance.GetUserRepo();
        }

        private static Lazy<LoginViewModel> lazy = new Lazy<LoginViewModel>(
               () => new LoginViewModel(),
               LazyThreadSafetyMode.PublicationOnly
           );
        public static LoginViewModel Instance => lazy.Value;
    }
}
