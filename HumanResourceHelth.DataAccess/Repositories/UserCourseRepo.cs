using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserCourseRepo : BaseRepository<UserCourse>
    {
        public HumanResourceContext Context { get; }

        public UserCourseRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
