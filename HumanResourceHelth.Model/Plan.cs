using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Plan : BaseEntity
    {[Required, Display(Name = "dddddd")]
        public string Name { get; set; }
        public string NameAR { get; set; }
        public double AnnualPrice { get; set; }
        public double ManthlyPrice { get; set; }
    }
}
