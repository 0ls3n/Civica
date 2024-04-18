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
        public Phase Fase { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }

        public int ProjectId { get; set; }

        public ProgressViewModel(Progress progress)
        {
            this.progress = progress;
            Fase = progress.Phase;
            Status = progress.Status;
            Description = progress.Description;
            ProjectId = progress.ProjectId;
        }

        public int GetId()
        {
            return progress.Id;
        }
    }
}
