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

        public void Remove(Project p)
        {
            DatabaseHelper.Remove(p);
            projects.Remove(p);
        }
        public void Update(Project project, string name, string owner, string manager, string description) 
        {
            project.Name = name;
            project.Owner = owner;
            project.Manager = manager;
            project.Description = description;

            DatabaseHelper.Update(project);
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
