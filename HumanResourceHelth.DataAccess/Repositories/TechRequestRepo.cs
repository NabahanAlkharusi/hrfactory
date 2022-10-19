using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class TechRequestRepo : BaseRepository<TechRequest>
    {
        public HumanResourceContext Context { get; }

        public TechRequestRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}