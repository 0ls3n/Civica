using Civica.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Civica.ViewModels
{
    public class UpdateProjectViewModel : INotifyPropertyChanged
    {
        public ICommand UpdateProjectCmd { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public string OldName;

        private string _projectName;
        public string ProjectName
        {
            get => _projectName;
            set
            {
                _projectName = value;
                OnPropertyChanged(nameof(ProjectName));
            }
        }
        private string _owner;
        public string Owner
        {
            get => _owner;
            set
            {
                _owner = value;
                OnPropertyChanged(nameof(Owner));
            }
        }
        private string _manager;
        public string Manager
        {
            get => _manager;
            set
            {
                _manager = value;
                OnPropertyChanged(nameof(Manager));
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

        public UpdateProjectViewModel(MainViewModel mvm)
        {
            UpdateProjectCmd = new UpdateProjectCmd(mvm);
            OldName = mvm.SelectedProject.Name;
            ProjectName = mvm.SelectedProject.Name;
            Owner = mvm.SelectedProject.Owner;
            Manager = mvm.SelectedProject.Manager;
            Description = mvm.SelectedProject.Description;
        }
    }
}
