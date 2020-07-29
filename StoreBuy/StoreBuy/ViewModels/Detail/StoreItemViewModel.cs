using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace StoreBuy.ViewModels.Detail
{
    public class StoreItemViewModel : BaseViewModel
    {
        #region Fields

        private static ISettings AppSettings => CrossSettings.Current;
        private List<ItemCatalogueModel> storeItems;
        private INavigation Navigation;
        private float itemsTotalPrice;

        #endregion

        #region Constructor

        public StoreItemViewModel(INavigation navigation, List<ItemCatalogueModel> StoreItemsList)
        {
            Navigation = navigation;
            itemsTotalPrice = 0;
            foreach (ItemCatalogueModel Item in StoreItemsList)
            {
                Item.TotalPrice = Item.Quantity * Item.Price;
                itemsTotalPrice += Item.TotalPrice;
            }
             StoreItems = StoreItemsList;
             ItemsTotalPrice = itemsTotalPrice;
        }

        #endregion

        #region Public Properties

        public List<ItemCatalogueModel> StoreItems
        {
            get
            {
                return  storeItems;
            }
            set
            {
                if ( storeItems == value)
                {
                    return;
                }
                 storeItems = value;
                 NotifyPropertyChanged();
            }
        }
        public float ItemsTotalPrice
        {
            get
            {
                return  itemsTotalPrice;
            }
            set
            {
                if ( itemsTotalPrice == value)
                {
                    return;
                }
                 itemsTotalPrice = value;
                 NotifyPropertyChanged();
            }
        }

        #endregion
    }
}