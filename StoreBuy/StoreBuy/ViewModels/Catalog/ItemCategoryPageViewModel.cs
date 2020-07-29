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
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.ViewModels.Catalog
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class ItemCategoryPageViewModel : BaseViewModel
    {
        #region Fields
        private static ISettings AppSettings => CrossSettings.Current;
        private ObservableCollection<ItemCategory> categories;
        private Command categorySelectedCommand;
        private Command searchCommand;
        private Command cartItemCommand;        
        private long cartItemCount;
        private INavigation Navigation;

        #endregion

        #region Constructor

        public ItemCategoryPageViewModel(INavigation navigation)
        {
             Navigation = navigation;
        }

        #endregion

        #region Properties

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
        [DataMember(Name = "categories")]
        public ObservableCollection<ItemCategory> Categories
        {
            get { return categories; }
            set
            {
                if (categories == value)
                {
                    return;
                }

                categories = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods

        public async void GetCategories()
        {
            ItemCategoryDataService DataCategory = new ItemCategoryDataService();
            var CategoryList = await DataCategory.AllCategories();
            Categories = new ObservableCollection<ItemCategory>(CategoryList);
            SetCartCount();
        }


        public async void SetCartCount()
        {
            var UserId = AppSettings.GetValueOrDefault(Resources.UserId, Resources.DefaultIntValue);
            SearchItemsDataService SearchCartCountdata = new SearchItemsDataService();
            CartItemCount = await SearchCartCountdata.GetCartServiceCount(UserId.ToString());
            AppSettings.AddOrUpdateValue(Resources.CartItemCount, CartItemCount.ToString());

        }

        private async void CategorySelected(object obj)
        {
            ItemCategory ItemCategory = (ItemCategory)obj;

            AppSettings.AddOrUpdateValue(Resources.CategoryId, ItemCategory.CategoryId.ToString());
            await Navigation.PushAsync(new CatalogListPage());
        }

        private async void SearchClicked(object obj)
        {
            await Navigation.PushAsync(new SearchItemsListPage());
        }
        private async void CartClicked(object obj)
        {
            if (CartItemCount == 0)
                await Navigation.PushAsync(new EmptyCartPage());
            else
            {
                await Navigation.PushAsync(new CartPage());
            }
        }


        #endregion

        #region Commands

        public Command CategorySelectedCommand
        {
            get { return categorySelectedCommand ?? (categorySelectedCommand = new Command(CategorySelected)); }
        }

        public Command CartItemCommand
        {
            get { return cartItemCommand ?? (cartItemCommand = new Command(CartClicked)); }
        }
        
        public Command SearchCommand
        {
            get { return searchCommand ?? (searchCommand = new Command( SearchClicked)); }
        }

        #endregion

    }
}

