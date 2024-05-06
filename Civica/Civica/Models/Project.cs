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

        public Project(string name, string owner, string manager, string description) : this(name)
        {
            Owner = owner;
            Manager = manager;
            Description = description;
            Created = DateTime.Now;
        }
        public Project(string name)
        {
            Name = name;
            Created = DateTime.Now;
        }
        public Project(int userId, string name, string owner, string manager, string description) : this(name)
        {
            Owner = owner;
            Manager = manager;
            Description = description;
            UserId = userId;
            Created = DateTime.Now;
        }
        public Project(int userId, string name)
        {
            Name = name;
            UserId = userId;
            Created = DateTime.Now;
        }
    }
}
