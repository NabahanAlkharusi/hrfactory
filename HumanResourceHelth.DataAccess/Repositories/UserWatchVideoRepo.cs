using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserWatchVideoRepo : BaseRepository<UserWatchVideo>
    {
        public HumanResourceContext Context { get; }

        public UserWatchVideoRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}

