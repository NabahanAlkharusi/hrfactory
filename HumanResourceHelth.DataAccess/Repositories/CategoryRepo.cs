using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class CategoryRepo : BaseRepository<Category>
    {
        public HumanResourceContext Context { get; }

        public CategoryRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}