using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SurveyResultRepo : BaseRepository<SurveyResult>
    {
        public HumanResourceContext Context { get; }

        public SurveyResultRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
