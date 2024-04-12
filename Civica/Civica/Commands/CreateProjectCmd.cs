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
    public class CreateProjectCmd : ICommand
    {
        public event EventHandler? CanExecuteChanged
        {
            add {
                CommandManager.RequerySuggested += value;
            }
            remove {
                CommandManager.RequerySuggested -= value;
            }
        }

        private MainViewModel mvm;

        public CreateProjectCmd(MainViewModel mvm)
        {
            this.mvm = mvm;
        }

        public bool CanExecute(object? parameter)
        {
            bool succes = false;

            if (parameter is CreateProjectViewModel mvm)
            {
                if (!string.IsNullOrEmpty(mvm.ProjectName))
                {
                    succes = true;
                }
            }
            return succes;
        }

        public void Execute(object? parameter)
        {
            if (parameter is CreateProjectViewModel cpvm)
            {
                if (mvm.Projects.FirstOrDefault(x => x.Name.ToLower() == cpvm.ProjectName.ToLower()) is null)
                {
                    mvm.CreateNewProject(cpvm.ProjectName, cpvm.Owner, cpvm.Manager, cpvm.Description);
                    MessageBox.Show($"Projekt '{cpvm.ProjectName}' oprettet!\nTryk for at afslutte.");
                }
                else
                {
                    MessageBox.Show($"Projekt '{cpvm.ProjectName}' findes allerede!\nTryk for at afslutte.");
                }
            }
        }
    }
}
