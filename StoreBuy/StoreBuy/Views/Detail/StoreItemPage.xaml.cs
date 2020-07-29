using StoreBuy.Models;
using StoreBuy.ViewModels.Detail;
using StoreBuy.Views.Detail;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Detail
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StoreItemPage : ContentPage
    {
       

        public StoreItemPage(List<ItemCatalogueModel> StoreItemsList)
        {
            InitializeComponent();
            BindingContext = new StoreItemViewModel(Navigation, StoreItemsList);
        }
       
    }
}