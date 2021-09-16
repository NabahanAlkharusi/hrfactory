using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class User : BaseEntity
    {
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MaxLength(50)]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string ContactInformation { get; set; }

        [MaxLength(255)]
        public string ContactPerson { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsAdmin { get; set; }
        
        [Required]
        public int NumberOFEmployees { get; set; }
        [Required]
        public int IndustryId { get; set; }
        [Required]
        public int CountryId { get; set; }
        public virtual Country Country { get; set; }
        public virtual Industry Industry { get; set; }
        public virtual List<Section> Sections { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public virtual List<UserWatchVideo> UserWatchVideos { get; set; }
        public virtual List<UserPlan> UserPlans { get; set; }
        public string DocumentName { get; set; }

        public string NameAr { get; set; }
        public string DocumentNameAr { get; set; }
        public string ThawaniKey { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
