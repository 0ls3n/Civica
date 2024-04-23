﻿using Civica.Commands;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.ViewModels
{
    public class InProgressViewModel : ObservableObject
    {
        private string _Windowtitle;
        public string WindowTitle
        {
            get => _Windowtitle;
            set
            {
                _Windowtitle = value;
                OnPropertyChanged(nameof(WindowTitle));
            }
        }

        #region Create
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

        private string _createVisibility;
        public string CreateVisibility
        {
            get => _createVisibility;
            set
            {
                _createVisibility = value;
                OnPropertyChanged(nameof(CreateVisibility));
            }
        }

        public void CreateProject()
        {
            Project p = new Project(ProjectName, ProjectOwner, ProjectManager, ProjectDescription);
            projectRepo.Add(p);

            UpdateList();

            ProjectName = "";
            ProjectManager = "";
            ProjectOwner = "";
            ProjectDescription = "";
        }

        public void CreateProgress(Phase fase, Status status, string description)
        {
            Progress prog = new Progress(SelectedProject.GetId(), fase, status, DateTime.Now, description);

            progressRepo.Add(prog);

            SelectedProgresses.Add(new ProgressViewModel(prog));
        }

        public ICommand CreateProjectViewCmd { get; set; } = new CreateProjectViewCmd();
        public ICommand CreateProjectCmd { get; set; } = new CreateProjectCmd();
        #endregion

        #region Update
        public string OldName;

        private string _editVisibility;
        public string EditVisibility
        {
            get => _editVisibility;
            set
            {
                _editVisibility = value;
                OnPropertyChanged(nameof(EditVisibility));
            }
        }

        public void UpdateProject(ProjectViewModel project)
        {
            int index = Projects.IndexOf(project);

            Projects[index] = null;

            //project.Name = name;
            //project.Owner = owner;
            //project.Manager = manager;
            //project.Description = description;

            Projects[index] = project;

            projectRepo.Update(projectRepo.Get(project.GetId()), project.Name, project.Owner, project.Manager, project.Description);
        }
        public ICommand UpdateProjectViewCmd { get; set; } = new UpdateProjectViewCmd();
        public ICommand UpdateProjectCmd { get; set; } = new UpdateProjectCmd();

        #endregion

        #region Progress
        public IEnumerable<Phase> Phases => Enum.GetValues(typeof(Phase)).Cast<Phase>();
        public IEnumerable<Status> Statuses => Enum.GetValues(typeof(Status)).Cast<Status>();
        public ObservableCollection<ProgressViewModel> SelectedProgresses { get; set; } = new ObservableCollection<ProgressViewModel>();
        private ProgressViewModel _selectedProgress = null;
        public ProgressViewModel SelectedProgress
        {
            get
            {
                return _selectedProgress;
            }
            set
            {
                _selectedProgress = value;
                OnPropertyChanged(nameof(SelectedProgress));
            }
        }

        private string _progressDescription = "";
        public string ProgressDescription
        {
            get
            {
                return _progressDescription;
            }
            set
            {
                _progressDescription = value;
                OnPropertyChanged(nameof(ProgressDescription));
            }
        }

        private Phase _phase;
        public Phase Phase
        {
            get
            { return _phase; }
            set
            {
                _phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }
        private Status _status;
        public Status Status
        {
            get
            {
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ICommand ProgressProjectCmd { get; set; } = new ProgressProjectCmd();
        public ICommand ProgressProjectViewCmd { get; set; } = new ProgressProjectViewCmd();

        private string _progressVisibility;
        public string ProgressVisibility
        {
            get => _progressVisibility;
            set
            {
                _progressVisibility = value;
                OnPropertyChanged(nameof(ProgressVisibility));
            }
        }
        #endregion

        private string _informationVisibility;
        public string InformationVisibility
        {
            get => _informationVisibility;
            set
            {
                _informationVisibility = value;
                OnPropertyChanged(nameof(InformationVisibility));
            }
        }

        public ObservableCollection<ProjectViewModel> Projects { get; set; } = new ObservableCollection<ProjectViewModel>();

        private ProjectViewModel _selectedProject;
        public ProjectViewModel SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                InformationVisibility = "Hidden";
                CreateVisibility = "Hidden";
                ProgressVisibility = "Hidden";
                EditVisibility = "Hidden";
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        public ICommand RemoveProjectCmd { get; set; } = new RemoveProjectCmd();

        private ProjectRepository projectRepo = new ProjectRepository();
        private ProgressRepository progressRepo = new ProgressRepository();

        public InProgressViewModel()
        {
            UpdateList();

            WindowTitle = "Igangværende";

            CreateVisibility = "Hidden";
            EditVisibility = "Hidden";
            ProgressVisibility = "Hidden";
        }

        public void UpdateList()
        {
            Projects.Clear();
            foreach (Project p in projectRepo.GetAll())
            {
                Projects.Add(new ProjectViewModel(p));
            }

            foreach (ProjectViewModel p in Projects)
            {
                List<Progress> sortedList = progressRepo.Get(p.GetId()).OrderByDescending(x => x.Date).ToList();

                Progress prog = sortedList.FirstOrDefault();

                if (prog != null)
                {
                    switch (prog.Status)
                    {
                        case Models.Enums.Status.ON_TRACK:
                            p.StatusColor = "#008000";
                            break;
                        case Models.Enums.Status.DELAYED:
                            p.StatusColor = "#FDC300";
                            break;
                        case Models.Enums.Status.CRITICAL:
                            p.StatusColor = "#E20F1A";
                            break;
                        default:
                            p.StatusColor = "#E8E8E8";
                            break;
                    }
                }
                else
                {
                    p.StatusColor = "#E8E8E8";
                }
            }
        }

        public void RemoveProject()
        {
            projectRepo.Remove(projectRepo.Get(SelectedProject.GetId()));
            UpdateList();
        }
    }
}
