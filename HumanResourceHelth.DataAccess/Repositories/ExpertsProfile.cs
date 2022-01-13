using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class ExpertsProfileRepo : BaseRepo<ExpertsProfile>
    {
        public HumanResourceContext Context { get; }

        public ExpertsProfileRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
