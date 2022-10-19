using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Audience
    {
        public Dictionary<int, string> audienceCat { get; }
        public Dictionary<int, string> audienceCatAR { get; }
        public Audience()
        {
            audienceCat = new Dictionary<int, string>();
            audienceCatAR = new Dictionary<int, string>();
            audienceCat.Add(1, "Only HR Factory App Members");
            audienceCat.Add(2, "Only HR Employees");
            audienceCat.Add(3, "Only Employees");
            audienceCat.Add(4, "Only Managers");
            audienceCat.Add(5, "Only HR Employees & Employees");
            audienceCat.Add(6, "Only Managers & Employees");
            audienceCat.Add(7, "Only Managers & HR Employees");
            audienceCat.Add(8, "Only Managers, HR Employees & Employees");
            audienceCat.Add(9, "All Employees");
            audienceCat.Add(10, "Public");
            audienceCatAR.Add(1, "أعضاء HR Factory App فقط");
            audienceCatAR.Add(2, "موظفي الموارد البشرية فقط");
            audienceCatAR.Add(3, "الموظفين فقط");
            audienceCatAR.Add(4, "المدراء فقط");
            audienceCatAR.Add(5, "موظفي الموارد البشرية وباقي الموظفين فقط");
            audienceCatAR.Add(6, "المدراء والموظفين فقط");
            audienceCatAR.Add(7, "المدراء وموظفي الموارد البشرية فقط");
            audienceCatAR.Add(8, "المدراء وموظفي الموارد البشرية وباقي الموظفين فقط");
            audienceCatAR.Add(9, "جميع الموظفين");
            audienceCatAR.Add(10, "للعامة");


        }
    }
}
