using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Civica.Models
{
    public class Project : DomainModel
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public string Manager { get; set; }
        public string Description { get; set; }

        public Project(int userId, string name, DateTime createdDate)
        {
            Name = name;
            UserId = userId;
            CreatedDate = createdDate;
        }
        public Project(int userId, string name, string owner, string manager, string description, DateTime createdDate) : this(userId, name, createdDate)
        {
            Owner = owner;
            Manager = manager;
            Description = description;
        }
    }
}
