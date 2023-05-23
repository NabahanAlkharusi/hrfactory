using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HPartnerShipPlansRepo : BaseRepository<HPartnerShipPlans>
    {
        public HumanResourceContext Context { get; }
        public HPartnerShipPlansRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}