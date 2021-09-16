using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using HumanResourceHelth.Model;
namespace HumanResourceHelth.Web.Data
{
    public class SubscriptionViewModel
    {
        public Plan Plan { get; set; }
        public UserPlan UserPlan{get;set;}
        public SubscriptionPeriod SubscriptionPeriod { get; set; }
    }
}