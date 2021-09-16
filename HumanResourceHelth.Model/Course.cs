using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastModifidDate { get; set; }
        public virtual List<Video> Videos { get; set; }
        public virtual List<CourseRate> CourseRates { get; set; }

    }
}
