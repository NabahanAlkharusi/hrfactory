using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class QuestionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public string Practice { get; set; }
        public string PracticeAr { get; set; }
        public int IndicatorId { get; set; }
        public List<Indicator> indicators { get; set; }
    }
}