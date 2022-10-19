using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class TermsConditionsRepo : BaseRepository<TermsConditions>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public TermsConditionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
