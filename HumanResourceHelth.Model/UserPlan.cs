using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class UserPlan : BaseEntity
    {
        
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public double Price { get; set; } = 0;

        public bool IsFreeUsed { get; set; }
        public bool IsFreeActive { get; set; }

    }
}
