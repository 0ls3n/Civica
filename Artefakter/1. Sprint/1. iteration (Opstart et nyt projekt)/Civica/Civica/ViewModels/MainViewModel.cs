using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Civica.Commands;
using Civica.Models;
using Civica.Models.Enums;

namespace Civica.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        #region Old Code
        //public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();
        //public ObservableCollection<ProgressViewModel> SelectedProgresses { get; set; } = new ObservableCollection<ProgressViewModel>();

        //private ProjectRepository projectRepo = new ProjectRepository();
        //private ProgressRepository progressRepo = new ProgressRepository();
        //private ProjectViewModel selectedProject = null;
        //public ProjectViewModel SelectedProject
        //{
        //    get
        //    {
        //        return selectedProject;
        //    }
        //    set
        //    {
        //        selectedProject = value;
        //        OnPropertyChanged(nameof(SelectedProject));
        //        OnPropertyChanged(nameof(CanUpdateProject));

        //        SelectedProgresses.Clear();

        //        foreach (Progress prog in progressRepo.Get(SelectedProject.GetId()))
        //        {
        //            SelectedProgresses.Add(new ProgressViewModel(prog));
        //        }
        //    }
        //}
        //private ProgressViewModel selectedProgress = null;
        //public ProgressViewModel SelectedProgress
        //{
        //    get
        //    {
        //        return selectedProgress;
        //    }
        //    set
        //    {
        //        selectedProgress = value;
        //        OnPropertyChanged(nameof(SelectedProgress));
        //    }
        //}

        //public bool CanUpdateProject => SelectedProject != null;

        //public MainViewModel()
        //{
        //    foreach (Project p in projectRepo.GetAll())
        //    {
        //        Projects.Add(new ProjectViewModel(p));
        //    }
        //}

        //public void CreateNewProject(string name, string owner = "", string manager = "", string description = "")
        //{
        //    Project p = new Project(name, owner, manager, description);

        //    projectRepo.Add(p);

        //    ProjectViewModel pvm = new ProjectViewModel(p);

        //    Projects.Add(pvm);
        //}

        //public void UpdateProject(ProjectViewModel project, string name, string owner = "", string manager = "", string description = "")
        //{
        //    int index = Projects.IndexOf(project);

        //    Projects[index] = null;

        //    project.Name = name;
        //    project.Owner = owner;
        //    project.Manager = manager;
        //    project.Description = description;

        //    Projects[index] = project;

        //    projectRepo.Update(projectRepo.Get(project.GetId()), name, owner, manager, description);
        //}

        //public void RemoveProject()
        //{
        //    Project p = projectRepo.Get(SelectedProject.GetId());
        //    projectRepo.Remove(p);
        //    Projects.Remove(SelectedProject);
        //}

        //public void ProgressProject(Phase fase, Status status, string description)
        //{
        //    Progress prog = new Progress(SelectedProject.GetId(), fase, status, DateTime.Now, description);
        //    prog.ProjectId = SelectedProject.GetId();

        //    progressRepo.Add(prog);

        //    SelectedProgresses.Add(new ProgressViewModel(prog));
        //}

        #endregion

        #region New code

        private string _viewTitle;
        public string ViewTitle
        {
            get => _viewTitle;
            set
            {
                _viewTitle = value;
                OnPropertyChanged(nameof(ViewTitle));
            }
        }

        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        private string _inProgressView;
        public string InProgressView
        {
            get => _inProgressView;

            set
            {
                _inProgressView = value;
                OnPropertyChanged(nameof(InProgressView));
            }
        }

        public InProgressViewModel ipvm { get; set; } = new InProgressViewModel(); 

        //public ICommand InProgressViewCmd { get; set; } = new InProgressViewCmd();
        public RelayCommand InProgressViewCmd { get; set; } = new RelayCommand
        (
            execute: (object? parameter) =>
            {
                if (parameter is MainViewModel mvm)
                {
                    mvm.CurrentView = mvm.ipvm;
                    mvm.ViewTitle = mvm.ipvm.WindowTitle;
                }
            },
            canExecute: (object? parameter) =>
            {
                return true;
            });

        public MainViewModel()
        {
            ipvm.Init(this);
            
            ViewTitle = ipvm.WindowTitle;
        }
        #endregion
    }
}