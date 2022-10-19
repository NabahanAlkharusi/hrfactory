using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{

    public class HrSettingRepository : BaseRepo<HrSetting>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public HrSettingRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
