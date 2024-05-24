using Civica.Interfaces;
using Civica.Models;
using GVMR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class CRUDAuditViewModel : ObservableObject, IViewModelChild
    {
        private ExpandedResourceViewModel ervm;
        private IRepository<Audit> auditRepo;

        private int _year;
        public int Year
        {
            get => _year;
            set
            {
                _year = value;
                OnPropertyChanged(nameof(Year));
            }
        }
        private string _amount;
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
                OnPropertyChanged(nameof(Amount));
            }
        }

        private string _description = "";
        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        public void Init(ObservableObject o)
        {
            ervm = (o as ExpandedResourceViewModel);

            auditRepo = ervm.mvm.GetAuditRepo();
        }

        public void Create()
        {
            Audit a = new Audit(ervm.mvm.CurrentUser.GetId(), ervm.SelectedResource.GetId(), Amount.IsNullOrEmpty() ? 0 : decimal.Parse(Amount), Year, Description, DateTime.Now);

            auditRepo.Add(a);

            ervm.Audits.Clear();

            ervm.UpdateList();
            Amount = "";
            Year = int.MinValue;
            Description = "";
        }

        public void Update()
        {
            AuditViewModel avm = ervm.SelectedAudit;
            string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
            avm.Amount = temp;

            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            a.Amount = decimal.Parse(avm.Amount);
            a.Year = avm.Year;
            a.Description = avm.Description;

            auditRepo.Update(a);

            ervm.Audits.Clear();

            ervm.UpdateList();

            ervm.SelectedAudit = avm;
        }

        public void Delete()
        {
            AuditViewModel avm = ervm.SelectedAudit;

            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            auditRepo.Delete(a);

            ervm.Audits.Clear();

            ervm.UpdateList();
        }
    }
}