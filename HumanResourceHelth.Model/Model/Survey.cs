using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
   public class Survey
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? SurveyDate { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("SurveyType")]
        public int SurveyTypeId { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<SurveyResult> SurveyResult { get; set; }
    }
}
