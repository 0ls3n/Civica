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
        public T GetById(Predicate<DomainModel> predicate);
        public List<T> GetListById(Predicate<DomainModel> predicate);
        public void Update(T o);
        public void Delete(T o);
        public void DeleteByRefId(int id);
        public Task Refresh();
    }
}