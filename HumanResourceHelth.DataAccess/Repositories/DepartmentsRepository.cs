using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DepartmentsRepository : BaseRepo<Departments>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public DepartmentsRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
