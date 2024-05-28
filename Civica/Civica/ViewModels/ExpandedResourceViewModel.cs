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
    public class ExpandedResourceViewModel : ObservableObject
    {
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
                AuditDetailsVisibility = WindowVisibility.Visible;
                UpdateAuditVisibility = WindowVisibility.Hidden;
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
                UpdateWorktimeVisibility = WindowVisibility.Hidden;
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
        private WindowVisibility _updateResourceVisibility;
        public WindowVisibility UpdateResourceVisibility
        {
            get => _updateResourceVisibility;
            set
            {
                _updateResourceVisibility = value;
                OnPropertyChanged(nameof(UpdateResourceVisibility));
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
        public WindowVisibility UpdateAuditVisibility
        {
            get => _editAuditVisibility;
            set
            {
                _editAuditVisibility = value;
                OnPropertyChanged(nameof(UpdateAuditVisibility));
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
        public WindowVisibility UpdateWorktimeVisibility
        {
            get => _editWorktimeVisibility;
            set
            {
                _editWorktimeVisibility = value;
                OnPropertyChanged(nameof(UpdateWorktimeVisibility));
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
                            ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                            ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
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
                            ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
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
        public RelayCommand UpdateViewCmd { get; set; } = new RelayCommand(
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.AuditListVisibility == WindowVisibility.Visible)
                    {
                        ervm.UpdateAuditVisibility = WindowVisibility.Visible;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                    }
                    else
                    {
                        ervm.UpdateWorktimeVisibility = WindowVisibility.Visible;
                        ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                        ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                    }
                }
            },
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand UpdateCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.AuditListVisibility == WindowVisibility.Visible)
                 {
                     ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     CRUDAuditViewModel.Instance.UpdateAudit(ervm.SelectedAudit);
                 }
                 else
                 {
                     ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
                     ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                     CRUDWorktimeViewModel.Instance.UpdateWorktime(ervm.SelectedWorktime);
                 }
             }
         },
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {
                 if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
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
                        ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                        ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateAuditVisibility = WindowVisibility.Visible;

                        CRUDAuditViewModel.Instance.Amount = "";
                        CRUDAuditViewModel.Instance.Year = DateTime.Now.Year;
                    }
                    else
                    {
                        ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                        ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
                        ervm.InformationPlaceholderVisibility = WindowVisibility.Hidden;
                        ervm.CreateWorktimeVisibility = WindowVisibility.Visible;

                        CRUDWorktimeViewModel.Instance.Description = "";
                        CRUDWorktimeViewModel.Instance.Description = "";
                        CRUDWorktimeViewModel.Instance.EstimatedHours = "";
                        CRUDWorktimeViewModel.Instance.InvolvedName = "";
                    }
                }
            },
            parameter =>
            {
                if (parameter is ExpandedResourceViewModel ervm)
                {
                    if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                    {
                        return true;
                    }
                }
                return false;
            });

        public RelayCommand DeleteCmd { get; set; } = new RelayCommand(
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
                          CRUDAuditViewModel.Instance.DeleteAudit(ervm.SelectedAudit);

                          ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                          ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                          ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                          ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                      }
                      else
                      {
                          CRUDWorktimeViewModel.Instance.DeleteWorktime(ervm.SelectedWorktime);

                          ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
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
                  if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
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
                      ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                      ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                      ervm.CreateAuditVisibility = WindowVisibility.Hidden;

                      CRUDAuditViewModel.Instance.Amount = "";
                      CRUDAuditViewModel.Instance.Year = DateTime.Now.Year;
                  }
                  else
                  {
                      ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
                      ervm.WorktimeDetailsVisibility = WindowVisibility.Hidden;
                      ervm.CreateWorktimeVisibility = WindowVisibility.Hidden;
                      ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;

                      CRUDWorktimeViewModel.Instance.Description = "";
                      CRUDWorktimeViewModel.Instance.Description = "";
                      CRUDWorktimeViewModel.Instance.EstimatedHours = "";
                      CRUDWorktimeViewModel.Instance.InvolvedName = "";
                  }
              }
          },
          parameter =>
          {
              if (parameter is ExpandedResourceViewModel ervm)
              {
                  if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
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
                     CRUDAuditViewModel.Instance.CreateAudit(MainViewModel.Instance.CurrentUser.GetId(), ervm.SelectedResource.GetId(), CRUDAuditViewModel.Instance.Amount, CRUDAuditViewModel.Instance.Year, CRUDAuditViewModel.Instance.Description);

                     ervm.UpdateAuditVisibility = WindowVisibility.Hidden;
                     ervm.AuditDetailsVisibility = WindowVisibility.Hidden;
                     ervm.CreateAuditVisibility = WindowVisibility.Hidden;
                     ervm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                 }
                 else
                 {
                     if (string.IsNullOrEmpty(CRUDWorktimeViewModel.Instance.InvolvedName))
                     {
                         MessageBox.Show("Der skal indtastes en afdeling eller et navn.");
                         return;
                     }
                     CRUDWorktimeViewModel.Instance.CreateWorktime(MainViewModel.Instance.CurrentUser.GetId(), ervm.SelectedResource.GetId(), CRUDWorktimeViewModel.Instance.EstimatedHours, CRUDWorktimeViewModel.Instance.InvolvedName, CRUDWorktimeViewModel.Instance.Description, DateTime.Now);

                     ervm.UpdateWorktimeVisibility = WindowVisibility.Hidden;
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
                 if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                 {
                     return true;
                 }
             }
             return false;
         });
        #endregion

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

        #region Resource Commands

        public RelayCommand UpdateResourceViewCmd { get; set; } = new RelayCommand(
           parameter =>
           {
               if (parameter is ExpandedResourceViewModel ervm)
               {
                   ervm.UpdateResourceVisibility = WindowVisibility.Visible;
                   ervm.ResourceVisiblity = WindowVisibility.Hidden;
               }
           },
           parameter =>
           {
               if (parameter is ExpandedResourceViewModel ervm)
               {
                   if (ervm.SelectedProject != null && MainViewModel.Instance.CurrentUser != null)
                   {
                       return true;
                   }
               }
               return false;
           });

        public RelayCommand UpdateResourceCmd { get; set; } = new RelayCommand(
         parameter =>
         {
             if (parameter is ExpandedResourceViewModel ervm)
             {

                 ervm.UpdateResource();

                 ervm.UpdateResourceVisibility = WindowVisibility.Hidden;
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

        //Singleton
        private ExpandedResourceViewModel() {
            auditRepo = MainViewModel.Instance.GetAuditRepo();
            resourceRepo = MainViewModel.Instance.GetResourceRepo();
            worktimeRepo = MainViewModel.Instance.GetWorktimeRepo();

            ResourceVisiblity = WindowVisibility.Visible;
            AuditDetailsVisibility = WindowVisibility.Hidden;
            UpdateAuditVisibility = WindowVisibility.Hidden;
            UpdateResourceVisibility = WindowVisibility.Hidden;
            CreateAuditVisibility = WindowVisibility.Hidden;
        }

        private static readonly object _lock = new object();
        private static ExpandedResourceViewModel _instance;

        public static ExpandedResourceViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new ExpandedResourceViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        //Singleton - Lazy
        //private static readonly Lazy<ExpandedResourceViewModel> lazy = new Lazy<ExpandedResourceViewModel>(() => new ExpandedResourceViewModel());

        //public static ExpandedResourceViewModel Instance => lazy.Value;
    }
}