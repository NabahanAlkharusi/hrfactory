using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class SemiNotificationRepo : BaseRepository<SemiNotifications>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public SemiNotificationRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
