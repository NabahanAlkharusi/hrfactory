using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class TechRequest : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }




        [Required]
        public string Description { get; set; }
        public DateTime RequestDate { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
       


        [MaxLength(255)]
        public string ContactInformation { get; set; }

        [MaxLength(255)]
        public string ContactPerson { get; set; }
        public string Notes { get; set; }


        public SubscriptionPlan? PlanName { get; set; }
        [Required]

        public int NumberOFEmployees { get; set; }
        [Required]

        public int IndustryId { get; set; }
        [Required]
        public int CountryId { get; set; }

        public virtual Country Country { get; set; }

        public virtual Industry Industry { get; set; }
    }
}
 