using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HumanResourceHelth.Model;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class PlanViewModel
    {
        public PlanViewModel()
        {
            plansList = new List<SelectListItem>();
            var items = Enum.GetValues(typeof(SubscriptionPlan));
            foreach(var item in items)
            {
                string name = Enum.GetName(typeof(SubscriptionPlan), item);
                plansList.Add(new SelectListItem() { Text = name, Value = item.ToString() });
            }
        }

        public DirectoryInfo[] DirectoriesEnglish { get; set; }
        public DirectoryInfo[] DirectoriesArabic { get; set; }
        public List<SelectListItem> plansList { get; set; }
        public string plan { get; set; }
    }
}