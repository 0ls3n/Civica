using Civica.Interfaces;
using Civica.Models;
using GVMR;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Civica.ViewModels
{
    public class CRUDAuditViewModel : ObservableObject
    {
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

        public void CreateAudit(int userId, int resourceId, string amount, int year, string description)
        {
            Audit a = new Audit(MainViewModel.Instance.CurrentUser.GetId(), ExpandedResourceViewModel.Instance.SelectedResource.GetId(), decimal.TryParse(amount, out decimal r) ? r : 0, year, description, DateTime.Now);

            auditRepo.Add(a);

            ExpandedResourceViewModel.Instance.Audits.Clear();

            ExpandedResourceViewModel.Instance.UpdateList();
            Amount = "";
            Year = int.MinValue;
            Description = "";
        }

        public void UpdateAudit(AuditViewModel avm)
        {
            string temp = string.Format("{0:#,0}", double.Parse(avm.Amount));
            avm.Amount = temp;

            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            a.Amount = decimal.Parse(avm.Amount);
            a.Year = avm.Year;
            a.Description = avm.Description;

            auditRepo.Update(a);

            ExpandedResourceViewModel.Instance.Audits.Clear();

            ExpandedResourceViewModel.Instance.UpdateList();

            ExpandedResourceViewModel.Instance.SelectedAudit = avm;
        }

        public void DeleteAudit(AuditViewModel avm)
        {
            Audit a = auditRepo.GetById(x => x.Id == avm.GetId());

            auditRepo.Delete(a);

            ExpandedResourceViewModel.Instance.Audits.Clear();

            ExpandedResourceViewModel.Instance.UpdateList();
        }

        //Singleton
        private CRUDAuditViewModel() {
            auditRepo = MainViewModel.Instance.GetAuditRepo();
        }

        private static readonly object _lock = new object();
        private static CRUDAuditViewModel _instance;

        public static CRUDAuditViewModel Instance
        {
            get
            {
                if (_instance is null)
                {
                    lock (_lock)
                    {
                        if (_instance is null)
                        {
                            _instance = new CRUDAuditViewModel();
                        }
                    }
                }
                return _instance;
            }
        }
    }
}