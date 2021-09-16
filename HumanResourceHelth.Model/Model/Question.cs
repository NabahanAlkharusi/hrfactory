using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
   public class Question
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string Name { get; set; }
        [StringLength(500)]
        public string? Practice { get; set; }
        [ForeignKey("Indicator")]
        public int IndicatorId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }


    }
}
