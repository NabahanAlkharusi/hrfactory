using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace HumanResourceHelth.Model
{
    public class AppUser : IdentityUser
    {
        public Country Country { get; set; }

    }
}
