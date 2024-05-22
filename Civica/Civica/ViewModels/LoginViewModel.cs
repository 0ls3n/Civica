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
    public class LoginViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm;
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


        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);

            userRepo = mvm.GetUserRepo();
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
                        lvm.mvm.CurrentUser = uvm;
                        lvm.mvm.LoginView = WindowVisibility.Hidden;
                        lvm.mvm.InProgressView = WindowVisibility.Visible;
                        lvm.mvm.ipvm.InformationVisibility = WindowVisibility.Visible;
                        lvm.mvm.ipvm.UpdateList();
                        lvm.Password = string.Empty;
                        lvm.mvm.UserIconPath = "/Resources/Images/logout.png";
                        lvm.mvm.ViewTitle = lvm.mvm.ipvm.WindowTitle;
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
                    lvm.mvm.LoginView = WindowVisibility.Hidden;
                    lvm.mvm.InProgressView = WindowVisibility.Visible;
                    lvm.mvm.ipvm.InformationVisibility = WindowVisibility.Visible;
                    lvm.mvm.ipvm.UpdateList();
                    lvm.Password = string.Empty;
                }
            },
            parameter =>
            {
                return true;
            }
        );
    }
}
