using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DocFileRepo : BaseRepository<DocFile>
    {
        public HumanResourceContext Context { get; }

        public DocFileRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}