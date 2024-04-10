using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using Microsoft.Identity.Client;

namespace Civica.ViewModels
{
    public class MainViewModel
    {

        private ObservableCollection<ProjectViewModel> projects;

        private ProjectRepository projectRepo;

        public MainViewModel()
        {
            projectRepo = new ProjectRepository();
            projects = new ObservableCollection<ProjectViewModel>();
            
        }

        public void CreateNewProject (string name, string owner, string manager, string description)
        {
            Project p = new Project(name, owner, manager, description);

            projectRepo.Add(p);

            ProjectViewModel pvm = new ProjectViewModel(p);

            projects.Add(pvm);
        }


    }
}
