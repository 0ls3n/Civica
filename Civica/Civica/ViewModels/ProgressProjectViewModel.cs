using Civica.Commands;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.ViewModels
{
    public class ProgressProjectViewModel : INotifyPropertyChanged
    {
        public ICommand ProgressProjectCmd { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private Phase _phase;
        public Phase Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                OnPropertyChanged(nameof(Phase));
            }
        }
        private Status _status;
        public Status Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        private string _description;
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ProgressProjectViewModel(MainViewModel mvm)
        {
            ProgressProjectCmd = new ProgressProjectCmd(mvm);
        }
    }
}
