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
            EmployeesAr.Add(2, "من 11 إلى 49");
            EmployeesAr.Add(3, "من 50 إلى 99");
            EmployeesAr.Add(4, "من 100 إلى 499");
            EmployeesAr.Add(5, "من 500 إلى 1000");
            EmployeesAr.Add(6, "أكثر من 1000");
        }
    }
}
