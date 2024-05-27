using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Civica.Models;

namespace Civica.ViewModels
{
    public class UserViewModel
    {
        private User user;

        public string FullName { get; set; }
        private string firstName;

        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                FullName = value + " " + LastName;
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                FullName = FirstName + " " + value;
            }
        }
        private string password;

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
            }
        }
        public UserViewModel(User user)
        {
            this.user = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password.ToString();
        }
        public int GetId()
        {
            return user.Id;
        }
    }
}
