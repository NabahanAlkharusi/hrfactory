using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CountryRepo : BaseRepository<Country>
    {
        public HumanResourceContext Context { get; }

        public CountryRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
