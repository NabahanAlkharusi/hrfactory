using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
   public class SurveyType
    {
        public int Id { get; set; }
        [StringLength(50)]
        public String Name { get; set; }
        public virtual ICollection<Survey> Survey { get; set; }
        public virtual ICollection<Indicator> Indicator { get; set; }
    }
}
