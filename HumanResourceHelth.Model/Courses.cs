using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

 namespace HumanResourceHelth.Model
{
    public class Courses
    {
        [Key]
        [Display(Name = "CourseID", ResourceType = typeof(Resources.Courses.Courses))]
        public int CourseID { get; set; }
        [Display(Name = "CourseName", ResourceType = typeof(Resources.Courses.Courses))]
        public string  CourseName { get; set; }
        [Display(Name = "DepartmentID", ResourceType = typeof(Resources.Courses.Courses))]
        public int DepartmentID { get; set; }
        [Display(Name = "TrainerID", ResourceType = typeof(Resources.Courses.Courses))]
        public int TrainerID { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "CoursePrice", ResourceType = typeof(Resources.Courses.Courses))]
        public decimal CoursePrice { get; set; }
        [Display(Name = "CourseType", ResourceType = typeof(Resources.Courses.Courses))]
        public int CourseType { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "CourseDate", ResourceType = typeof(Resources.Courses.Courses))]
        public DateTime? CourseDate { get; set; }
        [Display(Name = "IntroText", ResourceType = typeof(Resources.Courses.Courses))]
        public string  IntroText { get; set; }
        [Display(Name = "IntroVideo", ResourceType = typeof(Resources.Courses.Courses))]
        public string IntroVideo { get; set; }
        [Display(Name = "Requirements", ResourceType = typeof(Resources.Courses.Courses))]
        public string Requirements { get; set; }
        [Display(Name = "Description", ResourceType = typeof(Resources.Courses.Courses))]
        public string Description { get; set; }
        [Display(Name = "isfree", ResourceType = typeof(Resources.Courses.Courses))]
        public bool isfree { get; set; }
        [Display(Name = "Avatar", ResourceType = typeof(Resources.Courses.Courses))]
        public string Avatar { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        [Display(Name = "CreatedDate", ResourceType = typeof(Resources.AppResource))]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "CreatedBy", ResourceType = typeof(Resources.AppResource))]
        public string  CreatedBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddT24:mm:ssK}", ApplyFormatInEditMode = true)]
        [Display(Name = "ModifiedDate", ResourceType = typeof(Resources.AppResource))]
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "ModifiedBy", ResourceType = typeof(Resources.AppResource))]
        public string  ModifiedBy { get; set; }
        [Display(Name = "isActive", ResourceType = typeof(Resources.AppResource))]
        public bool isActive { get; set; }

        [Display(Name = "Name", ResourceType = typeof(Resources.Departments.Departments))]
        public Departments Department { get; set; }
        public Trainers Trainer { get; set; }

        //public ICollection<Attachments> Attachments { get; set; }
    }
}
