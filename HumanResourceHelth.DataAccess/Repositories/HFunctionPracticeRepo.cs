using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class HFunctionPracticeRepo : BaseRepository<HFunctionPractice>
    {
        public HumanResourceContext Context { get; }

        public HFunctionPracticeRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
