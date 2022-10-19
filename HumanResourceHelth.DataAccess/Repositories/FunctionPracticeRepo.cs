using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class FunctionPracticeRepo : BaseRepository<FunctionPractice>
    {
        public HumanResourceContext Context { get; }

        public FunctionPracticeRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
