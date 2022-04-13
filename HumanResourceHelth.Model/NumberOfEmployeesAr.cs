using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class NumberOfEmployeesAr
    {
        public Dictionary<int, string> EmployeesAr { get; }
        
        public NumberOfEmployeesAr()
        {
            EmployeesAr = new Dictionary<int, string>();
            EmployeesAr.Add(1, "من 1 إلى 10");
            EmployeesAr.Add(2, "من 11 إلى 50");
            EmployeesAr.Add(3, "من 51 إلى 100");
            EmployeesAr.Add(4, "أكثر من 100");
        }
    }
}
