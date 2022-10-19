using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class DoctorRequestRepo : BaseRepository<DoctorRequest>
    {
        public HumanResourceContext Context { get; }

        public DoctorRequestRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}