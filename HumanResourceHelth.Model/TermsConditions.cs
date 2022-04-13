using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace HumanResourceHelth.Model
{
    public class TermsConditions : BaseEntity
    {
        [Required]
        public int TermsConditionType { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Column(TypeName = "ntext")]
        [Required]
        public string ArabicText { get; set; }
        [Column(TypeName = "ntext")]
        [Required]
        public string EnglishText { get; set; }
        [Column(TypeName = "ntext")]
        [Required]
        public string ArabicTitle { get; set; }
        [Column(TypeName = "ntext")]
        [Required]
        public string EnglishTitle { get; set; }
    }
}
