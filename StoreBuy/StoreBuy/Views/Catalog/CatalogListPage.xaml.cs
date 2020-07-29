using Plugin.Settings;
using Plugin.Settings.Abstractions;
using StoreBuy.DataService;
using StoreBuy.Models;
using StoreBuy.ViewModels.Catalog;
using Syncfusion.DataSource;
using Syncfusion.ListView.XForms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace StoreBuy.Views.Catalog
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CatalogListPage
    {
        private static ISettings AppSettings =>
        CrossSettings.Current;
        public CatalogListPage()
        {
            InitializeComponent();
           
            BindingContext = new ItemCataloguePageViewModel(Navigation);
        }
        protected override void OnAppearing()
        {
            (BindingContext as ItemCataloguePageViewModel).GetItems();
           
        }

       
    }
}