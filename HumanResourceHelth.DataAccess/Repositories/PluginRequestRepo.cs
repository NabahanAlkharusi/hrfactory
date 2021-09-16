using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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