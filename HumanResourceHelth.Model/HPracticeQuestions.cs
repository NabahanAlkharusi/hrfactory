using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HPracticeQuestions:BaseEntity
    {
        [Required]
        [Display(Name = "Question in English")]
        public string Question { get; set; }
        [Required]
        [Display(Name = "Question in Arabic")]
        public string QuestionAr { get; set; }
        [Required]
        [Display(Name = "Practice ID")]
        public int Practice { get; set; }
        [Required]
        [Display(Name = "Respondent")]
        public int Respondent { get; set; }
        public bool Status { get; set; }
        public bool IsENPS { get; set; } = false;
    }
}
