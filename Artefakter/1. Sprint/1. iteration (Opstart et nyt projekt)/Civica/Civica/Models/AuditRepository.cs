using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class AuditRepository
    {
        private List<Audit> _auditList;

        public AuditRepository()
        {
            _auditList = DatabaseHelper.InitializeAudit();
        }
        public List<Audit> GetAll() => _auditList;

        public void Add(Audit audit)
        {
            audit.Id = DatabaseHelper.Add(audit);
            _auditList.Add(audit);
        }

        public void Update(Audit audit, DateTime year, decimal amount)
        {
            audit.Year = year;
            audit.Amount = amount;
            DatabaseHelper.Update(audit);
        }

        public void Remove(Audit audit)
        {
            DatabaseHelper.Remove(audit);
            _auditList.Remove(audit);
        }
        public Audit Get(int id) => _auditList.Find(x => x.Id == id);

        public List<Audit> GetByEconomyId(int economyId) => _auditList.FindAll(x=> x.EconomyId == economyId);
    }
}
