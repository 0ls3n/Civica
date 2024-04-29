using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class EconomyRepository
    {
        private List<Economy> _economyList;
        public EconomyRepository() 
        {
            _economyList = DatabaseHelper.InitializeEconomy();
        }

        public List<Economy> GetAll() => _economyList;

        public void Add(Economy economy)
        {
            economy.Id = DatabaseHelper.Add(economy);
            _economyList.Add(economy);

        }

        public void Update(Economy economy, decimal startAmount, decimal expectedYearlyCost)
        {
            economy.StartAmount = startAmount;
            economy.ExpectedYearlyCost = expectedYearlyCost;
            DatabaseHelper.Update(economy);
        }

        public void Remove(Economy economy)
        {
            DatabaseHelper.Remove(economy);
            _economyList.Remove(economy);
        }
        public Economy Get(int id)
        {
            return _economyList.Find(x => x.Id == id);
        }

        public Economy GetByProjectId(int projectId)
        {
            return _economyList.Find(x => x.ProjectId == projectId);
        }
    }
}
