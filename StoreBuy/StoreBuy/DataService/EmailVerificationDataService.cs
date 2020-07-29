using Newtonsoft.Json;
using StoreBuy.Models;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class EmailVerificationDataService
    {
        public async Task<HttpResponseMessage> ServiceEmail(FormUrlEncodedContent FormContent)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(Resources.APIPrefix + Resources.OTPVerifyURI, FormContent);
            return response;
        }

        public async Task<HttpResponseMessage> ServiceUser(StringContent Content)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage Response = await client.PostAsync(Resources.APIPrefix + Resources.UserRegisterURI, Content);
            return Response;
        }

        public async Task<Users> ServiceGetUser(string Email)
        {
            HttpClient client = new HttpClient();
            var URI = Resources.APIPrefix + Resources.GetUserByUserNameURI + Email;
            var Userresponse = await client.GetStringAsync(URI);
            var User = JSONConversion.DeserializeResponseToModel< Users >(Userresponse);
            return User;
        }
    }
}
