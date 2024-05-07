using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Progress : DomainModel
    {
        public Phase Phase { get; set; }
        public Status Status { get; set; }
        public string Description { get; set; }

        public Progress(int userId, int projectId, Phase fase, Status status, string description, DateTime createdDate)
        {
            UserId = userId;
            RefId = projectId;
            Phase = fase;
            Status = status;
            Description = description;
            CreatedDate = createdDate;
        }
    }
}
