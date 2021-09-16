using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SurveyRepo : BaseRepository<Survey>
    {
        public HumanResourceContext Context { get; }

        public SurveyRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
