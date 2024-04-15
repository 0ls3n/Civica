using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Civica.Commands;
using Civica.Models;

namespace Civica.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();
        private ProjectRepository projectRepo = new ProjectRepository();
        private ProjectViewModel selectedProject = null;

        public ProjectViewModel SelectedProject
        {
            get
            {
                return selectedProject;
            }
            set
            {
                selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
                OnPropertyChanged(nameof(CanUpdateProject));
            }
        }

        public bool CanUpdateProject => SelectedProject != null;

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        {
            foreach (Project p in projectRepo.GetAll())
            {
                Projects.Add(new ProjectViewModel(p));
            }
        }

        public void CreateNewProject(string name, string owner = "", string manager = "", string description = "")
        {
            Project p = new Project(name, owner, manager, description);

            projectRepo.Add(p);

            ProjectViewModel pvm = new ProjectViewModel(p);

            Projects.Add(pvm);
        }

        public void UpdateProject(ProjectViewModel project, string name, string owner = "", string manager = "", string description = "")
        {
            int index = Projects.IndexOf(project);

            Projects[index] = null;

            project.Name = name;
            project.Owner = owner;
            project.Manager = manager;
            project.Description = description;

            Projects[index] = project;

            projectRepo.Update(project.GetProject(), name, owner, manager, description);
        }

        public void RemoveProject() 
        {
            Project p = SelectedProject.GetProject();
            projectRepo.Remove(p);
            Projects.Remove(SelectedProject);
            
        }
    }
}
