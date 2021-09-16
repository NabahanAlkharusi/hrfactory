using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model.Model
{
    public class SurveyResult
    {
        public int Id { get; set; }
        public int? Result { get; set; }
        [ForeignKey("Survey")]
        public int? SurgeryId { get; set; }
        [ForeignKey("Indicator")]
        public int? IndicatorId { get; set; }
    }
}
