using System.ComponentModel.DataAnnotations.Schema;


namespace HumanResourceHelth.Model
{
    public class Content : BaseEntity
    {
        [Column(TypeName = "ntext")]
        public string ArabicText { get; set; }
        [Column(TypeName = "ntext")]
        public string EnglishText { get; set; }
    }
}
