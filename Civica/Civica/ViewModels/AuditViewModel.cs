using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Civica.Models;

namespace Civica.ViewModels
{
    public class AuditViewModel
    {
        private Audit audit;
        private string _amount = "";
        public string Amount
        {
            get => _amount;
            set
            {
                if (decimal.TryParse(value, out _) || value == "")
                {
                    _amount = value;
                }
                else
                {
                    MessageBox.Show("'Omkostning' må kun være tal.");
                }
            }
        }
        public int Year { get; set; }
        public string Description { get; set; }

        public AuditViewModel(Audit a)
        {
            Amount = string.Format("{0:#,0}", a.Amount);
            Year = a.Year;
            Description = a.Description;
            audit = a;
        }

        public int GetId()
        {
            return audit.Id;
        }

        public int GetRefId()
        {
            return audit.RefId;
        }
    }
}
