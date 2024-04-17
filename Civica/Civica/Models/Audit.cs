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
        public DateOnly Year { get; set; }
        public int Id { get; set; }

        public Audit(decimal amount, DateOnly year)
        {
            this.Amount = amount;
            this.Year = year;
        }
    }
}
