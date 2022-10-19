using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class TrainersRepository : BaseRepo<Trainers>
    {
        public HumanResourceContext Context
        {
            get;
        }
        public TrainersRepository(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
