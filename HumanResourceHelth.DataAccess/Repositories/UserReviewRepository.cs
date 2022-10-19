using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserReviewRepository : BaseRepo<UserReview>
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
