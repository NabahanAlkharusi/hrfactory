using HumanResourceHelth.Model;
using System.Linq;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DefaultMBRepo : BaseRepository<DefaultMB>
    {
        public HumanResourceContext Context { get; }
        public DefaultMBRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }

        public int NumberOfBuilders()
        {
            return Context.DefaultMB.Select(x => x.UserId).Distinct().Count();
        }
    }
}
