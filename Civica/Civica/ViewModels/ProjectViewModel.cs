using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
    public class ProjectViewModel
    {

        private Project project;

        public string Name { get; set; }

        public string Owner { get; set; }

        public string Manager { get; set; }

        public string Description { get; set; }

        public ProjectViewModel(Project p)
        {
            project = p;
            Name = p.Name;
            Owner = p.Owner;
            Manager = p.Manager;
            Description = p.Description;
        }

        public int GetId()
        {
            return project.Id;
        }
    }
}
