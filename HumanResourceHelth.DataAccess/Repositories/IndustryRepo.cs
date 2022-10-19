using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class IndustryRepo : BaseRepository<Industry>
    {
        public HumanResourceContext Context { get; }
        public IndustryRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
