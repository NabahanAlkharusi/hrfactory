using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceHelth.Model
{
    public class PaymentMethodOptions
    {
        public Dictionary<int, string> PaymentMethodOptionsList { get; }
        public Dictionary<int, string> PaymentMethodOptionsListAr { get; }
        public PaymentMethodOptions()
        {
            PaymentMethodOptionsList = new Dictionary<int, string>();
            PaymentMethodOptionsListAr = new Dictionary<int, string>();
            PaymentMethodOptionsList.Add(1, "Free");
            PaymentMethodOptionsList.Add(2, "On Service Required Payment");
            PaymentMethodOptionsList.Add(3, "Subscribe");
            PaymentMethodOptionsListAr.Add(1, "مجاني");
            PaymentMethodOptionsListAr.Add(2, "الدفع عند استخدام الخدمة");
            PaymentMethodOptionsListAr.Add(3, "اشتراك");
            

            
        }
    }
}
