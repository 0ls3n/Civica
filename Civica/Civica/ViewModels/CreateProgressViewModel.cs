﻿using Civica.Commands;
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
        }
        public void SetRepo(IRepository<Progress> progressRepo)
        {
            this.progressRepo = progressRepo;
        }

        public void CreateProgress()
        {
            Progress prog = new Progress(epvm.GetCurrentUser().GetId(), epvm.SelectedProject.GetId(), Phase, Status, Description, DateTime.Now);

            progressRepo.Add(prog);
            epvm.SelectedProject.SetColor(prog.Status);
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
                    cpvm.epvm.InformationVisibility = WindowVisibility.Hidden;
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
