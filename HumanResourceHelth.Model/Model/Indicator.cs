using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
    public class Indicator
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string? Name { get; set; }
        public virtual ICollection<Question> Question { get; set; }
        public virtual ICollection<SurveyResult> SurveyResult { get; set; }
        [ForeignKey("SurveyId")]
        public int? SurveyId { get; set; }
    }
}
