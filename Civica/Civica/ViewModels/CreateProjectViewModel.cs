using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
        private string _resourceStartAmount = "";
        public string ResourceStartAmount   
        {
            get => _resourceStartAmount;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _resourceStartAmount = value;
                }
                else
                {
                    MessageBox.Show("'Forventet start beløb' må kun være i tal");
                }
                OnPropertyChanged(nameof(ResourceStartAmount));
            }
        }
        private string _resourceExpectedYearlyCost = "";
        public string ResourceExpectedYearlyCost
        {
            get => _resourceExpectedYearlyCost;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _resourceExpectedYearlyCost = value;
                }
                else
                {
                    MessageBox.Show("'Forventet årlig omkostning' må kun være i tal");
                }
                OnPropertyChanged(nameof(ResourceExpectedYearlyCost));
            }
        }

        private InProgressViewModel ipvm;
        private IRepository<Project> projectRepo;
        private IRepository<Resource> resourceRepo;

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

        public void CreateProject()
        {
            Project p = new Project(ipvm.GetCurrentUser().GetId(), ProjectName, ProjectOwner, ProjectManager, ProjectDescription, DateTime.Now);
            
            projectRepo.Add(p);
           
            Resource r = new Resource(ipvm.GetCurrentUser().GetId(), p.Id, string.IsNullOrEmpty(ResourceExpectedYearlyCost) ? 0 : decimal.Parse(ResourceStartAmount), string.IsNullOrEmpty(ResourceExpectedYearlyCost) ? 0 : decimal.Parse(ResourceExpectedYearlyCost), DateTime.Now);
            resourceRepo.Add(r);

            ipvm.UpdateList();

            ProjectName = "";
            ProjectManager = "";
            ProjectOwner = "";
            ProjectDescription = "";
            ResourceStartAmount = "";
            ResourceExpectedYearlyCost = "";
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
