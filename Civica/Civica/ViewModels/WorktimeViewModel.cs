using Civica.Models;
using GVMR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class WorktimeViewModel : ObservableObject
    {
        private Worktime worktime;

        private string _estimatedHours = "";
        public string EstimatedHours
        {
            get => _estimatedHours;
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _estimatedHours = value;
                }
                else
                {
                    MessageBox.Show("'Estimeret arbejds timer' må kun være i tal");
                }
                OnPropertyChanged(nameof(EstimatedHours));
                OnPropertyChanged(nameof(RemainingHours));
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

        private string _spentHours = "";
        public string SpentHours
        {
            get
                => _spentHours; 
            set
            {
                if (int.TryParse(value, out _) || value == "")
                {
                    _spentHours = value;
                }
                else
                {
                    MessageBox.Show("'Brugte timer' må kun være i tal");
                }
                OnPropertyChanged(nameof(SpentHours));
                OnPropertyChanged(nameof(RemainingHours));
                ChangeColor();
            }
           
        }
        public double RemainingHours => int.Parse(EstimatedHours) - int.Parse(SpentHours);

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
            EstimatedHours = worktime.EstimatedHours.ToString();
            InvolvedName = worktime.InvolvedName;
            Description = worktime.Description;
            SpentHours = worktime.SpentHours.ToString();
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
