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
    public class CreateProjectCmd : ICommand
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
                if (!string.IsNullOrEmpty(ipvm.ProjectName))
                {
                    succes = true;
                }
            }
            return succes;
        }

        public void Execute(object? parameter)
        {

            if (parameter is InProgressViewModel ipvm)
            {
                ipvm.CreateProject();
                ipvm.CreateVisibility = "Hidden";
                ipvm.InformationVisibility = "Visible";
            }
        }
    }
}
