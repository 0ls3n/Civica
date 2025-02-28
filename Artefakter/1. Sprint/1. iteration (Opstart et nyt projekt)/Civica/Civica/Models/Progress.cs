﻿using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Progress
    {
        public int Id { get; set; }
        public Phase Phase { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
        public int ProjectId { get; set; }

        public Progress(int projectId, Phase fase, Status status, DateTime date, string description)
        {
            ProjectId = projectId;
            Phase = fase;
            Status = status;
            Date = date;
            Description = description;
        }
    }
}
