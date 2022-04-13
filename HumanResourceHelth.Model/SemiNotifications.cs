using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class SemiNotifications: BaseEntity
    {
        [Required]
        public int UserId { get; set; }
        [Column(TypeName ="ntext")]
        public string ReadNotifi { get; set; }
        [Column(TypeName = "ntext")]
        public string UnReadNotifi { get; set; }
    }
}
