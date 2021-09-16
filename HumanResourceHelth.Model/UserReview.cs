using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model
{
    public  class UserReview
    {
        [Key]
        public int ReviewID { get; set; }
        public int CourseID { get; set; }
        public string ReviewUser { get; set; }
        public string ReviewText { get; set; }
        public int FeedBackCount { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        public DateTime ReviewDate { get; set; }
    }
}
