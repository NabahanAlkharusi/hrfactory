using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Answer : BaseEntity
    {
        public int SurveyId { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public int Mark { get; set; }
    }
}
