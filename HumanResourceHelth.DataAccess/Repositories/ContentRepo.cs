using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class ContentRepo : BaseRepository<Content>
    {
        public HumanResourceContext Context { get; }

        public ContentRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}