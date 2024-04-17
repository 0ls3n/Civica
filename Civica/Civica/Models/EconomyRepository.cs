using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class EconomyRepository
    {
        private List<Economy> _economyList = new List<Economy>();
        public EconomyRepository() { }

        public List<Economy> GetAll()=> _economyList;
        
        public void Add(Economy economy)
        {
            _economyList.Add(economy);
        }
        
        public void Remove(Economy economy) 
        {
            _economyList.Remove(economy);
        }
        public Economy Get(int id)
        {
            return _economyList.Find(x => x.Id == id);
        }
    }
}
