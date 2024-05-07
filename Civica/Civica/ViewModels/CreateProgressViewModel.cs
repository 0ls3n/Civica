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
using System.Windows;

namespace Civica.ViewModels
{
    public class CreateProgressViewModel : ObservableObject, IViewModelChild
    {
        private InProgressViewModel ipvm;

        private IRepository<Progress> progressRepo;

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

        private Phase _selectedPhase;
        public Phase SelectedPhase
        {
            get { return _selectedPhase; }
            set
            {
                _selectedPhase = value;
                OnPropertyChanged(nameof(SelectedPhase));
            }
        }

        private Status _selectedStatus;

        public Status SelectedStatus
        {
            get { return _selectedStatus; }
            set
            {
                _selectedStatus = value;
                OnPropertyChanged(nameof(SelectedStatus));
            }
        }


        public void Init(ObservableObject o)
        {
            this.ipvm = (o as InProgressViewModel);
        }

        public void SetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void CreateProgress()
        {
            Progress prog = new Progress(ipvm.GetCurrentUser().GetId(), ipvm.SelectedProject.GetId(), SelectedPhase, SelectedStatus, ProgressDescription, DateTime.Now);

            progressRepo.Add(prog);
        }

        public RelayCommand ProgressProjectCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CreateProgressViewModel cpvm)
                {
                    cpvm.CreateProgress();

                    cpvm.ipvm.UpdateList();

                    cpvm.ipvm.ProgressVisibility = WindowVisibility.Hidden;
                    cpvm.ipvm.InformationVisibility = WindowVisibility.Visible;
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
