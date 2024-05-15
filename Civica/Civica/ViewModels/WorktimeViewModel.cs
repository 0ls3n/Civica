using Civica.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class WorktimeViewModel : ObservableObject
    {
        private Worktime worktime;

        private int _estimatedHours;
        public int EstimatedHours
        {
            get =>
                _estimatedHours;
            set
            {
                _estimatedHours = value;
                OnPropertyChanged(nameof(EstimatedHours));
                OnPropertyChanged(nameof(RemainingHours));

                ChangeColor();
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

        private int _spentHours;
        public int SpentHours
        {
            get
                => _spentHours; set
            {
                _spentHours = value;
                OnPropertyChanged(nameof(SpentHours));
                OnPropertyChanged(nameof(RemainingHours));

                ChangeColor();
            }
        }
        public double RemainingHours => EstimatedHours - SpentHours;

        private string _color;
        public string Color
        {
            get
            {
                return _color;
            }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
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

        public void ChangeColor()
        {
            if (RemainingHours > 0)
            {
                Color = "#FF000000";
            }
            else if (RemainingHours == 0)
            {
                Color = "#FF64DA21";
            }
            else if (RemainingHours < 0)
            {
                Color = "#E20F1A";
            }
        }
    }
}
