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

        public Resource(int userId, int projectId, DateTime createdDate)
        {
            UserId = userId;
            RefId = projectId;
            CreatedDate = createdDate;
        }
        public Resource(int userId, int projectId, decimal startAmount, decimal expectedYearlyCost, DateTime createdDate) : this(userId, projectId, createdDate)
        {
            StartAmount = startAmount;
            ExpectedYearlyCost = expectedYearlyCost;
        }
    }
}
