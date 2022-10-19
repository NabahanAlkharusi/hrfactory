using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PartnerShipPlansRepo : BaseRepository<PartnerShipPlans>
    {
        public HumanResourceContext Context { get; }
        public PartnerShipPlansRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}