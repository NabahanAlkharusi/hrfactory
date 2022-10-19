using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SpecialtiesRepository : BaseRepo<Specialties>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public SpecialtiesRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
