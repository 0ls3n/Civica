using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Audit
    {
        public decimal Amount { get; set; }
        public DateTime Year { get; set; }
        public int Id { get; set; }

        public int EconomyId { get; set; }

        public Audit(decimal amount, DateTime year)
        {
            this.Amount = amount;
            this.Year = year;
        }
    }
}
