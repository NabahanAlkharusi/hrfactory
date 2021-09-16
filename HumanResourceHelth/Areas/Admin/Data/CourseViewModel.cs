using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Areas.Admin.Data
{
    public class CourseViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Rate { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime PublishDate { get; set; }
        public DateTime LastModifidDate { get; set; }

    }
}