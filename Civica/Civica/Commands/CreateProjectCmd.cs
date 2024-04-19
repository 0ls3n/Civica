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

        public bool CanExecute(object? parameter)
        {
            bool succes = false;

            if (parameter is CreateProjectViewModel CPVM)
            {
                if (!string.IsNullOrEmpty(CPVM.ProjectName))
                {
                    succes = true;
                }
            }
            return succes;
        }

        public void Execute(object? parameter)
        {

            if (parameter is CreateProjectViewModel CPVM)
            {
                CPVM.CreateProject(CPVM.ProjectName, CPVM.ProjectOwner, CPVM.ProjectManager, CPVM.ProjectDescription);
                MessageBox.Show("Projekt er oprettet");
            }
        }
    }
}
