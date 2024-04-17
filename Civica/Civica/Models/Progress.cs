using Civica.Models.Enums;
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
        public Fase Fase { get; set; }
        public Status Status { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public Progress(Fase fase, Status status, string description)
        {
            Fase = fase;
            Status = status;
            Date = DateTime.Now;
            Description = description;
        }
    }
}
