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

        private ProjectRepository projectRepo;

        public CreateProjectCmd(ProjectRepository projectRepo)
        {
            this.projectRepo = projectRepo;
        }

        public bool CanExecute(object? parameter)
        {
            bool succes = false;

            if (parameter is MainViewModel mvm)
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
            if (parameter is MainViewModel mvm)
            {
                if (projectRepo.GetAll().Find(x => x.Name.ToLower() == mvm.ProjectName.ToLower()) is null)
                {
                    mvm.CreateNewProject(mvm.ProjectName, mvm.Owner, mvm.Manager, mvm.Description);
                    MessageBox.Show($"Projekt '{mvm.ProjectName}' oprettet!\nTryk for at afslutte.");
                }
                else
                {
                    MessageBox.Show($"Projekt '{mvm.ProjectName}' findes allerede!\nTryk for at afslutte.");
                }
            }
        }
    }
}
