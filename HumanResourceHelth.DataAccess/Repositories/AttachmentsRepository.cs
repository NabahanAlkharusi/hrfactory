

using HumanResourceHelth.Model;




namespace HumanResourceHelth.DataAccess.Repositories
{
    public class AttachmentsRepository : BaseRepo<Attachments>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public AttachmentsRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }

    }
}
