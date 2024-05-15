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
        private List<DomainModel> _list;

        private Func<int, List<DomainModel>> _executeById { get; set; }
        private Func<int, List<DomainModel>> _executeByUserId { get; set; }
        private Func<int, List<DomainModel>> _executeByRefId { get; set; }


        public Repository(Func<int, List<DomainModel>> executeById, Func<int, List<DomainModel>> executeByUserId)
        {
            _list = DatabaseHelper<DomainModel>.Initialize(typeof(T));
            _executeById = executeById;
            _executeByUserId = executeByUserId;
        }
        public Repository(Func<int, List<DomainModel>> executeById, Func<int, List<DomainModel>> executeByUserId, Func<int, List<DomainModel>> executeByRefId) : this(executeById, executeByUserId)
        {
            _executeByRefId = executeByRefId;
        }

            public void Add(T o)
        {
            DatabaseHelper<T>.Add(o);
            if (o is DomainModel d)
            {
                _list.Add(d);
            }
        }

        public List<DomainModel> GetAll() => _list;
        public List<T> GetByUserId(int id)
        {
            return _executeByUserId(id).OfType<T>().ToList();
        }
        public List<T> GetByRefId(int id)
        {
            List<T> list = _executeByRefId(id).OfType<T>().ToList();
            return list;
        }

        public T GetById(int id)
        {
            return _executeById(id).OfType<T>().FirstOrDefault();
        }

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
            List<T> tempList = GetByRefId(id);
            foreach (T o in tempList)
            {
                DatabaseHelper<T>.Remove(o);
            }
            _list.RemoveAll(x => x.RefId == id);
        }
    }
}
