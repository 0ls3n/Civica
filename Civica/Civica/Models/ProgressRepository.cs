using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class ProgressRepository
    {
        public List<Progress> Progresses { get; set; }

        public ProgressRepository()
        {
            Progresses = DatabaseHelper.InitializeProgress();
        }

        public void Add(Progress prog)
        {
            DatabaseHelper.Add(prog);

            Progresses.Add(prog);
        }

        public List<Progress> GetAll()
        {
            return Progresses;
        }

        public List<Progress> Get(int id)
        {
            return new List<Progress> { Progresses.Find(x => x.Id == id) };
        }
    }
}
