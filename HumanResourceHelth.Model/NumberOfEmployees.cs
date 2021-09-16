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
            Employees.Add(2, "From 11 to 50");
            Employees.Add(3, "From 51 to 100");
            Employees.Add(4, "More Than 100");
        }
    }
}
