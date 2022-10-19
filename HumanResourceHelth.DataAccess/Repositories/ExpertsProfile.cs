using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class ExpertsProfileRepo : BaseRepo<ExpertsProfile>
    {
        public HumanResourceContext Context { get; }

        public ExpertsProfileRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
