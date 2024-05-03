using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;

namespace Civica.ViewModels
{
   public class ResourceViewModel
    {
        private Resource resource;
        public decimal StartAmount { get; set; }
        public decimal ExpectedYearlyCost { get; set; }
        public DateTime Year {  get; set; }

        public ResourceViewModel(Resource r)
        {
            StartAmount = r.StartAmount;
            ExpectedYearlyCost = r.ExpectedYearlyCost;
            Year = r.Year;
            resource = r;

        }
        public int GetId()
        {
            return resource.Id;
        }


    }
}
