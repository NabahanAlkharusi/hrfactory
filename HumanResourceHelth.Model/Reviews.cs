using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model
{
  public class Reviews
    {
        [Key]
        public int ReviewID { get; set; }
        [Display(Name = "UserID", ResourceType = typeof(Resources.AppResource))]
        public string UserID { get; set; }
        [Display(Name = "CourseID", ResourceType = typeof(Resources.AppResource))]
        public int CourseID { get; set; }
        [Display(Name = "ReviewBody", ResourceType = typeof(Resources.AppResource))]
        public string ReviewBody { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        [Display(Name = "ReviewDate", ResourceType = typeof(Resources.AppResource))]
        public DateTime    ReviewDate  { get; set; }


}
}
