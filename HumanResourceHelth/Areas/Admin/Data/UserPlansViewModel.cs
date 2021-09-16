using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class UserPlansViewModel
    {
        public List<UserPlan> UserPlans { get; set; }
        public User User { get; set; }
        public List<Plan> Plans { get; set; }
    }
}