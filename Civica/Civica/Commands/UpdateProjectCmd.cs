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

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is InProgressViewModel ipvm)
            {
                if (ipvm.Projects.FirstOrDefault(x => x.Name.ToLower() == ipvm.ProjectName.ToLower()) is null || ipvm.ProjectName.ToLower() == ipvm.OldName.ToLower())
                {
                    ipvm.UpdateProject(ipvm.SelectedProject);

                    ipvm.EditVisibility = "Hidden";
                    ipvm.InformationVisibility = "Visible";
                }
            }
        }
    }
}
