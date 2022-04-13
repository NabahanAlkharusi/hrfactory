using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class UpdatesRepo :BaseRepository<Updates>    {
        public HumanResourceContext Context
        {
            get;
        }
        public UpdatesRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
