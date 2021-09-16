using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Models
{
    public class RegisterViewModels
    {
        public List<Country> Countries { get; set; }
        public List<Industry> Industries { get; set; }

        [Required]
        [MaxLength(255, ErrorMessage = "must be less than 255 charcter....")]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        [MaxLength(50)]
        public string Password { get; set; }

        [MaxLength(255, ErrorMessage = "must be less than 255 charcter....")]
        public string ContactInformation { get; set; }

        [MaxLength(255, ErrorMessage = "must be less than 255 charcter....")]
        public string ContactPerson { get; set; }
        public DateTime LastLoginDate { get; set; }
        public bool IsAdmin { get; set; }
        [Required]
        public int NumberOFEmployees { get; set; }
        public string NameAr { get; set; }
        public int CompanyId { get; set; }
        public int IndustryId { get; set; }
    }
}