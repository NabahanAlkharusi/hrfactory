using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HPartnershipRepo : BaseRepository<HPartnership>
    {
        public HumanResourceContext Context { get; }

        public HPartnershipRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
