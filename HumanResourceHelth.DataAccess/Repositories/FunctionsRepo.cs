using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class FunctionsRepo : BaseRepository<Functions>
    {
        public HumanResourceContext Context { get; }

        public FunctionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
