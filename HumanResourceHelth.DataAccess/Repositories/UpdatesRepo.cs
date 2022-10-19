using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UpdatesRepo : BaseRepository<Updates>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public UpdatesRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
