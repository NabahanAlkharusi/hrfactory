using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HumanResourceHelth.Model
{
    public class Specialties
    {
        [Key]
        [Display(Name = "SpecialtyID", ResourceType = typeof(Resources.Specialties.Specialties))]
        public int SpecialtyID { get; set; }
    
        [StringLength(500)]
        [Display(Name = "SpecialtyName", ResourceType = typeof(Resources.Specialties.Specialties))]
        public string SpecialtyName { get; set; }
        public ICollection<Trainers> Trainers { get; set; }
    }
}