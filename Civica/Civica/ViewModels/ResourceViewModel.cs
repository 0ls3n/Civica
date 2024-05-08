using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Civica.Models;
using Microsoft.Identity.Client.NativeInterop;

namespace Civica.ViewModels
{
   public class ResourceViewModel
    {
        private Resource resource;
        public string StartAmount { get; set; }
        public string ExpectedYearlyCost { get; set; }

        public ResourceViewModel(Resource r)
        {
            StartAmount = string.Format("{0:#,0}", r.StartAmount);
            ExpectedYearlyCost = string.Format("{0:#,0}", r.ExpectedYearlyCost);
            resource = r;
        }

        public int GetId()
        {
            return resource.Id;
        }
    }
}
