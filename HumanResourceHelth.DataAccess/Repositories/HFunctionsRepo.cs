using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HFunctionsRepo : BaseRepository<HFunctions>
    {
        public HumanResourceContext Context { get; }

        public HFunctionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
