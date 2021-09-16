using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model
{
    public class Developer
    {
        public int Id { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        public int Followers { get; set; }
    }
}
