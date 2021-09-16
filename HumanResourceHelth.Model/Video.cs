using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Video : BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public int Lenght { get; set; }
        public int CourseId { get; set; }
        public bool IsForPriview { get; set; }
        public int Ordering { get; set; }
    }
}
