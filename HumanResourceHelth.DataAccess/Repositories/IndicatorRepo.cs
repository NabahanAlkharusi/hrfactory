using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{
    public class IndicatorRepo : BaseRepository<Indicator>
    {
        public HumanResourceContext Context { get; }

        public IndicatorRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
