using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class NumberOfEmployees
    {
        public Dictionary<int, string> Employees { get; }
        public NumberOfEmployees()
        {
            Employees = new Dictionary<int, string>();
            Employees.Add(1, "From 1 to 10");
            Employees.Add(2, "From 11 to 49");
            Employees.Add(3, "From 50 to 99");
            Employees.Add(4, "From 100 to 499");
            Employees.Add(5, "From 500 to 1000");
            Employees.Add(6, "More Than 1000");
            
        }
    }
}
