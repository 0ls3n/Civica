using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
   public class AuditViewModel
    {
        private Audit audit;
        public decimal Amount { get; set; }
        public int Year { get; set; }


        public AuditViewModel(Audit a)
        {
            Amount = a.Amount;
            Year = a.Year;
            audit = a;
        }

        public int GetId ()
        {
            return audit.Id;
        }
    }
}
