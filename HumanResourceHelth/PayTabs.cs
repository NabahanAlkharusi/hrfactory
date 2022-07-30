using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HumanResourceHelth.Web
{
    public class PayTabs
    {
        //public int profile_id = 92253;//test
        public int profile_id = 92888;//live
        public string tran_type = "sale";
        public string tran_class = "ecom";
        public string cart_id = "4244b9fd-c7e9-4f16-8d3c-4fe7bf6c48ca";
        public string cart_description = "desc";
        public string cart_currency = "OMR";
        public double cart_amount = 1.999;
        public bool hide_shipping = true;
        public string callback = "https://";
        public string returnurl = "https://";
        public string key = "SKJN2RT2ZT-JD6DMZTLL6-ZL6BL9GRDB";//live
        //public string key = "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D";//test
        public bool framed = true;
        public string Fullname = "compnay Name";
        public string Email = "youremail@example.com";
        public string Address = "Oman";
        public string City = "Muscat";
        public string Country = "OM";

    }

    public class PaypageParams
    {
        public int profile_id = 0;

        public string tran_type = "sale";
        public string tran_class = "ecom";

        public double cart_amount = 0;
        public string cart_currency = "OMR";
        public string cart_id = "biolab_id_";
        public string cart_description = "";

        public bool framed = false;
        public bool framed_return_top = true;

        public bool hide_shipping = true;

        [JsonProperty("return")]
        public string return_url = "";

        public string callback = "";
    }

    public class PaypageResponse
    {
        public string tran_ref;
        public string cart_id;
        public string redirect_url;
        public string trace;

        public string message;

        public bool IsSuccessful()
        {
            return redirect_url != null;
        }
    }


    public class PaytabsReturnResponse: IHttpHandler
    {
        public PaytabsReturnResponse()
        {
        }

        public string tranRef { get; set; }

        public string respCode { get; set; }
        public string respMessage { get; set; }
        public string respStatus { get; set; }

        public string acquirerMessage { get; set; }
        public string acquirerRRN { get; set; }

        public string cartId { get; set; }
        public string customerEmail { get; set; }

        public string signature { get; set; }

        public string token { get; set; }

        public bool IsReusable => true;

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class PaytabsIPNResponse
    {
        public int merchant_id { get; set; }
        public int profile_id { get; set; }

        public string tran_ref { get; set; }
        public string tran_type { get; set; }

        public float cart_amount { get; set; }
        public string cart_currency { get; set; }
        public string cart_id { get; set; }
        public string cart_description { get; set; }

        public string tran_class { get; set; }
        public string tran_currency { get; set; }
        public float tran_total { get; set; }

        public PaymentResult payment_result { get; set; }

        //public CustomerDetails customer_details;
        //public Payment_Info payment_info;
    }

    public class PaymentResult
    {
        public string response_status { get; set; }
        public string response_code { get; set; }
        public string response_message { get; set; }

        public string cvv_result { get; set; }
        public string avs_result { get; set; }

        public DateTime transaction_time { get; set; }
    }
}