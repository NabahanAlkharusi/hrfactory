using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Indicator : BaseEntity
    {
        public string Name { get; set; }
        public string NameAr { get; set; }
        public int SurveyTypeId { get; set; }
        public virtual SurveyType SurveyType { get; set; }
        public virtual List<Question> Questions { get; set; }
    }
}
