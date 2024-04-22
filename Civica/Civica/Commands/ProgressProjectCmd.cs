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
    //public class ProgressProjectCmd : ICommand
    //{
    //    public event EventHandler? CanExecuteChanged
    //    {
    //        add
    //        {
    //            CommandManager.RequerySuggested += value;
    //        }
    //        remove
    //        {
    //            CommandManager.RequerySuggested -= value;
    //        }
    //    }

    //    private MainViewModel mvm;

    //    public ProgressProjectCmd(MainViewModel mvm)
    //    {
    //        this.mvm = mvm;
    //    }

    //    public bool CanExecute(object? parameter)
    //    {
    //        bool succes = false;

    //        if (parameter is ProgressProjectViewModel pvm)
    //        {
    //            if (!string.IsNullOrEmpty(pvm.Description))
    //            {
    //                succes = true;
    //            }
    //        }
    //        return succes;
    //    }

    //    public void Execute(object? parameter)
    //    {
    //        if (parameter is ProgressProjectViewModel ppvm)
    //        {
    //            mvm.ProgressProject(ppvm.Phase, ppvm.Status, ppvm.Description);
    //        }
    //    }
    //}
}
