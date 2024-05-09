using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class CreateProjectViewModel : ObservableObject, IViewModelChild
    {
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

        private string _projectOwner = "";
        public string ProjectOwner
        {
            get => _projectOwner;
            set
            {
                _projectOwner = value;
                OnPropertyChanged(nameof(ProjectOwner));
            }
        }

        private string _projectManager = "";

        public string ProjectManager
        {
            get => _projectManager;
            set
            {
                _projectManager = value;
                OnPropertyChanged(nameof(ProjectManager));
            }
        }

        private string _projectDescription = "";

        public string ProjectDescription
        {
            get => _projectDescription;
            set
            {
                _projectDescription = value;
                OnPropertyChanged(nameof(ProjectDescription));
            }
        }
        private decimal _resourceStartAmount;
        public decimal ResourceStartAmount   
        {
            get => _resourceStartAmount;
            set
            {
                _resourceStartAmount = value;
                OnPropertyChanged(nameof(ResourceStartAmount));
            }
        }
        private decimal _resourceExpectedYearlyCost;
        public decimal ResourceExpectedYearlyCost
        {
            get => _resourceExpectedYearlyCost;
            set
            {
                _resourceExpectedYearlyCost = value;
                OnPropertyChanged(nameof(ResourceExpectedYearlyCost));
            }
        }

        private InProgressViewModel ipvm;
        private IRepository<Project> projectRepo;
        private IRepository<Resource> resourceRepo;
        private IRepository<Audit> auditRepo;

        public void Init(ObservableObject o)
        {
            ipvm = (o as InProgressViewModel);
        }

        public void SetRepo(IRepository<Project> projectRepo)
        {
            this.projectRepo = projectRepo;
        }
        public void SetRepo(IRepository<Resource> resourceRepo)
        {
            this.resourceRepo = resourceRepo;
        }
        public void SetRepo(IRepository<Audit> auditRepo)
        {
            this.auditRepo = auditRepo;
        }

        public void CreateProject()
        {
            Project p = new Project(ipvm.GetCurrentUser().GetId(), ProjectName, ProjectOwner, ProjectManager, ProjectDescription, DateTime.Now);
            
            projectRepo.Add(p);

            Resource r = new Resource(ipvm.GetCurrentUser().GetId(), p.Id, ResourceStartAmount, ResourceExpectedYearlyCost, DateTime.Now);
            resourceRepo.Add(r);

            Audit a = new Audit(ipvm.GetCurrentUser().GetId(), r.Id, ResourceExpectedYearlyCost, DateTime.Now.Year, string.Empty, DateTime.Now);
            auditRepo.Add(a);

            ipvm.UpdateList();

            ProjectName = "";
            ProjectManager = "";
            ProjectOwner = "";
            ProjectDescription = "";
        }
        

        public RelayCommand CreateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CreateProjectViewModel cpvm)
                {
                    cpvm.CreateProject();
                    cpvm.ipvm.CreateVisibility = WindowVisibility.Hidden;
                    cpvm.ipvm.InformationVisibility = WindowVisibility.Visible;
                    cpvm.ResourceStartAmount = 0;
                    cpvm.ResourceExpectedYearlyCost = 0;
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CreateProjectViewModel cpvm)
                {
                    if (!string.IsNullOrEmpty(cpvm.ProjectName))
                    {
                        succes = true;
                    }
                }
                return succes;
            }
        );
    }
}
