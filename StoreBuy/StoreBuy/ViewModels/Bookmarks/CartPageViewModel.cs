using System.Collections.ObjectModel;
using StoreBuy.Models;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Runtime.Serialization;
using StoreBuy.Controls;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using StoreBuy.Views.ErrorAndEmpty;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using StoreBuy.Views;
using StoreBuy.Views.Detail;
using StoreBuy.Views.Catalog;
using StoreBuy.DataService;

namespace StoreBuy.ViewModels.Bookmarks
{

    [Preserve(AllMembers = true)]
    [DataContract]
    public class CartPageViewModel : BaseViewModel
    {
        #region Fields
        private static ISettings AppSettings => CrossSettings.Current;

        private long cartItemCount;

        private Command incQtyCommand;

        private Command decQtyCommand;

        private Command removeCommand;

        private Command backButtonCommand;

        private Command searchStoresCommand;

        private INavigation Navigation;

        #endregion

        #region Constructor
        public CartPageViewModel(INavigation _navigation)
        {
             Navigation = _navigation;
        }

        #endregion

        #region Public properties


        private ObservableCollection<ItemCatalogueModel> cartItems;
        [DataMember(Name = "CartItems")]
        public ObservableCollection<ItemCatalogueModel> CartItems
        {
            get { return cartItems; }
            set
            {
                if (cartItems == value)
                {
                    return;
                }
                cartItems = value;
                NotifyPropertyChanged();
            }
        }

        public long CartItemsCount
        {
            get { return  cartItemCount; }
            set
            {
                if ( cartItemCount == value)
                {
                    return;
                }
                 cartItemCount = value;
                 NotifyPropertyChanged();
            }
        }


        #endregion

        #region Command
        public Command RemoveCommand
        {
            get { return  removeCommand ?? ( removeCommand = new Command( RemoveClicked)); }
        }

        public Command BackButtonCommand
        {
            get { return  backButtonCommand ?? ( backButtonCommand = new Command( BackButtonClicked)); }
        }

        public Command IncQtyCommand
        {
            get { return  incQtyCommand ?? ( incQtyCommand = new Command( IncQtyClicked)); }
        }

        public Command DecQtyCommand
        {
            get { return  decQtyCommand ?? ( decQtyCommand = new Command( DecQtyClicked)); }
        }

        public Command SearchStoresCommand
        {
            get { return  searchStoresCommand ?? ( searchStoresCommand = new Command( searchStoresClicked)); }
        }
        #endregion

        #region Methods

        private void DecQtyClicked(object obj)
        {
            ItemCatalogueModel Item = (ItemCatalogueModel)obj;
            if (Item.Quantity > 1)
            {
                Item.Quantity -= 1;
                
                QuantitySelected(Item);
            }
        }

        private void IncQtyClicked(object obj)
        {
            ItemCatalogueModel Item = (ItemCatalogueModel)obj;
            Item.Quantity += 1;
            QuantitySelected(Item);

        }
        public async void GetCartItems()
        {
            CartPageDataService cartPageData = new CartPageDataService();
            var ItemCatalogue = await cartPageData.GetServiceCartItems();
            CartItems = new ObservableCollection<ItemCatalogueModel>(ItemCatalogue);
            CartItemsCount = CartItems.Count;
            AppSettings.AddOrUpdateValue(Resources.CartItemCount, CartItemsCount.ToString());
            if (CartItemsCount == 0)
            {
                await Navigation.PushAsync(new EmptyCartPage());
            }

        }

        private async void RemoveClicked(object obj)
        {
            CartPageDataService cartPageData = new CartPageDataService();
            var SelectedItem = (ItemCatalogueModel)obj;
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            var formcontent = new FormUrlEncodedContent(new[]
                                                        {
                                                            new KeyValuePair<string,string>(Resources.UserId,UserId),
                                                            new KeyValuePair<string, string>(Resources.ItemId,SelectedItem.ItemId.ToString()),
                                                        });
            HttpResponseMessage response = await cartPageData.ServiceRemove(obj);
            var status = response.StatusCode;
            if (status == HttpStatusCode.OK)
            {
                CartItems.Remove(SelectedItem);
                CartItemsCount = CartItems.Count;
                AppSettings.AddOrUpdateValue(Resources.CartItemCount, CartItemsCount.ToString());

                if (CartItemsCount == 0)
                {
                    await Navigation.PushAsync(new EmptyCartPage());
                }

            }
            else
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.ErrorMsg);
            }
            
        }

        private async void QuantitySelected(ItemCatalogueModel SelectedItem)
        {
            CartPageDataService cartPageData = new CartPageDataService();
            HttpResponseMessage response = await cartPageData.ServiceQuantityUpdate(SelectedItem);
            var status = response.StatusCode;
        }

        private async void BackButtonClicked(object obj)
        {
            await Navigation.PushAsync(new CategoryTilePage());
        }
        private async void searchStoresClicked(object obj)
        {
            await Navigation.PushAsync(new StoresPage());
        }

        #endregion
    }
}

