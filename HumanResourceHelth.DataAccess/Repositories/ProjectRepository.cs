using HumanResourceHelth.Model.Entities;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class ProjectRepository : BaseRepo<Project>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public ProjectRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
