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
        public Phase Phase { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public int ProjectId { get; set; }

        public ProgressViewModel(Progress progress)
        {
            this.progress = progress;
            Phase = progress.Phase;
            Status = progress.Status;
            Date = progress.Date;
            Description = progress.Description;
            ProjectId = progress.ProjectId;
        }

        public int GetId()
        {
            return progress.Id;
        }
    }
}
