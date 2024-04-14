using Civica.Models;
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
    public class UpdateProjectCmd : ICommand
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

        private MainViewModel mvm;

        public UpdateProjectCmd(MainViewModel mvm)
        {
            this.mvm = mvm;
        }

        public bool CanExecute(object? parameter)
        {
            bool succes = false;

            if (parameter is UpdateProjectViewModel upvm)
            {
                if (!string.IsNullOrEmpty(upvm.ProjectName))
                {
                    succes = true;
                }
            }
            return succes;
        }

        public void Execute(object? parameter)
        {
            if (parameter is UpdateProjectViewModel upvm)
            {
                mvm.UpdateProject(mvm.SelectedProject, upvm.ProjectName, upvm.Owner, upvm.Manager, upvm.Description);
                MessageBox.Show($"Projekt information for '{upvm.ProjectName}' opdateret!\nTryk for at afslutte.");
            }
        }
    }
}
