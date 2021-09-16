using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class SurveyResult : BaseEntity
    {
        public int SurveyId { get; set; }
        public int IndicatorId { get; set; }
        public virtual Indicator Indicator { get; set; }
        public int Result { get; set; }
    }
}
