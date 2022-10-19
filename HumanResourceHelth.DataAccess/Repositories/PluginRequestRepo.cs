using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class PluginRequestRepo : BaseRepository<PluginRequest>
    {
        public HumanResourceContext Context { get; }

        public PluginRequestRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}