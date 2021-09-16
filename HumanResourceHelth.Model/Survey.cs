using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Survey : BaseEntity
    {
        public string Name { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public int SurveyTypeId { get; set; }
        public SurveyType SurveyType { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public virtual List<SurveyResult> SurveyResults { get; set; }
        public DateTime SurveyDate { get; set; }
    }
}
