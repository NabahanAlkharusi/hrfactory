using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class Respondent
    {
        public Dictionary<int, string> respondent { get; }
        public Dictionary<int, string> respondentAR { get; }
        public Respondent()
        {
            respondent = new Dictionary<int, string>();
            respondentAR = new Dictionary<int, string>();
            respondent.Add(1, "Only HR Employees");
            respondent.Add(2, "Only Employees");
            respondent.Add(3, "Only Managers");
            respondent.Add(4, "Only HR Employees & Employees");
            respondent.Add(5, "Only Managers & Employees");
            respondent.Add(6, "Only Managers & HR Employees");
            respondent.Add(7, "All Employees");
            respondent.Add(8, "Public");

            respondentAR.Add(1, "موظفي الموارد البشرية فقط");
            respondentAR.Add(2, "الموظفين فقط");
            respondentAR.Add(3, "المدراء فقط");
            respondentAR.Add(4, "موظفي الموارد البشرية وباقي الموظفين فقط");
            respondentAR.Add(5, "المدراء والموظفين فقط");
            respondentAR.Add(6, "المدراء وموظفي الموارد البشرية فقط");
            respondentAR.Add(7, "جميع الموظفين");
            respondentAR.Add(8, "للعامة");


        }
    }
}
