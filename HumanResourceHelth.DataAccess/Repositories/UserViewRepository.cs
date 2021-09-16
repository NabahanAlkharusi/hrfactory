using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public  class UserCourseViewRepository:BaseRepo<UserCourseView>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public UserCourseViewRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
