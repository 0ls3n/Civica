using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Civica.ViewModels
{
    public class ResourceProjectViewModel : ObservableObject, IViewModelChild
    {
        private MainViewModel mvm { get; set; }

        IRepository<Audit> auditRepo;
        public ObservableCollection<AuditViewModel> Audits { get; set; } = new ObservableCollection<AuditViewModel>();

        private ProgressViewModel _selectedProgress;
        public ProgressViewModel SelectedProgress
        {
            get => _selectedProgress;
            set
            {
                _selectedProgress = value;
                OnPropertyChanged(nameof(SelectedProgress));
            }
        }

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

        private string _title = "Revision";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private ResourceViewModel _selectedResource;
        public ResourceViewModel SelectedResource
        {
            get => _selectedResource;
            set
            {
                _selectedResource = value;
                OnPropertyChanged(nameof(SelectedResource));
            }
        }

        private AuditViewModel _selectedAudit;
        public AuditViewModel SelectedAudit
        {
            get => _selectedAudit;
            set
            {
                InformationPlaceholderVisibility = WindowVisibility.Hidden;
                ResourceDetailsVisibility = WindowVisibility.Visible;
                _selectedAudit = value;
                OnPropertyChanged(nameof(SelectedAudit));
            }
        }

        private WindowVisibility _informationPlaceholderVisibility;
        public WindowVisibility InformationPlaceholderVisibility
        {
            get => _informationPlaceholderVisibility;
            set
            {
                _informationPlaceholderVisibility = value;
                OnPropertyChanged(nameof(InformationPlaceholderVisibility));
            }
        }

        private WindowVisibility _resourceDetailsVisibility;
        public WindowVisibility ResourceDetailsVisibility
        {
            get => _resourceDetailsVisibility;
            set
            {
                _resourceDetailsVisibility = value;
                OnPropertyChanged(nameof(ResourceDetailsVisibility));
            }
        }

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            auditRepo = mvm.GetAuditRepo();
        }

        public ResourceProjectViewModel()
        {
            ChangeViewCmd = new RelayCommand(
            parameter =>
            {
                if (parameter is ResourceProjectViewModel rvm)
                {
                    switch (Title)
                    {
                        case "Revision":
                            Title = "Resourceforbrug";
                            rvm.Audits.Clear();
                            rvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            rvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                            break;
                        case "Resourceforbrug":
                        default:
                            Title = "Revision";
                            rvm.UpdateList();
                            break;
                    }
                }
            },
            parameter =>
            {
                bool isEnabled = true;
                return isEnabled;
            });

            ResourceDetailsVisibility = WindowVisibility.Hidden;
        }

        public RelayCommand ChangeViewCmd { get; set; }

        public void UpdateList()
        {
            Audits.Clear();
            List<Audit> audits = auditRepo.GetByRefId(mvm.ipvm.SelectedResource.GetId()).OrderBy(x => x.Year).ToList<Audit>();
            foreach (Audit a in audits)
            {
                Audits.Add(new AuditViewModel(a));
            }
        }
    }
}