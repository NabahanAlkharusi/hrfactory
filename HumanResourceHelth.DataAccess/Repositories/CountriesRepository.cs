﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class CountriesRepository : BaseRepo<Countries>
    {
      
            public HumanResourceContext Context
            {
                get;
            }
            public CountriesRepository(HumanResourceContext context) : base(context)
            {
                Context = context;
            }
        }
    }

