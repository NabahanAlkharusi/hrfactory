using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HFunctionPractice : BaseEntity
    {
        [Required]
        [Display(Name = "Practice Title in English")]
        public string PracticeTitle { get; set; }
        [Required]
        [Display(Name = "Practice Title in Arabic")]
        public string PracticeTitleAr { get; set; }
        [Required]
        [Display(Name = "Function ID")]
        public int FunctionId { get; set; }
        public bool Status { get; set; }
        
    }
}
