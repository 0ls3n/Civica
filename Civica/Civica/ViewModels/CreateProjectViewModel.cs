using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Civica.Commands;
using Civica.Models;
using Microsoft.Identity.Client;

namespace Civica.ViewModels
{
    public class CreateProjectViewModel : INotifyPropertyChanged
    {
        //private ObservableCollection<ProjectViewModel> projects;

        private ProjectRepository projectRepo;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _projectName = "";
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }
        private string _owner = "";
        public string Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
        private string _manager = "";
        public string Manager
        {
            get => _manager;
            set
            {
                _manager = value;
                OnPropertyChanged(nameof(Manager));
            }
        }
        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public ICommand CreateProjectCmd { get; set; }

        public CreateProjectViewModel(ProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;

            //projects = new ObservableCollection<ProjectViewModel>(projectRepo.GetAll().Select(x => new ProjectViewModel(x)));

            CreateProjectCmd = new CreateProjectCmd(projectRepo);
        }

        public void CreateNewProject(string name, string owner = "", string manager = "", string description = "")
        {
            Project p = new Project(name, owner, manager, description);

            projectRepo.Add(p);

            //ProjectViewModel pvm = new ProjectViewModel(p);

        }
    }
}
