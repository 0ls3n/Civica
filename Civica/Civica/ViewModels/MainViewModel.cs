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
using Civica.Models.Enums;

namespace Civica.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();
        public ObservableCollection<ProgressViewModel> Progresses { get; set; } = new ObservableCollection<ProgressViewModel>();
        private ProjectRepository projectRepo = new ProjectRepository();
        private ProgressRepository progressRepo = new ProgressRepository();
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
            foreach (Progress prog in progressRepo.GetAll())
            {
                Progresses.Add(new ProgressViewModel(prog));
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

            projectRepo.Update(projectRepo.Get(project.GetId()), name, owner, manager, description);
        }

        public void RemoveProject() 
        {
            Project p = projectRepo.Get(SelectedProject.GetId());
            projectRepo.Remove(p);
            Projects.Remove(SelectedProject);
        }

        public void ProgressProject(Phase fase, Status status, string description)
        {
            Progress prog = new Progress(fase, status, description);
            prog.ProjectId = SelectedProject.GetId();

            progressRepo.Add(prog);
            Progresses.Add(new ProgressViewModel(prog));
        }
    }
}