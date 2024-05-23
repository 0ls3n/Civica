using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public abstract class DomainModel
    {
        public int Id { get; set; }

        public int RefId { get; protected set; }

        public int UserId { get; protected set; }
        public DateTime CreatedDate { get; protected set; }
    }
}
