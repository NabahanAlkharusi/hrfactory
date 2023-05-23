using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class OurPartnersRepo : BaseRepository<OurPartners>
    {
        public HumanResourceContext Context { get; }

        public OurPartnersRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
