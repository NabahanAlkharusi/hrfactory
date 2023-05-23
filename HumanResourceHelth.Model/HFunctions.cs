using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HFunctions : BaseEntity
    {
        [Required]
        [Display(Name ="Function Title in English")]
        public string FunctionTitle { get; set; }
        [Required]
        [Display(Name ="Function Title in Arabic")]
        public string FunctionTitleAr { get; set; }
        [Required]
        [Display(Name = "Plan")]
        public int planId { get; set; }
        [Required]
        [Display(Name = "Respondent")]
        public int Respondent { get; set; }
        public bool status { get; set; } = true;
    }
}
