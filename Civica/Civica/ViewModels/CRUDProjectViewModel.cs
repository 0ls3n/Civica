using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using GVMR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class CRUDProjectViewModel : ObservableObject, IViewModelChild
    {
        private string _name = "";
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _owner = "";
        public string Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }

        private string _manager = "";

        public string Manager
        {
            get => _manager;
            set
            {
                _manager = value;
                OnPropertyChanged(nameof(Manager));
            }
        }

        private string _description = "";

        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        private string _startAmount = "";
        public string StartAmount
        {
            get => _startAmount;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _startAmount = value;
                }
                else
                {
                    MessageBox.Show("'Forventet start beløb' må kun være i tal");
                }
                OnPropertyChanged(nameof(StartAmount));
            }
        }
        private string _expectedYearlyCost = "";
        public string ExpectedYearlyCost
        {
            get => _expectedYearlyCost;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _expectedYearlyCost = value;
                }
                else
                {
                    MessageBox.Show("'Forventet årlig omkostning' må kun være i tal");
                }
                OnPropertyChanged(nameof(ExpectedYearlyCost));
            }
        }

        private MainViewModel mvm;
        private IRepository<Project> projectRepo;
        private IRepository<Resource> resourceRepo;
        private IRepository<Worktime> worktimeRepo;
        private IRepository<Audit> auditRepo;
        private IRepository<Progress> progressRepo;

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);

            projectRepo = mvm.GetProjectRepo();
            resourceRepo = mvm.GetResourceRepo();
            worktimeRepo = mvm.GetWorktimeRepo();
            auditRepo = mvm.GetAuditRepo();
            progressRepo = mvm.GetProgressRepo();
        }

        public void CreateProject(int userId, string name, string owner, string manager, string description, int startAmount, decimal expectedYearlyCost)
        {
            Project p = new Project(userId, name, owner, manager, description, DateTime.Now);
            
            projectRepo.Add(p);
           
            Resource r = new Resource(userId, p.Id, startAmount, expectedYearlyCost, DateTime.Now);
            resourceRepo.Add(r);

            mvm.ipvm.UpdateList();

            Name = "";
            Manager = "";
            Owner = "";
            Description = "";
            this.StartAmount = "";
            ExpectedYearlyCost = "";
        }

        public void DeleteProject(ProjectViewModel pvm)
        {
            int pID = pvm.GetId();
            int rID = resourceRepo.GetById(x => x.RefId == pID).Id;
            auditRepo.DeleteByRefId(rID);
            worktimeRepo.DeleteByRefId(rID);
            progressRepo.DeleteByRefId(pID);
            resourceRepo.DeleteByRefId(pID);
            projectRepo.Delete(projectRepo.GetById(x => x.Id == pID));
            mvm.epvm.UpdateList();
            mvm.epvm.SelectedProject = null;
        }

        public void UpdateProject(ProjectViewModel pvm)
        {
            Project p = projectRepo.GetById(x => x.Id == pvm.GetId());
            p.Name = pvm.Name;
            p.Owner = pvm.Owner;
            p.Manager = pvm.Manager;
            p.Description = pvm.Description;

            projectRepo.Update(p);

            mvm.epvm.SelectedProject = pvm;
        }

        public RelayCommand CreateProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CRUDProjectViewModel cpvm)
                {
                    cpvm.CreateProject(cpvm.mvm.CurrentUser.GetId(), cpvm.Name, cpvm.Owner, cpvm.Manager, cpvm.Description, int.TryParse(cpvm.StartAmount, out int r) ? r : 0, decimal.TryParse(cpvm.ExpectedYearlyCost, out decimal r2) ? r2 : 0);
                    cpvm.mvm.ipvm.CreateVisibility = WindowVisibility.Hidden;
                    cpvm.mvm.ipvm.InformationVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CRUDProjectViewModel cpvm)
                {
                    if (!string.IsNullOrEmpty(cpvm.Name))
                    {
                        succes = true;
                    }
                }
                return succes;
            }
        );
    }
}
