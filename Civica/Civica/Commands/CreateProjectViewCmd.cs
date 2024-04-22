﻿using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.Commands
{
    public class CreateProjectViewCmd : ICommand
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
                ipvm.CreateVisibility = "Visible";
                ipvm.InformationVisibility = "Hidden";
                ipvm.EditVisibility = "Hidden";
                ipvm.ProgressVisibility = "Hidden";
            }
        }
    }
}
