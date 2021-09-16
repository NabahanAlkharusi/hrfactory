using HumanResourceHelth.Model;
using iTextSharp.text.io;
using System.Collections.Generic;

namespace HumanResourceHelth.Web.Models
{
    public class SurveyResultViewModels
    {
        public Survey Survey { get; set; }
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public Answer Answer { get; set; }
        public Question Question { get; set; }
        public Indicator Indicator { get; set; }
        public SurveyResult SurveyResult { get; set; }
    }
}