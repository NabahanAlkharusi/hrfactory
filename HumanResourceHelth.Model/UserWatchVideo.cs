using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class UserWatchVideo : BaseEntity
    {
        public int UserId { get; set; }
        public int VideoId { get; set; }
        public virtual Video Video { get; set; }
        public DateTime WatchingTime { get; set; }
    }
}
