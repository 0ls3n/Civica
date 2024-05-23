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

namespace Civica.ViewModels
{
    public class CRUDProgressViewModel : ObservableObject, IViewModelChild
    {
        private ExpandedProjectViewModel epvm;

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

        public void Init(ObservableObject o)
        {
            epvm = (o as ExpandedProjectViewModel);

            progressRepo = epvm.mvm.GetProgressRepo();
        }

        public void Create()
        {
            Progress prog = new Progress(epvm.mvm.CurrentUser.GetId(), epvm.SelectedProject.GetId(), Phase, Status, Description, DateTime.Now);

            progressRepo.Add(prog);
            epvm.SelectedProject.SetColor(prog.Status);
            Phase = Phase.IDENTIFIED;
            Status = Status.NONE;
            Description = "";
        }

        public void Update()
        {
            Progress p = progressRepo.GetById(x => x.Id == epvm.SelectedProgress.GetId());
            p.Phase = Helper.Phases.FirstOrDefault(x => x.Value == epvm.SelectedProgress.Phase).Key;
            p.Status = Helper.Statuses.FirstOrDefault(x => x.Value == epvm.SelectedProgress.Status).Key;
            p.Description = epvm.SelectedProgress.Description;
            progressRepo.Update(p);

            epvm.UpdateList();

            epvm.SelectedProgress = epvm.Progresses.FirstOrDefault(x => x.GetId() == p.Id);
            epvm.SelectedProject.SetColor(p.Status);
        }

        public void Delete()
        {
            progressRepo.Delete(progressRepo.GetById(x => x.Id == epvm.SelectedProgress.GetId()));
            epvm.UpdateList();
        }

        public RelayCommand CreateProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CRUDProgressViewModel cpvm)
                {
                    cpvm.Create();

                    cpvm.epvm.UpdateList();

                    cpvm.epvm.ProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.UpdateProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.ProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.InformationVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
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
    }
}
