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
    class OrderHistoryDataService
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public async Task<List<OrderModel>> GetOrderList()
        {
            using (var client = new HttpClient())
            {
                var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
                var uri = Resources.APIPrefix + Resources.GetOrders + UserId;
                var response = await client.GetStringAsync(uri);
                var OrdersList = JSONConversion.DeserializeResponseToModel<List<OrderModel>>(response);
                foreach (OrderModel Order in OrdersList)
                {
                    var DateAndTime = Order.SlotDate + " " + Order.SlotTime.Substring(0, 5);
                    Order.SlotDateTime = DateTime.ParseExact(DateAndTime, Resources.DateTimeFormat,
                                           System.Globalization.CultureInfo.InvariantCulture);
                    if (Order.SlotDateTime > DateTime.Now)
                    {
                        Order.QRCodeIsVisible = true;
                    }
                }
                return OrdersList;
            }
        }
    }
}
