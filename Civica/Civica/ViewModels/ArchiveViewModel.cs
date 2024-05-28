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
    public class ArchiveViewModel : ObservableObject
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

        IRepository<Project> projectRepo;
        IRepository<Progress> progressRepo;

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

        //Singleton
        private ArchiveViewModel()
        {
            projectRepo = MainViewModel.Instance.GetProjectRepo();
            progressRepo = MainViewModel.Instance.GetProgressRepo();

            UpdateList();
        }

        private static readonly object _lock = new object();
        private static ArchiveViewModel _instance;

        public static ArchiveViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new ArchiveViewModel();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}
