using Civica.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class InProgressViewModel : ObservableObject
    {
        private string _title;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        private ProjectViewModel _selectedProject = null;
        public ProjectViewModel SelectedProject 
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private ProjectRepository projectRepo = new ProjectRepository();

        public InProgressViewModel()
        {
            foreach (Project p in projectRepo.GetAll())
            {
                Projects.Add(new ProjectViewModel(p));
            }

            SelectedProject = Projects.FirstOrDefault(x => x.Name == "Tommy");

            Title = "Igangværende";
        }
    }
}
