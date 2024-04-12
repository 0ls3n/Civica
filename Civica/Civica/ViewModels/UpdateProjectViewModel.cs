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
        private MainViewModel mvm;
        private ProjectViewModel selectedProject;

        public ProjectViewModel SelectedProject
        {
            get { return selectedProject; }
            set { selectedProject = value; OnPropertyChanged(nameof(SelectedProject)); }
        }
        public ICommand UpdateProjectCmd { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public UpdateProjectViewModel(MainViewModel mvm)
        {
            this.mvm = mvm;
            UpdateProjectCmd = new UpdateProjectCmd(mvm);
        }
        public void UpdateProject(string name, string owner, string leader, string description)
        {

        }
    }
}
