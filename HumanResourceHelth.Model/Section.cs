using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Section : BaseEntity
    {
        [MaxLength(50)]
        public string Title { get; set; }
        public int? Ordering { get; set; }
        [ForeignKey("Childs")]
        public int? ParenId { get; set; }
        public virtual List<Section> Childs { get; set; }
        [Column(TypeName = "ntext")]
        public string Description { get; set; }
        [Column(TypeName ="ntext")]
        public string Content { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public bool IsActive { get; set; }
        public int LanguageId { get; set; }
        public bool IsHaveLineBefore { get; set; } = false;
        public int CountryID { get; set; }
        public int SectionId { get; set; }
    }
}
