using HumanResourceHelth.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class VideoViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int Lenght { get; set; }
        public int CourseId { get; set; }
        public bool IsForPriview { get; set; }
        public int Ordering { get; set; }
        public List<Course> Courses { get; set; }
    }
}