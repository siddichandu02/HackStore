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
    class CartPageDataService
    {
        private static ISettings AppSettings => CrossSettings.Current;
        public async Task<List<ItemCatalogueModel>> GetServiceCartItems()
        {
            using (var client = new HttpClient())
            {
                var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
                var uri = Resources.APIPrefix + Resources.GetCartItems + UserId;
                var response = await client.GetStringAsync(uri);
                var ItemCatalogue = JSONConversion.DeserializeResponseToModel<List<ItemCatalogueModel>>(response);
                return ItemCatalogue;
            }
        }

        public async Task<HttpResponseMessage> ServiceRemove(object obj)
        {
            var SelectedItem = (ItemCatalogueModel)obj;
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            var formcontent = new FormUrlEncodedContent(new[]
                                                        {
                                                            new KeyValuePair<string,string>(Resources.UserId,UserId),
                                                            new KeyValuePair<string, string>(Resources.ItemId,SelectedItem.ItemId.ToString()),
                                                        });
            using (var client = new HttpClient())
            {
                var uri = Resources.APIPrefix + Resources.DeleteCartItem;
                HttpResponseMessage response = await client.PostAsync(uri, formcontent);
                return response;
            }

        }

        public async Task<HttpResponseMessage> ServiceQuantityUpdate(ItemCatalogueModel SelectedItem)
        {
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            var formcontent = new FormUrlEncodedContent(new[]
              {
                        new KeyValuePair<string,string>(Resources.UserId,UserId),
                        new KeyValuePair<string, string>(Resources.ItemId,SelectedItem.ItemId.ToString()),
                        new KeyValuePair<string, string>(Resources.Quantity,SelectedItem.Quantity.ToString())
                    });
            using (var client = new HttpClient())
            {
                var uri = Resources.APIPrefix + Resources.UpdateItemQuantity;
                HttpResponseMessage response = await client.PostAsync(uri, formcontent);
                return response;
            }
        }

    }
}
