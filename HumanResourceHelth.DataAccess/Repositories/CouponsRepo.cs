using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CouponsRepo : BaseRepo<Coupons>
    {
        public HumanResourceContext Context { get; }

        public CouponsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
