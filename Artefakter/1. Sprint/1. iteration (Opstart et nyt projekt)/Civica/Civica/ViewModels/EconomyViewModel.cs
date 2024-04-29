using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
   public class EconomyViewModel
    {
        private Economy economy;
        public decimal StartAmount { get; set; }
        public decimal ExpectedYearlyCost { get; set; }

        public EconomyViewModel(Economy e)
        {
            StartAmount = e.StartAmount;
            ExpectedYearlyCost = e.ExpectedYearlyCost;
            economy = e;

        }
        public int GetId()
        {
            return economy.Id;
        }


    }
}
