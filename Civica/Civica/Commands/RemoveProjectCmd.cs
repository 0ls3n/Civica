using Civica.Models;
using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Civica.Commands
{
    public class RemoveProjectCmd : ICommand
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

            if (parameter is InProgressViewModel IPVM)
            {
                if (IPVM.SelectedProject != null)
                {
                    succes = true;
                }
            }

            return succes;
        }

        public void Execute(object? parameter)
        {
            if (parameter is InProgressViewModel IPVM)
            {
                IPVM.RemoveProject();
                IPVM.InformationVisivility = "Visible";
            }
        }
    }
}
