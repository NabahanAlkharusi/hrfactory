using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class IndicatorViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int SurveyTypeId { get; set; }
        public List<SurveyType> SurveyTypes { get; set; }
    }
}