using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class ChangePasswordDataService
    {
        public async Task<HttpResponseMessage> ServicePassword(FormUrlEncodedContent FormContent)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix + Resources.ChangePasswordURI, FormContent);
            return response;
        }
    }
}
