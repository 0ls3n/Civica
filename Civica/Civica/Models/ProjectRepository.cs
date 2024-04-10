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
            projects = DatabaseHelper.InitializeProjects();
        }

        public void Add(Project p) 
        {

            p.Id = DatabaseHelper.Add(p);

            projects.Add(p);
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
