using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model.Entities
{
    public class Login
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(Resources.AppResource))]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Password", ResourceType = typeof(Resources.AppResource))]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
