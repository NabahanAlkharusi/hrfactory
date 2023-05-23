using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HPracticeQuestionsRepo : BaseRepository<HPracticeQuestions>
    {
        public HumanResourceContext Context { get; }

        public HPracticeQuestionsRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
