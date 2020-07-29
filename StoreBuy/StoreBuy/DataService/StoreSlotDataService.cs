using Newtonsoft.Json;
using StoreBuy.Models.Detail;
using StoreBuy.Utilities;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace StoreBuy.DataService
{
    class StoreSlotDataService
    {
        private StoreModel Store;
        
        public async Task<List<string>> GetAllSlots(string DateIndex,string UserId, StoreModel store)
        {
            Store = store;
            using (var client = new HttpClient())
            {
                var uri = Resources.APIPrefix + Resources.GetFilledSlots + Store.StoreId + Resources.GetDate + DateIndex + Resources.GetUser + UserId;
                var response = await client.GetStringAsync(uri);
                var FilledSlots = JSONConversion.DeserializeResponseToModel<List<string>>(response);

                return FilledSlots;
            }

        }
    }
}
