using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Question : BaseEntity
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int IndicatorId { get; set; }
        public string Practice { get; set; }
        public string PracticeAr { get; set; }
        public virtual Indicator Indicator { get; set; }
        public virtual List<Answer> Answers { get; set; }
    }
}
