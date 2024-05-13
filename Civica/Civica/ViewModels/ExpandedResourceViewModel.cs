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
using System.Windows;

namespace Civica.ViewModels
{
    public class ExpandedResourceViewModel : ObservableObject, IViewModelChild
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

        private int _auditYear;
        public int AuditYear
        {
            get => _auditYear;
            set
            {
                _auditYear = value;
                OnPropertyChanged(nameof(AuditYear));
            }
        }
        private decimal _auditExpectedYearlyCost;
        public decimal AuditExpectedYearlyCost
        {
            get => _auditExpectedYearlyCost;
            set
            {
                _auditExpectedYearlyCost = value;
                OnPropertyChanged(nameof(AuditExpectedYearlyCost));
            }
        }

        private string _auditDescription;
        public string AuditDescription
        {
            get => _auditDescription;
            set
            {
                _auditDescription = value;
                OnPropertyChanged(nameof(AuditDescription));
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

        private WindowVisibility _createAuditVisibility;
        public WindowVisibility CreateAuditVisibility
        {
            get => _createAuditVisibility;
            set
            {
                _createAuditVisibility = value;
                OnPropertyChanged(nameof(CreateAuditVisibility));
            }
        }

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            auditRepo = mvm.GetAuditRepo();
            resourceRepo = mvm.GetResourceRepo();

            ResourceVisiblity = WindowVisibility.Visible;
            ResourceDetailsVisibility = WindowVisibility.Hidden;
            EditAuditVisiblity = WindowVisibility.Hidden;
            EditResourceVisibility = WindowVisibility.Hidden;
            CreateAuditVisibility = WindowVisibility.Hidden;
        }

        public RelayCommand ChangeViewCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    switch (ervm.Title)
                    {
                        case "Omkostninger":
                            ervm.Title = "Resourceforbrug";
                            ervm.Audits.Clear();
                            ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                            ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                            ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                            break;
                        case "Resourceforbrug":
                        default:
                            ervm.Title = "Omkostninger";
                            ervm.Audits.Clear();
                            ervm.UpdateList();
                            break;
                    }
                }
            },
            parameter =>
            {
                return true;
            });

        public RelayCommand EditAuditViewCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    ervm.EditAuditVisiblity = WindowVisibility.Visible;
                    ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                    ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand EditResourceCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    ervm.EditResourceVisibility = WindowVisibility.Visible;
                    ervm.ResourceVisiblity = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand SaveAuditCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 AuditViewModel avm = ervm.SelectedAudit;
                 string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
                 avm.Amount = temp;
                 ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                 ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                 ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                 Audit a = ervm.auditRepo.GetByRefId(avm.GetRefId()).FirstOrDefault(x => x.Id == avm.GetId());

                 a.Amount = decimal.Parse(ervm.SelectedAudit.Amount);
                 a.Year = ervm.SelectedAudit.Year;
                 a.Description = ervm.SelectedAudit.Description;

                 ervm.auditRepo.Update(a);

                 ervm.Audits.Clear();

                 ervm.UpdateList();

                 ervm.SelectedAudit = avm;
             }
         },
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                 {
                     return true;
                 }
             }
             return false;
         });

        public RelayCommand CreateAuditCmdView { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                    ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                    ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    ervm.CreateAuditVisibility = WindowVisibility.Visible;

                    ervm.AuditExpectedYearlyCost = 0;
                    ervm.AuditYear = DateTime.Now.Year;
                }
            },
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand RemoveAuditCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  MessageBoxButton button = MessageBoxButton.OKCancel;
                  MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette denne?", "Bekræft sletning", button);

                  if (result == MessageBoxResult.OK)
                  {
                      AuditViewModel avm = ervm.SelectedAudit;

                      Audit a = ervm.auditRepo.GetByRefId(avm.GetRefId()).FirstOrDefault(x => x.Id == avm.GetId());




                      ervm.auditRepo.Remove(a);

                      ervm.Audits.Clear();

                      ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                      ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                      ervm.CreateAuditVisibility = WindowVisibility.Hidden;

                      ervm.UpdateList();
                  }
              }
          },
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                  {
                      return true;
                  }
              }
              return false;
          });


        public RelayCommand CancelAuditCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                  ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                  ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                  ervm.CreateAuditVisibility = WindowVisibility.Hidden;

                  ervm.AuditExpectedYearlyCost = 0;
                  ervm.AuditYear = DateTime.Now.Year;
              }
          },
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                  {
                      return true;
                  }
              }
              return false;
          });

        public RelayCommand CreateAuditCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 Audit a = new Audit(ervm.mvm.ipvm.GetCurrentUser().GetId(), ervm.SelectedResource.GetId(), ervm.AuditExpectedYearlyCost, ervm.AuditYear, ervm.AuditDescription, DateTime.Now);

                 ervm.auditRepo.Add(a);

                 ervm.Audits.Clear();

                 ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                 ervm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                 ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                 ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                 ervm.UpdateList();
             }
         },
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.SelectedProject != null && ervm.mvm.ipvm.GetCurrentUser() != null)
                 {
                     return true;
                 }
             }
             return false;
         });

        public RelayCommand SaveResourceCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 ResourceViewModel resourceVm = ervm.SelectedResource;
                 string temp = string.Format("{0:#,0}", double.Parse(resourceVm.StartAmount));
                 resourceVm.StartAmount = temp;
                 temp = string.Format("{0:#,0}", double.Parse(resourceVm.ExpectedYearlyCost));
                 resourceVm.ExpectedYearlyCost = temp;

                 ervm.EditResourceVisibility = WindowVisibility.Hidden;
                 ervm.ResourceVisiblity = WindowVisibility.Visible;

                 Resource r = ervm.resourceRepo.GetById(ervm.SelectedResource.GetId());

                 r.StartAmount = decimal.Parse(ervm.SelectedResource.StartAmount);
                 r.ExpectedYearlyCost = decimal.Parse(ervm.SelectedResource.ExpectedYearlyCost);

                 ervm.resourceRepo.Update(r);

                 ervm.SelectedResource = resourceVm;
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