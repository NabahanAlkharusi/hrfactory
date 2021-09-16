using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class UserCourse : BaseEntity
    {
        public int UserId { get; set; }
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        public DateTime PurchaseDate { get; set; }
        public double Price { get; set; }
        public bool IsPurchasedViaAdmin { get; set; }
    }
}
