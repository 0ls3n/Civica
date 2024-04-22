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
                if (_owner == string.Empty)
                    return "Ingen";
                return _owner;
            }
            set { _owner = value; }
        }

        private string _manager;
        public string Manager
        {
            get
            {
                if (_manager == string.Empty)
                    return "Ingen";
                return _manager;
            }
            set { _manager = value; }
        }

        private string _description;
        public string Description
        {
            get
            {
                if (_description == string.Empty)
                    return "Ingen beskrivelse";

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
