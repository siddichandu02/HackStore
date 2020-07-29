using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.Views.Catalog;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace StoreBuy.ViewModels.ErrorAndEmpty
{

    [Preserve(AllMembers = true)]
    public class EmptyCartPageViewModel : BaseViewModel
    {
        #region Fields
                
        private string imagePath;

        private string header;

        private string content;

        private INavigation Navigation;

        #endregion

        #region Constructor

        public EmptyCartPageViewModel(INavigation navigation)
        {
             Navigation = navigation;
             ImagePath = "EmptyCart.svg";
             Header = "CART IS EMPTY";
             Content = "You don’t have any items in your cart";
             GoBackCommand = new Command( GoBack);
        }

        #endregion

        #region Commands


        public ICommand GoBackCommand { get; set; }

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
            await Navigation.PushAsync(new CategoryTilePage());
        }

        #endregion
    }
}
