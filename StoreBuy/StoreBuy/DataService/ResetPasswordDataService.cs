using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class ResetPasswordDataService
    {
        public async Task<HttpResponseMessage> ServiceResetPassword(FormUrlEncodedContent FormContent)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix + Resources.ResetPasswordURI, FormContent);
            return response;
        }
    }
}
