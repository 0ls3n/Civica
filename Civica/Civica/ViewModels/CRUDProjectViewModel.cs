﻿using Civica.Commands;
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

        public void Create()
        {
            Project p = new Project(mvm.CurrentUser.GetId(), ProjectName, ProjectOwner, ProjectManager, ProjectDescription, DateTime.Now);
            
            projectRepo.Add(p);
           
            Resource r = new Resource(mvm.CurrentUser.GetId(), p.Id, string.IsNullOrEmpty(ResourceExpectedYearlyCost) ? 0 : decimal.Parse(ResourceStartAmount), string.IsNullOrEmpty(ResourceExpectedYearlyCost) ? 0 : decimal.Parse(ResourceExpectedYearlyCost), DateTime.Now);
            resourceRepo.Add(r);

            mvm.ipvm.UpdateList();

            ProjectName = "";
            ProjectManager = "";
            ProjectOwner = "";
            ProjectDescription = "";
            ResourceStartAmount = "";
            ResourceExpectedYearlyCost = "";
        }

        public void Delete()
        {
            int pID = mvm.epvm.SelectedProject.GetId();
            int rID = resourceRepo.GetById(x => x.RefId == pID).Id;
            auditRepo.DeleteByRefId(rID);
            worktimeRepo.DeleteByRefId(rID);
            progressRepo.DeleteByRefId(pID);
            resourceRepo.DeleteByRefId(pID);
            projectRepo.Delete(projectRepo.GetById(x => x.Id == pID));
            mvm.epvm.UpdateList();
            mvm.epvm.SelectedProject = null;
        }

        public void Update()
        {
            ProjectViewModel pvm = mvm.epvm.SelectedProject;

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
                    cpvm.Create();
                    cpvm.mvm.ipvm.CreateVisibility = WindowVisibility.Hidden;
                    cpvm.mvm.ipvm.InformationVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CRUDProjectViewModel cpvm)
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
