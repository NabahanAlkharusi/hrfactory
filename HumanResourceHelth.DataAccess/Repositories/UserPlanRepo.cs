using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserPlanRepo : BaseRepository<UserPlan>
    {
        public HumanResourceContext Context { get; }

        public UserPlanRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
