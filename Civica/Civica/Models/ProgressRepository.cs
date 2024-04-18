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
            DatabaseHelper.Add(prog);

            _progresses.Add(prog);
        }

        public List<Progress> GetAll() => _progresses;

        public List<Progress> Get(int id) => _progresses.FindAll(x => x.ProjectId == id);
    }
}
