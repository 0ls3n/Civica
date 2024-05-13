using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Worktime : DomainModel
    {
        public double EstimatedHours { get; set; }
        public string InvolvedName { get; set; }
        public string Description { get; set; }
        public double SpentHours { get; set; } = 0;
        
        public Worktime(int userId, int resourceId, double estimatedHours, string involvedName, string desc, DateTime createdDate)
        {
            EstimatedHours = estimatedHours;
            InvolvedName = involvedName;
            UserId = userId;
            RefId = resourceId;
            CreatedDate = createdDate;
            Description = desc;
        }
    }
}
