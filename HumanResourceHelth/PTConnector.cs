using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;

namespace HumanResourceHelth.Web
{
    public class PTConnector
    {
        static string pt_endpoint = "https://secure-oman.paytabs.com/";
        //static int pt_profile_id = 92253;//test
        static int pt_profile_id = 92888;//live
        //static string pt_server_key = "SZJN2RT2TM-JDGJT62RJM-NDHJKNJR2D";//test
        static string pt_server_key = "SKJN2RT2ZT-JD6DMZTLL6-ZL6BL9GRDB";//live


        //

        public static async Task<PaypageResponse> CreatePayment(
            double amount, string currency, string order_id, string desc, bool frammed, string return_url, string callback_url)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new RestClient(pt_endpoint);

            var args = new PaypageParams
            {
                profile_id = pt_profile_id,

                cart_amount = amount,
                cart_currency = currency,
                cart_id = order_id,
                cart_description = desc,

                framed = frammed,

                return_url = return_url,
                callback = callback_url
            };

            var request = new RestRequest("payment/request");
            request.AddHeader("Authorization", pt_server_key);

            var body = JsonConvert.SerializeObject(args);
            //request.AddJsonBody(body);
            request.AddParameter("text/plain", body, ParameterType.RequestBody);


            try
            {
                
                var response = await client.ExecutePostAsync(request);

                if (response.IsSuccessful && response.Content != null)
                {
                    var pt_paypage = JsonConvert.DeserializeObject<PaypageResponse>(response.Content);
                    if (pt_paypage != null && pt_paypage.IsSuccessful())
                    {
                        return pt_paypage;
                    }
                }

                Console.Error.WriteLine(response.Content);
                Console.Error.WriteLine(response.ErrorMessage);

                return new PaypageResponse { message = response.Content };
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
                return new PaypageResponse { message = ex.Message };
            }
        }
    }
}