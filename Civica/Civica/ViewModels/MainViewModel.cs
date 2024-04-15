using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ICommand OpenUpdateWindowCmd { get; set; }
        public ObservableCollection<ProjectViewModel> Projects { get; set; }= new ObservableCollection<ProjectViewModel>();
        private ProjectRepository projectRepo = new ProjectRepository();
        private ProjectViewModel selectedProject = null;

        public ProjectViewModel SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; OnPropertyChanged(nameof(SelectedProject)); }
        }

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
             OpenUpdateWindowCmd = new OpenUpdateWindowCmd(ChangeCanExecute);
        }
        
        public bool ChangeCanExecute(object obj)
        {
            return SelectedProject != null;
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
            //int index = Projects.IndexOf(Projects.FirstOrDefault(p => p.Name == project.Name));
            
            project.Name = name;
            project.Owner = owner;
            project.Manager = manager;
            project.Description = description;
            //Projects[index] = project;

            projectRepo.Update(project.GetProject(), name, owner, manager, description);
        }
    }
}
