using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    [Table("HrSetting")]
    public  class HrSetting
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "LearnOnlineVideo", ResourceType = typeof(Resources.HrSetting.HrSetting))]
        public string  LearnOnlineVideo { get; set; }
   
        [Display(Name = "LearnOnlineText", ResourceType = typeof(Resources.HrSetting.HrSetting))]
        public string  LearnOnlineText { get; set; }
    }
}
