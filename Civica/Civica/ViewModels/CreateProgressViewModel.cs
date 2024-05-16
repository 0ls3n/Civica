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
    public class CreateProgressViewModel : ObservableObject, IViewModelChild
    {
        private ExpandedProjectViewModel epvm;

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
            epvm = (o as ExpandedProjectViewModel);
        }
        public void SetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void CreateProgress()
        {
            Progress prog = new Progress(epvm.GetCurrentUser().GetId(), epvm.SelectedProject.GetId(), SelectedPhase, SelectedStatus, ProgressDescription, DateTime.Now);

            progressRepo.Add(prog);
        }

        public RelayCommand CreateProgressCmd { get; set; } = new RelayCommand
        (
            parameter =>
            {
                if (parameter is CreateProgressViewModel cpvm)
                {
                    cpvm.CreateProgress();

                    cpvm.epvm.UpdateList();

                    cpvm.epvm.ProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.EditProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.ProgressVisibility = WindowVisibility.Hidden;
                    cpvm.epvm.InformationPlaceholderVisibility = WindowVisibility.Visible;
                }
            },
            parameter =>
            {
                bool succes = false;

                if (parameter is CreateProgressViewModel cpvm)
                {
                    succes = true;
                }
                return succes;
            }
        );
    }
}
