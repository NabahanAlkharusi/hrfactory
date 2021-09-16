using HumanResourceHelth.Model;

namespace HumanResourceHelth.DataAccess.Repositories
{
    public class VideoRepo : BaseRepository<Video>
    {
        public HumanResourceContext Context { get; }

        public VideoRepo(HumanResourceContext context) : base(context)
        {
            Context = context;
        }
    }
}
