using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HumanResourceHelth.Model
{
    public class ExpertsProfile : BaseEntity
    {

        [Required]
        public string ImageUrl { get; set; }
        
        [Url]
        public string LinkedInUrl { get; set; }
        [Url]
        
        public string YouToubeUrl { get; set; }
        [Url]
        
        public string InstagramUrl { get; set; }
        
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string DescriptionEn { get; set; }
        [Required]
        public string DescriptionAr { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string SummaryEn { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string EducationEn { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string ExperiencesEN { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string CertificatesEN { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string LanguagesEn { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string SummaryAr { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string EducationAr { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string ExperiencesAr { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string CertificatesAr { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string LanguagesAr { get; set; }
    }
}
