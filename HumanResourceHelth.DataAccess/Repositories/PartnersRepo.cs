using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PartnersRepo : BaseRepository<Partners>
    {
        public HumanResourceContext Context { get; }

        public PartnersRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
