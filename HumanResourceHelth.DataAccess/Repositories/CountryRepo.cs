using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
  public  class CountryRepo: BaseRepository<Country>
    {
        public HumanResourceContext Context { get; }

        public CountryRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
