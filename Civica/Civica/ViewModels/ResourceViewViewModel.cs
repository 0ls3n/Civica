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
    public class ResourceViewViewModel : ObservableObject, IViewModelChild
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
                if (parameter is ResourceViewViewModel rvvm)
                {
                    switch (rvvm.Title)
                    {
                        case "Omkostninger":
                            rvvm.Title = "Resourceforbrug";
                            rvvm.Audits.Clear();
                            rvvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                            rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                            rvvm.CreateAuditVisibility = WindowVisibility.Hidden;
                            break;
                        case "Resourceforbrug":
                        default:
                            rvvm.Title = "Omkostninger";
                            rvvm.Audits.Clear();
                            rvvm.UpdateList();
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
                if (parameter is ResourceViewViewModel rvvm)
                {
                    rvvm.EditAuditVisiblity = WindowVisibility.Visible;
                    rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                    rvvm.CreateAuditVisibility = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ResourceViewViewModel rvvm)
                {
                    if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand EditResourceCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ResourceViewViewModel rvvm)
                {
                    rvvm.EditResourceVisibility = WindowVisibility.Visible;
                    rvvm.ResourceVisiblity = WindowVisibility.Hidden;
                }
            },
            parameter =>
            {
                if (parameter is ResourceViewViewModel rvvm)
                {
                    if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand SaveAuditCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ResourceViewViewModel rvvm)
             {
                 AuditViewModel avm = rvvm.SelectedAudit;
                 string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
                 avm.Amount = temp;
                 rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                 rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                 rvvm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                 Audit a = rvvm.auditRepo.GetByRefId(avm.GetRefId()).FirstOrDefault(x => x.Id == avm.GetId());

                 a.Amount = decimal.Parse(rvvm.SelectedAudit.Amount);
                 a.Year = rvvm.SelectedAudit.Year;
                 a.Description = rvvm.SelectedAudit.Description;

                 rvvm.auditRepo.Update(a);

                 rvvm.Audits.Clear();



                 rvvm.UpdateList();

                 rvvm.SelectedAudit = avm;
             }
         },
         parameter =>
         {
             if (parameter is ResourceViewViewModel rvvm)
             {
                 if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                 {
                     return true;
                 }
             }
             return false;
         });

        public RelayCommand CreateAuditCmdView { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ResourceViewViewModel rvvm)
                {
                    rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                    rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                    rvvm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                    rvvm.CreateAuditVisibility = WindowVisibility.Visible;

                    rvvm.AuditExpectedYearlyCost = 0;
                    rvvm.AuditYear = DateTime.Now.Year;
                }
            },
            parameter =>
            {
                if (parameter is ResourceViewViewModel rvvm)
                {
                    if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand RemoveAuditCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ResourceViewViewModel rvvm)
              {
                  MessageBoxButton button = MessageBoxButton.OKCancel;
                  MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette denne?", "Bekræft sletning", button);

                  if (result == MessageBoxResult.OK)
                  {
                      AuditViewModel avm = rvvm.SelectedAudit;

                      Audit a = rvvm.auditRepo.GetByRefId(avm.GetRefId()).FirstOrDefault(x => x.Id == avm.GetId());




                      rvvm.auditRepo.Remove(a);

                      rvvm.Audits.Clear();

                      rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                      rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                      rvvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                      rvvm.CreateAuditVisibility = WindowVisibility.Hidden;

                      rvvm.UpdateList();
                  }
              }
          },
          parameter =>
          {
              if (parameter is ResourceViewViewModel rvvm)
              {
                  if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                  {
                      return true;
                  }
              }
              return false;
          });


        public RelayCommand CancelAuditCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ResourceViewViewModel rvvm)
              {
                  rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                  rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                  rvvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                  rvvm.CreateAuditVisibility = WindowVisibility.Hidden;

                  rvvm.AuditExpectedYearlyCost = 0;
                  rvvm.AuditYear = DateTime.Now.Year;
              }
          },
          parameter =>
          {
              if (parameter is ResourceViewViewModel rvvm)
              {
                  if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                  {
                      return true;
                  }
              }
              return false;
          });

        public RelayCommand CreateAuditCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ResourceViewViewModel rvvm)
             {
                 Audit a = new Audit(rvvm.mvm.ipvm.GetCurrentUser().GetId(), rvvm.SelectedResource.GetId(), rvvm.AuditExpectedYearlyCost, rvvm.AuditYear, rvvm.AuditDescription, DateTime.Now);

                 rvvm.auditRepo.Add(a);

                 rvvm.Audits.Clear();

                 rvvm.EditAuditVisiblity = WindowVisibility.Hidden;
                 rvvm.ResourceDetailsVisibility = WindowVisibility.Hidden;
                 rvvm.CreateAuditVisibility = WindowVisibility.Hidden;
                 rvvm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                 rvvm.UpdateList();
             }
         },
         parameter =>
         {
             if (parameter is ResourceViewViewModel rvvm)
             {
                 if (rvvm.SelectedProject != null && rvvm.mvm.ipvm.GetCurrentUser() != null)
                 {
                     return true;
                 }
             }
             return false;
         });

        public RelayCommand SaveResourceCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ResourceViewViewModel rvvm)
             {
                 ResourceViewModel resourceVm = rvvm.SelectedResource;
                 string temp = string.Format("{0:#,0}", double.Parse(resourceVm.StartAmount));
                 resourceVm.StartAmount = temp;
                 temp = string.Format("{0:#,0}", double.Parse(resourceVm.ExpectedYearlyCost));
                 resourceVm.ExpectedYearlyCost = temp;

                 rvvm.EditResourceVisibility = WindowVisibility.Hidden;
                 rvvm.ResourceVisiblity = WindowVisibility.Visible;

                 Resource r = rvvm.resourceRepo.GetById(rvvm.SelectedResource.GetId());

                 r.StartAmount = decimal.Parse(rvvm.SelectedResource.StartAmount);
                 r.ExpectedYearlyCost = decimal.Parse(rvvm.SelectedResource.ExpectedYearlyCost);

                 rvvm.resourceRepo.Update(r);

                 rvvm.SelectedResource = resourceVm;
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