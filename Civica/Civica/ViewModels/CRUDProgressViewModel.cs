using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using GVMR;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Civica.ViewModels
{
    public class CRUDProgressViewModel : ObservableObject
    {
        private IRepository<Progress> progressRepo;

        private string _description = "";
        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private Phase _phase;
        public Phase Phase
        {
            get { return _phase; }
            set
            {
                _phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }

        private Status _status;

        public Status Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public void CreateProgress(int userId, int projectId, Phase phase, Status status, string description)
        {
            Progress prog = new Progress(userId, projectId, phase, status, description, DateTime.Now);

            progressRepo.Add(prog);
            ExpandedProjectViewModel.Instance.SelectedProject.SetColor(prog.Status);
            Phase = Phase.IDENTIFIED;
            Status = Status.NONE;
            Description = "";
        }

        public void UpdateProgress(ProgressViewModel pvm)
        {
            Progress p = progressRepo.GetById(x => x.Id == pvm.GetId());
            p.Phase = Helper.Phases.FirstOrDefault(x => x.Value == pvm.Phase).Key;
            p.Status = Helper.Statuses.FirstOrDefault(x => x.Value == pvm.Status).Key;
            p.Description = pvm.Description;
            progressRepo.Update(p);

            ExpandedProjectViewModel.Instance.UpdateList();

            ExpandedProjectViewModel.Instance.SelectedProgress = ExpandedProjectViewModel.Instance.Progresses.FirstOrDefault(x => x.GetId() == p.Id);
            ExpandedProjectViewModel.Instance.SelectedProject.SetColor(p.Status);
        }

        public void DeleteProgress(ProgressViewModel pvm)
        {
            progressRepo.Delete(progressRepo.GetById(x => x.Id == pvm.GetId()));
            ExpandedProjectViewModel.Instance.UpdateList();
        }

        public RelayCommand CreateProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CRUDProgressViewModel cpvm)
                {
                    cpvm.CreateProgress(MainViewModel.Instance.CurrentUser.GetId(), ExpandedProjectViewModel.Instance.SelectedProject.GetId(), cpvm.Phase, cpvm.Status, cpvm.Description);

                    ExpandedProjectViewModel.Instance.UpdateList();

                    ExpandedProjectViewModel.Instance.ProgressVisibility = WindowVisibility.Hidden;
                    ExpandedProjectViewModel.Instance.UpdateProgressVisibility = WindowVisibility.Hidden;
                    ExpandedProjectViewModel.Instance.ProgressVisibility = WindowVisibility.Hidden;
                    ExpandedProjectViewModel.Instance.InformationVisibility = WindowVisibility.Hidden;
                    ExpandedProjectViewModel.Instance.InformationPlaceholderVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CRUDProgressViewModel cpvm)
                {
                    succes = true;
                }
                return succes;
            }
        );

        //Singleton
        private CRUDProgressViewModel() {
            progressRepo = MainViewModel.Instance.GetProgressRepo();
        }

        private static readonly object _lock = new object();
        private static CRUDProgressViewModel _instance;

        public static CRUDProgressViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new CRUDProgressViewModel();
                        }
                    }
                }
                return _instance;
            }
        }

        //Singleton - Lazy
        //private static readonly Lazy<CRUDProgressViewModel> lazy = new Lazy<CRUDProgressViewModel>(() => new CRUDProgressViewModel());

        //public static CRUDProgressViewModel Instance => lazy.Value;
    }
}
