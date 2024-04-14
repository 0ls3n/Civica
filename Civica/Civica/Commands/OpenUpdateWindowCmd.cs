using Civica.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.Commands
{
    public class OpenUpdateWindowCmd : ICommand
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
        private Predicate<object> canExecute;
        public OpenUpdateWindowCmd(Predicate<object> canExecute)
        {
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return this.canExecute !=null && this.canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
        }
    }
}
