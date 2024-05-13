using Civica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class WorktimeViewModel : ObservableObject
    {
        private Worktime worktime;

        private double _estimatedHours;
        public double EstimatedHours
        {
            get =>
                _estimatedHours;
            set
            {
                _estimatedHours = value;
                OnPropertyChanged(nameof(EstimatedHours));
            }
        }

        private string _involvedName;
        public string InvolvedName
        {
            get
                => _involvedName; set
            {
                _involvedName = value;
                OnPropertyChanged(nameof(InvolvedName));
            }
        }

        private string _description;
        public string Description
        {
            get
                => _description; set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private double _spentHours = 0;
        public double SpentHours
        {
            get
                => _spentHours; set
            {
                _spentHours = value;
                OnPropertyChanged(nameof(SpentHours));
            }
        }

        public WorktimeViewModel(Worktime worktime)
        {
            this.worktime = worktime;
            EstimatedHours = worktime.EstimatedHours;
            InvolvedName = worktime.InvolvedName;
            Description = worktime.Description;
            SpentHours = worktime.SpentHours;
        }

        public int GetId() => worktime.Id;
    }
}
