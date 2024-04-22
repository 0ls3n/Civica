using Civica.Models;
using Civica.Models.Enums;
using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Civica.Commands
{
    public class ProgressProjectCmd : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add
            {
                CommandManager.RequerySuggested += value;
            }
            remove
            {
                CommandManager.RequerySuggested -= value;
            }
        }

        public bool CanExecute(object? parameter)
        {
            bool succes = false;

            if (parameter is InProgressViewModel ipvm)
            {
                if (ipvm.SelectedProject is not null)
                {
                    if (!string.IsNullOrEmpty(ipvm.ProgressDescription))
                    {
                        succes = true;
                    }
                }
            }
            return succes;
        }

        public void Execute(object? parameter)
        {
            if (parameter is InProgressViewModel ipvm)
            {
                ipvm.Progress(ipvm.Phase, ipvm.Status, ipvm.ProgressDescription);

                ipvm.UpdateList();

                ipvm.ProgressVisibility = "Hidden";
                ipvm.InformationVisibility = "Visible";
            }
        }
    }
}
