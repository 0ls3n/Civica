using Civica.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Interfaces
{
    public interface IRepository<T>
    {
        public void Add(T o);
        public List<DomainModel> GetAll();
        public List<T> GetByUserId(int id);
        public List<T> GetByRefId(int id);
        public T GetById(int id);
        public void Update(T o);
        public void Remove(T o);
    }
}
