using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace HumanResourceHelth.Model
{
    public class Attachments
    {
        [Key]
        [Display(Name = "AttachID", ResourceType = typeof(Resources.Attachments.Attachments))]
        public int AttachID { get; set; }
        [StringLength(500)]
        [Display(Name = "AttachName", ResourceType = typeof(Resources.Attachments.Attachments))]
        public string AttachName { get; set; }
        [Display(Name = "CourseID", ResourceType = typeof(Resources.Attachments.Attachments))]
        public int CourseID { get; set; }
        [StringLength(200)]
        [Display(Name = "AttachType", ResourceType = typeof(Resources.Attachments.Attachments))]
        public string AttachType { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "AttachPrice", ResourceType = typeof(Resources.Attachments.Attachments))]
        public decimal AttachPrice { get; set; }
        [StringLength(500)]
        [Display(Name = "ExternalLink", ResourceType = typeof(Resources.Attachments.Attachments))]
        public string ExternalLink { get; set; }
        [StringLength(500)]
        [Display(Name = "InternalLink", ResourceType = typeof(Resources.Attachments.Attachments))]
        public string InternalLink { get; set; }
        [StringLength(500)]
        [Display(Name = "FilePath", ResourceType = typeof(Resources.Attachments.Attachments))]
        public string FilePath { get; set; }
        [Display(Name = "Isfree", ResourceType = typeof(Resources.Attachments.Attachments))]
        public bool Isfree { get; set; }
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
        [Display(Name = "CourseID", ResourceType = typeof(Resources.Attachments.Attachments))]
        public Courses Course { get; set; }
        

    }
}
