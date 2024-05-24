using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using Civica.Commands;
using System.Windows;
using Civica.Models.Enums;
using GVMR;

namespace Civica.ViewModels
{
    public class CRUDUserViewModel : ObservableObject, IViewModelChild
    {
        private SettingsViewModel svm;
        private IRepository<User> userRepo;
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

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

        public RelayCommand CreateUserCmd { get; set; } = new RelayCommand
        (
           parameter =>
           {
               if (parameter is CRUDUserViewModel cuvm)
               {
                   if (cuvm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.Password == int.Parse(cuvm.Password)) is null)
                   {
                       if (cuvm.userRepo.GetAll().OfType<User>().FirstOrDefault(x => x.FullName.ToLower() == cuvm.FirstName.ToLower() + " " + cuvm.LastName.ToLower()) is null)
                       {
                           cuvm.CreateUser();
                           cuvm.svm.CreateVisibility = WindowVisibility.Hidden;
                           cuvm.svm.InformationVisibility = WindowVisibility.Visible;
                           cuvm.svm.UpdateList();
                       }
                       else
                       {
                           MessageBox.Show("Navnet " + cuvm.FirstName + " " + cuvm.LastName + " findes allerede.");
                       }

                   }
                   else
                   {
                       MessageBox.Show("Adgangskoden " + cuvm.Password + " er allerede i brug");
                   }
               }
           },
           parameter =>
           {
               bool succes = false;

               if (parameter is CRUDUserViewModel cuvm)
               {
                   if (!string.IsNullOrEmpty(cuvm.FirstName) && !string.IsNullOrEmpty(cuvm.LastName) && cuvm.Password.Length == 4);
                   {
                       succes = true;
                   }
               }
               return succes;
           }
        );

        public void SetRepo(IRepository<User> userRepo)
        {
            this.userRepo = userRepo;
        }

        public void Init(ObservableObject o)
        {
            svm = (o as SettingsViewModel);
        }
        public void CreateUser()
        {
            User u = new User(FirstName, LastName, int.Parse(Password));
            userRepo.Add(u);
            svm.UpdateList();
            FirstName = "";
            LastName = "";
            Password = "";
        }
        public void UpdateUser()
        {
            UserViewModel user = svm.SelectedUser;

            User u = userRepo.GetById(x => x.Id == user.GetId());
            u.FirstName = user.FirstName;
            u.LastName = user.LastName;
            u.Password = int.Parse(user.Password);

            userRepo.Update(u);
        }

        public void DeleteUser()
        {
            userRepo.Delete(userRepo.GetById(x => x.Id == svm.SelectedUser.GetId()));
            svm.UpdateList();
        }
    }
}
