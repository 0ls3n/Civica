using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Resource : DomainModel
    {
 
        public decimal StartAmount { get; set; }
        public decimal ExpectedYearlyCost { get; set; }
        public int Year {  get; set; }

        public Resource(int projectId, decimal startAmount = default, decimal expectedYearlyCost = default, int year = default)
        {
            this.StartAmount = startAmount;
            this.ExpectedYearlyCost = expectedYearlyCost;
            this.Year = year;
            RefId = projectId;   
        }
    }
}
