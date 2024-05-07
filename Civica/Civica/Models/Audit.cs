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
        public Audit(int userId, int resourceId, decimal amount, int year, DateTime createdDate)
        {
            this.Amount = amount;
            RefId = resourceId;
            Year = year;
            CreatedDate = createdDate;
        }

    }
}
