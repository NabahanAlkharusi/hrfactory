using HumanResourceHelth.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class ProjectRepository :BaseRepo<Project>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public ProjectRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
