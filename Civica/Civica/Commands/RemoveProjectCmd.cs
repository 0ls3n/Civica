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

            if (parameter is InProgressViewModel ipvm)
            {
                if (ipvm.SelectedProject != null)
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
                MessageBoxButton button = MessageBoxButton.OKCancel;
                MessageBoxResult result = MessageBox.Show($"Er du sikker på du vil slette '{ipvm.SelectedProject.Name}'?", "Bekræft sletning", button);

                if (result == MessageBoxResult.OK)
                {
                    MessageBox.Show($"'{ipvm.SelectedProject.Name}' slettet.");
                    ipvm.RemoveProject();
                    ipvm.InformationVisibility = "Visible";
                }
                else
                {
                    MessageBox.Show($"'{ipvm.SelectedProject.Name}' blev ikke slettet.");
                }
            }
        }
    }
}
