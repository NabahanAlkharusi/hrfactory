using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class HPartners : BaseEntity
    {
        [Required]
        [Display(Name = "Partner UserID in English")]
        public int UserID { get; set; }
        [Required]
        [Display(Name = "Partner Name in English")]
        public string PartnerName { get; set; }
        [Required]
        [Display(Name = "Partner Name in Arabic")]
        public string PartnerNameAr { get; set; }
        [Required, EmailAddress]
        [Display(Name = "Partner Offical Email")]
        public string PartnerofficalEmail { get; set; }
        [Required]
        [Display(Name = "Partner Offical Phone")]
        public string PartnerofficalPhone { get; set; }
        [Required]
        [Display(Name = "Partner Country")]
        public int PartnerCountry { get; set; }
        [Required]
        [Display(Name = "Partner Address")]
        public string PartnerAddress { get; set; }
        [Required]
        [Display(Name = "Partner Focal Point Name in English")]
        public string PartnerFocalPointName { get; set; }
        [Required]
        [Display(Name = "Partner Focal Point Name in Arabic")]
        public string PartnerFocalPointNameAr { get; set; }
        [Required, EmailAddress]
        [Display(Name = "Partner Focal Point Email")]
        public string PartnerFocalPointEmail { get; set; }
        [Required]
        [Display(Name = "Partner Focal Point Phone")]
        public string PartnerFocalPointPhone { get; set; }

        [Display(Name = "Partner Sub-Domain")]
        public string PartnerSubDomain { get; set; }
        [Display(Name = "Partner Logo")]
        public string PartnerLogo { get; set; }
        [ Display(Name = "Partner Banner")]
        public string PartnerBanner { get; set; }
        public bool status { get; set; } = true;
    }
}
