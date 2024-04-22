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

        private string _owner;
        public string Owner
        {
            get
            {
                return _owner;
            }
            set { _owner = value; }
        }

        private string _manager;
        public string Manager
        {
            get
            {
                return _manager;
            }
            set { _manager = value; }
        }

        private string _description;
        public string Description
        {
            get
            {
                return _description;
            }
            set { _description = value; }
        }

        private string _statusColor;
        public string StatusColor
        {
            get => _statusColor;
            set => _statusColor = value;
        }

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
