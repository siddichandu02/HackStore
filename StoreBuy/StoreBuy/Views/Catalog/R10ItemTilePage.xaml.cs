using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.ViewModels.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Catalog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class R10ItemTilePage 
    {
        private static ISettings AppSettings =>
           CrossSettings.Current;
        public R10ItemTilePage()
        {
            InitializeComponent();
            BindingContext = new R10ItemCataloguePageViewModel(Navigation);

        }
        protected override void OnAppearing()
        {
            (BindingContext as R10ItemCataloguePageViewModel).GetItems();

        }
    }
}