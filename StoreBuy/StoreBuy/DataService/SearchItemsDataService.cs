using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Models;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class SearchItemsDataService
    {
        private static ISettings AppSettings => CrossSettings.Current;
        public List<ItemCatalogueModel> CartItems;
        private long CartItemCount;
        public async Task<long> GetCartServiceCount(string UserId)
        {
            var URI = Resources.APIPrefix + Resources.GetCartItems + UserId;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(URI);
            CartItems = JSONConversion.DeserializeResponseToModel<List<ItemCatalogueModel>>(response);
            CartItemCount = CartItems.Count;
            return CartItemCount;
        }

        public async Task<List<ItemCatalogueModel>> GetCartServiceItem(string SearchString)
        {
            var URI = Resources.APIPrefix + Resources.ItemList + SearchString;
            HttpClient client = new HttpClient();
            var response = await client.GetStringAsync(URI);
            var ItemList = JSONConversion.DeserializeResponseToModel<List<ItemCatalogueModel>>(response);
            foreach (ItemCatalogueModel Item in ItemList)
            {
                Item.Quantity = 1;
            }

            return ItemList;
        }

        public async Task<HttpResponseMessage> AddItemToCartService(object obj)
        {
            ItemCatalogueModel Item = (ItemCatalogueModel)obj;
            long Quantity = Item.Quantity;
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            var formContent = MakeFormContent(UserId, Item.ItemId, Quantity);
            string url = Resources.APIPrefix + Resources.AddToCart;
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.PostAsync(url, formContent);
            return response;
        }

        private FormUrlEncodedContent MakeFormContent(string UserId, long ItemId, long Quantity)
        {

            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.UserId,UserId),
                        new KeyValuePair<string,string>(Resources.ItemId,ItemId.ToString()),
                        new KeyValuePair<string,string>(Resources.Quantity,Quantity.ToString())
            });
            return formcontent;
        }
    }
}
