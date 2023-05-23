using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class RequestMBServiceRepo : BaseRepository<RequestMBService>
    {

        public HumanResourceContext Context { get; }
        public RequestMBServiceRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
