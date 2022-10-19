using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PracticeQuestionsRepo : BaseRepository<PracticeQuestions>
    {
        public HumanResourceContext Context { get; }

        public PracticeQuestionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
