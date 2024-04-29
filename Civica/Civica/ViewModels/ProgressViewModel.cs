using Civica.Models;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.ViewModels
{
    public class ProgressViewModel
    {
        private Progress progress;
        public string Phase { get; set; }
        public string Status { get; set; }
        public string Date { get; set; }
        public string Description { get; set; }

        public ProgressViewModel(Progress progress)
        {
            this.progress = progress;
            Phase = Helper.Phases[progress.Phase];
            Status = Helper.Statuses[progress.Status];
            Date = "d. " + progress.Date.ToString("dd. MMMM yyyy kl. HH:mm");
            Description = progress.Description;
        }

        public int GetId()
        {
            return progress.Id;
        }

        public int GetProjectId()
        {
            return progress.ProjectId;
        }
    }
}
