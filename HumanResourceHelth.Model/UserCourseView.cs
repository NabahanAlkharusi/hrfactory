using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HumanResourceHelth.Model
{
    public class UserCourseView
    {
        [Key]
        public int UserViewID { get; set; }
        public string UserID { get; set; }
        public int AttachID { get; set; }
        public int CourseID { get; set; }
    }
}
