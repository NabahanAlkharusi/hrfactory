using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DeveloperRepository : BaseRepo<Developer>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public DeveloperRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
