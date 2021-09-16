using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class IntroVedioRepo : BaseRepository<IntroVedio>
    {
        public HumanResourceContext Context { get; }

        public IntroVedioRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
