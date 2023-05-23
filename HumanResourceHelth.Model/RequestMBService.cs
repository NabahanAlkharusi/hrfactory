
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class RequestMBService : BaseEntity
    {
        //nullable field
        public int? UserID { get; set; }
        // Add service ID with message
        [Required, Display(Name = "Service")]
        public int ServiceID { get; set; }
        //Add nullable Organization Name field with message 
        [Display(Name = "Organization Name")]
        public string OrganizationName { get; set; } = "";
        // nullable string Number OF Employees field with message
        [Display(Name = "Number OF Employees")]
        public int? NumberOFEmployees { get; set; }
        // nullable string Industry Id field with message
        [ Display(Name = "Industry")]
        public int? IndustryId { get; set; }
        // string Country Id field with message
        [Required, Display(Name = "Country")]
        public int CountryId { get; set; }
        // string Contact Person field with message
        [Required, Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        // Nullable string Contact Person Email field with message
        [ Display(Name = "Contact Person Email")]
        public string ContactPEmail { get; set; }
        // Nullable Contact Person Phone field with message
        [Display(Name = "Contact Person Phone")]
        public string ContactPersonPhone { get; set; }
        // Nullable boolean Have Existing HR Manual policy field with message
        [Display(Name = "Have Existing HR Manual policy")]
        public bool? HaveExistingHRMP { get; set; }
        // Nullable string Existing HR Manual policy path field with message
        [Display(Name = "Existing HR Manual policy path")]
        public string ExistingHRMPPath { get; set; }
    }
}
