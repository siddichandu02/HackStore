using Newtonsoft.Json;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.Views.Bookmarks;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.ErrorAndEmpty;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ItemCataloguePageViewModel : BaseViewModel
    {
        #region Fields  
        private static ISettings AppSettings => CrossSettings.Current;
        private Command addToCartCommand;
        private Command searchCommand;
        private Command cardItemCommand;
        private Command backButtonCommand;
        private Command incQtyCommand;
        private Command decQtyCommand;
        private long cartItemCount;
        private List<ItemCatalogueModel> cartItems;
        private long ItemCategoryId { set; get; }
        private readonly INavigation Navigation;
        private ObservableCollection<ItemCatalogueModel> items;

        #endregion

        #region Constructor

        public ItemCataloguePageViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
             ItemCategoryId = Int64.Parse(AppSettings.GetValueOrDefault(Resources.CategoryId, Resources.DefaultIntValue));
        }

        #endregion

        #region Properties

        [DataMember(Name = "ItemCatalogueList")]
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

        private List<ItemCatalogueModel> CartItems
        {
            get
            {
                return  cartItems;
            }
            set
            {
                 cartItems = value;
                 NotifyPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public Command IncQtyCommand
        {
            get { return  incQtyCommand ?? ( incQtyCommand = new Command( IncQtyClicked)); }
        }

        public Command DecQtyCommand
        {
            get { return  decQtyCommand ?? ( decQtyCommand = new Command( DecQtyClicked)); }
        }

        public Command AddToCartCommand
        {
            get { return  addToCartCommand ?? ( addToCartCommand = new Command( AddToCartClicked)); }
        }

        public Command CardItemCommand
        {
            get { return  cardItemCommand ?? ( cardItemCommand = new Command( CartClicked)); }
        }

        public Command BackButtonCommand
        {
            get { return  backButtonCommand ?? ( backButtonCommand = new Command( BackButtonClicked)); }
        }

        public Command SearchCommand
        {
            get { return searchCommand ?? (searchCommand = new Command( SearchClicked)); }
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
        
        public async void GetItems()
        {
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            SearchItemsDataService SearchCartCountdata = new SearchItemsDataService();
            CartItemCount = await SearchCartCountdata.GetCartServiceCount(UserId.ToString());
            AppSettings.AddOrUpdateValue(Resources.CartItemCount, CartItemCount.ToString());

            ItemCatalogueDataService DataItems = new ItemCatalogueDataService();
            var ItemCatalogue = await DataItems.GetServiceItems(ItemCategoryId);
            ItemCatalogueList = new ObservableCollection<ItemCatalogueModel>(ItemCatalogue);
            
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

        private async void CartClicked(object obj)
        {
            if ( CartItemCount == 0)
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


        private async void SearchClicked(object obj)
        {
            await Navigation.PushAsync(new SearchItemsListPage());
        }
        #endregion
    }
}