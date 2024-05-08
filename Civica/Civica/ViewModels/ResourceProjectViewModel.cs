using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Resources;
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
        IRepository<Resource> resourceRepo;
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

        private string _title = "Omkostninger";
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged(nameof(Title));
            }
        }

        private string _combinedCost;
        public string CombinedCost
        {
            get => _combinedCost;
            set
            {
                _combinedCost = value;
                OnPropertyChanged(nameof(CombinedCost));
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

        private WindowVisibility _editAuditVisiblity;
        public WindowVisibility EditAuditVisiblity
        {
            get => _editAuditVisiblity;
            set
            {
                _editAuditVisiblity = value;
                OnPropertyChanged(nameof(EditAuditVisiblity));
            }
        }

        private WindowVisibility _editResourceVisibility;
        public WindowVisibility EditResourceVisibility
        {
            get => _editResourceVisibility;
            set
            {
                _editResourceVisibility = value;
                OnPropertyChanged(nameof(EditResourceVisibility));
            }
        }

        private WindowVisibility _resourceVisiblity;
        public WindowVisibility ResourceVisiblity
        {
            get => _resourceVisiblity;
            set
            {
                _resourceVisiblity = value;
                OnPropertyChanged(nameof(ResourceVisiblity));
            }
        }

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            auditRepo = mvm.GetAuditRepo();
            resourceRepo = mvm.GetResourceRepo();
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
                        case "Omkostninger":
                            Title = "Resourceforbrug";
                            rvm.Audits.Clear();
                            rvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            rvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                            rvm.EditAuditVisiblity = WindowVisibility.Hidden;
                            break;
                        case "Resourceforbrug":
                        default:
                            Title = "Omkostninger";
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

            ResourceVisiblity = WindowVisibility.Visible;
            ResourceDetailsVisibility = WindowVisibility.Hidden;
            EditAuditVisiblity = WindowVisibility.Hidden;
            EditResourceVisibility = WindowVisibility.Hidden;
        }

        public RelayCommand ChangeViewCmd { get; set; }
        public RelayCommand EditAuditViewCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ResourceProjectViewModel rvm)
                {
                    rvm.EditAuditVisiblity = WindowVisibility.Visible;
                    rvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                bool isEnabled = true;
                return isEnabled;
            });

        public RelayCommand EditResourceCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ResourceProjectViewModel rvm)
                {
                    rvm.EditResourceVisibility = WindowVisibility.Visible;
                    rvm.ResourceVisiblity = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                bool isEnabled = true;
                return isEnabled;
            });

        public RelayCommand SaveAuditCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ResourceProjectViewModel rvm)
             {
                 AuditViewModel avm = rvm.SelectedAudit;
                 string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
                 avm.Amount = temp;
                 rvm.EditAuditVisiblity = WindowVisibility.Hidden;
                 rvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                 rvm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                 Audit a = rvm.auditRepo.GetById(rvm.SelectedAudit.GetRefId());

                 a.Amount = decimal.Parse(rvm.SelectedAudit.Amount);
                 a.Year = rvm.SelectedAudit.Year;

                 rvm.auditRepo.Update(a);

                 rvm.UpdateList();

                 rvm.SelectedAudit = avm;
             }
         },
         parameter =>
         {
             bool isEnabled = true;
             return isEnabled;
         });

        public RelayCommand SaveResourceCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ResourceProjectViewModel rvm)
             {
                 ResourceViewModel resourceVm = rvm.SelectedResource;
                 string temp = string.Format("{0:#,0}", double.Parse(resourceVm.StartAmount));
                 resourceVm.StartAmount = temp;
                 temp = string.Format("{0:#,0}", double.Parse(resourceVm.ExpectedYearlyCost));
                 resourceVm.ExpectedYearlyCost = temp;

                 rvm.EditResourceVisibility = WindowVisibility.Hidden;
                 rvm.ResourceVisiblity = WindowVisibility.Visible;

                 Resource r = rvm.resourceRepo.GetById(rvm.SelectedResource.GetId());

                 r.StartAmount = decimal.Parse(rvm.SelectedResource.StartAmount);
                 r.ExpectedYearlyCost = decimal.Parse(rvm.SelectedResource.ExpectedYearlyCost);

                 rvm.resourceRepo.Update(r);

                 rvm.UpdateList();

                 rvm.SelectedResource = resourceVm;
             }
         },
         parameter =>
         {
             bool isEnabled = true;
             return isEnabled;
         });

        public void UpdateList()
        {
            Audits.Clear();
            double temp = double.Parse(SelectedResource.StartAmount);
            List<Audit> audits = auditRepo.GetByRefId(SelectedResource.GetId()).OrderBy(x => x.Year).ToList<Audit>();
            foreach (Audit a in audits)
            {
                Audits.Add(new AuditViewModel(a));
                temp += double.Parse(a.Amount.ToString());
            }
            CombinedCost = string.Format("{0:#,0}", temp);
        }
    }
}