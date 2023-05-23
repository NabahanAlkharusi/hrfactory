using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class OurPartners : BaseEntity
    {

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

        [Display(Name = "Partner Website")]
        public string PartnerWebsite { get; set; }
        [Display(Name = "Partner Logo")]
        public string PartnerLogo { get; set; }
        [Display(Name = "Partner English Description"), Column(TypeName = "ntext"),Required]        
        public string PartnerDes { get; set; }
        [Display(Name = "Partner Arabic Description"), Column(TypeName = "ntext"), Required]
        public string PartnerDesAr { get; set; }
        //tag to sort partners 
        [Display(Name = "Partner Sort")]
        public int PartnerSort { get; set; }
        public bool status { get; set; } = true;
    }
}
