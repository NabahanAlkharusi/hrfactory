using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class DepartmentsRepository :BaseRepo<Departments>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public DepartmentsRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
