using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HPartnersRepo : BaseRepository<HPartners>
    {
        public HumanResourceContext Context { get; }

        public HPartnersRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
