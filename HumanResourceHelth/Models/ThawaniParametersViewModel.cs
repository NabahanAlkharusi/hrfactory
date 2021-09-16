using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web.Models
{
    public class ThawaniParametersViewModel
    {
        public string client_reference_id { get; set; }
        public string customer_id { get; set; }
        public List<products> products { get; set; }
        public string success_url { get; set; }
        public string cancel_url { get; set; }
        public metadata metadata { get; set; }
    }

    public class products
    {
        public string name { get; set; }
        public string unit_amount { get; set; }
        public string quantity { get; set; }

    }

    public class metadata
    {
        public int order_id { get; set; }
        public string customer { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
    }
}