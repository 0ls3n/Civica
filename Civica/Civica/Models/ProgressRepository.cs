using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class ProgressRepository
    {
        private List<Progress> _progresses { get; set; }

        public ProgressRepository()
        {
            _progresses = DatabaseHelper.InitializeProgress();
        }

        public void Add(Progress prog)
        {
            DatabaseHelper.Add(prog);

            _progresses.Add(prog);
        }

        public List<Progress> GetAll()
        {
            return _progresses;
        }

        public List<Progress> Get(int id)
        {
            return new List<Progress> { _progresses.Find(x => x.Id == id) };
        }
    }
}
