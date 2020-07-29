using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Models.Detail;
using StoreBuy.Views.Catalog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Transaction
{
    
    [Preserve(AllMembers = true)]
    public class OrderViewModel : BaseViewModel
    {
        #region Fields

        private string orderIcon;
        private string storeName;
        private string locationName;
        private string slot;
        private INavigation Navigation;
        private string message;
        private string qrtext;
        private Command okCommand;
        private bool displayPopup = true;

        private static ISettings AppSettings => CrossSettings.Current;

        #endregion

        #region Constructor

        public OrderViewModel(INavigation navigation,string SlotDate, string SlotTime, StoreModel Store)
        {
            
            Navigation = navigation;
            StoreName = Store.StoreName;
            LocationName = Store.StoreAddress;
            Slot = SlotDate + " " + SlotTime;
            PlaceOrder(Store.StoreId,SlotDate,SlotTime);
            
        }
        #endregion

        #region Commands


        #endregion

        #region Properties

        public string OrderIcon
        {
            get
            {
                return  orderIcon;
            }

            set
            {
                 orderIcon = value;
                NotifyPropertyChanged();
            }
        }

        public string QRText
        {
            get
            {
                return qrtext;
            }

            set
            {
                qrtext = value;
                NotifyPropertyChanged();
            }
        }

        public bool DisplayPopup
        {
            get
            {
                return displayPopup;
            }

            set
            {
                displayPopup = value;
                NotifyPropertyChanged();
            }
        }

        public string StoreName
        {
            get
            {
                return  storeName;
            }

            set
            {
                 storeName = value;
                NotifyPropertyChanged();
            }
        }
        public string LocationName
        {
            get
            {
                return  locationName;
            }

            set
            {
                 locationName = value;
                NotifyPropertyChanged();
            }
        }
        public string Slot
        {
            get
            {
                return  slot;
            }

            set
            {
                 slot = value;
                NotifyPropertyChanged();
            }
        }
        public string Message
        {
            get
            {
                return  message;
            }

            set
            {
                 message = value;
                NotifyPropertyChanged();
            }
        }


        #endregion

        #region Methods
        private async void PlaceOrder(long StoreId, string SlotDate, string SlotTime)
        {
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            OrderDataService orderData = new OrderDataService();
            var formContent = MakeFormContent(UserId, StoreId, SlotDate, SlotTime);
            HttpResponseMessage response = await orderData.ServicePlaceOrder(formContent);
            var status = response.StatusCode;
            string result = await response.Content.ReadAsStringAsync();       
            if (status == HttpStatusCode.OK)
            {
                QRText = Slot;
                OrderIcon = "OrderSuccess.svg";
                 Message = Resources.SlotSuccess;
                DisplayPopup = false;
            }
            else
            {
                OrderIcon = "OrderFailure.svg";
                 Message = Resources.SlotUnsuccess;
            }
        }
        private FormUrlEncodedContent MakeFormContent(string UserId, long StoreId, string SlotDate,string SlotTime)
        {

            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.UserId,UserId.ToString()),
                        new KeyValuePair<string,string>(Resources.StoreId,StoreId.ToString()),
                        new KeyValuePair<string,string>(Resources.SlotDate,SlotDate),
                        new KeyValuePair<string,string>(Resources.SlotTime,SlotTime)

            });
            return formcontent;
        }

        #endregion

        public Command OKCommand
        {
            get { return okCommand ?? (okCommand = new Command(OKClicked)); }
        }

        private async void OKClicked(object obj)
        {
            await Navigation.PushAsync(new CategoryTilePage());
        }
    }
}
