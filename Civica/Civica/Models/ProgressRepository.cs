using Civica.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class ProgressRepository
    {
        private List<Progress> _progresses;

        public ProgressRepository()
        {
            _progresses = DatabaseHelper.InitializeProgress();
        }

        public void Add(Progress prog)
        {
            prog.Id = DatabaseHelper.Add(prog);

            _progresses.Add(prog);
        }

        public List<Progress> GetAll() => _progresses;

        public List<Progress> Get(int id) => _progresses.FindAll(x => x.ProjectId == id);

        public void Update(Progress prog, Phase phase, Status status, string description) 
        {
            prog.Phase = phase;
            prog.Status = status;
            prog.Description = description;
            DatabaseHelper.Update(prog); 
        }

        public void Remove(Progress prog) 
        {
            DatabaseHelper.Remove(prog);
            _progresses.Remove(prog);
        }
    }
}
