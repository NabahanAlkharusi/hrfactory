using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Data
{
    public class AnswersViewModel
    {
        public int QuestionId { get; set; }
        public int Answer { get; set; }
        public int IndicatorId { get; set; }
    }
}