using Xamarin.Forms;
using StoreBuy.Models.Detail;
using System.Collections.ObjectModel;
using Xamarin.Forms.Internals;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Essentials;
using System;
using Plugin.Settings.Abstractions;
using Plugin.Settings;
using StoreBuy.Views.Bookmarks;
using System.Linq;
using StoreBuy.Models;
using StoreBuy.Views.Detail;
using Xamarin.Forms.Maps;
using StoreBuy.Views.ErrorAndEmpty;
using StoreBuy.DataService;

namespace StoreBuy.ViewModels.Detail
{
    [Preserve(AllMembers = true)]
    public class StorePageViewModel : BaseViewModel
    {
        #region Fields

        private static ISettings AppSettings => CrossSettings.Current;
        private bool displayPopup;
        private ObservableCollection<StoreModel> stores;
        private long latitude;
        private long longitude;
        private readonly INavigation Navigation;

        #endregion

        #region Properties
        public ObservableCollection<StoreModel> StoreDetails
        {
            get { return  stores; }
            set
            {
                if ( stores == value)
                {
                    return;
                }
                 stores = value;
                 NotifyPropertyChanged();
            }
        }


        private long Latitude
        {
            get { return latitude; }
            set
            {
                if (latitude == value)
                {
                    return;
                }
                latitude = value;
                
            }
        }

        private long Longitude
        {
            get { return longitude; }
            set
            {
                if (longitude == value)
                {
                    return;
                }
                longitude = value;
                SortStoresByItems(new object());
            }
        }

        public bool DisplayPopup
        {
            get { return  displayPopup; }
            set
            {
                if ( displayPopup == value)
                {
                    return;
                }

                 displayPopup = value;
                 NotifyPropertyChanged();
            }
        }


        #endregion

        #region Constructor

        public StorePageViewModel(INavigation _navigation)
        {
             ItemDetails = new Command( ItemDetailsClicked);
             BackCommand = new Command( BackButtonClicked);
             SortCommand = new Command( SortButtonCliked);
             SortDistanceCommand = new Command( SortStoresByDistance);
             SortItemCommand = new Command( SortStoresByItems);
             MapIconCommand = new Command( MapIconClicked);
             ViewItemsCommand = new Command( ViewItemsClicked);
             BookSlotCommand = new Command( BookSlotClicked);
             Navigation = _navigation;
             GetLocation();
        }

        #endregion

        #region Methods

        private  async void MapIconClicked(object obj)
        {
            try
            {
                StoreModel Store = (StoreModel)obj;
                var placemarksUser = await Geocoding.GetPlacemarksAsync(Latitude, Longitude);
                Placemark placemarkUser = placemarksUser.FirstOrDefault();
                var placemarksStore = await Geocoding.GetPlacemarksAsync(Store.Latitude, Store.Longitude);
                Placemark placemarkStore = placemarksStore.FirstOrDefault();
                await Launcher.OpenAsync("http://maps.google.com/?daddr=" + placemarkUser.FeatureName + ",&saddr=" + placemarkStore.FeatureName);
                
            }
            catch (Exception exe)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.NoLocationErrorMsg);
            }

        }
        public async void SortStoresByItems(Object obj)
        {
             DisplayPopup = false;
            StorePageDataService storePageData = new StorePageDataService();
            var StoreList = await storePageData.GetStoresByItemCount(Latitude, Longitude);
            StoreDetails = new ObservableCollection<StoreModel>(StoreList);
    
        }
        public async void SortStoresByDistance(object obj)
        {
             DisplayPopup = false;
            StorePageDataService storePageData = new StorePageDataService();
            var StoreList = await storePageData.GetStoresByDistance(Latitude, Longitude);
             StoreDetails = new ObservableCollection<StoreModel>(StoreList);
            
        }


        private void SortButtonCliked(Object obj)
        {
             DisplayPopup = true;
        }
       
        public async void GetLocation()
        {
            try
            {

                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                     Latitude = Convert.ToInt64(location.Latitude);
                    Longitude = Convert.ToInt64(location.Longitude);

                }
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.FeatureErrorMsg);
            }
            catch (FeatureNotEnabledException fneEx)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.LocationEnableMsg);
            }
            catch (PermissionException pEx)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.PermissionMsg);
                await Navigation.PushAsync(new LocationDeniedPage());

            }
            catch (Exception ex)
            {
                DependencyService.Get<IMessage>().LongAlert(Resources.LocationErrorMsg);
            }
        }
        private async void BackButtonClicked(object obj)
        {
            await Navigation.PushAsync(new CartPage());
        }

        private void ItemDetailsClicked(object obj)
        {
            
        }
        #endregion

        #region Command

        public Command BackCommand { get; set; }

        public Command ItemDetails { get; set; }

        public Command ViewItemsCommand { get; set; }

        private async void ViewItemsClicked(object obj)
        {
            var storeId = (long)obj;
            StoreModel SelectedStore = StoreDetails.Where(x => x.StoreId == storeId).SingleOrDefault();
            List<ItemCatalogueModel> ItemsList = SelectedStore.ListItems;
            await Navigation.PushAsync(new StoreItemPage(ItemsList));
        }
        private async void BookSlotClicked(object obj)
        {
            StoreModel SelectedStore = (StoreModel)obj;
            await Navigation.PushAsync(new StoreSlotPage(SelectedStore));
        }
        public Command SortCommand { get; set; }

        public Command SortDistanceCommand { get; set; }

        public Command SortItemCommand { get; set; }

        public Command MapIconCommand { get; set; }

        public Command BookSlotCommand { get; set; }

        #endregion
    }
}