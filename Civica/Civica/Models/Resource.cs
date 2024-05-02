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

        public Resource(int projectId, decimal startAmount = default, decimal expectedYearlyCost = default)
        {
            this.StartAmount = startAmount;
            this.ExpectedYearlyCost = expectedYearlyCost;
            RefId = projectId;   
        }
        public Resource(int userId, int projectId, decimal startAmount = default, decimal expectedYearlyCost = default)
        {
            this.StartAmount = startAmount;
            this.ExpectedYearlyCost = expectedYearlyCost;
            RefId = projectId;
            UserId = userId;   
        }
    }
}
