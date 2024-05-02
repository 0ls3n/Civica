using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
    public class UserViewModel
    {
        private User user;


        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Password { get; set; }
        public UserViewModel(User user)
        {
            this.user = user;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Password = user.Password;  
        }
        public int GetId () 
        { 
         return user.Id;
        }


    }
}
