using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Audit : DomainModel
    {
        public decimal Amount { get; set; }
        public int Year { get; set; }
        public Audit(decimal amount, int year)
        {
            this.Amount = amount;
            this.Year = year;
        }
    }
}
