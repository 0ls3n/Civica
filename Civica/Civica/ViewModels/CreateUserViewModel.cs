using Civica.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using Civica.Commands;
using System.Windows;

namespace Civica.ViewModels
{
    public class CreateUserViewModel : ObservableObject, IViewModelChild
    {
        private InProgressViewModel ipvm;
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
        private string password;

        public string Password
        {
            get { return password; }
            set 
            { 
                if (value.Length > 4) 
                {
                    MessageBox.Show("Max 4 tal");
                }
                else 
                {
                    password = value;
                    OnPropertyChanged(nameof(Password));
                }
               
            }
        }

        public RelayCommand CreateUserCmd { get; set; } = new RelayCommand
       (
           parameter =>
           {
               if (parameter is CreateUserViewModel cuvm)
               {
                   cuvm.CreateUser();
                   //cuvm.ipvm.CreateVisibility = WindowVisibility.Hidden;
                   //cuvm.ipvm.InformationVisibility = WindowVisibility.Visible;
               }
           },
           parameter =>
           {
               bool succes = false;

               if (parameter is CreateUserViewModel cuvm)
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
            ipvm = (o as InProgressViewModel); //Change to appropriate viewmodel
       
        }
        public void CreateUser() 
        {
            User u = new User(FirstName, LastName, int.Parse(Password));
            userRepo.Add(u);
            ipvm.UpdateList(); //change to appropriate viewmodel
            FirstName = "";
            LastName = "";
            Password = "";
        }
    }
}
