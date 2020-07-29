using Newtonsoft.Json;
using StoreBuy.Mappers;
using StoreBuy.Models;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class ItemCategoryDataService
    {
        public async Task<List<ItemCategory>> AllCategories()
        {
            using (var client = new HttpClient())
            {
                var uri = Resources.APIPrefix + Resources.GetCategoryItems;
                var response = await client.GetStringAsync(uri);
                var CategoryList = JSONConversion.DeserializeResponseToModel<List<ItemCategory>>(response);
                return CategoryList;
            }
        }
        public async Task<List<ItemCategory>> AllR10Categories()
        {
            List<ItemCategory> R10Categories = new List<ItemCategory>();
            using (var client = new HttpClient())
            {
                String SessionId = await GetSessionId();

                var Categoriesuri = Resources.RGWIP+"RetailGateway/api/catalog/v1/stores/0/products/categories?type=Merchandise";
                client.DefaultRequestHeaders.Add("Retailer", "Retailer");
                client.DefaultRequestHeaders.Add("TouchPoint", "SelfScanDevice");
                client.DefaultRequestHeaders.Add("R10SessionId", SessionId);
                HttpResponseMessage response2 = await client.GetAsync(Categoriesuri);
                var CategoryResponse = await response2.Content.ReadAsStringAsync();
                R10Categories= JsonToModel.MapFields(CategoryResponse);
            }
            return R10Categories;
        }
 

        private async Task<String> GetSessionId()
        {
            using (var client = new HttpClient())
            {
                var startUpuri = Resources.RGWIP + "RetailGateway/api/selling/v1/selfScan/Startup";
                var authArray = Encoding.ASCII.GetBytes("dms_user:dmsuser");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(authArray));
                client.DefaultRequestHeaders.Add("Retailer", "Retailer");
                client.DefaultRequestHeaders.Add("TouchPoint", "SelfScanDevice");

                string json = "{\"DomainName\": \"string\",\"RegistrationKey\":\"100;f54fa560-fbb3-465c-82fa-10414c38baa5\"}";
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response1 = await client.PostAsync(startUpuri, content);
                var StartUpResponse = await response1.Content.ReadAsStringAsync();
                dynamic StartUpJSON = JsonConvert.DeserializeObject<dynamic>(StartUpResponse);
                String SessionId = StartUpJSON.SessionId;
                return SessionId;
            }
        }
    }
}
