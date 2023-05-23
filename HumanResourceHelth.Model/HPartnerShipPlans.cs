using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HPartnerShipPlans : BaseEntity
    {
        [Required]
        [Display(Name = "Plan Title in English")]
        public string PlanTitle { get; set; }
        [Required]
        [Display(Name = "Plan Objective in English")]
        [Column(TypeName = "ntext")]
        public string Objective { get; set; }
        [Required]
        [Display(Name = "Plan Process in English")]
        public string Process { get; set; }
        [Required]
        [Display(Name = "Plan Audience")]
        public int audience { get; set; }
        [Required]
        [Display(Name = "Plan Report in English")]
        public string Report { get; set; }
        [Required]
        [Display(Name = "Plan Delivery Mode in English")]
        public string DeliveryMode { get; set; }
        [Required]
        [Display(Name = "Plan Limitations in English")]
        [Column(TypeName = "ntext")]
        public string Limitations { get; set; }
        [Required]
        [Display(Name = "Plan Title in Arabic")]
        public string PlanTitleAr { get; set; }
        [Required]
        [Display(Name = "Plan Objective in Arabic")]
        [Column(TypeName = "ntext")]
        public string ObjectiveAr { get; set; }
        [Required]
        [Display(Name = "Plan Process in Arabic")]
        public string ProcessAr { get; set; }
        [Required]
        [Display(Name = "Plan Report in Arabic")]
        public string ReportAr { get; set; }
        [Required]
        [Display(Name = "Plan Delivery Mode in Arabic")]
        public string DeliveryModeAr { get; set; }
        [Required]
        [Display(Name = "Plan Limitations in Arabic")]
        [Column(TypeName = "ntext")]
        public string LimitationsAr { get; set; }
        public string TamplatePath { get; set; }
        public double Price { get; set; }
        public int PaymentMethod { get; set; }
        public bool Status { get; set; }=false;
    }
}
