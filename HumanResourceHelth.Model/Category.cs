﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Category : BaseEntity
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public virtual List<DocFile> Documents { get; set; }
    }
}
