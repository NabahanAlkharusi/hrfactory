using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
   public class Industry:BaseEntity
    {
        public string Name { get; set; }
        public virtual List<User> Users{ get; set; }
        public string NameAr { get; set; }
    }
}
