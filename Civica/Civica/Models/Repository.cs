using Civica.Interfaces;
using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class Repository<T> : IRepository<T>
    {
        private List<DomainModel> _list = DatabaseHelper<DomainModel>.Initialize(typeof(T));

        public void Add(T o)
        {
            DatabaseHelper<T>.Add(o);
            if (o is DomainModel d)
            {
                _list.Add(d);
            }
        }

        public List<DomainModel> GetAll() => _list;

        public List<T> GetListById(Predicate<DomainModel> predicate) => _list.Where(item => predicate(item)).OfType<T>().ToList();
        public T GetById(Predicate<DomainModel> predicate) => _list.Where(item => predicate(item)).OfType<T>().FirstOrDefault();

        public void Update(T o)
        {
            DatabaseHelper<T>.Update(o);
        }

        public void Remove(T o)
        {
            DatabaseHelper<T>.Remove(o);
            if (o is DomainModel d)
            {
                _list.Remove(d);
            }
        }
        public void RemoveByRefId(int id)
        {
            GetListById(x => x.RefId == id).ForEach(DatabaseHelper<T>.Remove);
            _list.RemoveAll(x => x.RefId == id);
        }
    }
}
