using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Data
{
    public class SurveyViewModel
    {
        public Answer Answer { get; set; }
        public List<Indicator> indicators { get; set; }
     }
}