using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Economy
    {
        private List<Audit> _auditlist = new List<Audit>();
        public decimal StartAmount { get; set; }
        public decimal ExpectedYearlyCost { get; set; }

        public int Id { get; set; }
        public Economy(decimal startAmount = default, decimal expectedYearlyCost = default)
        {
            this.StartAmount = startAmount;
            this.ExpectedYearlyCost = expectedYearlyCost;
        }

        public void Add(Audit audit) 
        {
            _auditlist.Add(audit);
        }
        public void Update(Audit audit) 
        {
            // overvej hvor mange gange den kan anvendes og om der skal være en log på det tidligere indtastet.
        }
    }
}
