using HumanResourceHelth.Model;
using System.Linq;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class UserRepo : BaseRepository<User>
    {
        public HumanResourceContext Context { get; }

        public UserRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }

        public bool Delete()
        {
            HumanResourceContext db = new HumanResourceContext();
            //var answers = from a in db.Answers
            //              join qu
            //              select a;


            return true;
        }
    }
}
