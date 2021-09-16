using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CourseRateRepo : BaseRepository<CourseRate>
    {
        public HumanResourceContext Context { get; }

        public CourseRateRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
