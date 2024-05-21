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
        public string Description { get; set; } 
        public Audit(int userId, int resourceId, decimal amount, int year, string description, DateTime createdDate)
        {
            Amount = amount;
            RefId = resourceId;
            UserId = userId;
            Year = year;
            Description = description;
            CreatedDate = createdDate;
        }
    }
}
