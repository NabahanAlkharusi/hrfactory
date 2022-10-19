using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserCourseViewRepository : BaseRepo<UserCourseView>
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
