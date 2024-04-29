using Civica.Commands;
using Civica.Interfaces;
using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class CreateProgressViewModel : ObservableObject, IViewModelChild
    {
        private InProgressViewModel ipvm;

        private ProgressRepository progressRepo;
        public Dictionary<Phase, string> PhaseDict { get; set; } = new Dictionary<Phase, string>
        {
            { Phase.IDENTIFIED, "Identificeret" },
            { Phase.PLANNING, "Planlægning" },
            { Phase.IMPLEMENTATION, "Gennemførsel" },
            { Phase.OPERATION, "Drift" },
            { Phase.FOLLOW_UP, "Opfølgning" },
            { Phase.DONE, "Afsluttet" }
        };

        public Dictionary<Status, string> StatusDict { get; set; } = new Dictionary<Status, string>
        {
            { Status.NONE, "Ingen vurdering" },
            { Status.CRITICAL, "Kritisk" },
            { Status.DELAYED, "Forsinket" },
            { Status.ON_TRACK, "Planmæssigt" }
        };
        public ObservableCollection<string> Phases { get; set; } = new ObservableCollection<string> { "Identificeret", "Planlægning", "Gennemførsel", "Drift", "Opfølgning", "Afsluttet" };
        public ObservableCollection<string> Statuses { get; set; } = new ObservableCollection<string> { "Ingen vurdering", "Kritisk", "Forsinket", "Planmæssigt" };

        private string _progressDescription = "";
        public string ProgressDescription
        {
            get
            {
                return _progressDescription;
            }
            set
            {
                _progressDescription = value;
                OnPropertyChanged(nameof(ProgressDescription));
            }
        }

        private string _selectedPhase;
        public string SelectedPhase
        {
            get { return _selectedPhase; }
            set
            {
                _selectedPhase = value;
                OnPropertyChanged(nameof(SelectedPhase));
                switch (value)
                {
                    case "Identificeret":
                        EnumPhase = Phase.IDENTIFIED;
                        break;
                    case "Planlægning":
                        EnumPhase = Phase.PLANNING;
                        break;
                    case "Gennemførsel":
                        EnumPhase = Phase.IMPLEMENTATION;
                        break;
                    case "Drift":
                        EnumPhase = Phase.OPERATION;
                        break;
                    case "Opfølgning":
                        EnumPhase = Phase.FOLLOW_UP;
                        break;
                    case "Afsluttet":
                        EnumPhase = Phase.DONE;
                        break;
                }
            }
        }

        private Phase _enumPhase;
        public Phase EnumPhase
        {
            get
            { return _enumPhase; }
            set
            {
                _enumPhase = value;
                OnPropertyChanged(nameof(EnumPhase));
            }
        }
        private string _selectedStatus;

        public string SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
                switch (value)

                {
                    case "Ingen Vurdering":
                        EnumStatus = Status.NONE;
                        break;
                    case "Kritisk":
                        EnumStatus = Status.CRITICAL;
                        break;
                    case "Forsinket":
                        EnumStatus = Status.DELAYED;
                        break;
                    case "Planmæssigt":
                        EnumStatus = Status.ON_TRACK;
                        break;
                }
            }
        }

        private Status _enumStatus;
        public Status EnumStatus
        {
            get
            {
                return _enumStatus;
            }
            set
            {
                _enumStatus = value;
                OnPropertyChanged(nameof(EnumStatus));
            }
        }

        public void Init(ObservableObject o)
        {
            this.ipvm = (o as InProgressViewModel);
        }

        public void GetRepo(ProgressRepository progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void CreateProgress(Phase phase, Status status, string description)
        {
            Progress prog = new Progress(ipvm.SelectedProject.GetId(), phase, status, DateTime.Now, description);

            progressRepo.Add(prog);

            //ipvm.SelectedProgresses.Add(new ProgressViewModel(prog));
        }



        public RelayCommand ProgressProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CreateProgressViewModel cpvm)
                {
                    cpvm.CreateProgress(cpvm.EnumPhase, cpvm.EnumStatus, cpvm.ProgressDescription);

                    cpvm.ipvm.UpdateList();

                    cpvm.ipvm.ProgressVisibility = "Hidden";
                    cpvm.ipvm.InformationVisibility = "Visible";
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CreateProgressViewModel cpvm)
                {
                    if (cpvm.ipvm.SelectedProject is not null)
                    {
                        if (!string.IsNullOrEmpty(cpvm.ProgressDescription))
                        {
                            succes = true;
                        }
                    }
                }
                return succes;
            }
        );
    }
}
