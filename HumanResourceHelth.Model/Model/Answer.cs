using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
   public class Answer
    {
        public int Id { get; set; }
        [ForeignKey("Survey")]
        public int SurveyId { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }
        public int? Answerr { get; set; }
    }
}
