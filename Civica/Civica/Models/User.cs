using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class User : DomainModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Password {  get; set; }
        public User(string firstName, string lastName, int password)
        {
            FirstName = firstName;
            LastName = lastName;
            Password = password;
        }



    }
}
