using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class ForgotPasswordDataService
    {
        public async Task<HttpResponseMessage> ServiceForgotPassword(FormUrlEncodedContent FormContent)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix + Resources.ForgotPasswordURI, FormContent);
            return response;
        }
    }
}
