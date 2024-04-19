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

        private string _informationVisibility;
        public string InformationVisivility
        {
            get => _informationVisibility;
            set
            {
                _informationVisibility = value;
                OnPropertyChanged(nameof(InformationVisivility));
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
                InformationVisivility = "Hidden";
                OnPropertyChanged(nameof(SelectedProject));
            }
        }

        private ProjectRepository projectRepo = new ProjectRepository();
        private ProgressRepository progressRepo = new ProgressRepository();

        public InProgressViewModel()
        {
            foreach (Project p in projectRepo.GetAll())
            {
                Projects.Add(new ProjectViewModel(p));
            }

            #region ColorCodingForProjects
            foreach (ProjectViewModel p in Projects)
            {
                List<Progress> sortedList = progressRepo.Get(p.GetId()).OrderByDescending(x => x.Date).ToList();

                switch(sortedList.FirstOrDefault().Status)
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
            #endregion

            Title = "Igangværende";
        }
    }
}
