using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class OTPVerificationDataService
    {
        public async Task<HttpResponseMessage> ServiceVerifyOTP(FormUrlEncodedContent FormContent)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix + Resources.OTPVerifyURI, FormContent);
            return response;
        }
    }
}
