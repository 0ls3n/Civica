using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class ProjectRepository
    {

        private List<Project> projects;

        public ProjectRepository()
        {
            
        }

        public void Add(Project project) 
        {
            projects.Add(project);
        }

        public List<Project> GetAll()
        {
            return projects;
        }

        public Project Get(int id) 
        { 
            return projects.Find(x => x.Id == id);
        }




    }

}
