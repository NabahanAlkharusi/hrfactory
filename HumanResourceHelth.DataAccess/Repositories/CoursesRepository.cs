using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class CoursesRepository :BaseRepo<Courses>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public CoursesRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
