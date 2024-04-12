using Civica.Models;
using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //if (parameter is MainViewModel mvm)
            //{
            //    if (!projectRepo.Get(mvm.Selected.Id) is not null) 
            //    {
            //        succes = true;
            //    }
            //}
            return succes;
        }

        public void Execute(object? parameter)
        {
            //if (parameter is MainViewModel mvm)
            //{
            //    if (projectRepo.GetAll().Find(x => x.Name.ToLower() == mvm.SelectedProject.Name.ToLower()) is not null)
            //    {
            //        mvm.RemoveProject(mvm.SelectedProject);
            //        MessageBox.Show($"Projekt '{mvm.SelectedProject.Name}' slettet!\nTryk for at afslutte.");
            //    }
            //    else
            //    {
            //        MessageBox.Show($"Projekt '{mvm.SelectedProject.Name}' findes ikke!\nTryk for at afslutte.");
            //    }
            //}
        }
    }
}
