using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class User : DomainModel
    {
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
        public int Password {  get; set; }
        public User(string firstName, string lastName, int password)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }



    }
}
