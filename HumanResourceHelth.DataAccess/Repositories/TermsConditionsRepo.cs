﻿using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class TermsConditionsRepo :BaseRepository<TermsConditions>    {
        public HumanResourceContext Context
        {
            get;
        }
        public TermsConditionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
