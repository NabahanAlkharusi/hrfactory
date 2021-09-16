using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model
{
    public class Trainers 
    {
        [Key]
        [Display(Name = "TrainerID", ResourceType = typeof(Resources.Trainers.Trainers))]
        public int TrainerID { get; set; }
        [StringLength(500)]
        [Display(Name = "TrainerName", ResourceType = typeof(Resources.Trainers.Trainers))]
        public string TrainerName { get; set; }
        [Display(Name = "SpecialtyID", ResourceType = typeof(Resources.Trainers.Trainers))]
        public int SpecialtyID { get; set; }
        [Display(Name = "TrainerBio", ResourceType = typeof(Resources.Trainers.Trainers))]
        public string TrainerBio { get; set; }
        [Display(Name = "TrainerAvatar", ResourceType = typeof(Resources.Trainers.Trainers))]
        public string TrainerAvatar { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        [Display(Name = "CreatedDate", ResourceType = typeof(Resources.AppResource))]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "CreatedBy", ResourceType = typeof(Resources.AppResource))]
        public string  CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        [Display(Name = "ModifiedDate", ResourceType = typeof(Resources.AppResource))]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "ModifiedBy", ResourceType = typeof(Resources.AppResource))]
        public string  ModifiedBy { get; set; }
        [Display(Name = "isActive", ResourceType = typeof(Resources.AppResource))]
        public bool isActive { get; set; }

        public Specialties Specialtie { get; set; }
        public string ImagePath { get; set; }
        //public ICollection<Attachments> Attachments { get; set; }
    }
}
