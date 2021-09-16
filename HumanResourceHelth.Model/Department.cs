using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HumanResourceHelth.Model
{
    public class Departments
    {
        [Key]
        [Display(Name = "DepartmentID", ResourceType = typeof(Resources.Departments.Departments))]
        public int DepartmentID { get; set; }

        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Name", ResourceType = typeof(Resources.Departments.Departments))]
        public string Name { get; set; }
        public ICollection<Courses> Courses { get; set; }
    }
}