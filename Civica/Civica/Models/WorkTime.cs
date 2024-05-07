using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class WorkTime : DomainModel
    {
        public double Time { get; set; }
        public string InvolvedName { get; set; }
        
        public WorkTime(int userId, int resourceId, double time, string involvedName, DateTime createdDate)
        {
            Time = time;
            InvolvedName = involvedName;
            UserId = userId;
            RefId = resourceId;
            CreatedDate = createdDate;
        }

    }
}
