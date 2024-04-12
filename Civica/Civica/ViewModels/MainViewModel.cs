using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ObservableCollection<ProjectViewModel> Projects { get; set; }= new ObservableCollection<ProjectViewModel>();
        private ProjectRepository projectRepo = new ProjectRepository();


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
    }
}
