using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CourseRepo : BaseRepository<Course>
    {
        public HumanResourceContext Context { get; }

        public CourseRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
