using HumanResourceHelth.Model;


namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PlanRepo : BaseRepository<Plan>
    {
        public HumanResourceContext Context { get; }
        public PlanRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}