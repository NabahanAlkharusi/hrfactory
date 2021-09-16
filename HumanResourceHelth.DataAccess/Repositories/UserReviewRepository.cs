using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.DataAccess.Repositories
{
   public class UserReviewRepository: BaseRepo<UserReview>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public UserReviewRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
