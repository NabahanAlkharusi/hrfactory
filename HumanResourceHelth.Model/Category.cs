using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Category : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string NameAr { get; set; }
        public bool isActive { get; set; }
        [Required]
        public int CategoryPlan { get; set; }
        public virtual List<DocFile> Documents { get; set; }
    }
}
