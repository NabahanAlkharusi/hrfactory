﻿using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DeveloperRepository :BaseRepo<Developer>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public DeveloperRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
