using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class SignUpDataService
    {
        public async Task<HttpResponseMessage> ServiceSignUp(FormUrlEncodedContent FormContent)
        {
            string url = Resources.APIPrefix + Resources.UserCheck;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, FormContent);
            return response;
        }
    }
}
