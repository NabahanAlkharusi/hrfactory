using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SpecialtiesRepository :BaseRepo<Specialties>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public SpecialtiesRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
