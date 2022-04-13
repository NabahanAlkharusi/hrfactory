using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class DocFile : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string EngDocPath { get; set; }
        [Required]
        public string ArbDocPath { get; set; }
        
        public string EngVedio { get; set; }
        
        public string ArbVedio { get; set; }
        [Required]
        public bool isFileFree { get; set; }
        public bool isFileActive { get; set; }
        public bool isvedioYouTube { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int CategoryId { get; set; }
    }
}
