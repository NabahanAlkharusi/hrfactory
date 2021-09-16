

using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{


    public class PymentsRepository : BaseRepo<Pyments>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public PymentsRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}