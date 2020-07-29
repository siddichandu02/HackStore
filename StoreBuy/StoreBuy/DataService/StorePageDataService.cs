using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Models.Detail;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class StorePageDataService
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public async Task<List<StoreModel>> GetStoresByItemCount(long Latitude, long Longitude)
        {
            using (var client = new HttpClient())
            {
                var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
                var uri = Resources.APIPrefix + Resources.FindStoreByItemsURI + UserId + Resources.Latitude + Latitude + Resources.Longitude + Longitude;
                var response = await client.GetStringAsync(uri);
                var StoreList = JSONConversion.DeserializeResponseToModel<List<StoreModel>>(response);
                var CartItemCount = AppSettings.GetValueOrDefault(Resources.CartItemCount, Resources.DefaultIntValue);
                var IntCartItemCount = Int64.Parse(CartItemCount);
                foreach (StoreModel Store in StoreList)
                {
                    Store.ItemsCount = Resources.AvailableItems + Store.ListItems.Count + "/" + IntCartItemCount;
                }
                return StoreList;
            }
        }

        public async Task<List<StoreModel>> GetStoresByDistance(long Latitude, long Longitude)
        {
            using (var client = new HttpClient())
            {
                var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
                var uri = Resources.APIPrefix + Resources.FindStoreByDistanceURI + UserId + Resources.Latitude + Latitude + Resources.Longitude + Longitude;
                var response = await client.GetStringAsync(uri);
                var StoreList = JSONConversion.DeserializeResponseToModel<List<StoreModel>>(response);
                var CartItemCount = AppSettings.GetValueOrDefault(Resources.CartItemCount, Resources.DefaultIntValue);
                var IntCartItemCount = Int64.Parse(CartItemCount);
                foreach (StoreModel Store in StoreList)
                {
                    Store.ItemsCount = Resources.AvailableItems + Store.ListItems.Count + "/" + IntCartItemCount;
                }
                return StoreList;
            }
        }
    }
}
