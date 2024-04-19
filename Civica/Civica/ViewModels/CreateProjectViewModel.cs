using Civica.Commands;
using Civica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.ViewModels
{
    public class CreateProjectViewModel : ObservableObject
    {
        private string _windowTitle;
        public string WindowTitle
        {
            get => _windowTitle;
            set
            {
                _windowTitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }

        private string _projectOwner;
        public string ProjectOwner
        {
            get => _projectOwner;
            set
            {
                _projectOwner = value;
                OnPropertyChanged(nameof(ProjectOwner));
            }
        }

        private string _projectManager;

        public string ProjectManager
        {
            get => _projectManager; 
            set 
            { 
                _projectManager = value; 
                OnPropertyChanged(nameof(ProjectManager)); 
            }
        }

        private string _projectDescription;

        public string ProjectDescription
        {
            get => _projectDescription;
            set
            {
                _projectDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }

        public ICommand CreateProjectCmd { get; set; } = new CreateProjectCmd();

        ProjectRepository projectRepo = new ProjectRepository();

        public CreateProjectViewModel()
        {
            WindowTitle = "Opret";
        }

        public void CreateProject(string name, string owner = "", string manager = "", string description = "")
        {
            Project p = new Project(name, owner, manager, description);
            projectRepo.Add(p);
        }

        //public void CreateNewProject(string name, string owner = "", string manager = "", string description = "")
        //{
        //    Project p = new Project(name, owner, manager, description);

        //    projectRepo.Add(p);

        //    ProjectViewModel pvm = new ProjectViewModel(p);

        //    Projects.Add(pvm);
        //}
    }
}
