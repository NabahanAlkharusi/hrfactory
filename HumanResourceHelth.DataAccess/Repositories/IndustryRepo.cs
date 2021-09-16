using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public class IndustryRepo: BaseRepository<Industry>
    {
        public HumanResourceContext Context { get; }
        public IndustryRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
