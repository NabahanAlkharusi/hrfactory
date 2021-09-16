using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class AnswerRepo : BaseRepository<Answer>
    {
        public HumanResourceContext Context { get; }

        public AnswerRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
