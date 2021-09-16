using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SurveyTypeRepo : BaseRepository<SurveyType>
    {
        public HumanResourceContext Context { get; }

        public SurveyTypeRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
