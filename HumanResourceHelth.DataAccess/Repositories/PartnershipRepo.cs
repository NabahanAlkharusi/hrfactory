using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PartnershipRepo : BaseRepository<Partnership>
    {
        public HumanResourceContext Context { get; }

        public PartnershipRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
