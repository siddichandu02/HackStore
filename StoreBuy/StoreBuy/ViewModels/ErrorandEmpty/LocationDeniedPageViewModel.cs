using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Views.Bookmarks;
using StoreBuy.Views.Catalog;
using StoreBuy.Views.Detail;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.ErrorAndEmpty
{

    [Preserve(AllMembers = true)]
    public class LocationDeniedPageViewModel : BaseViewModel
    {
        #region Fields

        private INavigation Navigation;
        private string imagePath;
        private string header;
        private string content;
        private static ISettings AppSettings => CrossSettings.Current;

        #endregion

        #region Constructor

        public LocationDeniedPageViewModel(INavigation navigation)
        {
            Navigation = navigation;
             ImagePath = "LocationAccessDenied.svg";
             Header = "LOCATION BLOCKED";
             Content = "Please Enable Location to get near by stores";
             GoBackCommand = new Command(GoBack);
            HomeCommand = new Command(GoHome);
        }



        #endregion

        #region Commands

        public ICommand GoBackCommand { get; set; }
        public ICommand HomeCommand { get; set; }

        #endregion

        #region Properties

        public string ImagePath
        {
            get
            {
                return  imagePath;
            }



            set
            {
                 imagePath = value;
                 NotifyPropertyChanged();
            }
        }

        public string Header
        {
            get
            {
                return  header;
            }

            set
            {
                 header = value;
                 NotifyPropertyChanged();
            }
        }

        public string Content
        {
            get
            {
                return  content;
            }

            set
            {
                 content = value;
                 NotifyPropertyChanged();
            }
        }

        #endregion

        #region Methods

        private async void GoBack(object obj)
        {
            await Navigation.PushAsync(new StoresPage());
        }

        private async void GoHome(object obj)
        {
            await Navigation.PushAsync(new CategoryTilePage());
        }

        #endregion
    }
}
