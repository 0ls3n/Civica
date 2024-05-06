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
        public Audit(int resourceId, decimal amount, int year)
        {
            this.Amount = amount;
            this.Year = year;
            RefId = resourceId;
        }
        public Audit(int userId,int resourceId, decimal amount, int year)
        {
            this.Amount = amount;
            this.Year = year;
            RefId = resourceId;
            UserId = userId;
        }

    }
}
