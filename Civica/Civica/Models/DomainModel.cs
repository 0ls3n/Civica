﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Civica.Models
{
    public class DomainModel
    {
        public int Id { get; set; }

        public int RefId { get; set; }
        public int UserId { get; set; }
        public DateTime Created { get; set; }
    }
}
