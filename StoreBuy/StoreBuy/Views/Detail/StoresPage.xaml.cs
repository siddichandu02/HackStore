using StoreBuy.ViewModels.Detail;
using System;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Detail
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoresPage
    {
        public StoresPage()
        {
            InitializeComponent();
            BindingContext = new StorePageViewModel(Navigation);
        }

        protected override void OnAppearing()
        {
            //(BindingContext as StorePageViewModel).SortStoresByDistance(new object());
        }
      
    }
}