using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class HomeViewModel
    {
        public int FreeIndicators { get; set; }
        public int BusinessIndicators { get; set; }
        public int PaidIndicators { get; set; }
        public int Users { get; set; }
        public int Countries { get; set; }
        public int Industreis { get; set; }
        public int TechRequests { get; set; }
        public int PluginRequests { get; set; }
        public int Builders { get; set; }
        public int Experts { get; set; }
        public int Courses { get; set; }
        public int SurveyTypes { get; set; }
        public int Terms { get; set; }
        public int Updates { get; set; }
        public int Files { get; set; }
    }
}