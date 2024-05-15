using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Worktime : DomainModel
    {
        public int EstimatedHours { get; set; }
        public string InvolvedName { get; set; }
        public string Description { get; set; }
        public int SpentHours { get; set; } = 0;

        public Worktime(int userId, int resourceId, int estimatedHours, string involvedName, string desc, DateTime createdDate)
        {
            EstimatedHours = estimatedHours;
            InvolvedName = involvedName;
            UserId = userId;
            RefId = resourceId;
            CreatedDate = createdDate;
            Description = desc;
        }
        public Worktime(int userId, int resourceId, int estimatedHours, int spent, string involvedName, string desc, DateTime createdDate) : this(userId, resourceId, estimatedHours, involvedName, desc, createdDate)
        {
            SpentHours = spent;
        }
    }
}
