using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class ResourceRepository
    {
        private List<Resource> _resourceList;
        public ResourceRepository() 
        {
            _resourceList = DatabaseHelper.InitializeEconomy();
        }

        public List<Resource> GetAll() => _resourceList;

        public void Add(Resource resource)
        {
            resource.Id = DatabaseHelper.Add(resource);
            _resourceList.Add(resource);

        }

        public void Update(Resource resource, decimal startAmount, decimal expectedYearlyCost)
        {
            resource.StartAmount = startAmount;
            resource.ExpectedYearlyCost = expectedYearlyCost;
            DatabaseHelper.Update(resource);
        }

        public void Remove(Resource resource)
        {
            DatabaseHelper.Remove(resource);
            _resourceList.Remove(resource);
        }
        public Resource Get(int id)
        {
            return _resourceList.Find(x => x.Id == id);
        }

        public Resource GetByProjectId(int projectId)
        {
            return _resourceList.Find(x => x.ProjectId == projectId);
        }
    }
}
