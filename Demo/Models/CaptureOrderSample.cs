using PayPalHttp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;

namespace Demo.Models
{
    public class CaptureOrderSample
    {
        public async static Task<HttpResponse> CaptureOrder(string OrderId, bool debug = false)
        {
            var request = new OrdersCaptureRequest(OrderId);
            request.Prefer("return=representation");
            request.RequestBody(new OrderActionRequest());
            var response = await PayPalClient.client().Execute(request);
            return response;
        }

      
    }
}
