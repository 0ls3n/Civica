using Civica.Models.Enums;
using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.Commands
{
    public class ProgressProjectViewCmd : ICommand
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
            if (parameter is InProgressViewModel ipvm)
            {
                if (ipvm.SelectedProject != null)
                {
                    return true;
                }
            }
            return false;
        }

        public void Execute(object? parameter)
        {
            if (parameter is InProgressViewModel ipvm)
            {
                ipvm.ProgressDescription = "";
                ipvm.Phase = Phase.IDENTIFIED;
                ipvm.Status = Status.NONE;

                ipvm.ProgressVisibility = "Visible";
                ipvm.EditVisibility = "Hidden";
                ipvm.InformationVisibility = "Hidden";
                ipvm.CreateVisibility = "Hidden";
            }
        }
    }
}