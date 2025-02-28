﻿using Civica.Models;
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
        public string Date { get; set; }
        public string Description { get; set; }

        public ProgressViewModel(Progress progress)
        {
            this.progress = progress;
            Phase = progress.Phase;
            Status = progress.Status;
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
