using HumanResourceHelth.Model;
//using iTextSharp.text;
using System.Linq;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SectionRepo : BaseRepository<Section>
    {
        public HumanResourceContext Context { get; }

        public SectionRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }

        public int NumberOfBuilders()
        {
            return Context.Sections.Select(x => x.UserId).Distinct().Count();
        }
    }
}
