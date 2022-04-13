using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Updates : BaseEntity
    {
        [Required]
        public string ArabicTitle { get; set; }
        [Required]
        public string EnglisTitle { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string ArabicText { get; set; }
        [Required]
        [Column(TypeName = "ntext")]
        public string EnglisText { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public DateTime IssuedDate { get; set; }
    }
}
