﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Economy
    {
 
        public decimal StartAmount { get; set; }
        public decimal ExpectedYearlyCost { get; set; }

        public int ProjectId { get; set; }

        public int Id { get; set; }



        public Economy(int projectId, decimal startAmount = default, decimal expectedYearlyCost = default)
        {
            this.StartAmount = startAmount;
            this.ExpectedYearlyCost = expectedYearlyCost;
            ProjectId = projectId;
            
        }

      
    }
}
