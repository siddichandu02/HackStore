using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Views;
using StoreBuy.Views.Bookmarks;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.ErrorAndEmpty;
using StoreBuy.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.Catalog
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class SearchItemsPageViewModel : BaseViewModel
    {
        #region Fields
        private static ISettings AppSettings =>CrossSettings.Current;
        private Command addToCartCommand;
        private Command cardItemCommand;
        private Command backButtonCommand;
        private Command incQtyCommand;
        private Command decQtyCommand;
        private bool displayPopup;
        private long cartItemCount;
        private readonly INavigation Navigation;
        private ObservableCollection<ItemCatalogueModel> items;


        #endregion

        #region Constructor

        public SearchItemsPageViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
            
        }

        #endregion

        #region Properties

        [DataMember(Name = "ItemCatalogueModel")]
        public ObservableCollection<ItemCatalogueModel> ItemCatalogueList
        {
            get { return  items; }
            set
            {
                if ( items == value)
                {
                    return;
                }

                 items = value;
                 NotifyPropertyChanged();
            }
        }

        public bool DisplayPopup
        {
            get { return displayPopup; }
            set
            {
                if (displayPopup == value)
                {
                    return;
                }

                displayPopup = value;
                NotifyPropertyChanged();
            }
        }

        public long CartItemCount
        {
            get
            {
                return  cartItemCount;
            }
            set
            {
                 cartItemCount = value;
                 NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands
        public Command AddToCartCommand
        {
            get { return  addToCartCommand ?? ( addToCartCommand = new Command( AddToCartClicked)); }
        }


        public Command IncQtyCommand
        {
            get { return  incQtyCommand ?? ( incQtyCommand = new Command( IncQtyClicked)); }
        }

        public Command DecQtyCommand
        {
            get { return  decQtyCommand ?? ( decQtyCommand = new Command( DecQtyClicked)); }
        }

        public Command CardItemCommand
        {
            get { return  cardItemCommand ?? ( cardItemCommand = new Command( CartClicked)); }
        }

        public Command BackButtonCommand
        {
            get { return  backButtonCommand ?? ( backButtonCommand = new Command( BackButtonClicked)); }
        }

        #endregion

        #region Methods

        private void DecQtyClicked(Object obj)
        {
            ItemCatalogueModel Item = (ItemCatalogueModel)obj;
            if (Item.Quantity > 1)
            {
                Item.Quantity -= 1;
            }
        }

        private void IncQtyClicked(Object obj)
        {
            ItemCatalogueModel Item = (ItemCatalogueModel)obj;
            Item.Quantity += 1;    
        }

       
        public async void SetCartCount()
        {
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            SearchItemsDataService SearchCartCountdata = new SearchItemsDataService();
            CartItemCount = await SearchCartCountdata.GetCartServiceCount(UserId.ToString());
            AppSettings.AddOrUpdateValue(Resources.CartItemCount,  CartItemCount.ToString());
        }
        
        public async void GetItems(string SearchString)
        {
            SearchItemsDataService SearchCartItemdata = new SearchItemsDataService();
            var ItemList = await SearchCartItemdata.GetCartServiceItem(SearchString);
            ItemCatalogueList = new ObservableCollection<ItemCatalogueModel>(ItemList);
        }
            

        private async void AddToCartClicked(object obj)
        {
            SearchItemsDataService AddCartItemdata = new SearchItemsDataService();
            HttpResponseMessage response = await AddCartItemdata.AddItemToCartService(obj);
            var status = response.StatusCode;
            if (status == HttpStatusCode.OK)
            {
                 CartItemCount += 1;
                AppSettings.AddOrUpdateValue(Resources.CartItemCount,  CartItemCount.ToString());

                DependencyService.Get<IMessage>().LongAlert(Resources.CartAddMsg);
            }
            else if (status == HttpStatusCode.Found)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.CartUpdateMsg);
            }
            else if (status == HttpStatusCode.NotModified)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.CartErrorMsg);
            }

        }       
        private FormUrlEncodedContent MakeFormContent(string UserId,long ItemId,long Quantity)
        {
           
            var formcontent = new FormUrlEncodedContent(new[]
            {
                        new KeyValuePair<string,string>(Resources.UserId,UserId),
                        new KeyValuePair<string,string>(Resources.ItemId,ItemId.ToString()),
                        new KeyValuePair<string,string>(Resources.Quantity,Quantity.ToString())
            });
            return formcontent;
        }


        private async void CartClicked(object obj)
        {
            if( CartItemCount==0)
            await Navigation.PushAsync(new EmptyCartPage());
            else
            {
                await Navigation.PushAsync(new CartPage());

            }
        }

        private async void BackButtonClicked(object obj)
        {
            await Navigation.PushAsync(new CategoryTilePage());
        }

        #endregion
    }
}