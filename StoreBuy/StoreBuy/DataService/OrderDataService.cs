using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class OrderDataService
    {
        public async Task<HttpResponseMessage> ServicePlaceOrder(FormUrlEncodedContent formContent)
        {
            string url = Resources.APIPrefix + Resources.PlaceOrder;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, formContent);
            return response;
        }
    }
}
