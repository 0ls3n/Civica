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
        
        public WorkTime(double time, string involvedName)
        {
            this.Time = time;
            this.InvolvedName = involvedName;
        }
    }
}
