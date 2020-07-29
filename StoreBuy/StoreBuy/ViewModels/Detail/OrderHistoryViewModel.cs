using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Models.Detail;
using StoreBuy.Views.Detail;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace StoreBuy.ViewModels.Detail
{
    public class OrderHistoryViewModel : BaseViewModel
    {
        #region Fields

        private ObservableCollection<OrderModel> orders;
        private static ISettings AppSettings => CrossSettings.Current;

        private Command viewItemsCommand;

        private INavigation Navigation;



        #endregion

        #region Properties

        private Command viewQRcodeCommand;

        public ObservableCollection<OrderModel> OrderDetails
        {
            get { return  orders; }
            set
            {
                if ( orders == value)
                {
                    return;
                }
                 orders = value;
                 NotifyPropertyChanged();
            }
        }
        #endregion

        #region Constructor
        public OrderHistoryViewModel(INavigation navigation)
        {
             Navigation = navigation;
            GetOrders();
        }

        #endregion

        #region Methods

        public async void GetOrders()
        {
            OrderHistoryDataService orderHistoryData = new OrderHistoryDataService();
            var OrdersList = await orderHistoryData.GetOrderList();
            List<OrderModel> SortedList = OrdersList.OrderByDescending(o => o.SlotDateTime).ToList();
            OrderDetails = new ObservableCollection<OrderModel>(SortedList);
            
        }

       
        private async void ViewItemsClicked(object obj)
        {
            var orderId = (long)obj;
            OrderModel SelectedOrder = OrderDetails.Where(x => x.OrderId == orderId).SingleOrDefault();
            List<ItemCatalogueModel> ItemsList = SelectedOrder.ListItems;
            await Navigation.PushAsync(new StoreItemPage(ItemsList));
        }

        #endregion

        #region Command

       
        public Command ViewItemsCommand
        {
            get { return  viewItemsCommand ?? ( viewItemsCommand = new Command( ViewItemsClicked)); }

        }
        
            public Command ViewQRcodeCommand
        {
            get { return  viewQRcodeCommand ?? ( viewQRcodeCommand = new Command( ViewQRcodeClicked)); }

        }

        private async void ViewQRcodeClicked(object obj)
        {
            var Order = (OrderModel)obj;
            var Slot = Order.SlotDate + " " + Order.SlotTime;
            await Navigation.PushAsync(new QRCodePage(Slot));
        }


        #endregion
    }
}
