using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{
    public class QuestionRepo : BaseRepository<Question>
    {
        public HumanResourceContext Context { get; }
        public QuestionRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
