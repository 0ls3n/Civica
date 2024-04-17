//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Civica.Models
//{
//    public class AuditRepository
//    {
//        private List<Audit> _auditList = new List<Audit>();

//        public AuditRepository()
//        {
            
//        }
//        public List<Audit> GetAll() => _auditList;

//        public void Add(Audit audit)
//        {
//            _auditList.Add(audit);
//        }

//        public void Remove(Audit audit)
//        {
//            _auditList.Remove(audit);
//        }
//        public Audit Get(int id)
//        {
//            return _auditList.Find(x => x.Id == id);
//        }

//    }
//}
