using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CoursesRepository : BaseRepo<Courses>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public CoursesRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
