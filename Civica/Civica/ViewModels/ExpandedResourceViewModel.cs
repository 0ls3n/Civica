using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using Civica.Views;
using GVMR;
using Microsoft.IdentityModel.Tokens;
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
        private string _auditAmount;
        public string AuditAmount
        {
            get => _auditAmount;
            set
            {
                if (decimal.TryParse(value, out _) || value == "")
                {
                    _auditAmount = value;
                }
                else
                {
                    MessageBox.Show("'Omkostning' må kun være tal.");
                }
                OnPropertyChanged(nameof(AuditAmount));
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
        private string _worktimeEstimatedHours = "";
        public string WorktimeEstimatedHours
        {
            get => _worktimeEstimatedHours;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _worktimeEstimatedHours = value;
                }
                else
                {
                    MessageBox.Show("'Est. timer' må kun være i tal.");
                }
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
                EditAuditVisibility = WindowVisibility.Hidden;
                CreateAuditVisibility = WindowVisibility.Hidden;
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
                EditWorktimeVisibility = WindowVisibility.Hidden;
                CreateWorktimeVisibility = WindowVisibility.Hidden;
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
        private WindowVisibility _editAuditVisibility;
        public WindowVisibility EditAuditVisibility
        {
            get => _editAuditVisibility;
            set
            {
                _editAuditVisibility = value;
                OnPropertyChanged(nameof(EditAuditVisibility));
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
        private WindowVisibility _editWorktimeVisibility;
        public WindowVisibility EditWorktimeVisibility
        {
            get => _editWorktimeVisibility;
            set
            {
                _editWorktimeVisibility = value;
                OnPropertyChanged(nameof(EditWorktimeVisibility));
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
            EditAuditVisibility = WindowVisibility.Hidden;
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
                            ervm.EditAuditVisibility = WindowVisibility.Hidden;
                            ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
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
                            ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
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
                        ervm.EditAuditVisibility = WindowVisibility.Visible;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                    }
                    else
                    {
                        ervm.EditWorktimeVisibility = WindowVisibility.Visible;
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
                     ervm.EditAuditVisibility = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     ervm.UpdateAudit();


                 }
                 else
                 {
                     ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
                     ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     ervm.UpdateWorktime();
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
                        ervm.EditAuditVisibility = WindowVisibility.Hidden;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Visible;

                        ervm.AuditAmount = "";
                        ervm.AuditYear = DateTime.Now.Year;
                    }
                    else
                    {
                        ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                        ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateWorktimeVisibility = WindowVisibility.Visible;

                        ervm.Description = "";
                        ervm.WorktimeEstimatedHours = "";
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
                          ervm.DeleteAudit();

                          ervm.EditAuditVisibility = WindowVisibility.Hidden;
                          ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                          ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                          ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                      }
                      else
                      {
                          ervm.DeleteWorktime();

                          ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
                          ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                          ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                          ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                      }
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
                      ervm.EditAuditVisibility = WindowVisibility.Hidden;
                      ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                      ervm.CreateAuditVisibility = WindowVisibility.Hidden;

                      ervm.AuditAmount = "";
                      ervm.AuditYear = DateTime.Now.Year;
                  }
                  else
                  {
                      ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
                      ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                      ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                      ervm.Description = "";
                      ervm.WorktimeEstimatedHours = "";
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
                     ervm.CreateAudit();

                     ervm.EditAuditVisibility = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                 }
                 else
                 {
                     if (string.IsNullOrEmpty(ervm.WorktimeInvolvedName))
                     {
                         MessageBox.Show("Der skal indtastes en afdeling eller et navn.");
                         return;
                     }
                     ervm.CreateWorktime();

                     ervm.EditWorktimeVisibility = WindowVisibility.Hidden;
                     ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                     ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
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

        #region Methods
        public void CreateWorktime()
        {
            Worktime w = new Worktime(mvm.CurrentUser.GetId(), SelectedResource.GetId(), WorktimeEstimatedHours.IsNullOrEmpty() ? 0 : int.Parse(WorktimeEstimatedHours), WorktimeInvolvedName, Description, DateTime.Now);

            worktimeRepo.Add(w);

            Worktimes.Clear();

            UpdateList();
        }

        public void UpdateWorktime()
        {
            WorktimeViewModel wvm = SelectedWorktime;

            Worktime w = worktimeRepo.GetById(x => x.Id == wvm.GetId());

            w.SpentHours = int.Parse(wvm.SpentHours);
            w.EstimatedHours = int.Parse(wvm.EstimatedHours);
            w.Description = wvm.Description;
            w.InvolvedName = wvm.InvolvedName;

            worktimeRepo.Update(w);

            Worktimes.Clear();

            UpdateList();

            SelectedWorktime = wvm;
        }

        public void DeleteWorktime()
        {
            WorktimeViewModel wvm = SelectedWorktime;

            Worktime w = worktimeRepo.GetById(x => x.Id == wvm.GetId());

            worktimeRepo.Remove(w);

            Worktimes.Clear();

            UpdateList();
        }

        public void CreateAudit()
        {
            Audit a = new Audit(mvm.CurrentUser.GetId(), SelectedResource.GetId(), AuditAmount.IsNullOrEmpty() ? 0 : decimal.Parse(AuditAmount), AuditYear, Description, DateTime.Now);

            auditRepo.Add(a);

            Audits.Clear();

            UpdateList();
        }

        public void UpdateAudit()
        {
            AuditViewModel avm = SelectedAudit;
            string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
            avm.Amount = temp;

            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            a.Amount = decimal.Parse(SelectedAudit.Amount);
            a.Year = SelectedAudit.Year;
            a.Description = SelectedAudit.Description;

            auditRepo.Update(a);

            Audits.Clear();

            UpdateList();

            SelectedAudit = avm;
        }

        public void DeleteAudit()
        {
            AuditViewModel avm = SelectedAudit;

            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            auditRepo.Remove(a);

            Audits.Clear();

            UpdateList();
        }

        public void UpdateResource()
        {
            ResourceViewModel resourceVm = SelectedResource;
            AuditViewModel auditVm = SelectedAudit;
            WorktimeViewModel worktimeVm = SelectedWorktime;

            string temp = string.Format("{0:#,0}", double.Parse(resourceVm.StartAmount));
            resourceVm.StartAmount = temp;
            temp = string.Format("{0:#,0}", double.Parse(resourceVm.ExpectedYearlyCost));
            resourceVm.ExpectedYearlyCost = temp;


            Resource r = resourceRepo.GetById(x => x.Id == SelectedResource.GetId());

            r.StartAmount = decimal.Parse(SelectedResource.StartAmount);
            r.ExpectedYearlyCost = decimal.Parse(SelectedResource.ExpectedYearlyCost);

            resourceRepo.Update(r);

            UpdateList();

            SelectedResource = resourceVm;
            if (auditVm is not null)
            {
                SelectedAudit = auditVm;
            }
            if (WorktimeDetailsVisibility == WindowVisibility.Visible)
            {
                if (worktimeVm is not null)
                {
                    SelectedWorktime = worktimeVm;
                }
            }
        }
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

                 ervm.UpdateResource();

                 ervm.EditResourceVisibility = WindowVisibility.Hidden;
                 ervm.ResourceVisiblity = WindowVisibility.Visible;
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
                double temp = SelectedResource.StartAmount.IsNullOrEmpty() ? 0 : double.Parse(SelectedResource.StartAmount);
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