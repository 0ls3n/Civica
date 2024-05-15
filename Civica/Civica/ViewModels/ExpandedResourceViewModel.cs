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
using System.Runtime.Intrinsics.X86;
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
        IRepository<Worktime> worktimeRepo;
        public ObservableCollection<AuditViewModel> Audits { get; set; } = new ObservableCollection<AuditViewModel>();
        public ObservableCollection<WorktimeViewModel> Worktimes { get; set; } = new ObservableCollection<WorktimeViewModel>();

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

        #region Audit Properties
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

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }
        #endregion

        #region Worktime Properties
        private string _worktimeInvolvedName;
        public string WorktimeInvolvedName
        {
            get => _worktimeInvolvedName;
            set
            {
                _worktimeInvolvedName = value;
                OnPropertyChanged(nameof(WorktimeInvolvedName));
            }
        }
        private int _worktimeEstimatedHours;
        public int WorktimeEstimatedHours
        {
            get => _worktimeEstimatedHours;
            set
            {
                _worktimeEstimatedHours = value;
                OnPropertyChanged(nameof(WorktimeEstimatedHours));
            }
        }
        #endregion

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
                AuditDetailsVisibility = WindowVisibility.Visible;
                _selectedAudit = value;
                OnPropertyChanged(nameof(SelectedAudit));
            }
        }

        private WorktimeViewModel _selectedWorktime;
        public WorktimeViewModel SelectedWorktime
        {
            get => _selectedWorktime;
            set
            {
                InformationPlaceholderVisibility = WindowVisibility.Hidden;
                WorktimeDetailsVisibility = WindowVisibility.Visible;
                _selectedWorktime = value;
                OnPropertyChanged(nameof(SelectedWorktime));
            }
        }

        #region WindowVisibility
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

        #region Resource
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
        #endregion

        #region Audit
        private WindowVisibility _auditDetailsVisibility;
        public WindowVisibility AuditDetailsVisibility
        {
            get => _auditDetailsVisibility;
            set
            {
                _auditDetailsVisibility = value;
                OnPropertyChanged(nameof(AuditDetailsVisibility));
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

        private WindowVisibility _auditListVisibility;
        public WindowVisibility AuditListVisibility
        {
            get => _auditListVisibility;
            set
            {
                _auditListVisibility = value;
                OnPropertyChanged(nameof(AuditListVisibility));
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
        #endregion

        #region Worktime
        private WindowVisibility _worktimeDetailsVisibility;
        public WindowVisibility WorktimeDetailsVisibility
        {
            get => _worktimeDetailsVisibility;
            set
            {
                _worktimeDetailsVisibility = value;
                OnPropertyChanged(nameof(WorktimeDetailsVisibility));
            }
        }
        private WindowVisibility _editWorktimeVisiblity;
        public WindowVisibility EditWorktimeVisiblity
        {
            get => _editWorktimeVisiblity;
            set
            {
                _editWorktimeVisiblity = value;
                OnPropertyChanged(nameof(EditWorktimeVisiblity));
            }
        }

        private WindowVisibility _createWorktimeVisibility;
        public WindowVisibility CreateWorktimeVisibility
        {
            get => _createWorktimeVisibility;
            set
            {
                _createWorktimeVisibility = value;
                OnPropertyChanged(nameof(CreateWorktimeVisibility));
            }
        }
        #endregion

        #endregion

        public void Init(ObservableObject o)
        {
            mvm = (o as MainViewModel);
            auditRepo = mvm.GetAuditRepo();
            resourceRepo = mvm.GetResourceRepo();
            worktimeRepo = mvm.GetWorktimeRepo();

            ResourceVisiblity = WindowVisibility.Visible;
            AuditDetailsVisibility = WindowVisibility.Hidden;
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
                            ervm.Worktimes.Clear();
                            ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                            ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                            ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                            ervm.AuditListVisibility = WindowVisibility.Hidden;
                            ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                            ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                            ervm.UpdateList();
                            break;
                        case "Resourceforbrug":
                        default:
                            ervm.Title = "Omkostninger";
                            ervm.Worktimes.Clear();
                            ervm.Audits.Clear();
                            ervm.AuditListVisibility = WindowVisibility.Visible;
                            ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                            ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                            ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                            ervm.UpdateList();
                            break;
                    }
                }
            },
            parameter =>
            {
                return true;
            });

        #region Audit/Worktime Commands
        public RelayCommand EditViewCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.AuditListVisibility == WindowVisibility.Visible)
                    {
                        ervm.EditAuditVisiblity = WindowVisibility.Visible;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                    }
                    else
                    {
                        ervm.EditWorktimeVisiblity = WindowVisibility.Visible;
                        ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                        ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
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

        public RelayCommand SaveCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.AuditListVisibility == WindowVisibility.Visible)
                 {
                     AuditViewModel avm = ervm.SelectedAudit;
                     string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
                     avm.Amount = temp;
                     ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     Audit a = ervm.auditRepo.GetById(x => x.Id == avm.GetId());

                     a.Amount = decimal.Parse(ervm.SelectedAudit.Amount);
                     a.Year = ervm.SelectedAudit.Year;
                     a.Description = ervm.SelectedAudit.Description;

                     ervm.auditRepo.Update(a);

                     ervm.Audits.Clear();

                     ervm.UpdateList();

                     ervm.SelectedAudit = avm;
                 }
                 else
                 {
                     WorktimeViewModel wvm = ervm.SelectedWorktime;

                     ervm.EditWorktimeVisiblity = WindowVisibility.Hidden;
                     ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     Worktime w = ervm.worktimeRepo.GetById(x => x.Id == wvm.GetId());

                     w.SpentHours = wvm.SpentHours;
                     w.EstimatedHours = wvm.EstimatedHours;
                     w.Description = wvm.Description;
                     w.InvolvedName = wvm.InvolvedName;

                     ervm.worktimeRepo.Update(w);

                     ervm.Worktimes.Clear();

                     ervm.UpdateList();

                     ervm.SelectedWorktime = wvm;
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

        public RelayCommand CreateCmdView { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.AuditListVisibility == WindowVisibility.Visible)
                    {
                        ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Visible;

                        ervm.AuditExpectedYearlyCost = 0;
                        ervm.AuditYear = DateTime.Now.Year;
                    }
                    else
                    {
                        ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                        ervm.EditWorktimeVisiblity = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateWorktimeVisibility = WindowVisibility.Visible;

                        ervm.Description = "";
                        ervm.WorktimeEstimatedHours = 0;
                        ervm.WorktimeInvolvedName = "";
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

        public RelayCommand RemoveCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  MessageBoxButton button = MessageBoxButton.OKCancel;
                  MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette denne?", "Bekræft sletning", button);

                  if (result == MessageBoxResult.OK)
                  {
                      if (ervm.AuditListVisibility == WindowVisibility.Visible)
                      {
                          AuditViewModel avm = ervm.SelectedAudit;

                          Audit a = ervm.auditRepo.GetById(x => x.Id == avm.GetId());

                          ervm.auditRepo.Remove(a);

                          ervm.Audits.Clear();

                          ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                          ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                          ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                          ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                      }
                      else
                      {
                          WorktimeViewModel wvm = ervm.SelectedWorktime;

                          Worktime w = ervm.worktimeRepo.GetById(x => x.Id == wvm.GetId());

                          ervm.worktimeRepo.Remove(w);

                          ervm.Worktimes.Clear();
                          ervm.EditWorktimeVisiblity = WindowVisibility.Hidden;
                          ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                          ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                          ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                      }
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


        public RelayCommand CancelCmd { get; set; } = new RelayCommand(
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  if (ervm.AuditListVisibility == WindowVisibility.Visible)
                  {
                      ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                      ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                      ervm.CreateAuditVisibility = WindowVisibility.Hidden;

                      ervm.AuditExpectedYearlyCost = 0;
                      ervm.AuditYear = DateTime.Now.Year;
                  }
                  else
                  {
                      ervm.EditWorktimeVisiblity = WindowVisibility.Hidden;
                      ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                      ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                      ervm.Description = "";
                      ervm.WorktimeEstimatedHours = 0;
                      ervm.WorktimeInvolvedName = "";
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

        public RelayCommand CreateCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.AuditListVisibility == WindowVisibility.Visible)
                 {
                     Audit a = new Audit(ervm.mvm.CurrentUser.GetId(), ervm.SelectedResource.GetId(), ervm.AuditExpectedYearlyCost, ervm.AuditYear, ervm.Description, DateTime.Now);

                     ervm.auditRepo.Add(a);

                     ervm.Audits.Clear();

                     ervm.EditAuditVisiblity = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     ervm.UpdateList();
                 }
                 else
                 {
                     if (string.IsNullOrEmpty(ervm.WorktimeInvolvedName))
                     {
                         MessageBox.Show("Der skal indtastes en afdeling eller et navn.");
                         return;
                     }
                     Worktime w = new Worktime(ervm.mvm.CurrentUser.GetId(), ervm.SelectedResource.GetId(), ervm.WorktimeEstimatedHours, ervm.WorktimeInvolvedName, ervm.Description, DateTime.Now);

                     ervm.worktimeRepo.Add(w);

                     ervm.Worktimes.Clear();

                     ervm.EditWorktimeVisiblity = WindowVisibility.Hidden;
                     ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                     ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

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
        #endregion

        #region Resource Commands

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

                 Resource r = ervm.resourceRepo.GetById(x => x.Id == ervm.SelectedResource.GetId());

                 r.StartAmount = decimal.Parse(ervm.SelectedResource.StartAmount);
                 r.ExpectedYearlyCost = decimal.Parse(ervm.SelectedResource.ExpectedYearlyCost);

                 ervm.resourceRepo.Update(r);

                 //ervm.UpdateList();

                 ervm.SelectedResource = resourceVm;
             }
         },
         parameter =>
         {
             bool isEnabled = true;
             return isEnabled;
         });
        #endregion

        public void UpdateList()
        {
            Audits.Clear();
            Worktimes.Clear();
            if (AuditListVisibility == WindowVisibility.Visible)
            {
                double temp = double.Parse(SelectedResource.StartAmount);
                List<Audit> audits = auditRepo.GetListById(x => x.RefId == SelectedResource.GetId()).OrderBy(x => x.Year).ToList<Audit>();

                foreach (Audit a in audits)
                {
                    Audits.Add(new AuditViewModel(a));
                    temp += double.Parse(a.Amount.ToString());
                }
                CombinedCost = string.Format("{0:#,0}", temp);
            }
            else
            {
                List<Worktime> worktimes = worktimeRepo.GetListById(x => x.RefId == SelectedResource.GetId()).OrderBy(x => x.CreatedDate).ToList<Worktime>();

                foreach (Worktime a in worktimes)
                {
                    Worktimes.Add(new WorktimeViewModel(a));
                }
            }
        }
    }
}