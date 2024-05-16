using Civica.Interfaces;
using Civica.Models;
using GVMR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class ArchiveViewModel : ObservableObject, IViewModelChild
    {
        public string WindowTitle { get; } = "Arkiv";

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject 
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private MainViewModel mvm;

        IRepository<Project> projectRepo;
        IRepository<Progress> progressRepo;
        
        public void Init(ObservableObject o)
        {
            this.mvm = (o as MainViewModel);
            projectRepo = this.mvm.GetProjectRepo();
            progressRepo = this.mvm.GetProgressRepo();

            UpdateList();
        }

        public void UpdateList()
        {
            this.Projects.Clear();
            foreach (Project project in projectRepo.GetAll())
            {
                Progress latestProg = progressRepo.GetById(x => x.RefId == project.Id);
                if (latestProg != null)
                {
                    if (latestProg.Phase == Models.Enums.Phase.DONE)
                    {
                        Projects.Add(new ProjectViewModel(project));
                    }
                }
            }
        }
    }
}
