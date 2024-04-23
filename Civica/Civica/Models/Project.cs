using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Civica.Models
{
    public class Project
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public string Owner { get; set; }

        public string Manager { get; set; }
        public string Description { get; set; }

        public Project(string name, string owner, string manager, string description) : this(name)
        {
            Owner = owner;
            Manager = manager;
            Description = description;
        }
        public Project(string name)
        {
            Name = name;
        }
    }
}
