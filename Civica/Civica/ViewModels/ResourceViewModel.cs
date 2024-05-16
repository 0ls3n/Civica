using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Civica.Models;
using Microsoft.Identity.Client.NativeInterop;

namespace Civica.ViewModels
{
   public class ResourceViewModel
    {
        private Resource resource;
        private string _startAmount = "";

        public string StartAmount
        {
            get { return _startAmount; }
            set 
            { 
                if (double.TryParse(value, out _) || value == "")
                {
                    _startAmount = value; 
                }
                else
                {
                    MessageBox.Show("'Startbeløb' må kun være i tal.");
                }
            }
        }

        private string _expectedYearlyCost = "";
        public string ExpectedYearlyCost 
        {
            get
            {
                return _expectedYearlyCost;
            }
            set
            {
                if (double.TryParse(value, out _) || value == "")
                {
                    _expectedYearlyCost = value;
                }
                else
                {
                    MessageBox.Show("'Fast årlig omkostning' må kun være i tal.");
                }
            }
        }

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
