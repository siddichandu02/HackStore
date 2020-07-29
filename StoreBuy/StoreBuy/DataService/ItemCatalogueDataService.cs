using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Mappers;
using StoreBuy.Models;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class ItemCatalogueDataService
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public async Task<List<ItemCatalogueModel>> GetR10ServiceItems(long ItemCategoryId)
        {
            List<ItemCatalogueModel> R10Categories = new List<ItemCatalogueModel>();
            using (var client = new HttpClient())
            {
                String SessionId = await GetSessionId();

                var Categoriesuri = Resources.RGWIP+"RetailGateway/api/catalog/v1/stores/10072/products?category="+ItemCategoryId+"&startIndex=0&count=100";
                client.DefaultRequestHeaders.Add("Retailer", "Retailer");
                client.DefaultRequestHeaders.Add("TouchPoint", "SelfScanDevice");
                client.DefaultRequestHeaders.Add("R10SessionId", SessionId);
                client.DefaultRequestHeaders.Add("TouchPointID", "100");
                HttpResponseMessage response2 = await client.GetAsync(Categoriesuri);
                var CategoryResponse = await response2.Content.ReadAsStringAsync();
                R10Categories = JsonToModel.MapItemFields(CategoryResponse);
            }
            return R10Categories;
        }

        public async Task<List<ItemCatalogueModel>> GetServiceItems(long ItemCategoryId)
        {
            using (var client = new HttpClient())
            {

                var URI = Resources.APIPrefix + Resources.GetItemsOfCategory + ItemCategoryId;
                var response = await client.GetStringAsync(URI);
                var ItemCatalogue = JSONConversion.DeserializeResponseToModel<List<ItemCatalogueModel>>(response);
                foreach (ItemCatalogueModel Item in ItemCatalogue)
                {
                    Item.Quantity = 1;
                }
                return ItemCatalogue;

            }

        }
        private async Task<String> GetSessionId()
        {
            using (var client = new HttpClient())
            {
                var startUpuri = Resources.RGWIP+"RetailGateway/api/selling/v1/selfScan/Startup";
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
