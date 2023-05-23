using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HPartnership : BaseEntity
    {
        [Required]
        [Display(Name = "Partner ID")]
        public int PartnerID { get; set; }
        [Required]
        [Display(Name = "Plan ID")]
        public int PlanID { get; set; }
        [Required]
        [Display(Name = "Partnership Domain")]
        public int PartnershipDomain { get; set; }
        [Required, Display(Name ="Partnership Start Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [Display(Name ="Partnership End Date")]
        [DataType(DataType.Date, ErrorMessage = "Date only")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        public bool Status { get; set; }
    }
}
