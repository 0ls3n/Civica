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

        private Func<int, List<DomainModel>> _execute { get; set; }

        public Repository(Func<int, List<DomainModel>> execute)
        {
            _list = DatabaseHelper<DomainModel>.Initialize(typeof(T));
            _execute = execute;
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

        public List<T> GetByRefId(int id)
        {
            return _execute(id).OfType<T>().ToList();
        }

        public T GetById(int id)
        {
            return _execute(id).OfType<T>().FirstOrDefault();
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

        public async Task RefreshAsync()
        {
            _list.Clear();
            _list = await DatabaseHelper<DomainModel>.InitializeAsync(typeof(T));
        }
    }
}
