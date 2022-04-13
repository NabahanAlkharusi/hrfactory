using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class SemiNotificationRepo : BaseRepository<SemiNotifications>    {
        public HumanResourceContext Context
        {
            get;
        }
        public SemiNotificationRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
