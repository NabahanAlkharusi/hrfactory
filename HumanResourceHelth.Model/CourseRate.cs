using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class CourseRate: BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int CourseId { get; set; }
        public DateTime RateTime { get; set; }
        public int Rate { get; set; }
        public string Review { get; set; }
    }
}
